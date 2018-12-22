// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32567,y:32582|emission-499-OUT;n:type:ShaderForge.SFN_Append,id:7,x:34929,y:32420|A-8-OUT,B-10-OUT;n:type:ShaderForge.SFN_Slider,id:8,x:35233,y:32497,ptlb:map01_u_move,ptin:_map01_u_move,min:-5,cur:-1.391042,max:5;n:type:ShaderForge.SFN_Slider,id:10,x:35237,y:32600,ptlb:map01_v_move,ptin:_map01_v_move,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Add,id:11,x:34314,y:32305|A-12-UVOUT,B-14-OUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:34860,y:32202,uv:0;n:type:ShaderForge.SFN_Time,id:13,x:35042,y:32613;n:type:ShaderForge.SFN_Multiply,id:14,x:34653,y:32487|A-7-OUT,B-13-T;n:type:ShaderForge.SFN_Multiply,id:15,x:34155,y:32419|A-11-OUT,B-23-OUT;n:type:ShaderForge.SFN_Append,id:23,x:34265,y:32654|A-24-OUT,B-26-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:34489,y:32642,ptlb:map01_u_tiling,ptin:_map01_u_tiling,min:0,cur:0.28,max:5;n:type:ShaderForge.SFN_Slider,id:26,x:34495,y:32748,ptlb:map01_v_tiling,ptin:_map01_v_tiling,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Tex2d,id:106,x:33659,y:32449,ptlb:material01,ptin:_material01,tex:82e64d9582ca2a14b858feb7fa020a66,ntxv:0,isnm:False|UVIN-15-OUT;n:type:ShaderForge.SFN_Tex2d,id:149,x:33241,y:32975,ptlb:mask(RGBA),ptin:_maskRGBA,tex:afe1c837b7e418644bafdcb20cfc2cc2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:499,x:33145,y:32469|A-106-RGB,B-149-A;proporder:106-24-26-8-10-149;pass:END;sub:END;*/

Shader "Shader Forge/anniu01_ui" {
    Properties {
        _material01 ("material01", 2D) = "white" {}
        _map01_u_tiling ("map01_u_tiling", Range(0, 5)) = 0.28
        _map01_v_tiling ("map01_v_tiling", Range(0, 5)) = 1
        _map01_u_move ("map01_u_move", Range(-5, 5)) = -1.391042
        _map01_v_move ("map01_v_move", Range(-5, 5)) = 0
        _maskRGBA ("mask(RGBA)", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+2000"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
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
            uniform float _map01_u_move;
            uniform float _map01_v_move;
            uniform float _map01_u_tiling;
            uniform float _map01_v_tiling;
            uniform sampler2D _material01; uniform float4 _material01_ST;
            uniform sampler2D _maskRGBA; uniform float4 _maskRGBA_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_13 = _Time + _TimeEditor;
                float2 node_15 = ((i.uv0.rg+(float2(_map01_u_move,_map01_v_move)*node_13.g))*float2(_map01_u_tiling,_map01_v_tiling));
                float2 node_504 = i.uv0;
                float3 emissive = (tex2D(_material01,TRANSFORM_TEX(node_15, _material01)).rgb*tex2D(_maskRGBA,TRANSFORM_TEX(node_504.rg, _maskRGBA)).a);
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
