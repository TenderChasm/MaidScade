using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlgigMaid : MovingActor
{
    public int MaxDashCount = 3;
    public float TirednessTime = 3;
    public int Damage = 1;
    public float DashInitialSpeed = 6;
    public float TimeoutBetweenDashes = 0.5F;
    public Collider2D SpinningCollider;

    public bool IsAggressive = true;

    private bool isDashing { get; set; } =  false;
    private int performedDashes { get; set; } = 0;
    private bool isTired { get; set; } = false;

    protected override void Start()
    {
        base.Start();
        isApplyingMovement = false;
        SpinningCollider.enabled = false;
    }

    void Dash()
    {
        Health.IsInvisible = true;
        Audio.Play();
        SpinningCollider.enabled = true;
        Vector2 protagonistPosition = GameManager.Hr.Protagonist.transform.position;
        Vector2 direction = (protagonistPosition - (Vector2)transform.position).normalized;
        Rigidbody.velocity = direction * DashInitialSpeed;
        isDashing = true;
        WalkingController.Animator.Play("spinning", 0);
    }

    void ChooseAnimationLookingTowardsPlayer()
    {
        Vector2 protagonistPosition = GameManager.Hr.Protagonist.transform.position;
        Vector2 direction = (protagonistPosition - (Vector2)transform.position).normalized;

        if (direction.y > 0.5)
            WalkingController.Animator.Play("idleUp", 0);
        else if (direction.x > 0.5)
            WalkingController.Animator.Play("idleRight", 0);
        else if (direction.y < 0.5)
            WalkingController.Animator.Play("idleDown", 0);
        else if (direction.x < 0.5)
            WalkingController.Animator.Play("idleLeft", 0);
    }

    IEnumerator StopDashingAndTimeoutOrTire()
    {
        Health.IsInvisible = false;
        Audio.Stop();
        SpinningCollider.enabled = false;
        isDashing = false;
        Rigidbody.velocity = Vector2.zero;
        performedDashes++;
        if (performedDashes == MaxDashCount)
        {
            StartCoroutine(PerformTiredness());
        }
        else
        {
            ChooseAnimationLookingTowardsPlayer();
            yield return new WaitForSeconds(TimeoutBetweenDashes);
            Dash();
        }
    }

    IEnumerator PerformTiredness()
    {
        isDashing = false;
        performedDashes = 0;
        isTired = true;

        WalkingController.Animator.Play("tired", 0);

        yield return new WaitForSeconds(TirednessTime);

        isTired = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Health.Health > 0 && IsAggressive)
        {
            if (!isDashing && !isTired && performedDashes == 0)
            {
                Dash();
            }

            if (isDashing && Rigidbody.velocity.magnitude < DashInitialSpeed * 0.6)
            {
                StartCoroutine(StopDashingAndTimeoutOrTire());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(isDashing)
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Health.Health -= Damage;
            }
        }
    }

    protected override void OnDeath()
    {
        StopAllCoroutines();
        RestoreValues();

        Audio.Stop();
        Rigidbody.drag = 1;
        SpinningCollider.enabled = false;
        isDashing = false;
        Health.IsInvisible = true;
        WalkingController.Animator.Play("goingToSleep");
    }

    public void EnteringSleep()
    {
        WalkingController.Animator.Play("sleeping");
    }


}
