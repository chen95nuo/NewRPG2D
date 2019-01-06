// Simplified VertexLit shader. Differences from regular VertexLit one:
// - no per-material color
// - no specular
// - no emission

Shader "Kod/Environment/Opaque-VertexLit" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	// ---- Dual texture cards
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		
		// Non-lightmapped
		Pass 
		{
			Tags { "LightMode" = "Vertex" }
			
			Material 
			{
				Diffuse [_Color]
				Ambient [_Color]
			} 
			Lighting On
			SetTexture [_MainTex] 
			{
				Combine texture * primary DOUBLE, texture * primary
			} 
		}
		
		// Lightmapped, encoded as dLDR
		Pass 
		{
			Tags { "LightMode" = "VertexLM" }
			
			BindChannels 
			{
				Bind "Vertex", vertex
				Bind "normal", normal
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord", texcoord1 // main uses 1st uv
			}
			
			SetTexture [unity_Lightmap] 
			{
				matrix [unity_LightmapMatrix]
				constantColor [_Color]
				combine texture * constant
			}
			SetTexture [_MainTex] 
			{
				combine texture * previous DOUBLE, texture * primary
			}
		}
		
		// Lightmapped, encoded as RGBM
		Pass 
		{
			Tags { "LightMode" = "VertexLMRGBM" }
			
			BindChannels 
			{
				Bind "Vertex", vertex
				Bind "normal", normal
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord1", texcoord1 // unused
				Bind "texcoord", texcoord2 // main uses 1st uv
			}
			
			SetTexture [unity_Lightmap] 
			{
				matrix [unity_LightmapMatrix]
				combine texture * texture alpha DOUBLE
			}
			SetTexture [unity_Lightmap] 
			{
				constantColor [_Color]
				combine previous * constant
			}
			SetTexture [_MainTex] 
			{
				combine texture * previous QUAD, texture * primary
			}
		}	
	}
}
