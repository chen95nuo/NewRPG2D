//带透明通道的双面材质球//
Shader "Custom/DoubleSide02" {
//	Properties {
//		_MainTex ("Base (RGB)", 2D) = "white" {}
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		#pragma surface surf Lambert
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = c.rgb;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	} 
	Properties {
        _Color ("Main Color", Color) = (1,1,1,1) 
        _MainTex ("Base (RGB) Trans (A)", 2D) = "" {}
}

	SubShader {
	        Pass {
	                Cull Off 
	                Blend SrcAlpha OneMinusSrcAlpha 
	                Alphatest Greater 0
	                SetTexture [_MainTex] { 
	                constantColor [_Color]
	                combine texture * constant } 
	        }
	}
//	FallBack "Specular"
	FallBack "Diffuse"
}
