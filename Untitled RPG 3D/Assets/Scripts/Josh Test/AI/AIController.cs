using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using static AIStateManager;

public class AIController : MonoBehaviour
{

    public Animator anim;
    public NavMeshAgent agent;
    public EnemyScriptableObject stats;
    public GameObject player;

    public enum AnimState {Idle, IdleWalk, Chase, Attack, Stagger, Death, Flee, Hide};
    public AnimState animState;

    float tempStrength;
    public bool isHit;
    float weight;
    Vector3 playerPos;

    private void Start()
    {
        player = GameObject.Find("Player");
        animState = AnimState.Idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHit)
        {
            //hitObject.transform.Translate(direction * knockbackStrength * Time.deltaTime);
            this.transform.position += playerPos * Time.deltaTime * (tempStrength -= weight);
            if (tempStrength <= 0)
            {
                tempStrength = 0;
                isHit = false;
            }
        }
    }

    private void Update()
    {
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public void ChangeAnimationState(AnimState nextState, float transitionTime, int layer)
    {

        if (nextState == animState)
        {
            return;
        }

        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        animState = nextState;
    }

    public void RepeatAnimationState(AnimState nextState, float transitionTime, int layer)
    {
        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        animState = nextState;
    }

    public bool IsAnimationPlaying(Animator anim, AnimState stateName)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(stateName.ToString()) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAnimationDone(Animator anim, AnimState stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName.ToString()) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KnockedBack(float knockbackStrength, Vector3 playerPosition)
    {
        tempStrength = 0;
        tempStrength = knockbackStrength;
        isHit = false;
        isHit = true;
        weight = knockbackStrength / 25;
        playerPos = playerPosition; 
        // anim.SetTrigger("Hit");

    }

    public void RotateToPlayer()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100);
    }


}
