// Simplified Transparent Vertex Colored shader mobile device. Differences from Mobile Transparent Vertex Colored shader:
// -Fog on
Shader "Kod/Environment/Alpha-VertexColor-Unlit-4" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Spec Color", Color) = (1,1,1,0)
		_Emission ("Emmisive Color", Color) = (0,0,0,0)
		_Shininess ("Shininess", Range (0.1, 1)) = 0.7
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	Category 
	{
		Tags {"Queue"="Transparent-4" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Alphatest Off
		Blend SrcAlpha OneMinusSrcAlpha 
		SubShader 
		{
			Material 
			{
				Diffuse [_Color]
				Ambient [_Color]
				Shininess [_Shininess]
				Specular [_SpecColor]
				Emission [_Emission]	
			}
			Pass 
			{
				ColorMaterial AmbientAndDiffuse
				Fog { Mode Global }
				Lighting Off
				SeparateSpecular On
				SetTexture [_MainTex] 
				{
					Combine texture * primary, texture * primary
				}
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					Combine previous * constant DOUBLE, previous * constant
				}  
			}
		} 
	}
}