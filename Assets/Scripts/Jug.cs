using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jug : Actor, IUsable
{
    public bool IsAccessible { get; set; } = true;

    public Sprite FullSprite;
    public Sprite EmptySprite;

    public float TimeToRefill;

    private bool isFull = true;

    protected override void Start()
    {
        base.Start();
    }

    public void Use()
    {
        if (isFull)
        {
            GameManager.Hr.Protagonist.Weapon.currentAmmo =
                GameManager.Hr.Protagonist.Weapon.MaxAmmoCount;
            Renderer.sprite = EmptySprite;
            isFull = false;

            StartCoroutine(AutoRefill());
        }
    }

    private IEnumerator AutoRefill()
    {
        yield return new WaitForSeconds(TimeToRefill);

        isFull = true;
        Renderer.sprite = FullSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
