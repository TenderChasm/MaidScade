using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaDamagable : MonoBehaviour
{
    public AnimatedTile Lava;
    public HealthComponent Health { get; set; } 

    void Start()
    {
        Health = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePos = GameManager.Hr.Floor.WorldToCell(transform.position);
        TileBase tile = GameManager.Hr.Floor.GetTile(tilePos);

        if (tile == Lava)
            Health.Health -= 1;
    }
}
