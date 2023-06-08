using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class WhitesocksEnter : TriggerCatscene
{
    public Whitesock Whitesocks { get; set; }
    public Door BossRoomDoor;
    public WhitesocksSpawner Spawner;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.Protagonist.Movement = Vector2.zero;

        GameManager.Hr.IsInputLocked = true;

        Whitesocks.WalkingController.DisableAnimation();
        Whitesocks.WalkingController.Animator.Play("idleUp");

        GameManager.Hr.Camera.ChangeTarget(Whitesocks.transform);
        yield return new WaitForSeconds(2);

        GameManager.Hr.MusicManager.Fade();

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        bool dialogueSemaphore = false;
        Monologue mono = new Monologue() { Name = "Whitesock" };
        mono.Lines = new List<string> { "18 packs here...27 packs there.........did they forget about cookies?",
        "Hm?"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        Whitesocks.WalkingController.Animator.Play("idleDown");

        GameManager.Hr.Camera.ChangeTarget(GameManager.Hr.Protagonist.transform);
        yield return new WaitForSeconds(1.5F);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "It is me!","I want to know where is She and why are all mansion staff hunting me????!!!!"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.Camera.ChangeTarget(Whitesocks.transform);
        yield return new WaitForSeconds(1.5F);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        List<Monologue> dialogue = new List<Monologue>()
        {
            new Monologue(){Name = "Whitesock", Lines = new List<string>()
            {"*Sigh*...Sorry,Sir,I am unable to tell you.And I was told to not let you investigate further"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"What is wrong with you all today?!","When i find She I will show her where the crayfish hibernate",
                "Oh, that idiom translation is too dumb -.-"  }},
            new Monologue(){Name = "Whitesock", Lines = new List<string>()
            {"Get ready,Sir"}}
        };
        GameManager.Hr.Dialogue.TextToShow = dialogue;
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.Whitesocks);

        GameManager.Hr.Camera.ChangeTarget(GameManager.Hr.Protagonist.transform);
        yield return new WaitForSeconds(1.5F);

        Whitesocks.isAttacking = true;
        Whitesocks.WalkingController.EnableAnimation();

        GameManager.Hr.IsInputLocked = false;

        BossRoomDoor.AlwaysAllow = true;
        BossRoomDoor.ActionToPlayOnRoomEnter = () => GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.Whitesocks);

        Spawner.AttackOnSpawn = true;

        Whitesocks.StartAttackLoop();
    }
        void Update()
    {
        
    }
}
