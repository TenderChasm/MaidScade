using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthComponent : MonoBehaviour
{
    public event Action OnHealthEnd;
    public event Action<int> OnHealthDown;
    public event Action<int> OnHealthUp;
    public bool IsInvisible { get; set; }

    public int MaxHealth => maxHealth;

    [SerializeField] int maxHealth;
    [SerializeField] private int health;

    private void Awake()
    {
        health = maxHealth;
    }

    public int Health
    {
        get => health;
        set
        {
            int oldHealth = health;

            if (!IsInvisible)
            {
                if (value < oldHealth)
                {
                    health = value;
                    OnHealthDown?.Invoke(value);
                }

                if (value <= 0)
                    OnHealthEnd?.Invoke();
            }

            if (value > oldHealth)
            {
                health = value;
                OnHealthUp?.Invoke(value);
            }

            if (value >= maxHealth)
                health = maxHealth;
        }
    }

    public IEnumerator SetInvisibleForTime(float time)
    {
        IsInvisible = true;
        yield return new WaitForSeconds(time);
        IsInvisible = false;
    }

}