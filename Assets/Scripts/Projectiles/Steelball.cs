using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steelball : Projectile
{
    public float Speed = 0.5F;
    public HealthComponent Health;

    protected override void Awake()
    {
        base.Awake();
        Health = GetComponent<HealthComponent>();
        Health.OnHealthEnd += OnCrack;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = GameManager.Hr.Protagonist.transform.position + AimCorrection;
        Rigidbody.velocity = (targetPosition - (Vector2)transform.position).normalized * Speed;
    }

    void OnCrack()
    {
        Collider.enabled = false;
        Rigidbody.velocity = Vector2.zero;
        Animator.Play("pop");
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            base.OnCollisionEnter2D(collision);
    }
}
