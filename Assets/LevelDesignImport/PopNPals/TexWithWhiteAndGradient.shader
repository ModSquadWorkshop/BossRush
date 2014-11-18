Shader "Custom/TexWithWhiteAndGradient" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		_TopColor ("Top Color", Color) = (0.5, 0.5, 0.5, 1)
		_BotColor ("Bottom Color", Color) = (0.5, 0.5, 0.5, 1)
		
		_ScrollRate	("Scroll Rate", Range (0, 10)) = 3
		_GradientScale ("Gradient Scale", Range (0, 0.3)) = 0.15
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
		float4 _TopColor;
		float4 _MidColor;
		float4 _BotColor;
		float _ScrollRate;
		float _GradientScale;

		struct Input 
		{
			float2 uv_MainTex;
			float3 localPos;
		};

		void vert (inout appdata_full v, out Input o) 
		{
	  		UNITY_INITIALIZE_OUTPUT(Input,o);
	   		o.localPos = v.vertex.xyz;
	 	}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			const half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			if(c.r == 0 && c.g == 0 && c.b == 0)
			{								
				// bottom half
				// lerp between mid and bot colors
				o.Albedo = lerp(_TopColor, _BotColor, (sin(IN.localPos.y * _GradientScale + _Time.y * _ScrollRate) + 1)/2).rgb;
			}
			else
			{
				o.Albedo = c.rgb;
			}
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
