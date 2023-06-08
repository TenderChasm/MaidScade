using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Spawnee;
    public SpriteRenderer Renderer { get; set; }

    protected virtual void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = null;
    }

    
    public virtual GameObject Spawn()
    {
        if (enabled)
        {
            GameObject entity = Instantiate(Spawnee, transform.position, Quaternion.identity);
            entity.transform.position = transform.position;
            return entity;
        }
        else
        {
            return null;
        }
    }

}
