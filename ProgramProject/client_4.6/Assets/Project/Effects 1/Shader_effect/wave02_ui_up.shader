// Shader created with Shader Forge Beta 0.34 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32567,y:32582|emission-386-OUT;n:type:ShaderForge.SFN_Append,id:7,x:36375,y:32848|A-8-OUT,B-10-OUT;n:type:ShaderForge.SFN_Slider,id:8,x:36679,y:32835,ptlb:map01_u_move,ptin:_map01_u_move,min:-5,cur:-1.15029,max:5;n:type:ShaderForge.SFN_Slider,id:10,x:36691,y:32954,ptlb:map01_v_move,ptin:_map01_v_move,min:-5,cur:-0.7517293,max:5;n:type:ShaderForge.SFN_Add,id:11,x:35760,y:32733|A-12-UVOUT,B-14-OUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:37429,y:32570,uv:0;n:type:ShaderForge.SFN_Time,id:13,x:36488,y:33041;n:type:ShaderForge.SFN_Multiply,id:14,x:36099,y:32915|A-7-OUT,B-13-T;n:type:ShaderForge.SFN_Multiply,id:15,x:35601,y:32847|A-11-OUT,B-23-OUT;n:type:ShaderForge.SFN_Append,id:23,x:35711,y:33082|A-24-OUT,B-26-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:35975,y:33101,ptlb:map01_u_tiling,ptin:_map01_u_tiling,min:0,cur:0.3208152,max:5;n:type:ShaderForge.SFN_Slider,id:26,x:36036,y:33209,ptlb:map01_v_tiling,ptin:_map01_v_tiling,min:0,cur:0.4991528,max:5;n:type:ShaderForge.SFN_Tex2d,id:106,x:35105,y:32877,ptlb:material01,ptin:_material01,tex:ba6e7297eb2dda245b5b83eab3c84160,ntxv:0,isnm:False|UVIN-15-OUT;n:type:ShaderForge.SFN_Tex2d,id:107,x:34811,y:33241,ptlb:material02,ptin:_material02,tex:ba6e7297eb2dda245b5b83eab3c84160,ntxv:0,isnm:False|UVIN-114-OUT;n:type:ShaderForge.SFN_Multiply,id:108,x:33975,y:32673|A-355-OUT,B-165-OUT,C-131-OUT;n:type:ShaderForge.SFN_Add,id:113,x:35767,y:33427|A-12-UVOUT,B-124-OUT;n:type:ShaderForge.SFN_Multiply,id:114,x:35148,y:33380|A-113-OUT,B-115-OUT;n:type:ShaderForge.SFN_Append,id:115,x:35416,y:33512|A-116-OUT,B-117-OUT;n:type:ShaderForge.SFN_Slider,id:116,x:36013,y:33334,ptlb:map02_u_tiling,ptin:_map02_u_tiling,min:0,cur:0.3320175,max:5;n:type:ShaderForge.SFN_Slider,id:117,x:35989,y:33450,ptlb:map02_v_tiling,ptin:_map02_v_tiling,min:0,cur:0.2513776,max:5;n:type:ShaderForge.SFN_Append,id:119,x:36276,y:33486|A-120-OUT,B-121-OUT;n:type:ShaderForge.SFN_Slider,id:120,x:36823,y:33363,ptlb:map02_u_move,ptin:_map02_u_move,min:-5,cur:0.7745565,max:5;n:type:ShaderForge.SFN_Slider,id:121,x:36823,y:33512,ptlb:map02_v_move,ptin:_map02_v_move,min:-5,cur:0.9454641,max:5;n:type:ShaderForge.SFN_Time,id:123,x:36477,y:33594;n:type:ShaderForge.SFN_Multiply,id:124,x:36025,y:33578|A-119-OUT,B-123-T;n:type:ShaderForge.SFN_Tex2d,id:125,x:35002,y:33582,ptlb:material03,ptin:_material03,tex:ba6e7297eb2dda245b5b83eab3c84160,ntxv:0,isnm:False|UVIN-136-OUT;n:type:ShaderForge.SFN_Slider,id:131,x:34255,y:32753,ptlb:brightness,ptin:_brightness,min:0,cur:10,max:10;n:type:ShaderForge.SFN_Multiply,id:136,x:35259,y:33744|A-137-OUT,B-144-OUT;n:type:ShaderForge.SFN_Add,id:137,x:35554,y:33645|A-12-UVOUT,B-138-OUT;n:type:ShaderForge.SFN_Multiply,id:138,x:36085,y:33798|A-139-OUT,B-140-T;n:type:ShaderForge.SFN_Append,id:139,x:36433,y:33813|A-141-OUT,B-143-OUT;n:type:ShaderForge.SFN_Time,id:140,x:36435,y:33997;n:type:ShaderForge.SFN_Slider,id:141,x:36654,y:33752,ptlb:map03_u_move,ptin:_map03_u_move,min:-5,cur:-0.4396741,max:5;n:type:ShaderForge.SFN_Slider,id:143,x:36652,y:33842,ptlb:map03_v_move,ptin:_map03_v_move,min:-5,cur:1.202759,max:5;n:type:ShaderForge.SFN_Append,id:144,x:35569,y:33917|A-145-OUT,B-146-OUT;n:type:ShaderForge.SFN_Slider,id:145,x:35827,y:33942,ptlb:map03_u_tiling,ptin:_map03_u_tiling,min:0,cur:0.2498314,max:5;n:type:ShaderForge.SFN_Slider,id:146,x:35852,y:34040,ptlb:map03_v_tiling,ptin:_map03_v_tiling,min:0,cur:0.2807708,max:5;n:type:ShaderForge.SFN_Tex2d,id:149,x:34171,y:33028,ptlb:mask(RGBA),ptin:_maskRGBA,tex:ab39d5e4865e68b42864a231d0ca9c74,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:165,x:33591,y:33287|A-293-OUT,B-166-OUT;n:type:ShaderForge.SFN_Slider,id:166,x:33701,y:33666,ptlb:transparency,ptin:_transparency,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Tex2d,id:254,x:33727,y:32136,ptlb:map_add,ptin:_map_add,tex:a28451917deb11b429ec2b39edcdee88,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:255,x:32847,y:32456|A-263-OUT,B-108-OUT;n:type:ShaderForge.SFN_Multiply,id:263,x:33242,y:32209|A-254-RGB,B-264-OUT;n:type:ShaderForge.SFN_Slider,id:264,x:33444,y:32335,ptlb:map_add_transparency,ptin:_map_add_transparency,min:0,cur:0.6893204,max:1;n:type:ShaderForge.SFN_Multiply,id:290,x:33958,y:33071|A-149-R,B-291-OUT;n:type:ShaderForge.SFN_Slider,id:291,x:34167,y:33212,ptlb:chanel_R,ptin:_chanel_R,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Add,id:293,x:33765,y:33157|A-290-OUT,B-296-OUT,C-305-OUT,D-312-OUT;n:type:ShaderForge.SFN_Multiply,id:296,x:33939,y:33261|A-149-G,B-297-OUT;n:type:ShaderForge.SFN_Slider,id:297,x:34165,y:33306,ptlb:chanel_G,ptin:_chanel_G,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:303,x:34176,y:33408,ptlb:chanel_B,ptin:_chanel_B,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:305,x:33928,y:33421|A-149-B,B-303-OUT;n:type:ShaderForge.SFN_Slider,id:310,x:34162,y:33510,ptlb:chanel_A,ptin:_chanel_A,min:0,cur:0.6493112,max:1;n:type:ShaderForge.SFN_Multiply,id:312,x:33941,y:33543|A-149-A,B-310-OUT;n:type:ShaderForge.SFN_Multiply,id:355,x:34189,y:32384|A-107-RGB,B-125-RGB,C-106-RGB;n:type:ShaderForge.SFN_VertexColor,id:381,x:33334,y:32761;n:type:ShaderForge.SFN_Multiply,id:386,x:32871,y:32674|A-255-OUT,B-381-RGB,C-381-A;proporder:131-106-24-26-8-10-107-116-117-120-121-125-145-146-141-143-149-291-297-303-310-166-254-264;pass:END;sub:END;*/

