using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{

    public NavMeshAgent agent;
    public BehaviourTree tree;

    public enum ActionState { IDLE, MOVING };
    public ActionState state = ActionState.IDLE;

    public Node.Status treeStatus = Node.Status.RUNNING;

    WaitForSeconds waitForSeconds;
    Vector3  rememberedLocation;

    // Start is called before the first frame update
  public void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();

        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));

        StartCoroutine("Behave");

    }

    //This doesn't work so feel free to edit
    public Node.Status TargetExists(Vector3 target, string tag, float distance, float maxAngle)
    {

        Vector3 directionToTarget = target - this.transform.position;
        float angle = Vector3.Angle(directionToTarget, this.transform.forward);

       
        //To Fake Hearing behind AI make it OR ||
        if (angle <= maxAngle || directionToTarget.magnitude <= distance)

        {
            RaycastHit hitInfo;
                Debug.DrawRay(this.transform.position, directionToTarget, Color.red);
            if(Physics.Raycast(this.transform.position, directionToTarget, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag(tag))
                {
                    
                    return Node.Status.SUCCESS;
                }
            }
        }

        return Node.Status.FAILURE;
    }

    public Node.Status RunAway(Vector3 location, float distance)
    {
        if (state == ActionState.IDLE)
        {
            rememberedLocation = this.transform.position + (transform.position - location).normalized * distance;
        }
        return GoToLocation(rememberedLocation);

    }




    //Manage Moving to Destination
    public Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToPlayer = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.MOVING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToPlayer < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }


    //To Avoid it from running every frame in update
    IEnumerator Behave()
    {
        while (true)
        {          
                treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }

}

