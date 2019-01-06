Shader "Extra/UV2"
{
	 Properties 
	 {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlendTex ("Alpha Blended (RGBA) ", 2D) = "black" {}
    }
	
    SubShader 
	{
        Pass 
		{
			BindChannels {
				Bind "Vertex", vertex
				Bind "texcoord", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord1", texcoord1 // main uses 1st uv
			}
		
            // Apply base texture
            SetTexture [_MainTex] 
			{
                combine texture
            }
            
			// Blend in the alpha texture using the lerp operator
            SetTexture [_BlendTex] 
			{
                combine texture lerp (texture) previous
            }
        }
    }
}