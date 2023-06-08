using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlannyAnimationController : WalkingAnimationController
{
    public bool isFlying;

    protected override void Start()
    {
        base.Start();
    }

    public override void AnimateActor(float speedMod = 1f)
    {
        if (!AnimationActive)
            return;

        if (isFlying)
        {
            Flanny.Direction dir = isFlannyUpperOrLower();

            string animationName;

            if (dir == Flanny.Direction.Upper)
                animationName = "flying";
            else
                animationName = "flyingBack";

            Animator.speed = AnimationSpeed * speedMod;

            if (!CurrentAnimationName.Equals(animationName))
            {
                Animator.Play(animationName, 0);
                CurrentAnimationName = animationName;
            }
        }
        else
        {
            base.AnimateActor();
        }
    }

    Flanny.Direction isFlannyUpperOrLower()
    {
        float diff = transform.position.y - GameManager.Hr.Protagonist.transform.position.y;
        if (diff >= 0)
            return Flanny.Direction.Upper;
        else
            return Flanny.Direction.Lower;
    }

    void Update()
    {
        
    }
}
