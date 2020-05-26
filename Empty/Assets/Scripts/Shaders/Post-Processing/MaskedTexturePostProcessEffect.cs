//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        10.02.20
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LeakyfingersUtility
{
    // The class used to handle the settings for the post-processing effect.
    [Serializable]
    [PostProcess(typeof(MaskedTexturePostProcessEffectRenderer), PostProcessEvent.AfterStack, "Masked Texture")] // Tells Unity that this class holds the data for a post-processing effect.
    public sealed class MaskedTexturePostProcessEffect : PostProcessEffectSettings
    {
        public ColorParameter MaskInputColor = new ColorParameter();
        public TextureParameter MaskTexture = new TextureParameter();
    }

    // The class used to handle the C# side of the post-process effect rendering.
    public sealed class MaskedTexturePostProcessEffectRenderer : PostProcessEffectRenderer<MaskedTexturePostProcessEffect>
    {
        // Renders the image output by the camera using the post-process shader and outputs it to the appropriate destination.
        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(Shader.Find("LEAKYFINGERS/Masked Texture Post-Process"));  // The property sheet used to handle the post-process shader code.

            sheet.properties.SetColor("_MaskColor", settings.MaskInputColor);
            if (settings.MaskTexture.value)
                sheet.properties.SetTexture("_MaskReplaceTex", settings.MaskTexture);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);  // Applies the shader to the source image and outputs the result to the appropriate destination.
        }
    }
}
