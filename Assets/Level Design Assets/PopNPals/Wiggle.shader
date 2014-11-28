Shader "Custom/Wiggle" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor ("Color (RGBA)", Color) = (1, 1, 1, 1)
		
		_WiggleSpeed ("Wiggle Speed", Float) = 0
		_WiggleDist ("Vert Dist", Float) = 0.012
		
		_CurWiggleStrength ("Wiggle Force", Float) = 0

		_MaxWigglePos ("Desired Wiggle Source", Vector) = (0, 0, 0, 0)
		_CurWigglePos ("Current Wiggle Source", Vector) = (0, 0, 0, 0)
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert addshadow

		sampler2D _MainTex;
		float4 _MainColor;
		
		float _WiggleSpeed;
		float _WiggleDist;
		
		float _CurWiggleStrength;
		
		float4 _MaxWigglePos;
		float4 _CurWigglePos;

		struct Input 
		{
			float2 uv_MainTex;
			float3 localPos;
			float3 worldPos;
		};

		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);

			o.localPos = v.vertex;
			o.worldPos = mul(_Object2World, v.vertex);

			float3 dirToVert = normalize(o.worldPos - _MaxWigglePos);
			float distMod = _WiggleDist - clamp(length(_MaxWigglePos - o.worldPos), 0, _WiggleDist);
			//float distMod = lerp(1, 0, clamp(distance(normalize(_MaxWigglePos), normalize(o.worldPos)), 0, 1)) * _WiggleDist;
			float appliedWiggleStrength = sin(_WiggleSpeed * _Time.y);
			
			v.vertex.xyz += dirToVert * _CurWiggleStrength * distMod * appliedWiggleStrength;
			//v.vertex += _CurWigglePos * _CurWiggleStrength * distMod * appliedWiggleStrength;
		}
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb * _MainColor;
			//o.Albedo = lerp(float4(1, 0, 0, 1), float4(0, 1, 0, 1), clamp(distance(normalize(_MaxWigglePos), normalize(IN.worldPos)), 0, 1));
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
