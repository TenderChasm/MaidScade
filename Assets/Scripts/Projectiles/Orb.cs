using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Projectile
{

    public Explosion Explosion;

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        CircleCollider2D coll = (CircleCollider2D)Collider;
        coll.radius *= 3;
        coll.isTrigger = true;


        HealthComponent health = collision?.gameObject?.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= damage;

        Animator.Play("pop");
        Audio.Play();

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        HealthComponent health = collision?.gameObject?.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= damage;
    }
}
