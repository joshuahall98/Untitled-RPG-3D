using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;
using UnityEngine.AI;

public class AIFleeState : AIState
{

    [SerializeField]AIStateManager stateManager;
    [SerializeField]AIController controller;

    [SerializeField]LayerMask ignoreTheseLayers;

    bool timeToHide;
    Vector3 destPoint;

    public override void EnterState(AIStateManager state)
    {
        controller.anim.SetBool("isFleeing", true);
        controller.anim.ResetTrigger("Hit");
        timeToHide = false;
    }

    public override void UpdateState(AIStateManager state)
    {
        

        //hide state
        /*if (Vector3.Distance(this.transform.position, player.transform.position) > controller.stats.sightDistance * 2)
        {
            controller.anim.SetBool("isFleeing", false);
             state.SwitchToTheNextState(state.HideState);
            stateManager.angry = false;
        }*/

        if(Vector3.Distance(transform.position, controller.player.transform.position) > controller.stats.sightDistance)
        {

            if(timeToHide == false)
            {
                float z = Random.Range(-10f, 10f);
                float x = Random.Range(-10f, 10f);

                destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

                RaycastHit hit;

                if (Physics.Linecast(transform.position, destPoint, out hit, ~ignoreTheseLayers))
                {
                    Debug.Log("blocked");
                    Debug.Log(hit.collider);
                    Debug.DrawLine(transform.position, controller.player.transform.position, Color.red);
                    timeToHide = true;
                    
                }
                
            }
            else
            {
                controller.agent.isStopped = false;
                controller.agent.SetDestination(destPoint);
            }

            if (Vector3.Distance(transform.position, destPoint) < 1)
            {

                controller.anim.SetBool("isFleeing", false);
                state.SwitchToTheNextState(state.HideState);
                stateManager.angry = false;

            }




            /*RaycastHit hit;
            if (Physics.Linecast(transform.position, controller.player.transform.position, out hit, ~ignoreTheseLayers))
            {
                //all this stuff helps with debugging
                Debug.Log("blocked");
                Debug.Log(hit.collider);
                Debug.DrawLine(transform.position, controller.player.transform.position, Color.red);

                controller.anim.SetBool("isFleeing", false);
                state.SwitchToTheNextState(state.HideState);
                stateManager.angry = false;
            }*/
        }
        else
        {
            Vector3 playerDirection = this.transform.position - controller.player.transform.position;

            controller.agent.destination = this.transform.position + playerDirection;

            timeToHide = false;
        }
    }

    public override void ExitState(AIStateManager state)
    {

    }
}
