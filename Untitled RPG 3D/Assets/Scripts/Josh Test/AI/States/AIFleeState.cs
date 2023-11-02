using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Joshua 2023/11/02

public class AIFleeState : AIState
{

    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    [SerializeField]LayerMask ignoreTheseLayers;
    [SerializeField] LayerMask groundLayer;

    [SerializeField]bool timeToHide;
    [SerializeField]Vector3 destPoint;

    [Header ("Sound Storage")]
    int scream;

    public override void EnterState(AIStateManager state)
    {
        controller.ChangeAnimationState(AIController.AnimState.Flee, 0.1f, 0);
        controller.agent.speed = controller.stats.speed;
        timeToHide = false;
        scream = AudioSystem.AudioManager.AudioManagerInstance.PlaySound("SCREAM");
       // this.GetComponentInParent<SoundController>().PlaySound(0);
    }

    public override void UpdateState(AIStateManager state)
    {
        FleeBehaviour(state);
    }

    public override void ExitState(AIStateManager state)
    {
        AudioSystem.AudioManager.AudioManagerInstance.StopSound(scream);
       // this.GetComponentInParent<SoundController>().StopSound(0);
    }

    private void FleeBehaviour(AIStateManager state)
    {
        Debug.DrawLine(controller.player.transform.position, destPoint, Color.blue);

        //check to see if AI has moved far enough away from player to look for hiding spot
        if (Vector3.Distance(transform.position, controller.player.transform.position) > controller.stats.agroRange)
        {

            if (timeToHide == false)
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
                        if (path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(destPoint, controller.player.transform.position) > controller.stats.agroRange)
                        {
                            timeToHide = true;
                        }
                    }
                }
            }
            //run to detination and switch to hide state
            else
            {
                //make sure player still can't see location  **NOT FULLY TESTED**
                if (Physics.Linecast(controller.player.transform.position, destPoint, ~ignoreTheseLayers))
                {

                    

                    controller.agent.isStopped = false;
                    controller.agent.SetDestination(destPoint);

                    Debug.DrawLine(transform.position, destPoint, Color.red);

                    if (Vector3.Distance(transform.position, destPoint) < 1f)
                    {
                        state.SwitchToTheNextState(state.HideState);
                    }
                }
                else
                {
                    timeToHide = false;
                }
            }
        }
        //if AI within range of player, run away
        else
        {

            Vector3 playerDirection = this.transform.position - controller.player.transform.position;

            controller.agent.destination = this.transform.position + playerDirection;

            timeToHide = false;
        }

    }
}
