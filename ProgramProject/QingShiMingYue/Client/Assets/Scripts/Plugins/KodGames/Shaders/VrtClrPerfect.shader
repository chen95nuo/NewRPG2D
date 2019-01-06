Shader "Extra/VertexColorPerfect" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
      Tags { "RenderType" = "Transparent" }
	  LOD 200
	  
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert alpha
	  
      struct Input {
          float2 uv_MainTex;
          float4 customColor;
      };
	  
      void vert (inout appdata_full v, out Input o) {
          o.customColor = v.color;
      }
	  
      sampler2D _MainTex;
	  
      void surf (Input IN, inout SurfaceOutput o) {
		  half4 c = tex2D (_MainTex, IN.uv_MainTex);
          o.Albedo = c.rgb;
          o.Albedo *= IN.customColor.rgb;
		  o.Alpha = IN.customColor.a * c.a;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }




