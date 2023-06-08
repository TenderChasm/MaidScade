using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    public SpriteRenderer Renderer { get; set; }

    public bool IsAccessible { get; set; }

    void Use();

    void OutlineOn()
    {
        Renderer.material = GameManager.Hr.Outline;
    }

    void OutlineOff()
    {
        Renderer.material = GameManager.Hr.Standart;
    }
}
