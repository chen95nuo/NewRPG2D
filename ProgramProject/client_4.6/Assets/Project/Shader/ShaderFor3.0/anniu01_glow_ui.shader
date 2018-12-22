// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32567,y:32582|emission-445-OUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:35804,y:32947,uv:0;n:type:ShaderForge.SFN_Tex2d,id:403,x:33677,y:32730,ptlb:material04,ptin:_material04,tex:e62e4bde57ccbca4b8cc90b682c22958,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:405,x:35290,y:33112|A-12-UVOUT,B-423-OUT;n:type:ShaderForge.SFN_Multiply,id:407,x:34671,y:33065|A-405-OUT,B-409-OUT;n:type:ShaderForge.SFN_Append,id:409,x:34939,y:33197|A-411-OUT,B-413-OUT;n:type:ShaderForge.SFN_Slider,id:411,x:35255,y:33260,ptlb:map02_u_tiling_copy_copy,ptin:_map02_u_tiling_copy_copy,min:0,cur:0.05,max:5;n:type:ShaderForge.SFN_Slider,id:413,x:35184,y:33410,ptlb:map02_v_tiling_copy_copy,ptin:_map02_v_tiling_copy_copy,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Append,id:415,x:35986,y:33309|A-417-OUT,B-419-OUT;n:type:ShaderForge.SFN_Slider,id:417,x:36533,y:33186,ptlb:map02_u_move_copy_copy,ptin:_map02_u_move_copy_copy,min:-5,cur:5,max:5;n:type:ShaderForge.SFN_Slider,id:419,x:36533,y:33335,ptlb:map02_v_move_copy_copy,ptin:_map02_v_move_copy_copy,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Time,id:421,x:36187,y:33417;n:type:ShaderForge.SFN_Multiply,id:423,x:35735,y:33401|A-415-OUT,B-421-T;n:type:ShaderForge.SFN_Multiply,id:445,x:33460,y:32893|A-403-RGB,B-462-OUT,C-482-RGB;n:type:ShaderForge.SFN_Slider,id:462,x:33768,y:32913,ptlb:brightness,ptin:_brightness,min:0,cur:0.6696144,max:1;n:type:ShaderForge.SFN_Tex2d,id:482,x:34421,y:32906,ptlb:node_482,ptin:_node_482,tex:2a8b88f2e586aa448992d6ce333520b2,ntxv:0,isnm:False|UVIN-407-OUT;proporder:403-462-411-413-417-419-482;pass:END;sub:END;*/

Shader "Shader Forge/anniu01_glow_ui" {
    Properties {
        _material04 ("material04", 2D) = "white" {}
        _brightness ("brightness", Range(0, 1)) = 0.6696144
        _map02_u_tiling_copy_copy ("glowmap_u_tiling", Range(0, 5)) = 0.05
        _map02_v_tiling_copy_copy ("glowmap_v_tiling", Range(0, 5)) = 1
        _map02_u_move_copy_copy ("glowmap_u_move", Range(-5, 5)) = 5
        _map02_v_move_copy_copy ("glowmap_v_move", Range(-5, 5)) = 0
        _node_482 ("node_482", 2D) = "white" {}
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
            uniform sampler2D _material04; uniform float4 _material04_ST;
            uniform float _map02_u_tiling_copy_copy;
            uniform float _map02_v_tiling_copy_copy;
            uniform float _map02_u_move_copy_copy;
            uniform float _map02_v_move_copy_copy;
            uniform float _brightness;
            uniform sampler2D _node_482; uniform float4 _node_482_ST;
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
                float2 node_516 = i.uv0;
                float2 node_12 = i.uv0;
                float4 node_421 = _Time + _TimeEditor;
                float2 node_407 = ((node_12.rg+(float2(_map02_u_move_copy_copy,_map02_v_move_copy_copy)*node_421.g))*float2(_map02_u_tiling_copy_copy,_map02_v_tiling_copy_copy));
                float3 emissive = (tex2D(_material04,TRANSFORM_TEX(node_516.rg, _material04)).rgb*_brightness*tex2D(_node_482,TRANSFORM_TEX(node_407, _node_482)).rgb);
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
