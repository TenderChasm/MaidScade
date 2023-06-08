using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueBox;

public class Pistol : Actor, IUsable
{
    public bool IsAccessible { get; set; } = true;
    public GameObject WaterBar;

    protected override void Start()
    {
        base.Start();
    }

    public void Use()
    {
        if (!IsAccessible)
            return;

        GameManager.Hr.Protagonist.GetArmed();

        Renderer.color = new Color(0,0,0,0);

        GameManager.Hr.Dialogue.gameObject.SetActive(true);

        Monologue mono = new Monologue() { Name = "Al" };
        mono.Lines = new List<string> { "Great,it is here!","Hm, I know that my lovely herbal tea makes maids sleepy...",
            "But if i manage to make them drink even more... then it should transfer them to the dreamland completely!",
            "This brand tea pistol loaded with perfectly brewed solution should help me with bothering maids!",
            "Now I need to find whitehaired quartermaster.She is probably rearraging boxes in the warehouse"};
        GameManager.Hr.Dialogue.TextToShow = new List<Monologue>() { mono };
        GameManager.Hr.Dialogue.StartShowing();

        WaterBar.SetActive(true);

        Destroy(gameObject);

        GameManager.Hr.MusicManager.FadeAndPlay(GameManager.Hr.MusicManager.MainroomsAfterGun);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
