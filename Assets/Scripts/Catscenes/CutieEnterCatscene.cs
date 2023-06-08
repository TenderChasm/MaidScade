using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class CutieEnterCatscene : Catscene
{

    public WhirlgigMaid Cutie;
    public Door Door;
    public CutieBecomesAgressiveCatscene NextCatscene;
    public Room NextRoom;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PlayScene()
    {
        GameManager.Hr.IsInputLocked = true;

        yield return new WaitForSeconds(0.5F);

        Door.Toggle();

        yield return new WaitForSeconds(0.3F);

        GameManager.Hr.Protagonist.WalkingController.DisableAnimation();
        GameManager.Hr.Protagonist.WalkingController.Animator.Play("idleDown");

        WhirlgigMaid maid = Instantiate(Cutie, new Vector2(-3.4375F, -3.6875F), Quaternion.identity);
        yield return new WaitForEndOfFrame();
        maid.IsAggressive = false;
        maid.WalkingController.DisableAnimation();

        maid.WalkingController.Animator.Play("idleRight");

        yield return new WaitForSeconds(1.2F);

        Door.Toggle();

        yield return new WaitForSeconds(0.2F);

        GameManager.Hr.Camera.ChangeTarget(maid.transform);

        yield return new WaitForSeconds(0.5F);

        maid.WalkingController.Animator.Play("idleUp");

        yield return new WaitForSeconds(0.5F);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        dialogueSemaphore = false;
        List<Monologue> dialogue = new List<Monologue>()
        {
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"What has happened?","Why did you suddenly come in without knocking?!!!"}},
            new Monologue(){Name = "Maid", Lines = new List<string>()
            {"I am very sorry","There is a subject of the utmost importance whether requires your urgent attention"}},
            new Monologue(){Name = "Al", Lines = new List<string>()
            {"I bet it is She who taught you to talk like that........ -.-", "I am coming now"}}
        };
        GameManager.Hr.Dialogue.TextToShow = dialogue;
        GameManager.Hr.Dialogue.StartShowing(() => dialogueSemaphore = true);

        yield return new WaitUntil(() => dialogueSemaphore);

        GameManager.Hr.IsInputLocked = true;

        GameManager.Hr.MusicManager.Fade();

        maid.WalkingController.Animator.Play("idleLeft");

        yield return new WaitForSeconds(0.5F);

        Door.Toggle();

        maid.transform.position = new Vector2(Door.transform.position.x - 6, Door.transform.position.y);
        maid.WalkingController.Animator.Play("idleRight");

        GameManager.Hr.Camera.ChangeTarget(null);

        yield return new WaitForSeconds(1F);

        Door.Toggle();

        yield return new WaitForSeconds(0.2F);

        Door.LockedExternal = false;
        Door.ActionToPlayOnRoomEnter = () => StartCoroutine(NextCatscene.PlayScene());

        NextCatscene.Maid = maid;

        foreach (Spawner spawner in NextRoom.Spawners)
            spawner.enabled = false;

        GameManager.Hr.Protagonist.WalkingController.EnableAnimation();
        GameManager.Hr.IsInputLocked = false;
        GameManager.Hr.Camera.ChangeTarget(GameManager.Hr.Protagonist.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
