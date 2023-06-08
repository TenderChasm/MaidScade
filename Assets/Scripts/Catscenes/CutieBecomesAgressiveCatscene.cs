using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class CutieBecomesAgressiveCatscene : Catscene
{

    public WhirlgigMaid Maid { get; set; }
    public Room ThisRoom;
    public GameObject Hearts;
    public Door DoorToGun;
    public Door ThisDoor;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        List<Monologue> dialogue = new List<Monologue>()
        {
            new Monologue(){Name = "Maid", Lines = new List<string>()
            {"I offer my deepest apologies"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"Wait, what? What are you talki-"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"It is the mistress order"}}
        };
        GameManager.Hr.Dialogue.TextToShow = dialogue;
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);

        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.MusicManager.Play(GameManager.Hr.MusicManager.MainroomsBeforeGun);

        Maid.WalkingController.EnableAnimation();
        Maid.IsAggressive = true;

        foreach (Spawner spawner in ThisRoom.Spawners)
            spawner.enabled = true;

        Hearts.SetActive(true);

        DoorToGun.ActionToPlayOnRoomEnter = 
            () =>
            {
                if (Maid != null)
                    Destroy(Maid.gameObject);
            };

        yield return null;

        ThisDoor.ActionToPlayOnRoomEnter = null;
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
