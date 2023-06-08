using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Actor, IUsable
{
    public Sprite[] BedStates;
    public bool IsAccessible { get; set; } = true;
    public AudioClip MakeSound;
    public Catscene CutieEnterScene;

    private AudioSource Audio { get; set; }

    public void Use()
    {
        if (!IsAccessible)
            return;

        Audio.PlayOneShot(MakeSound);

        int indexOfCurrentSprite = Array.IndexOf(BedStates, Renderer.sprite);
        if (indexOfCurrentSprite == BedStates.Length - 2)
        {
            IsAccessible = false;
            PlayCatscene();
        }
        Renderer.sprite = BedStates[indexOfCurrentSprite + 1];
    }

    private void PlayCatscene()
    {
        StartCoroutine(CutieEnterScene.PlayScene());
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
