using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int Damage = 2;

    public Collider2D Collider { get; set; }
    public Animator Animator { get; set; }
    public AudioSource Audio { get; set; }

    void Start()
    {
        Collider = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
        Audio.Play();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= Damage;
    }

    private void BlowDecay()
    {
        Collider.enabled = false;
    }

    private void onEnd()
    {
        Destroy(gameObject);
    }
}
