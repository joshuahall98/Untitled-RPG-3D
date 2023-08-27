using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimController : MonoBehaviour
{
    public enum PlayerAnimState { Idle, Run, Roll, Falling, Attack1, Attack2, HeavyHold, HeavyRelease, RewindStart, RewindEnd, Knockdown, Death }
    public PlayerAnimState playerAnimState;
    public enum PlayerAnimAffectState { None, Dizzy }
    public PlayerAnimAffectState playerAnimAffectState;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        playerAnimAffectState = PlayerAnimAffectState.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAnimationState(PlayerAnimState nextState, float transitionTime, int layer)
    {

        if (nextState == playerAnimState)
        {
            return;
        }

        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        playerAnimState = nextState;
    }

    public void RepeatAnimationState(PlayerAnimState nextState, float transitionTime, int layer)
    {
        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        playerAnimState = nextState;
    }

    public bool IsAnimationPlaying(PlayerAnimState stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName.ToString()) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAnimationDone(PlayerAnimState stateName)
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

    public void ChangeAnimationAffectState(PlayerAnimAffectState nextState, float transitionTime, int layer)
    {

        if (nextState == playerAnimAffectState)
        {
            return;
        }

        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        playerAnimAffectState = nextState;
    }

    public void RepeatAnimationAffectState(PlayerAnimAffectState nextState, float transitionTime, int layer)
    {
        anim.CrossFade(nextState.ToString(), transitionTime, layer);

        playerAnimAffectState = nextState;
    }

    public bool IsAnimationAffectPlaying(PlayerAnimAffectState stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName.ToString()) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAnimationAffectDone(PlayerAnimAffectState stateName)
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
}
