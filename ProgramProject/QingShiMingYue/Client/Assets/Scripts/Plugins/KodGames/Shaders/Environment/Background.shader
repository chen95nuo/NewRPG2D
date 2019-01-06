// Simplified Skybox shader. Differences from regular Skybox one:
// - no tint color

Shader "Kod/Environment/Background" 
{
	Properties 
	{
		_MainTex ("Background", 2D) = "white" {}
	}
	
	SubShader 
	{
		Tags { "Queue"="Background" "RenderType"="Background" }
		Cull Off ZWrite Off Fog { Mode Off }
		Pass 
		{
			SetTexture [_MainTex] { combine texture }
		}
	}
}
