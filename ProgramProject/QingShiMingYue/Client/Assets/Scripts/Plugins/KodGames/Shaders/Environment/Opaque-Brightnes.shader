Shader "Kod/Environment/Opaque-Brightnes" 
{
	Properties
	{
		_MainTex ("Overlap Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_OverlapTex ("Overlap Texture", 2D) = "white" {}
	}

	Category
	{
		Lighting Off
		Fog { Mode Global }

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
				SetTexture [_OverlapTex]
				{
	            	constantColor [_Color]
		            Combine texture * constant DOUBLE, texture * constant
				}
				
				SetTexture [_MainTex]
				{
					combine texture + previous, texture
				}
			}
	    }
	}
}
