//不带透明通道的双面材质球//
Shader "Custom/DoubleSide" {
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
//	FallBack "Diffuse"


   Properties {
      _Color ("Main Color", Color) = (1,1,1,1)
      _MainTex ("Base (RGB)", 2D) = "white" {}
      //_BumpMap ("Bump (RGB) Illumin (A)", 2D) = "bump" {}
   }
   SubShader {      
      //UsePass "Self-Illumin/VertexLit/BASE"
      //UsePass "Bumped Diffuse/PPL"
      
      // Ambient pass
      Pass {
      Name "BASE"
      Tags {"LightMode" = "Always" /* Upgrade NOTE: changed from PixelOrNone to Always */}
      Color [_PPLAmbient]
      SetTexture [_BumpMap] {
         constantColor (.5,.5,.5)
         combine constant lerp (texture) previous
         }
      SetTexture [_MainTex] {
         constantColor [_Color]
         Combine texture * previous DOUBLE, texture*constant
         }
      }
   
   // Vertex lights
   Pass {
      Name "BASE"
      Tags {"LightMode" = "Vertex"}
      Material {
         Diffuse [_Color]
         Emission [_PPLAmbient]
         Shininess [_Shininess]
         Specular [_SpecColor]
         }
      SeparateSpecular On
      Lighting On
      Cull Off
      SetTexture [_BumpMap] {
         constantColor (.5,.5,.5)
         combine constant lerp (texture) previous
         }
      SetTexture [_MainTex] {
         Combine texture * previous DOUBLE, texture*primary
         }
      }
   }
   FallBack "Diffuse", 1
}
