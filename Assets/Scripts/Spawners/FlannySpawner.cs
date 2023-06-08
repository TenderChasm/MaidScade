using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlannySpawner : Spawner
{
    public List<Marker> TopMarkers;
    public List<Marker> BottomMarkers;

    public FlannyLeave LeaveCatscene;
    public FlannyEnter EnterCatscene;

    public bool AttackOnSpawn;

    protected override void Start()
    {
        base.Start();
    }

    public override GameObject Spawn()
    {
        Flanny flanny = base.Spawn().GetComponent<Flanny>();
        flanny.ThreeUpperPoints = TopMarkers;
        flanny.ThreeLowerPoints = BottomMarkers;

        flanny.GazeLockTarget = GameManager.Hr.Protagonist.gameObject;
        flanny.isAttacking = AttackOnSpawn;

        LeaveCatscene.Flanny = flanny;
        EnterCatscene.Flanny = flanny;

        flanny.LeaveCatscene = LeaveCatscene;


        return flanny.gameObject;
    }

}
