using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitesocksSpawner : Spawner
{
    public List<Marker> Markers;
    public Marker CentralMarker;

    public WhitesocksEnter EnterCatscene;
    public WhitesockLeave LeaveCatscene;

    public bool AttackOnSpawn;

    protected override void Start()
    {
        base.Start();
    }

    public override GameObject Spawn()
    {
        Whitesock whitesocks = base.Spawn().GetComponent<Whitesock>();
        whitesocks.BorderStandingPoints = Markers;
        whitesocks.CentralStandingPoint = CentralMarker;

        whitesocks.GazeLockTarget = GameManager.Hr.Protagonist.gameObject;
        whitesocks.isAttacking = AttackOnSpawn;

        EnterCatscene.Whitesocks = whitesocks;
        LeaveCatscene.Whitesocks = whitesocks;

        whitesocks.LeaveCatscene = LeaveCatscene;

        return whitesocks.gameObject;
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
