using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public SpriteRenderer Renderer;
    public Rigidbody2D Rigidbody { get; set; }
    public Collider2D Collider { get; set; }
    public Animator Animator { get; set; }
    public AudioSource Audio { get; set; }

    public Vector3 AimCorrection { get => -1 * GetComponentInChildren<Collider2D>(true).offset; }

    protected virtual void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Collider.enabled = false;
        Rigidbody.velocity = Vector2.zero;
        Animator.Play("pop");
        if(Audio.clip != null)
            Audio.Play();

        HealthComponent health =  collision.gameObject.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= damage;
    }

    public void OnAnimationEndEvent()
    {
        Destroy(this.gameObject);
    }
}
