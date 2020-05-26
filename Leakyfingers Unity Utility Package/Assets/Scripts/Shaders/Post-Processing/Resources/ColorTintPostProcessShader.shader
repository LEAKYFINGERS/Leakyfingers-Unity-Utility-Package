//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        09.09.19
// Date last edited:    22.05.20
//////////////////////////////////////////////////
Shader "LEAKYFINGERS/Color Tint Post-Process"
{
	HLSLINCLUDE	
		
		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl" // Holds pre-configured vertex shaders, varying structs, and most of the data required to create common effects.
				
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex); // The sampler used to sample the texture.		
		float4 _Color; // The color of the tint to be applied to the image.		
		float _Blend; // The blend ratio of the color tint effect applied to the image - 0.0f for none, 1.0f for the color to be completely solid.

		// The fragment shader used to apply the post-processing effect to each pixel of the supplied rendered image. 
		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);			
			color.rgb = lerp(color.rgb, _Color.rgb, _Blend.xxx);
			return color;
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
