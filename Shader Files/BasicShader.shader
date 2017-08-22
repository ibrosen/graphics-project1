//UNITY_SHADER_NO_UPGRADE

Shader "Custom/BasicShader"
{
	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		Tags{ "LightMode" = "ForwardBase" }
		//Lighting On
		Pass
	{
		//Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	//#pragma surface surf Lambert

	#include "UnityCG.cginc"

	struct vertIn
	{
		float4 vertex : POSITION;
		float4 color : COLOR;

	};

	struct vertOut
	{
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
	};

	// Implementation of the vertex shader
	vertOut vert(vertIn v)
	{
		vertOut o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		return o;
	}

	// Implementation of the fragment shader
	fixed4 frag(vertOut v) : SV_Target
	{
		//return float4(255.0f, 255.0f, 0.0f, 1.0f);
		return v.color;
	}
		//void surf(Input IN, inout SurfaceOutput o) {
		//	o.Albedo = Color.red;
		//}
		ENDCG
	}
	}
}
