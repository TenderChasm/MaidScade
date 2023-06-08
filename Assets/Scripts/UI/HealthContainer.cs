using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{

    public Sprite Hearts2;
    public Sprite Hearts1;
    public Sprite Hearts0;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value <= 0)
                CurrentHeart.sprite = Hearts0;
            else if (value == 1)
                CurrentHeart.sprite = Hearts1;
            else if (value >= 2)
                CurrentHeart.sprite = Hearts2;

            currentHealth = Mathf.Clamp(value, 0, 2);
        }
    }

    private int currentHealth = 2;
    private Image CurrentHeart;

    void Start()
    {
        CurrentHeart = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
