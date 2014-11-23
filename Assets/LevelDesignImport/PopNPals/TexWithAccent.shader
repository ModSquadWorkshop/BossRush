Shader "Custom/TexWithAccent" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AccentColor ("Accent Color", Color) = (1, 1, 1, 1)
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _AccentColor;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			const half4 c = tex2D (_MainTex, IN.uv_MainTex);		
			o.Albedo = c.r == 1 && c.g == 1 && c.b == 1 ? _AccentColor.rgb : c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
