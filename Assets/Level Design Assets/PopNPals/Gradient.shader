Shader "Custom/Gradient" 
{
	Properties 
	{
		_TopTex ("Base (RGB)", 2D) = "white" {}
		_BottomTex ("Base (RGB)", 2D) = "white" {}
		
		_TopColor ("Top Color(RGB)", Color) = (1, 1, 1, 1)
		_BottomColor ("Bottom Color(RGB)", Color) = (1, 1, 1, 1)
		
		_Max ("Max Y", Range(-50, 50)) = 5
		_Min ("Min Y", Range(-50, 50)) = -5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
		#pragma surface surf Lambert
		#pragma vertex vert

		sampler2D _TopTex;
		sampler2D _BottomTex;
		
		float4 _TopColor;
		float4 _BottomColor;
		
		float _Max;
		float _Min;

		struct Input 
		{
			float2 uv_TopTex;
			float2 uv_BottomTex;
			float3 localPos;
		};

		void vert (inout appdata_full v, out Input o) 
		{
		   o.localPos = v.vertex;
		 }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 top = tex2D (_TopTex, IN.uv_TopTex);
			half4 bot = tex2D (_BottomTex, IN.uv_BottomTex);

			float height = lerp(0, 1, IN.localPos.y/(_Max - _Min));

			o.Albedo = lerp(top.rgb * _TopColor.rgb, bot.rgb * _BottomColor.rgb, height);
			o.Alpha = lerp(top.a * _TopColor.a, bot.a * _BottomColor.a, height);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
