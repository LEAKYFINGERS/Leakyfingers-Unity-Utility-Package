//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        10.02.20
// Date last edited:    22.05.20
//////////////////////////////////////////////////
Shader "LEAKYFINGERS/Masked Texture Post-Process"
{
	HLSLINCLUDE
			
		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl" // Holds pre-configured vertex shaders, varying structs, and most of the data required to create common effects.

		sampler2D _MainTex;	
		
		float4 _MaskColor; // The color to be replaced by the mask texture.
		sampler2D _MaskReplaceTex; // The texture used to replace all pixels containing the mask color.

		float RoundToFirstDecimalPlace(float value)
		{
			return round(value * 10) / 10;
		}

		// The fragment shader which used to apply the post-processing effect to each pixel of the supplied rendered image. 
		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float4 color = tex2D(_MainTex, i.texcoord);
			float4 result = color;
			
			if (RoundToFirstDecimalPlace(result.r) == RoundToFirstDecimalPlace(_MaskColor.r) &&
				RoundToFirstDecimalPlace(result.g) == RoundToFirstDecimalPlace(_MaskColor.g) &&
				RoundToFirstDecimalPlace(result.b) == RoundToFirstDecimalPlace(_MaskColor.b))
			{
				result.rgb = tex2D(_MaskReplaceTex, i.texcoord).rgb;
			}

			return result;
		}

	ENDHLSL

	SubShader
	{		
		Cull Off // Disables face culling.		
		ZWrite Off // Turns off writing to the depth buffer.		
		ZTest Always // Depth testing - always draw.

		Pass
		{
			HLSLPROGRAM
				 
				#pragma vertex VertDefault // Specifies that the default vertex function will be used as it isn't required for post-processing.
				#pragma fragment Frag

			ENDHLSL
		}
	}
}
