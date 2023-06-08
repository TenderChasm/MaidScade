using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static DialogueBox;

public class InitialCatscene : Catscene
{
    public Volume Volume;
    public Player Protagonist;
    public Bed Bed;

    public GameObject FuelType;
    public GameObject Syringe;


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.MusicManager.Play(GameManager.Hr.MusicManager.Intro);

        BlindURPEffect blindEffect;
        Volume.profile.TryGet(out blindEffect);

        for(float value = blindEffect.Width.value; value < 0.9; value += 0.02F )
        {
            blindEffect.Width.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        for (float value = blindEffect.IrisWideness.value; value < 1; value += 0.02F)
        {
            blindEffect.IrisWideness.value = value;
            yield return new WaitForSeconds(0.05F);
        }

        BlindURPEffect.IsOn = false;

        FuelType.active = true;
        Syringe.active = true;

        yield return new WaitForSeconds(2);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);

        dialogueSemaphore = false;
        Monologue mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "Ouch", "It seems I drank too much tea yesterday",
            "It is so hard to get up................Okay, I'll lie down a little more", "Wait!",
            "Where is She?"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);

        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.IsInputLocked = true;

        Bed.Use();

        Player mainPlayer = Instantiate(Protagonist);
        mainPlayer.transform.position = new Vector2(-1.5F, -0.5F);
        GameManager.Hr.Protagonist = mainPlayer;

        yield return new WaitForSeconds(1);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "I need to get where is she",
        "If she is making breakfast without asking first then it is even more suspicious!"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing();

        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.Camera.Target = mainPlayer.transform;

        Bed.IsAccessible = false;

    }
}
