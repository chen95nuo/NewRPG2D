// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32567,y:32582|emission-255-OUT,alpha-165-OUT;n:type:ShaderForge.SFN_Append,id:7,x:34397,y:32497|A-8-OUT,B-10-OUT;n:type:ShaderForge.SFN_Slider,id:8,x:34701,y:32484,ptlb:map01_u_move,ptin:_map01_u_move,min:-5,cur:-0.03765822,max:5;n:type:ShaderForge.SFN_Slider,id:10,x:34713,y:32603,ptlb:map01_v_move,ptin:_map01_v_move,min:-5,cur:-0.0150376,max:5;n:type:ShaderForge.SFN_Add,id:11,x:33782,y:32382|A-12-UVOUT,B-14-OUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:35451,y:32219,uv:0;n:type:ShaderForge.SFN_Time,id:13,x:34510,y:32690;n:type:ShaderForge.SFN_Multiply,id:14,x:34121,y:32564|A-7-OUT,B-13-T;n:type:ShaderForge.SFN_Multiply,id:15,x:33623,y:32496|A-11-OUT,B-23-OUT;n:type:ShaderForge.SFN_Append,id:23,x:33733,y:32731|A-24-OUT,B-26-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:33997,y:32750,ptlb:map01_u_tiling,ptin:_map01_u_tiling,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:26,x:34020,y:32903,ptlb:map01_v_tiling,ptin:_map01_v_tiling,min:0,cur:2.5,max:5;n:type:ShaderForge.SFN_Tex2d,id:106,x:33127,y:32526,ptlb:material01,ptin:_material01,tex:f90dbe0f708e9464f9fb9dba53d95a7d,ntxv:0,isnm:False|UVIN-15-OUT;n:type:ShaderForge.SFN_Tex2d,id:107,x:33438,y:32869,ptlb:material02,ptin:_material02,tex:f90dbe0f708e9464f9fb9dba53d95a7d,ntxv:0,isnm:False|UVIN-114-OUT;n:type:ShaderForge.SFN_Multiply,id:108,x:32916,y:32713|A-106-RGB,B-107-RGB,C-125-RGB,D-131-OUT;n:type:ShaderForge.SFN_Add,id:113,x:34394,y:33055|A-12-UVOUT,B-124-OUT;n:type:ShaderForge.SFN_Multiply,id:114,x:33775,y:33008|A-113-OUT,B-115-OUT;n:type:ShaderForge.SFN_Append,id:115,x:34043,y:33140|A-116-OUT,B-117-OUT;n:type:ShaderForge.SFN_Slider,id:116,x:34360,y:33217,ptlb:map02_u_tiling,ptin:_map02_u_tiling,min:0,cur:1.1,max:5;n:type:ShaderForge.SFN_Slider,id:117,x:34344,y:33316,ptlb:map02_v_tiling,ptin:_map02_v_tiling,min:0,cur:2,max:5;n:type:ShaderForge.SFN_Append,id:119,x:34903,y:33114|A-120-OUT,B-121-OUT;n:type:ShaderForge.SFN_Slider,id:120,x:35450,y:32991,ptlb:map02_u_move,ptin:_map02_u_move,min:-5,cur:-0.03,max:5;n:type:ShaderForge.SFN_Slider,id:121,x:35450,y:33140,ptlb:map02_v_move,ptin:_map02_v_move,min:-5,cur:-0.03,max:5;n:type:ShaderForge.SFN_Time,id:123,x:35104,y:33222;n:type:ShaderForge.SFN_Multiply,id:124,x:34652,y:33206|A-119-OUT,B-123-T;n:type:ShaderForge.SFN_Tex2d,id:125,x:33629,y:33210,ptlb:material03,ptin:_material03,tex:f90dbe0f708e9464f9fb9dba53d95a7d,ntxv:0,isnm:False|UVIN-136-OUT;n:type:ShaderForge.SFN_Slider,id:131,x:33324,y:32746,ptlb:brightness,ptin:_brightness,min:0,cur:2.755641,max:10;n:type:ShaderForge.SFN_Multiply,id:136,x:33886,y:33372|A-137-OUT,B-144-OUT;n:type:ShaderForge.SFN_Add,id:137,x:34181,y:33273|A-12-UVOUT,B-138-OUT;n:type:ShaderForge.SFN_Multiply,id:138,x:34712,y:33426|A-139-OUT,B-140-T;n:type:ShaderForge.SFN_Append,id:139,x:35060,y:33441|A-141-OUT,B-143-OUT;n:type:ShaderForge.SFN_Time,id:140,x:35062,y:33625;n:type:ShaderForge.SFN_Slider,id:141,x:35281,y:33380,ptlb:map03_u_move,ptin:_map03_u_move,min:-5,cur:-0.01373429,max:5;n:type:ShaderForge.SFN_Slider,id:143,x:35279,y:33470,ptlb:map03_v_move,ptin:_map03_v_move,min:-5,cur:-0.02907109,max:5;n:type:ShaderForge.SFN_Append,id:144,x:34196,y:33545|A-145-OUT,B-146-OUT;n:type:ShaderForge.SFN_Slider,id:145,x:34454,y:33570,ptlb:map03_u_tiling,ptin:_map03_u_tiling,min:0,cur:2,max:5;n:type:ShaderForge.SFN_Slider,id:146,x:34479,y:33668,ptlb:map03_v_tiling,ptin:_map03_v_tiling,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Tex2d,id:149,x:33182,y:32989,ptlb:mask(RGBA),ptin:_maskRGBA,tex:361f033918735af40beb3de294f7a3eb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:165,x:32602,y:33248|A-293-OUT,B-166-OUT;n:type:ShaderForge.SFN_Slider,id:166,x:32712,y:33627,ptlb:transparency,ptin:_transparency,min:0,cur:0.4463561,max:1;n:type:ShaderForge.SFN_Slider,id:237,x:34189,y:32942,ptlb:node_24cc,ptin:_node_24cc,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:239,x:34253,y:33006,ptlb:node_24cc_copy,ptin:_node_24cc_copy,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Tex2d,id:254,x:33727,y:32136,ptlb:map_add,ptin:_map_add,tex:139670255ba51f5488e748256d7d5745,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:255,x:32847,y:32456|A-263-OUT,B-108-OUT;n:type:ShaderForge.SFN_Multiply,id:263,x:33242,y:32209|A-254-RGB,B-264-OUT;n:type:ShaderForge.SFN_Slider,id:264,x:33444,y:32335,ptlb:map_add_transparency,ptin:_map_add_transparency,min:0,cur:0.6893204,max:1;n:type:ShaderForge.SFN_Slider,id:266,x:34317,y:33070,ptlb:node_24cc_copy_copy,ptin:_node_24cc_copy_copy,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:268,x:34381,y:33134,ptlb:node_24cc_copy_copy_copy,ptin:_node_24cc_copy_copy_copy,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:270,x:34445,y:33198,ptlb:node_24cc_copy_copy_copy_copy,ptin:_node_24cc_copy_copy_copy_copy,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Multiply,id:290,x:32969,y:33032|A-149-R,B-291-OUT;n:type:ShaderForge.SFN_Slider,id:291,x:33178,y:33173,ptlb:chanel_R,ptin:_chanel_R,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Add,id:293,x:32776,y:33118|A-290-OUT,B-296-OUT,C-305-OUT,D-312-OUT;n:type:ShaderForge.SFN_Multiply,id:296,x:32950,y:33222|A-149-G,B-297-OUT;n:type:ShaderForge.SFN_Slider,id:297,x:33176,y:33267,ptlb:chanel_G,ptin:_chanel_G,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:303,x:33187,y:33369,ptlb:chanel_B,ptin:_chanel_B,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:305,x:32939,y:33382|A-149-B,B-303-OUT;n:type:ShaderForge.SFN_Slider,id:310,x:33173,y:33471,ptlb:chanel_A,ptin:_chanel_A,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:312,x:32952,y:33504|A-149-A,B-310-OUT;proporder:131-106-24-26-8-10-107-116-117-120-121-125-145-146-141-143-149-291-297-303-310-166-254-264;pass:END;sub:END;*/

