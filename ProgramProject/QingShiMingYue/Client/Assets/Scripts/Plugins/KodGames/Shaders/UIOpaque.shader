Shader "Extra/UIOpaque"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "IgnoreProjector"="True" "RenderType"="Opaque" }
		Cull Off Lighting Off ZWrite Off Fog { Mode Off }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					Combine texture * primary
				}
			}
		}
	}
}