using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class ColorPaletteEffect : ScriptableRendererFeature
{
    public Texture2D colorPaletteTexture; // Texture containing the color palette
    public Shader colorPaletteShader; // Shader to apply the color palette

    class ColorPalettePass : ScriptableRenderPass
    {
        private Material material;

        public ColorPalettePass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null || material.mainTexture == null)
            {
                Debug.LogError("Missing material or color palette texture.");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("Color Palette");
            cmd.SetRenderTarget(renderingData.cameraData.renderer.cameraColorTargetHandle);

            cmd.ClearRenderTarget(true, true, Color.clear);

            material.SetTexture("_ColorPalette", material.mainTexture);
            //cmd.DrawMesh(Blitter.BlitCameraTexture(cmd, ), Matrix4x4.identity, material);
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public override void Create()
    {
        if (colorPaletteShader == null || colorPaletteTexture == null)
        {
            Debug.LogError("Missing shader or color palette texture.");
            return;
        }

        var material = new Material(colorPaletteShader);
        material.hideFlags = HideFlags.HideAndDontSave;

        var pass = new ColorPalettePass(material);
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        var rendererAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        if (rendererAsset != null)
        {
            var renderer = rendererAsset.scriptableRenderer;
            if (renderer != null)
            {
                renderer.EnqueuePass(pass);
            }
            else
            {
                Debug.LogError("Failed to access the scriptable renderer from the Universal Render Pipeline Asset.");
            }
        }
        else
        {
            Debug.LogError("No Universal Render Pipeline Asset found in Graphics Settings.");
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        Create();
    }
}