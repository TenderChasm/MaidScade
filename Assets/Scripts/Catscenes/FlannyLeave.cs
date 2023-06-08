using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static DialogueBox;

public class FlannyLeave : Catscene
{
    public Flanny Flanny { get; set; }
    public Marker LeaveMarker;
    public Volume Volume;

    public Image BlackScreen;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.IsInputLocked = true;
        GameManager.Hr.Protagonist.Movement = Vector2.zero;

        GameManager.Hr.MusicManager.Fade();

        yield return new WaitForSeconds(1F);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        bool dialogueSemaphore = false;
        List<Monologue> dialogue = new List<Monologue>()
        {
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"Aaaaaaaaarghhhhhhhhh!","Flandre wants to go to her bed...",}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"You destroyed half of the hall!!!"}},
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"It is nothing!!!The terrible thing happened!"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"What happened?"}},
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"You aren't still seeing?","I HAVE LOST MY CAP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"Oh...Okay,I will help you, calm down"}},
            new Monologue(){Name = "Flanny", Lines = new List<string>()
            {"Flandre will be happier much!","Maybe Flandre will even say you about her"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"That's fine i guess", "So, lets find your cap", "I have a feeling that my journey has just started"}},
        };
        GameManager.Hr.Dialogue.TextToShow = dialogue;
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.MusicManager.Play(GameManager.Hr.MusicManager.Outro);

        GameManager.Hr.IsInputLocked = true;

        WaypointWalker walker = GameManager.Hr.Protagonist.Walker;
        walker.enabled = true;
        bool walkingCompleted = false;

        walker.ResetWaypoints();
        walker.AddWaypoint(LeaveMarker.transform.position, () => walkingCompleted = true);
        walker.StartWalking();

        yield return new WaitUntil(() => walkingCompleted);

        BlindURPEffect.IsOn = true;

        BlindURPEffect blindEffect;
        Volume.profile.TryGet(out blindEffect);
        blindEffect.Width.value = 5F;
        blindEffect.IrisWideness.value = 5F;

        for (float value = 5; value > 1; value -= 0.1F)
        {
            blindEffect.IrisWideness.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        for (float value = 5F; value > 1.5; value -= 0.1F)
        {
            blindEffect.Width.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        for (float value = 1; value > 0.1; value -= 0.005F)
        {
            blindEffect.IrisWideness.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        for (float value = 1.5F; value > 0.1; value -= 0.007F)
        {
            blindEffect.Width.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        BlackScreen.color = new Color(0, 0, 0, 1);
        BlindURPEffect.IsOn = false;

        yield return new WaitForSeconds(2);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        Monologue mono = new Monologue() { Name = "???" };
        mono.Lines = new List<string> { "So, our hero is on the way to search for the Flandre's cap, Her,...",
            "Lets wish him good luck in everything he wants"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);
        yield return new WaitUntil(() => dialogueSemaphore);

        Application.Quit();



    }
}
