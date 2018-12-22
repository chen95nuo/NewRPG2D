// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32567,y:32582|emission-255-OUT;n:type:ShaderForge.SFN_Append,id:7,x:34717,y:32474|A-8-OUT,B-10-OUT;n:type:ShaderForge.SFN_Slider,id:8,x:35021,y:32461,ptlb:map01_u_move,ptin:_map01_u_move,min:-5,cur:-0.09765822,max:5;n:type:ShaderForge.SFN_Slider,id:10,x:35033,y:32580,ptlb:map01_v_move,ptin:_map01_v_move,min:-5,cur:-0.0750376,max:5;n:type:ShaderForge.SFN_Add,id:11,x:34102,y:32359|A-12-UVOUT,B-14-OUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:35771,y:32196,uv:0;n:type:ShaderForge.SFN_Time,id:13,x:34830,y:32667;n:type:ShaderForge.SFN_Multiply,id:14,x:34441,y:32541|A-7-OUT,B-13-T;n:type:ShaderForge.SFN_Multiply,id:15,x:33943,y:32473|A-11-OUT,B-23-OUT;n:type:ShaderForge.SFN_Append,id:23,x:34053,y:32708|A-24-OUT,B-26-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:34317,y:32727,ptlb:map01_u_tiling,ptin:_map01_u_tiling,min:0,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:26,x:34340,y:32880,ptlb:map01_v_tiling,ptin:_map01_v_tiling,min:0,cur:2.5,max:5;n:type:ShaderForge.SFN_Tex2d,id:106,x:33447,y:32503,ptlb:material01,ptin:_material01,tex:f4a237fe42da3df4ba59a9b267d55843,ntxv:0,isnm:False|UVIN-15-OUT;n:type:ShaderForge.SFN_Tex2d,id:107,x:33606,y:32828,ptlb:material02,ptin:_material02,tex:f4a237fe42da3df4ba59a9b267d55843,ntxv:0,isnm:False|UVIN-114-OUT;n:type:ShaderForge.SFN_Multiply,id:108,x:32986,y:32634|A-334-OUT,B-165-OUT,C-131-OUT;n:type:ShaderForge.SFN_Add,id:113,x:34562,y:33014|A-12-UVOUT,B-124-OUT;n:type:ShaderForge.SFN_Multiply,id:114,x:33943,y:32967|A-113-OUT,B-115-OUT;n:type:ShaderForge.SFN_Append,id:115,x:34211,y:33099|A-116-OUT,B-117-OUT;n:type:ShaderForge.SFN_Slider,id:116,x:34661,y:32913,ptlb:map02_u_tiling,ptin:_map02_u_tiling,min:0,cur:1.1,max:5;n:type:ShaderForge.SFN_Slider,id:117,x:34512,y:33275,ptlb:map02_v_tiling,ptin:_map02_v_tiling,min:0,cur:2,max:5;n:type:ShaderForge.SFN_Append,id:119,x:35071,y:33073|A-120-OUT,B-121-OUT;n:type:ShaderForge.SFN_Slider,id:120,x:35618,y:32950,ptlb:map02_u_move,ptin:_map02_u_move,min:-5,cur:-0.08,max:5;n:type:ShaderForge.SFN_Slider,id:121,x:35618,y:33099,ptlb:map02_v_move,ptin:_map02_v_move,min:-5,cur:-0.08,max:5;n:type:ShaderForge.SFN_Time,id:123,x:35272,y:33181;n:type:ShaderForge.SFN_Multiply,id:124,x:34820,y:33165|A-119-OUT,B-123-T;n:type:ShaderForge.SFN_Tex2d,id:125,x:33797,y:33169,ptlb:material03,ptin:_material03,tex:f4a237fe42da3df4ba59a9b267d55843,ntxv:0,isnm:False|UVIN-136-OUT;n:type:ShaderForge.SFN_Slider,id:131,x:33431,y:32714,ptlb:brightness,ptin:_brightness,min:0,cur:1.278196,max:10;n:type:ShaderForge.SFN_Multiply,id:136,x:34054,y:33331|A-137-OUT,B-144-OUT;n:type:ShaderForge.SFN_Add,id:137,x:34349,y:33232|A-12-UVOUT,B-138-OUT;n:type:ShaderForge.SFN_Multiply,id:138,x:34880,y:33385|A-139-OUT,B-140-T;n:type:ShaderForge.SFN_Append,id:139,x:35228,y:33400|A-141-OUT,B-143-OUT;n:type:ShaderForge.SFN_Time,id:140,x:35230,y:33584;n:type:ShaderForge.SFN_Slider,id:141,x:35449,y:33339,ptlb:map03_u_move,ptin:_map03_u_move,min:-5,cur:-0.06373429,max:5;n:type:ShaderForge.SFN_Slider,id:143,x:35447,y:33429,ptlb:map03_v_move,ptin:_map03_v_move,min:-5,cur:-0.07907109,max:5;n:type:ShaderForge.SFN_Append,id:144,x:34364,y:33504|A-145-OUT,B-146-OUT;n:type:ShaderForge.SFN_Slider,id:145,x:34622,y:33529,ptlb:map03_u_tiling,ptin:_map03_u_tiling,min:0,cur:2,max:5;n:type:ShaderForge.SFN_Slider,id:146,x:34647,y:33627,ptlb:map03_v_tiling,ptin:_map03_v_tiling,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Tex2d,id:149,x:33182,y:32989,ptlb:mask(RGBA),ptin:_maskRGBA,tex:361f033918735af40beb3de294f7a3eb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:165,x:32602,y:33248|A-293-OUT,B-166-OUT;n:type:ShaderForge.SFN_Slider,id:166,x:32712,y:33627,ptlb:transparency,ptin:_transparency,min:0,cur:0.2307473,max:1;n:type:ShaderForge.SFN_Tex2d,id:254,x:33727,y:32136,ptlb:map_add,ptin:_map_add,tex:b22cda9d0f88dc140a350a2f799150db,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:255,x:32847,y:32456|A-263-OUT,B-108-OUT;n:type:ShaderForge.SFN_Multiply,id:263,x:33242,y:32209|A-254-RGB,B-264-OUT;n:type:ShaderForge.SFN_Slider,id:264,x:33444,y:32335,ptlb:map_add_transparency,ptin:_map_add_transparency,min:0,cur:0.6893204,max:1;n:type:ShaderForge.SFN_Multiply,id:290,x:32969,y:33032|A-149-R,B-291-OUT;n:type:ShaderForge.SFN_Slider,id:291,x:33178,y:33173,ptlb:chanel_R,ptin:_chanel_R,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:293,x:32776,y:33118|A-290-OUT,B-296-OUT,C-305-OUT,D-312-OUT;n:type:ShaderForge.SFN_Multiply,id:296,x:32950,y:33222|A-149-G,B-297-OUT;n:type:ShaderForge.SFN_Slider,id:297,x:33176,y:33267,ptlb:chanel_G,ptin:_chanel_G,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:303,x:33187,y:33369,ptlb:chanel_B,ptin:_chanel_B,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:305,x:32939,y:33382|A-149-B,B-303-OUT;n:type:ShaderForge.SFN_Slider,id:310,x:33173,y:33471,ptlb:chanel_A,ptin:_chanel_A,min:0,cur:0.6493112,max:1;n:type:ShaderForge.SFN_Multiply,id:312,x:32952,y:33504|A-149-A,B-310-OUT;n:type:ShaderForge.SFN_Add,id:334,x:33133,y:32489|A-106-RGB,B-107-RGB,C-125-RGB;proporder:131-106-24-26-8-10-107-116-117-120-121-125-145-146-141-143-149-291-297-303-310-166-254-264;pass:END;sub:END;*/

