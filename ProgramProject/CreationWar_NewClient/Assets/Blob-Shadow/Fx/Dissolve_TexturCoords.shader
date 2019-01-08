Shader "Dissolve/Dissolve_TexturCoords" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Amount ("Amount", Range (0, 1)) = 0.5
	_StartAmount("StartAmount", float) = 0.1
	_Illuminate ("Illuminate", Range (0, 1)) = 0.5
	_Tile("Tile", float) = 0.7
	_DissColor ("DissColor", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_DissolveSrc ("DissolveSrc", 2D) = "white" {}
	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
    _RimPower ("Rim Power", Range(0.0,4.0)) = 1.0
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 100
	cull off
		
CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong addshadow

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _DissolveSrc;

fixed4 _Color;
half4 _DissColor;
half _Amount;
static half3 Color = float3(1,1,1);
half _Illuminate;
half _Tile;
half _StartAmount;
float4 _RimColor;
float _RimPower;


struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float2 uvDissolveSrc;
	float3 viewDir;
};

void vert (inout appdata_full v, out Input o) {}

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = tex.rgb * _Color.rgb ;	
	float ClipTex = tex2D (_DissolveSrc, IN.uv_MainTex/_Tile).r ;
	float ClipAmount = ClipTex - _Amount;
	float Clip = 0;
if (_Amount > 0)
{
	if (ClipAmount <0)
	{
		Clip = 1; //clip(-0.1);	
	}
	 else
	 {	
		if (ClipAmount < _StartAmount)
		{
				Color.x = ClipAmount/_StartAmount;
				Color.y = ClipAmount/_StartAmount;
				Color.z = ClipAmount/_StartAmount;
			o.Albedo  = (o.Albedo *((Color.x+Color.y+Color.z))*  _DissColor.rgb*((Color.x+Color.y+Color.z)))/(1 - _Illuminate);		
		}
	 }
 } 
if (Clip == 1)
{
clip(-0.1);
}
	o.Alpha = tex.a * _Color.a;	
	o.Gloss = tex.a*0.5;
//	o.Normal = tex2D(_BumpMap, IN.uv_BumpMap).rgb * 2.0 - 1.0;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	half rim = 1 - saturate(dot (normalize(IN.viewDir), o.Normal));
    o.Emission = _RimColor.rgb *rim*rim* _RimPower;
}
ENDCG
}

FallBack "Specular"
}
