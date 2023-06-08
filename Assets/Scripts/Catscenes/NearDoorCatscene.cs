using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class NearDoorCatscene : TriggerCatscene
{
    public Bed Bed;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.Dialogue.gameObject.SetActive(true);

        dialogueSemaphore = false;
        Monologue mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "I think I need to make my bed first"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);

        yield return new WaitUntil(() => dialogueSemaphore);

        Bed.IsAccessible = true;

        yield return null;

    }

}
