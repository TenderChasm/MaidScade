using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ray : Projectile
{
    public bool isFiring;
    public Explosion Explosion;
    public AnimatedTile Lava;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetfireOn()
    {
        Animator.Play("firing");
        Audio.Play();
        isFiring = true;
        Collider.enabled = true;
    }

    public void SetfireOff()
    {
        Animator.Play("aiming");
        Audio.Stop();
        isFiring = false;
        Collider.enabled = false;
    }

    public void SetLength(float length)
    {
        BoxCollider2D coll = (BoxCollider2D)Collider;
        Renderer.size = new Vector2(1, length);
        coll.offset = new Vector2(coll.offset.x, -1 * length / 2);
        coll.size = new Vector2(coll.size.x, length);
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        HealthComponent health = collision?.gameObject?.GetComponent<HealthComponent>();
        if (health != null)
            health.Health -= damage;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Objects"))
        {
            Explosion exp = Instantiate(Explosion);
            exp.transform.position = collision.gameObject.transform.position;

            Vector3Int tileDestroyedPosition = GameManager.Hr.Floor.WorldToCell(collision.gameObject.transform.position);
            TileBase tileDestroyedObjectsIsOn = GameManager.Hr.Floor.GetTile(tileDestroyedPosition);
            if(tileDestroyedObjectsIsOn != null)
                GameManager.Hr.Floor.SetTile(tileDestroyedPosition, Lava);

            Destroy(collision.gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
    
    }
}
