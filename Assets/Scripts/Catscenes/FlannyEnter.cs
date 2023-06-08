using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class FlannyEnter : TriggerCatscene
{
    public Flanny Flanny { get; set; }
    public FlannySpawner Spawner;
    public Door BossRoomDoor;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {

        GameManager.Hr.IsInputLocked = true;
        GameManager.Hr.Protagonist.Movement = Vector2.zero;
        GameManager.Hr.Protagonist.WalkingController.Animator.Play("IdleUp");

        Flanny.WalkingController.DisableAnimation();
        Flanny.WalkingController.Animator.Play("idleDown");

        GameManager.Hr.Camera.ChangeTarget(Flanny.transform);
        yield return new WaitForSeconds(3);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        bool dialogueSemaphore = false;
        Monologue mono = new Monologue() { Name = "Flanny" };
        mono.Lines = new List<string> { "Don't you see I AM BUSY!!!!!!" };
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.IsInputLocked = true;

        GameManager.Hr.Camera.ChangeTarget(GameManager.Hr.Protagonist.transform);
        yield return new WaitForSeconds(3);


        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "I don't see anything, I need to find your mistress,Do you know whe-" };
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.IsInputLocked = true;

        GameManager.Hr.Camera.ChangeTarget(Flanny.transform);
        yield return new WaitForSeconds(3);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        List<Monologue> dialogue = new List<Monologue>()
        {
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"i DONT KNOW and even if i maybe KNOW I WONT say","Don't you see what HAPPENED??!!!!!!"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"Probably accomodating the strongest yokai in my mansion wasn't the best idea"}},
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"LEAVE LEAVE LEAVE LEAVE!!!!!!!!"}}
        };
        GameManager.Hr.Dialogue.TextToShow = dialogue;
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.Flanny);

        GameManager.Hr.IsInputLocked = true;

        GameManager.Hr.Camera.ChangeTarget(GameManager.Hr.Protagonist.transform);
        yield return new WaitForSeconds(1.5F);

        Flanny.isAttacking = true;
        Flanny.WalkingController.EnableAnimation();

        GameManager.Hr.Protagonist.WalkingController.EnableAnimation();

        GameManager.Hr.IsInputLocked = false;

        BossRoomDoor.AlwaysAllow = true;
        BossRoomDoor.ActionToPlayOnRoomEnter = () => GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.Flanny);

        Spawner.AttackOnSpawn = true;

        Flanny.StartAttackLoop();
    }
}
