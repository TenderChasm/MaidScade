using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class LifePicture : Actor, IUsable
{
    public bool IsAccessible { get; set; } = true;
    public Room RoomPictureIn;

    public Sprite Left;
    public Sprite Center;
    public Sprite Right;

    protected override void Start()
    {
        base.Start();
    }

    public void Use()
    {
        if (!IsAccessible)
            return;

        GameManager.Hr.Protagonist.Health.Health = GameManager.Hr.Protagonist.Health.MaxHealth;

        GameManager.Hr.CurrentRespawnPoint = new Vector2(transform.position.x, transform.position.y - 2);
        GameManager.Hr.CurrentRespawnRoom = RoomPictureIn;

        GameManager.Hr.Dialogue.gameObject.SetActive(true);
        Monologue mono = new Monologue() { Name = "???" };
        mono.Lines = new List<string> { "You feel her reassuring gaze" };
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionToPlayer = Vector2.zero;
        if(GameManager.Hr.Protagonist != null)
            directionToPlayer = (GameManager.Hr.Protagonist.transform.position - transform.position).normalized;

        if (directionToPlayer.x > 0.5)
            Renderer.sprite = Right;
        else if (directionToPlayer.x < -0.5)
            Renderer.sprite = Left;
        else
            Renderer.sprite = Center;
    }
}
