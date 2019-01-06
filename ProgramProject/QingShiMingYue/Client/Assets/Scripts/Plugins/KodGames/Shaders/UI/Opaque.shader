Shader "Kod/UI/Opaque"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}

	Category
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque" }
		Cull Off Lighting Off ZWrite Off Fog { Mode Off }
		
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] {}
			}
		}
	}
}