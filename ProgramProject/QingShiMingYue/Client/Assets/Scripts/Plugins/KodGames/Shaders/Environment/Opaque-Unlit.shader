// Simplified Vertex Colored shader mobile device. Differences from Mobile Vertex Colored shader:
// -Fog on
Shader "Kod/Environment/Opaque-Unlit" 
{
	Properties 
	{
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	}
 
	SubShader 
	{
	    Pass 
	    {
	        Lighting Off
	        Fog { Mode Global }

	        SetTexture [_MainTex] 
	        {
	            constantColor [_Color]
	            Combine texture * constant DOUBLE, texture * constant
	        } 
	    }
	}
	 
	Fallback "VertexLit", 1
}