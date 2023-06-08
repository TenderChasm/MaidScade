using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsundereballs : MovingActor
{
    public float RandomStrandingTime = 2;
    public float BaseMovingInOneDirectionDuration = 1;
    public Steelball SteelballPrefab;

    private bool ballsDropped = false;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
    }

    IEnumerator MoveRandomlyHpDependent(float seconds)
    {

        float healthDependentMovingDuration = BaseMovingInOneDirectionDuration *
            (Health.Health / (float)Health.MaxHealth);
        healthDependentMovingDuration = Mathf.Max(healthDependentMovingDuration,
            BaseMovingInOneDirectionDuration * 0.3F);

        float oldSpeed = Speed;
        Speed = Mathf.Min(Speed / (Health.Health / (float)Health.MaxHealth), Speed * 3);

        int timesToMove = (int)Mathf.Ceil(seconds / healthDependentMovingDuration);

        for (int i = 0; i < timesToMove; i++)
        {
            Rigidbody.velocity = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1) * Speed;
            yield return new WaitForSeconds(healthDependentMovingDuration);
        }

        Speed = oldSpeed;
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;
        ballsDropped = false;

        WalkingController.Animator.Play("idle");
        yield return StartCoroutine(MoveRandomlyHpDependent(RandomStrandingTime));

        Rigidbody.velocity = Vector2.zero;
        WalkingController.Animator.Play("dropBalls");
        yield return new WaitUntil(() => ballsDropped);

        Steelball firstBall = Instantiate(SteelballPrefab, transform, false);
        firstBall.gameObject.transform.localPosition = new Vector2(1, -0.125F);
        Steelball secondBall = Instantiate(SteelballPrefab, transform, false);
        secondBall.gameObject.transform.localPosition = new Vector2(-0.935F, -0.125F);

        WalkingController.Animator.Play("idleEmpty");
        yield return StartCoroutine(MoveRandomlyHpDependent(RandomStrandingTime));

        isAttacking = false;

    }

    private void BallsDroppedCallback()
    {
        ballsDropped = true;
    }

    private void BallsHitGround()
    {
        Audio.Play();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isAttacking)
            StartCoroutine(AttackSequence());
    }

    protected override void OnDeath()
    {
        StopAllCoroutines();
        RestoreValues();

        Rigidbody.drag = 10;
        isAttacking = true;
        Health.IsInvisible = true;
        WalkingController.Animator.Play("tired");
    }
}
