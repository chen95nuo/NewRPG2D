// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32452,y:32643|emission-547-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33440,y:32513,ptlb:map01,ptin:_map01,tex:5c353c262714d9641abbda8550752022,ntxv:0,isnm:False|UVIN-441-OUT;n:type:ShaderForge.SFN_Append,id:3,x:34677,y:32629|A-9-OUT,B-438-OUT;n:type:ShaderForge.SFN_Time,id:8,x:34908,y:32816;n:type:ShaderForge.SFN_Slider,id:9,x:35280,y:32546,ptlb:Yspeed_M1,ptin:_Yspeed_M1,min:-1,cur:-1,max:1;n:type:ShaderForge.SFN_Slider,id:438,x:35276,y:32781,ptlb:Xspeed_M1,ptin:_Xspeed_M1,min:-1,cur:0.1144662,max:1;n:type:ShaderForge.SFN_TexCoord,id:439,x:34449,y:32402,uv:0;n:type:ShaderForge.SFN_Add,id:440,x:34135,y:32363|A-439-UVOUT,B-443-OUT;n:type:ShaderForge.SFN_Multiply,id:441,x:33683,y:32526|A-440-OUT,B-446-OUT;n:type:ShaderForge.SFN_Multiply,id:443,x:34420,y:32709|A-3-OUT,B-8-T;n:type:ShaderForge.SFN_Slider,id:445,x:34607,y:33382,ptlb:Xtile_M1,ptin:_Xtile_M1,min:0,cur:0.5763013,max:1;n:type:ShaderForge.SFN_Append,id:446,x:33939,y:32764|A-450-OUT,B-445-OUT;n:type:ShaderForge.SFN_Slider,id:450,x:34647,y:33187,ptlb:Ytile_M1,ptin:_Ytile_M1,min:0,cur:0.01711646,max:1;n:type:ShaderForge.SFN_Tex2d,id:451,x:33297,y:33280,ptlb:map_mask,ptin:_map_mask,tex:1f0c1d1fe0fa54941b419c5bdd4b838c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:464,x:35205,y:32210,ptlb:Xtile_M2,ptin:_Xtile_M2,min:0,cur:0.7282118,max:1;n:type:ShaderForge.SFN_Slider,id:466,x:35253,y:32074,ptlb:Ytile_M2,ptin:_Ytile_M2,min:0,cur:0.01711646,max:1;n:type:ShaderForge.SFN_Tex2d,id:468,x:33253,y:32051,ptlb:map02,ptin:_map02,tex:5c353c262714d9641abbda8550752022,ntxv:0,isnm:False|UVIN-482-OUT;n:type:ShaderForge.SFN_Append,id:470,x:34647,y:31565|A-474-OUT,B-476-OUT;n:type:ShaderForge.SFN_Time,id:472,x:34903,y:31758;n:type:ShaderForge.SFN_Slider,id:474,x:35175,y:31372,ptlb:Yspeed_M2,ptin:_Yspeed_M2,min:-1,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:476,x:35246,y:31717,ptlb:Xspeed_M2,ptin:_Xspeed_M2,min:-1,cur:-0.07255197,max:1;n:type:ShaderForge.SFN_TexCoord,id:478,x:34350,y:31245,uv:0;n:type:ShaderForge.SFN_Add,id:480,x:34105,y:31299|A-478-UVOUT,B-484-OUT;n:type:ShaderForge.SFN_Multiply,id:482,x:33811,y:31541|A-480-OUT,B-486-OUT;n:type:ShaderForge.SFN_Multiply,id:484,x:34390,y:31645|A-470-OUT,B-472-T;n:type:ShaderForge.SFN_Append,id:486,x:34393,y:31830|A-466-OUT,B-464-OUT;n:type:ShaderForge.SFN_Multiply,id:487,x:32907,y:32284|A-468-RGB,B-2-RGB,C-451-RGB,D-509-OUT,E-530-RGB;n:type:ShaderForge.SFN_Slider,id:509,x:33123,y:32649,ptlb:brightnes,ptin:_brightnes,min:0,cur:4.237694,max:10;n:type:ShaderForge.SFN_VertexColor,id:530,x:33031,y:32808;n:type:ShaderForge.SFN_Multiply,id:547,x:32702,y:32462|A-487-OUT,B-530-A;proporder:451-2-468-509-438-9-445-450-476-474-466-464;pass:END;sub:END;*/

Shader "Shader Forge/tiaoguang" {
    Properties {
        _map_mask ("map_mask", 2D) = "white" {}
        _map01 ("map01", 2D) = "white" {}
        _map02 ("map02", 2D) = "white" {}
        _brightnes ("brightnes", Range(0, 10)) = 4.237694
        _Xspeed_M1 ("Xspeed_M1", Range(-1, 1)) = 0.1144662
        _Yspeed_M1 ("Yspeed_M1", Range(-1, 1)) = -1
        _Xtile_M1 ("Xtile_M1", Range(0, 1)) = 0.5763013
        _Ytile_M1 ("Ytile_M1", Range(0, 1)) = 0.01711646
        _Xspeed_M2 ("Xspeed_M2", Range(-1, 1)) = -0.07255197
        _Yspeed_M2 ("Yspeed_M2", Range(-1, 1)) = 1
        _Ytile_M2 ("Ytile_M2", Range(0, 1)) = 0.01711646
        _Xtile_M2 ("Xtile_M2", Range(0, 1)) = 0.7282118
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _map01; uniform float4 _map01_ST;
            uniform float _Yspeed_M1;
            uniform float _Xspeed_M1;
            uniform float _Xtile_M1;
            uniform float _Ytile_M1;
            uniform sampler2D _map_mask; uniform float4 _map_mask_ST;
            uniform float _Xtile_M2;
            uniform float _Ytile_M2;
            uniform sampler2D _map02; uniform float4 _map02_ST;
            uniform float _Yspeed_M2;
            uniform float _Xspeed_M2;
            uniform float _brightnes;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_472 = _Time + _TimeEditor;
                float2 node_482 = ((i.uv0.rg+(float2(_Yspeed_M2,_Xspeed_M2)*node_472.g))*float2(_Ytile_M2,_Xtile_M2));
                float4 node_8 = _Time + _TimeEditor;
                float2 node_441 = ((i.uv0.rg+(float2(_Yspeed_M1,_Xspeed_M1)*node_8.g))*float2(_Ytile_M1,_Xtile_M1));
                float2 node_571 = i.uv0;
                float4 node_530 = i.vertexColor;
                float3 emissive = ((tex2D(_map02,TRANSFORM_TEX(node_482, _map02)).rgb*tex2D(_map01,TRANSFORM_TEX(node_441, _map01)).rgb*tex2D(_map_mask,TRANSFORM_TEX(node_571.rg, _map_mask)).rgb*_brightnes*node_530.rgb)*node_530.a);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
