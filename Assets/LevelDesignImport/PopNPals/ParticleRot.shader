Shader "Custom/ParticleRot" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RotForce ("Rotation Force", float) = 1
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
		float _RotForce;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
			float3 localPos;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			
			o.worldPos = mul(_Object2World, v.vertex);
			
			//float distFromCenter = distance(o.worldPos, float4(0, 0, 0, 0));
			float distFromCenter = distance(v.vertex.xyz, float4(0, 0, 0, 0));
			v.vertex += float4(cos(distFromCenter) - o.worldPos.x, sin(distFromCenter) - o.worldPos.y, 0, 0) * distFromCenter * _RotForce;
			
			o.localPos = v.vertex;
		}
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
