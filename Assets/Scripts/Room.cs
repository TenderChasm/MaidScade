using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Spawner[] Spawners;
    public Vector4 Boundaries;
    public bool HasBoundaries;

    public SpriteRenderer Renderer { get; set; }

    public List<GameObject> Entities;
    public int EntitiesDestroyed = 0;

    void Start()
    {
        Entities = new List<GameObject>();
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = null;
    }

    public void OnEnter()
    {
        foreach(Spawner spawner in Spawners)
        {
            Entities.Add(spawner.Spawn());
        }
    }

    public void OnLeave()
    {
        foreach (GameObject entity in Entities)
        {
            Destroy(entity);
        }

        Entities.Clear();
        EntitiesDestroyed = 0;
    }

    public bool CanLeave()
    {
        return Spawners == null || EntitiesDestroyed >= Spawners?.Length ;       
    }

    public static void IncreaseCurrentRoomDeadEntityCount()
    {
        GameManager.Hr.CurrentRoom.EntitiesDestroyed++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
