// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Kod/Environment/Particle-Additive" 
{
	Properties 
	{
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One, One One
		//ColorMask RGB
		Cull Off 
		Lighting Off 
		ZWrite Off 
		Fog { Color (0,0,0,0) }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader
		{
			Fog { Mode Global }
			Pass 
			{
				SetTexture [_MainTex] 
				{
					combine texture * primary
				}
			}
		}
	}
}