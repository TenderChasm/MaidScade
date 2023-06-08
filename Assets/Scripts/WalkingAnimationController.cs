using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(MovingActor))]
public class WalkingAnimationController : MonoBehaviour
{
    public bool AnimationActive = true;
    public float AnimationSpeed = 1;
    public Animator Animator;

    protected MovingActor Actor;
    protected string CurrentAnimationName;

    protected virtual void Start()
    {
        Actor = GetComponent<MovingActor>();
        Animator = GetComponent<Animator>();
        CurrentAnimationName = "idleDown";
    }

    public void DisableAnimation()
    {
        AnimationActive = false;
        CurrentAnimationName = "";
    }

    public void EnableAnimation()
    {
        AnimationActive = true;
        CurrentAnimationName = "";
    }

    public virtual void AnimateActor(float speedMod = 1f)
    {
        if (!AnimationActive)
            return;

        string animationName = "idleDown";

        if (Actor.LookingDirection.normalized.y > 0.5)
        {
            animationName = "goingUp";
            if (Actor.Movement == Vector2.zero)
                animationName = "idleUp";
            if (Actor.Movement.y < 0)
                animationName += "Backwards";
        }
        else
        if (Actor.LookingDirection.normalized.y < -0.5)
        {
            animationName = "goingDown";
            if (Actor.Movement == Vector2.zero)
                animationName = "idleDown";
            if (Actor.Movement.y > 0)
                animationName += "Backwards";
        }
        else
        if (Actor.LookingDirection.normalized.x > 0.5)
        {
            animationName = "goingRight";
            if (Actor.Movement == Vector2.zero)
                animationName = "idleRight";
            if (Actor.Movement.x < 0)
                animationName += "Backwards";
        }
        else
        if (Actor.LookingDirection.normalized.x < -0.5)
        {
            animationName = "goingLeft";
            if (Actor.Movement == Vector2.zero)
                animationName = "idleLeft";
            if (Actor.Movement.x > 0)
                animationName += "Backwards";
        }

        Animator.speed = AnimationSpeed * speedMod;

        if (!CurrentAnimationName.Equals(animationName))
        {
            Animator.Play(animationName, 0);
            CurrentAnimationName = animationName;
        }
    }
}
