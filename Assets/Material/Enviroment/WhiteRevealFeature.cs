using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WhiteRevealFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class WhiteRevealSettings
    {
        public Shader shader;
        public Material material;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;


    }

    public WhiteRevealSettings settings = new WhiteRevealSettings();
    WhiteRevealPass pass;


    public override void Create()
    {
        if (settings.material == null && settings.shader != null)
            settings.material = new Material(settings.shader);

        pass = new WhiteRevealPass(settings.material)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }
    public void SetRevealCenters(Vector2[] centers)
    {
        pass?.SetCenters(centers);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material != null)
            renderer.EnqueuePass(pass);
    }

    class WhiteRevealPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle cameraColorTarget;

        public WhiteRevealPass(Material material)
        {
            this.material = material;
            material.SetFloat("_Radius", 1.0f);
            material.SetFloat("_Visibility", 1.0f);
            material.SetVector("_Center0", new Vector2(0.5f, 0.5f));
            material.SetTexture("_MaskTex", Texture2D.whiteTexture);

        }
        public void SetCenters(Vector2[] centers)
        {
            for (int i = 0; i < centers.Length && i < 4; i++)
            {
                material.SetVector($"_Center{i}", centers[i]);
            }
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("WhiteRevealPass");

            RenderTargetIdentifier source = cameraColorTarget;
            int tempRTID = Shader.PropertyToID("_TempWhiteRevealRT");
            cmd.GetTemporaryRT(tempRTID, renderingData.cameraData.cameraTargetDescriptor);

            cmd.Blit(source, tempRTID);
            cmd.Blit(tempRTID, source, material);

            cmd.ReleaseTemporaryRT(tempRTID);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }


        public override void OnCameraCleanup(CommandBuffer cmd) { }
    }
}
