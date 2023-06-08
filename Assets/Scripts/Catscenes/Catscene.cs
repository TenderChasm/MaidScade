using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catscene : MonoBehaviour
{
    protected bool dialogueSemaphore;

    protected virtual void Start()
    {
        
    }

    public virtual IEnumerator PlayScene()
    {
        yield return 1;
    }

    void Update()
    {
        
    }
}