Shader "NGUI/wave02_ui_up" {
    Properties {
        _brightness ("brightness", Range(0, 10)) = 10
        _material01 ("material01", 2D) = "white" {}
        _map01_u_tiling ("map01_u_tiling", Range(0, 5)) = 0.3208152
        _map01_v_tiling ("map01_v_tiling", Range(0, 5)) = 0.4991528
        _map01_u_move ("map01_u_move", Range(-5, 5)) = -1.15029
        _map01_v_move ("map01_v_move", Range(-5, 5)) = -0.7517293
        _material02 ("material02", 2D) = "white" {}
        _map02_u_tiling ("map02_u_tiling", Range(0, 5)) = 0.3320175
        _map02_v_tiling ("map02_v_tiling", Range(0, 5)) = 0.2513776
        _map02_u_move ("map02_u_move", Range(-5, 5)) = 0.7745565
        _map02_v_move ("map02_v_move", Range(-5, 5)) = 0.9454641
        _material03 ("material03", 2D) = "white" {}
        _map03_u_tiling ("map03_u_tiling", Range(0, 5)) = 0.2498314
        _map03_v_tiling ("map03_v_tiling", Range(0, 5)) = 0.2807708
        _map03_u_move ("map03_u_move", Range(-5, 5)) = -0.4396741
        _map03_v_move ("map03_v_move", Range(-5, 5)) = 1.202759
        _maskRGBA ("mask(RGBA)", 2D) = "white" {}
        _chanel_R ("chanel_R", Range(0, 1)) = 1
        _chanel_G ("chanel_G", Range(0, 1)) = 0
        _chanel_B ("chanel_B", Range(0, 1)) = 0
        _chanel_A ("chanel_A", Range(0, 1)) = 0.6493112
        _transparency ("transparency", Range(0, 10)) = 1
        _map_add ("map_add", 2D) = "white" {}
        _map_add_transparency ("map_add_transparency", Range(0, 1)) = 0.6893204
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+1000"
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
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_406 = i.uv0;
                float2 node_12 = i.uv0;
                float4 node_123 = _Time + _TimeEditor;
                float2 node_114 = ((node_12.rg+(float2(_map02_u_move,_map02_v_move)*node_123.g))*float2(_map02_u_tiling,_map02_v_tiling));
                float4 node_140 = _Time + _TimeEditor;
                float2 node_136 = ((node_12.rg+(float2(_map03_u_move,_map03_v_move)*node_140.g))*float2(_map03_u_tiling,_map03_v_tiling));
                float4 node_13 = _Time + _TimeEditor;
                float2 node_15 = ((node_12.rg+(float2(_map01_u_move,_map01_v_move)*node_13.g))*float2(_map01_u_tiling,_map01_v_tiling));
                float4 node_149 = tex2D(_maskRGBA,TRANSFORM_TEX(node_406.rg, _maskRGBA));
                float4 node_381 = i.vertexColor;
                float3 emissive = (((tex2D(_map_add,TRANSFORM_TEX(node_406.rg, _map_add)).rgb*_map_add_transparency)+((tex2D(_material02,TRANSFORM_TEX(node_114, _material02)).rgb*tex2D(_material03,TRANSFORM_TEX(node_136, _material03)).rgb*tex2D(_material01,TRANSFORM_TEX(node_15, _material01)).rgb)*(((node_149.r*_chanel_R)+(node_149.g*_chanel_G)+(node_149.b*_chanel_B)+(node_149.a*_chanel_A))*_transparency)*_brightness))*node_381.rgb*node_381.a);
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