Shader "Shader Forge/wave02" {
    Properties {
        _brightness ("brightness", Range(0, 10)) = 1.278196
        _material01 ("material01", 2D) = "white" {}
        _map01_u_tiling ("map01_u_tiling", Range(0, 5)) = 5
        _map01_v_tiling ("map01_v_tiling", Range(0, 5)) = 2.5
        _map01_u_move ("map01_u_move", Range(-5, 5)) = -0.09765822
        _map01_v_move ("map01_v_move", Range(-5, 5)) = -0.0750376
        _material02 ("material02", 2D) = "white" {}
        _map02_u_tiling ("map02_u_tiling", Range(0, 5)) = 1.1
        _map02_v_tiling ("map02_v_tiling", Range(0, 5)) = 2
        _map02_u_move ("map02_u_move", Range(-5, 5)) = -0.08
        _map02_v_move ("map02_v_move", Range(-5, 5)) = -0.08
        _material03 ("material03", 2D) = "white" {}
        _map03_u_tiling ("map03_u_tiling", Range(0, 5)) = 2
        _map03_v_tiling ("map03_v_tiling", Range(0, 5)) = 1
        _map03_u_move ("map03_u_move", Range(-5, 5)) = -0.06373429
        _map03_v_move ("map03_v_move", Range(-5, 5)) = -0.07907109
        _maskRGBA ("mask(RGBA)", 2D) = "white" {}
        _chanel_R ("chanel_R", Range(0, 1)) = 0
        _chanel_G ("chanel_G", Range(0, 1)) = 0
        _chanel_B ("chanel_B", Range(0, 1)) = 0
        _chanel_A ("chanel_A", Range(0, 1)) = 0.6493112
        _transparency ("transparency", Range(0, 1)) = 0.2307473
        _map_add ("map_add", 2D) = "white" {}
        _map_add_transparency ("map_add_transparency", Range(0, 1)) = 0.6893204
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
                float2 node_357 = i.uv0;
                float2 node_12 = i.uv0;
                float4 node_13 = _Time + _TimeEditor;
                float2 node_15 = ((node_12.rg+(float2(_map01_u_move,_map01_v_move)*node_13.g))*float2(_map01_u_tiling,_map01_v_tiling));
                float4 node_123 = _Time + _TimeEditor;
                float2 node_114 = ((node_12.rg+(float2(_map02_u_move,_map02_v_move)*node_123.g))*float2(_map02_u_tiling,_map02_v_tiling));
                float4 node_140 = _Time + _TimeEditor;
                float2 node_136 = ((node_12.rg+(float2(_map03_u_move,_map03_v_move)*node_140.g))*float2(_map03_u_tiling,_map03_v_tiling));
                float4 node_149 = tex2D(_maskRGBA,TRANSFORM_TEX(node_357.rg, _maskRGBA));
                float3 emissive = ((tex2D(_map_add,TRANSFORM_TEX(node_357.rg, _map_add)).rgb*_map_add_transparency)+((tex2D(_material01,TRANSFORM_TEX(node_15, _material01)).rgb+tex2D(_material02,TRANSFORM_TEX(node_114, _material02)).rgb+tex2D(_material03,TRANSFORM_TEX(node_136, _material03)).rgb)*(((node_149.r*_chanel_R)+(node_149.g*_chanel_G)+(node_149.b*_chanel_B)+(node_149.a*_chanel_A))*_transparency)*_brightness));
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
