using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Actor : MonoBehaviour
{
    public SpriteRenderer Renderer { get; set; }
    protected Collider2D Collider { get; set; }

    protected virtual void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        
    }
}
