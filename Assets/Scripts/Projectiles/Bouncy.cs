using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : Orb
{
    public float TimeToPop = 6;
    public AudioClip Bounce;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        StartCoroutine(SetOffTimer());
    }

    IEnumerator SetOffTimer()
    {
        yield return new WaitForSeconds(TimeToPop);
        base.OnCollisionEnter2D(null);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Player>() != null)
            base.OnCollisionEnter2D(collision);
        else
            Audio.PlayOneShot(Bounce);
    }


}
