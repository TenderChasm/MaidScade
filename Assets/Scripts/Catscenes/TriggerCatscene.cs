using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCatscene : Catscene
{
    
    public Collider2D Collider { get; set; }
    public bool wasPlayed;

    protected override void Start()
    {
        base.Start();
        Collider = GetComponent<Collider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasPlayed && collision.GetComponent<Player>() != null)
        {
            StartCoroutine(PlayScene());
            wasPlayed = true;
        }
    }
}
