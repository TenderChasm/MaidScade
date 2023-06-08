using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilStock 
{
    public static int RandomExcept(int min, int maxExclusive, int except)
    {
        int random = Random.Range(min, maxExclusive);
        if (random == except) random = (random + 1) % maxExclusive;
        return random;
    }
}
