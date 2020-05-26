//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        09.09.19
// Date last edited:    25.05.20
//////////////////////////////////////////////////
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace LeakyfingersUtility
{
    // The class used to handle the settings for the post-processing effect.
    [Serializable]
    [PostProcess(typeof(ColorTintPostProcessEffectRenderer), PostProcessEvent.AfterStack, "Color Tint")] // Tells Unity that this class holds the data for a post-processing effect.
    public sealed class ColorTintPostProcessEffect : PostProcessEffectSettings
    {
        [Tooltip("Color used to tint the image")]
        public ColorParameter Color = new ColorParameter();

        [Range(0.0f, 1.0f), Tooltip("Color tint effect intensity")]
        public FloatParameter Blend = new FloatParameter { value = 0.5f };
    }

    // The class used to handle the C# side of the post-process effect rendering.
    public sealed class ColorTintPostProcessEffectRenderer : PostProcessEffectRenderer<ColorTintPostProcessEffect>
    {
        // Renders the image output by the camera using the post-process shader and outputs it to the appropriate destination.
        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(Shader.Find("LEAKYFINGERS/Color Tint Post-Process")); // The property sheet used to handle the post-process shader code.

            sheet.properties.SetColor("_Color", settings.Color);
            sheet.properties.SetFloat("_Blend", settings.Blend);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0); // Applies the shader to the source image and outputs the result to the appropriate destination.
        }
    }
}
