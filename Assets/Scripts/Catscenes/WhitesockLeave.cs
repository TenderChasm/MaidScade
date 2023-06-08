using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class WhitesockLeave : Catscene
{
    public Whitesock Whitesocks { get; set; }
    public Door ExitDoor;
    public Door ChambersDoor; 

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.IsInputLocked = true;
        GameManager.Hr.Protagonist.Movement = Vector2.zero;

        yield return new WaitForSeconds(0.5F);

        GameManager.Hr.MusicManager.Fade();

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        bool dialogueSemaphore = false;

        Monologue monoWhitesocks = new Monologue() { Name = "Whitesock" };
        monoWhitesocks.Lines = new List<string> { "I am...too tired...to continue...Oh","Here is the key to scarled devil' quarters",
        "You can access it through the room with orange carpet, as you remember",
        "I locked her because she became... recently", "Ask her about your girl,Sir"};

        Monologue monoHero = new Monologue() { Name = "Al" };
        monoHero.Lines = new List<string> { "Doesn't sound reassuring"};

        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { monoWhitesocks, monoHero };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        Whitesocks.GetComponent<Collider2D>().isTrigger = true;

        ExitDoor.ActionToPlayOnRoomEnter =
            () =>
            {
                ExitDoor.LockedExternal = true;
                ExitDoor.AlwaysAllow = false;
                GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.MainroomsAfterGun);
            };

        ChambersDoor.LockedExternal = false;
        GameManager.Hr.ChambersOpened = true;
        ChambersDoor.ActionToPlayOnRoomEnter = 
            () => GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.MainroomsChambers);

        ExitDoor.Use();
    }


}
