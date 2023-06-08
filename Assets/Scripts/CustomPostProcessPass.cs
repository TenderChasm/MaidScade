using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class CustomPostProcessPass : ScriptableRenderPass
{
    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public CustomPostProcessPass()
    {
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;

        var renderer = renderingData.cameraData.renderer;
        source = renderer.cameraColorTarget;

        cmd.GetTemporaryRT(temporaryRTIdA, descriptor, FilterMode.Bilinear);
        destinationA = new RenderTargetIdentifier(temporaryRTIdA);
        cmd.GetTemporaryRT(temporaryRTIdB, descriptor, FilterMode.Bilinear);
        destinationB = new RenderTargetIdentifier(temporaryRTIdB);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isSceneViewCamera)
            return;

        CommandBuffer cmd = CommandBufferPool.Get("Custom Post Processing");
        cmd.Clear();

        VolumeStack stack = VolumeManager.instance.stack;

        #region Local Methods

        void BlitTo(Material mat, int pass = 0)
        {
            RenderTargetIdentifier first = latestDest;
            RenderTargetIdentifier last = first == destinationA ? destinationB : destinationA;
            Blit(cmd, first, last, mat, pass);

            latestDest = last;
        }

        #endregion

        latestDest = source;

        //---Custom effect here---
        var customEffect = stack.GetComponent<BlindURPEffect>();
        if (customEffect.IsActive() && Application.isPlaying)
        {
            Material material = GameManager.Hr.Blind;
            material.SetFloat(Shader.PropertyToID("_Width"), customEffect.Width.value);
            material.SetFloat(Shader.PropertyToID("_IrisWideness"), customEffect.IrisWideness.value);
            material.SetVector(Shader.PropertyToID("_PointOfSight"), customEffect.PointOfSight.value);

            BlitTo(material);
        }

        Blit(cmd, latestDest, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(temporaryRTIdA);
        cmd.ReleaseTemporaryRT(temporaryRTIdB);
    }
}
