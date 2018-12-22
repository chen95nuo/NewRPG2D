Shader "Particles/Alpha_Special"
 {
 
    Properties 
 
    {
        _A ("A", Range (0, 10)) = 1
		_B ("B", Range (0, 10)) = 1
        _MainTex ("Base (RGB)", 2D) = "white" {}
	
    }
     
 
     
 
    SubShader 
     
    {

	Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
     			ZWrite Off
     			Cull Off
				Blend SrcAlpha OneMinusSrcAlpha
        Pass 
     
        {
 
     
            CGPROGRAM
   

            #pragma vertex Vert
 
            #pragma fragment Frag
     
            #include "UnityCG.cginc"
     
 
            sampler2D _MainTex;
			float _A;
			float _B;
            struct V2F
         
            {
         
                float4 pos:POSITION;
				float4 col:COLOR;
                float2 txr1:TEXCOORD0;
         
            };
               
         
            V2F Vert(appdata_full v)
     
            {
                 
                V2F output;
     
                output.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				output.col = v.color;
                output.txr1 = v.texcoord;
             
                return output;
             
            }
             
         
            half4 Frag(V2F i):COLOR
         
            {
                float4 col = tex2D(_MainTex,i.txr1);
				float3 rgb = col.rgb * i.col.rgb * _A;
				float a = col.a * i.col.a * _B;
                return float4(rgb, a);
     
            }
 
         
            ENDCG
       
       
     
        }
 
         
    }
     
     
FallBack "Diffuse"
 
}