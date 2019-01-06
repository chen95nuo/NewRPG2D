Shader "Kod/UI/Transparent Color"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader 
		{
			Fog { Mode Off }
			Pass 
			{
				SetTexture [_MainTex] 
				{
					combine texture * previous
				}
				
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					combine previous * constant
				}
			}
		}
	}
}