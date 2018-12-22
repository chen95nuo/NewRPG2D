// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32638,y:32732|emission-144-OUT,alpha-12-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:34185,y:32505,ptlb:material01,ptin:_material01,tex:c8cd796884814204297a4b03c54b6f1c,ntxv:0,isnm:False|UVIN-97-OUT;n:type:ShaderForge.SFN_Tex2d,id:11,x:33736,y:33281,ptlb:mask_alpha,ptin:_mask_alpha,tex:14171b018f70c544abdc40fbf8aec71d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:12,x:33273,y:32762|A-162-OUT,B-11-A,C-109-OUT;n:type:ShaderForge.SFN_Time,id:17,x:35351,y:32815;n:type:ShaderForge.SFN_Slider,id:18,x:35514,y:32512,ptlb:map01_u_move,ptin:_map01_u_move,min:-2,cur:0,max:2;n:type:ShaderForge.SFN_Slider,id:22,x:35539,y:32625,ptlb:map01_v_move,ptin:_map01_v_move,min:-2,cur:-0.1,max:2;n:type:ShaderForge.SFN_Append,id:23,x:35285,y:32551|A-18-OUT,B-22-OUT;n:type:ShaderForge.SFN_Add,id:30,x:34722,y:32561|A-42-UVOUT,B-36-OUT;n:type:ShaderForge.SFN_Multiply,id:36,x:35135,y:32702|A-23-OUT,B-17-T;n:type:ShaderForge.SFN_TexCoord,id:42,x:35040,y:32345,uv:0;n:type:ShaderForge.SFN_Multiply,id:97,x:34514,y:32559|A-30-OUT,B-102-OUT;n:type:ShaderForge.SFN_Slider,id:99,x:34748,y:32932,ptlb:map01_u_tiling,ptin:_map01_u_tiling,min:0,cur:0.9405251,max:6;n:type:ShaderForge.SFN_Slider,id:101,x:34744,y:33030,ptlb:map01_v_tiling,ptin:_map01_v_tiling,min:0,cur:1,max:6;n:type:ShaderForge.SFN_Append,id:102,x:34524,y:32722|A-99-OUT,B-101-OUT;n:type:ShaderForge.SFN_Slider,id:109,x:33239,y:33044,ptlb:brightness,ptin:_brightness,min:0,cur:2.775397,max:6;n:type:ShaderForge.SFN_Time,id:123,x:35390,y:33678;n:type:ShaderForge.SFN_Slider,id:125,x:35553,y:33375,ptlb:map02_u_move,ptin:_map02_u_move,min:-2,cur:0,max:2;n:type:ShaderForge.SFN_Slider,id:127,x:35578,y:33488,ptlb:map02_v_move,ptin:_map02_v_move,min:-2,cur:-0.15,max:2;n:type:ShaderForge.SFN_Append,id:129,x:35324,y:33414|A-125-OUT,B-127-OUT;n:type:ShaderForge.SFN_Add,id:131,x:34761,y:33424|A-135-UVOUT,B-133-OUT;n:type:ShaderForge.SFN_Multiply,id:133,x:35174,y:33565|A-129-OUT,B-123-T;n:type:ShaderForge.SFN_TexCoord,id:135,x:35079,y:33208,uv:0;n:type:ShaderForge.SFN_Multiply,id:137,x:34553,y:33422|A-131-OUT,B-143-OUT;n:type:ShaderForge.SFN_Slider,id:139,x:34787,y:33795,ptlb:map02_u_tiling,ptin:_map02_u_tiling,min:0,cur:0.9405251,max:6;n:type:ShaderForge.SFN_Slider,id:141,x:34783,y:33893,ptlb:map02_v_tiling,ptin:_map02_v_tiling,min:0,cur:1,max:6;n:type:ShaderForge.SFN_Append,id:143,x:34563,y:33585|A-139-OUT,B-141-OUT;n:type:ShaderForge.SFN_Add,id:144,x:33959,y:32968|A-2-RGB,B-146-RGB;n:type:ShaderForge.SFN_Tex2d,id:146,x:34205,y:33059,ptlb:material02,ptin:_material02,tex:c8cd796884814204297a4b03c54b6f1c,ntxv:0,isnm:False|UVIN-137-OUT;n:type:ShaderForge.SFN_Add,id:162,x:33869,y:32766|A-2-A,B-146-A;proporder:2-18-22-99-101-146-125-127-139-141-11-109;pass:END;sub:END;*/

Shader "Shader Forge/mask_smoke_alpha_01" {
    Properties {
        _material01 ("material01", 2D) = "white" {}
        _map01_u_move ("map01_u_move", Range(-2, 2)) = 0
        _map01_v_move ("map01_v_move", Range(-2, 2)) = -0.1
        _map01_u_tiling ("map01_u_tiling", Range(0, 6)) = 0.9405251
        _map01_v_tiling ("map01_v_tiling", Range(0, 6)) = 1
        _material02 ("material02", 2D) = "white" {}
        _map02_u_move ("map02_u_move", Range(-2, 2)) = 0
        _map02_v_move ("map02_v_move", Range(-2, 2)) = -0.15
        _map02_u_tiling ("map02_u_tiling", Range(0, 6)) = 0.9405251
        _map02_v_tiling ("map02_v_tiling", Range(0, 6)) = 1
        _mask_alpha ("mask_alpha", 2D) = "white" {}
        _brightness ("brightness", Range(0, 6)) = 2.775397
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform sampler2D _material01; uniform float4 _material01_ST;
            uniform sampler2D _mask_alpha; uniform float4 _mask_alpha_ST;
            uniform float _map01_u_move;
            uniform float _map01_v_move;
            uniform float _map01_u_tiling;
            uniform float _map01_v_tiling;
            uniform float _brightness;
            uniform float _map02_u_move;
            uniform float _map02_v_move;
            uniform float _map02_u_tiling;
            uniform float _map02_v_tiling;
            uniform sampler2D _material02; uniform float4 _material02_ST;
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
                float4 node_17 = _Time + _TimeEditor;
                float2 node_97 = ((i.uv0.rg+(float2(_map01_u_move,_map01_v_move)*node_17.g))*float2(_map01_u_tiling,_map01_v_tiling));
                float4 node_2 = tex2D(_material01,TRANSFORM_TEX(node_97, _material01));
                float4 node_123 = _Time + _TimeEditor;
                float2 node_137 = ((i.uv0.rg+(float2(_map02_u_move,_map02_v_move)*node_123.g))*float2(_map02_u_tiling,_map02_v_tiling));
                float4 node_146 = tex2D(_material02,TRANSFORM_TEX(node_137, _material02));
                float3 emissive = (node_2.rgb+node_146.rgb);
                float3 finalColor = emissive;
                float2 node_166 = i.uv0;
/// Final Color:
                return fixed4(finalColor,((node_2.a+node_146.a)*tex2D(_mask_alpha,TRANSFORM_TEX(node_166.rg, _mask_alpha)).a*_brightness));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
