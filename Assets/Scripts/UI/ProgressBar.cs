using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image Filling;
    public int MaxValue;
    public Action ExternalValueChecker;

    public int CurrentValue
    {
        get => currentValue;
        set
        {
            int newValue = Mathf.Clamp(value, 0, MaxValue);
            currentValue = newValue;
            Filling.fillAmount = currentValue / (float)MaxValue;
        }
    }

    private int currentValue;

    void Start()
    {
        currentValue = MaxValue;
    }

    private void Update()
    {
        ExternalValueChecker?.Invoke();
    }
}
