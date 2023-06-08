using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float EjectionVelocity;
    public Projectile Projectile;
    public float Cooldown = 0.5F;

    public Vector2 Position;

    public int MaxAmmoCount;
    public int currentAmmo;
    public bool InfiniteAmmo;

    public ParticleSystem ParticleChildObject;
    public AudioClip ShootingSound;

    public event Action OnShoot;

    private bool isReady = true;
    private AudioSource Audio { get; set; }



    void Start()
    {
        currentAmmo = MaxAmmoCount;
        ParticleChildObject = GetComponentInChildren<ParticleSystem>();
        Audio = GetComponent<AudioSource>();
    }

    IEnumerator PerformCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(Cooldown);
        isReady = true;
    }

    private bool CheckAmmo()
    {
        return (InfiniteAmmo || currentAmmo > 0);
    }

    public bool Shoot(Vector2 direction, bool DoesInheritVelocity = true)
    {
        if(isReady && CheckAmmo())
        {
            Vector2 velocity =  GetComponent<Rigidbody2D>()?.velocity ?? Vector2.zero;

            Projectile emitted = Instantiate(Projectile);
            emitted.transform.position = (Vector2)transform.position + Position;
            emitted.GetComponent<Rigidbody2D>().velocity = direction * EjectionVelocity +
                velocity * Convert.ToInt32(DoesInheritVelocity);

            if (ParticleChildObject != null)
            {
                ParticleChildObject.transform.position = (Vector2)transform.position + Position;
                ParticleChildObject.transform.up = direction.normalized;
                ParticleChildObject.Play();
            }

            Audio.PlayOneShot(ShootingSound);

            currentAmmo--;

            OnShoot?.Invoke();

            StartCoroutine(PerformCooldown());

            return true;
        }

        return false;
    }

    void Update()
    {
        
    }
}
