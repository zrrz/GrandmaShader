Shader "Custom/NormalMap" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D)        = "white" {}
		_BumpMap ("BumpMap", 2D)             = "bump"{}
		_Up      ("Up", Vector)              = (0,1,0,0)
		_Color   ("Cover", Color)            = (0,0,0,0)
		_Thresh  ("Threshold", Range (-1,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		half4     _Up;
		half4     _Color;
		half      _Thresh;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal (tex2D (_BumpMap , IN.uv_BumpMap));
			
			o.Albedo = dot( o.Normal, _Up) < _Thresh ? c.rgb : _Color.rgb;
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