Shader "Shader Forge/wave" {
    Properties {
        _brightness ("brightness", Range(0, 10)) = 2.755641
        _material01 ("material01", 2D) = "white" {}
        _map01_u_tiling ("map01_u_tiling", Range(0, 5)) = 5
        _map01_v_tiling ("map01_v_tiling", Range(0, 5)) = 2.5
        _map01_u_move ("map01_u_move", Range(-5, 5)) = -0.03765822
        _map01_v_move ("map01_v_move", Range(-5, 5)) = -0.0150376
        _material02 ("material02", 2D) = "white" {}
        _map02_u_tiling ("map02_u_tiling", Range(0, 5)) = 1.1
        _map02_v_tiling ("map02_v_tiling", Range(0, 5)) = 2
        _map02_u_move ("map02_u_move", Range(-5, 5)) = -0.03
        _map02_v_move ("map02_v_move", Range(-5, 5)) = -0.03
        _material03 ("material03", 2D) = "white" {}
        _map03_u_tiling ("map03_u_tiling", Range(0, 5)) = 2
        _map03_v_tiling ("map03_v_tiling", Range(0, 5)) = 1
        _map03_u_move ("map03_u_move", Range(-5, 5)) = -0.01373429
        _map03_v_move ("map03_v_move", Range(-5, 5)) = -0.02907109
        _maskRGBA ("mask(RGBA)", 2D) = "white" {}
        _chanel_R ("chanel_R", Range(0, 1)) = 1
        _chanel_G ("chanel_G", Range(0, 1)) = 0
        _chanel_B ("chanel_B", Range(0, 1)) = 0
        _chanel_A ("chanel_A", Range(0, 1)) = 0
        _transparency ("transparency", Range(0, 1)) = 0.4463561
        _map_add ("map_add", 2D) = "white" {}
        _map_add_transparency ("map_add_transparency", Range(0, 1)) = 0.6893204
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
            uniform float _map01_u_move;
            uniform float _map01_v_move;
            uniform float _map01_u_tiling;
            uniform float _map01_v_tiling;
            uniform sampler2D _material01; uniform float4 _material01_ST;
            uniform sampler2D _material02; uniform float4 _material02_ST;
            uniform float _map02_u_tiling;
            uniform float _map02_v_tiling;
            uniform float _map02_u_move;
            uniform float _map02_v_move;
            uniform sampler2D _material03; uniform float4 _material03_ST;
            uniform float _brightness;
            uniform float _map03_u_move;
            uniform float _map03_v_move;
            uniform float _map03_u_tiling;
            uniform float _map03_v_tiling;
            uniform sampler2D _maskRGBA; uniform float4 _maskRGBA_ST;
            uniform float _transparency;
            uniform sampler2D _map_add; uniform float4 _map_add_ST;
            uniform float _map_add_transparency;
            uniform float _chanel_R;
            uniform float _chanel_G;
            uniform float _chanel_B;
            uniform float _chanel_A;
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
                float2 node_329 = i.uv0;
                float2 node_12 = i.uv0;
                float4 node_13 = _Time + _TimeEditor;
                float2 node_15 = ((node_12.rg+(float2(_map01_u_move,_map01_v_move)*node_13.g))*float2(_map01_u_tiling,_map01_v_tiling));
                float4 node_123 = _Time + _TimeEditor;
                float2 node_114 = ((node_12.rg+(float2(_map02_u_move,_map02_v_move)*node_123.g))*float2(_map02_u_tiling,_map02_v_tiling));
                float4 node_140 = _Time + _TimeEditor;
                float2 node_136 = ((node_12.rg+(float2(_map03_u_move,_map03_v_move)*node_140.g))*float2(_map03_u_tiling,_map03_v_tiling));
                float3 emissive = ((tex2D(_map_add,TRANSFORM_TEX(node_329.rg, _map_add)).rgb*_map_add_transparency)+(tex2D(_material01,TRANSFORM_TEX(node_15, _material01)).rgb*tex2D(_material02,TRANSFORM_TEX(node_114, _material02)).rgb*tex2D(_material03,TRANSFORM_TEX(node_136, _material03)).rgb*_brightness));
                float3 finalColor = emissive;
                float4 node_149 = tex2D(_maskRGBA,TRANSFORM_TEX(node_329.rg, _maskRGBA));
/// Final Color:
                return fixed4(finalColor,(((node_149.r*_chanel_R)+(node_149.g*_chanel_G)+(node_149.b*_chanel_B)+(node_149.a*_chanel_A))*_transparency));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
