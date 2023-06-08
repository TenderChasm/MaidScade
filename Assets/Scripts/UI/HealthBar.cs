using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public HealthContainer[] Hearts;

    public void SetHP(int number)
    {
        number = Mathf.Clamp(number, 0, Hearts.Length * 2);
        foreach(HealthContainer heart in Hearts)
        {
            int healthToDistribute = Mathf.Min(2, number);
            heart.CurrentHealth = healthToDistribute;
            number -= healthToDistribute;
        }
    }

    void Update()
    {

    }
}
