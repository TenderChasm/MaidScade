using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenEffects : MonoBehaviour
{

    public Material[] Materials;
    public Camera MainCamera { get; set; }

    void Start()
    {
        MainCamera = GetComponent<Camera>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        foreach (Material material in Materials)
            Graphics.Blit(source, destination, material);
    }
}
