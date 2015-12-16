Shader "Custom/Gam2" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DampTex ("Dampen pattern", 2D)= "white" {}
		_BurningTex ("Burn pattern", 2D)= "white" {}
		_AshTex ("Ash pattern", 2D)= "white" {}
		_Coverage ("% Coverage", Range(0,1)) = 0
		
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DampTex;
		sampler2D _BurningTex;
		sampler2D _AshTex;
		half _Coverage;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BurningTex;
			float2 uv_AshTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			half2 fire = IN.uv_BurningTex;
			fire.x += 1 * _Time.y;
			fire.y += 1 * _Time.y;

			half2 ash = IN.uv_AshTex;
			ash.x += 1 * _Time.y;
			ash.y += 1 * _Time.y;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 d = tex2D (_DampTex, IN.uv_MainTex);
			fixed4 e = tex2D (_BurningTex, fire);
			fixed4 f = tex2D (_AshTex, ash);

			c.a = d.r < _Coverage ? 0 : c.a;
			c.rgb = d.r < _Coverage*3 ? e.rgb : c.rgb;
			c.rgb = d.r < _Coverage*2 ? f.rgb : c.rgb;

			o.Albedo = c.rgb;
			o.Alpha = c.a;

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
