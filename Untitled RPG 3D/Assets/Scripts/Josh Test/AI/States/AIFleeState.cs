using FullscreenEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Joshua
public class AIFleeState : AIState
{

    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    [SerializeField]LayerMask ignoreTheseLayers;
    [SerializeField] LayerMask groundLayer;

    [SerializeField]bool timeToHide;
    [SerializeField]Vector3 destPoint;

    public override void EnterState(AIStateManager state)
    {
        controller.anim.SetBool("isFleeing", true);
        controller.anim.ResetTrigger("Hit");
        timeToHide = false;
    }

    public override void UpdateState(AIStateManager state)
    {
        Debug.DrawLine(controller.player.transform.position, destPoint, Color.blue);

        //check to see if AI has moved far enough away from player to look for hiding spot
        if (Vector3.Distance(transform.position, controller.player.transform.position) > controller.stats.sightDistance)
        {

            if(timeToHide == false)
            {
                float range = 20f;//keeping this number high will prevent wurgle from running into walls

                float z = Random.Range(-range, range);
                float x = Random.Range(-range, range);

                destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);//select random point

                //confirm random point is hidden from player
                if (Physics.Linecast(controller.player.transform.position, destPoint, ~ignoreTheseLayers))
                {

                    //this runs a line from desired hiding point towards the player, hits the first object, then decides to run closer to the object
                    RaycastHit hit;
                    if (Physics.Linecast(destPoint, controller.player.transform.position, out hit, ~ignoreTheseLayers) && Vector3.Distance(destPoint, hit.transform.position) > 2)
                    {
                        destPoint = Vector3.Lerp(destPoint, hit.transform.position, 0.9f);// new destination is close towards object

                        //set up for seeing if destination is on navmesh
                        NavMeshPath path = new NavMeshPath();
                        NavMesh.CalculatePath(transform.position, destPoint, NavMesh.AllAreas, path);

                        //make sure destination can be reached via navmesh
                        if (path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(destPoint, controller.player.transform.position) > controller.stats.sightDistance)
                        {
                            timeToHide = true;
                        }
                    }
                }   
            }
            //run to detination and switch to hide state
            else
            {
                
                controller.agent.isStopped = false;
                controller.agent.SetDestination(destPoint);

                Debug.DrawLine(transform.position, destPoint, Color.red);

                if (Vector3.Distance(transform.position, destPoint) < 1f)
                {

                    controller.anim.SetBool("isFleeing", false);
                    state.SwitchToTheNextState(state.HideState);
                    stateManager.angry = false;

                }
                
            }
        }
        //if AI within range of player run away
        else
        {
            Vector3 playerDirection = this.transform.position - controller.player.transform.position;

            controller.agent.destination = this.transform.position + playerDirection;

            timeToHide = false;
        }
    }

    public override void ExitState(AIStateManager state)
    {
        //unused
    }
}
