using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MovingActor
{
    public float minFluctuation = -4F;
    public float maxFluctuation = 4F;
    public float durationOfRandomVelocityComponent = 0.1F;

    public Vector2 randomVelocityComponent;

    public Weapon Weapon { get; set; }

    protected override void Start()
    {
        base.Start();
        Weapon = GetComponent<Weapon>();
        StartCoroutine(ChangeRandomVelocityComponentOverTime());
    }

    IEnumerator ChangeRandomVelocityComponentOverTime()
    {
        while (true)
        {
            randomVelocityComponent = new Vector2(Random.Range(minFluctuation, maxFluctuation),
                Random.Range(minFluctuation, maxFluctuation));
            yield return new WaitForSeconds(durationOfRandomVelocityComponent);
        }
    }

    protected override void Update()
    {
        Vector2 playerPos = GameManager.Hr.Protagonist.transform.position + Weapon.Projectile.AimCorrection;
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;
        Rigidbody.velocity = direction * Speed + randomVelocityComponent;
        Weapon.Shoot(direction, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= 1;
    }
}
