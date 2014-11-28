Shader "Custom/AlwaysVisibleUnlit" 
{
	Properties 
	{
		_Color ("Main Color(RGB)", Color) = (1, 1, 1, 1)
		_MainTex ("Base Texture(RGBA)", 2D) = "white"{}
	}
	SubShader 
	{
		Tags { "RenderType" = "Transparent" "Queue"="Transparent"}
		LOD 200
		
		Ztest Always
		Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Unlit
		#include "UnityCG.cginc"
		
		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		float4 _Color;

		struct Input 
		{
			float4 worldPos;
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float4 t = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = t.rgb * _Color.rgb;
			o.Alpha = t.a * _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
