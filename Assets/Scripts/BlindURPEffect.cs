using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class BlindURPEffect : VolumeComponent, IPostProcessComponent
{
    public static bool IsOn = true;
    public FloatParameter Width = new FloatParameter(0.3f, true);
    public FloatParameter IrisWideness = new FloatParameter(0.1f, true);
    public Vector2Parameter PointOfSight = new Vector2Parameter(new Vector2(0.5F, 0.5F), true);

    public bool IsActive() => IsOn;

    public bool IsTileCompatible() => true;
}
