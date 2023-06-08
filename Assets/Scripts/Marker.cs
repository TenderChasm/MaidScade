using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    public SpriteRenderer Renderer { get; set; }

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
