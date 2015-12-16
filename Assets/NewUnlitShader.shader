Shader "Unlit/unlit clouds"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Outline ("Outline", Color) = (0,0,0,1)
		_Extrude ("Extrude Amt", Float) = 0
		_Alerted("Alerted", int) = 0
		_DistToCamera("Distance to Camera", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
		Cull Front
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};


			float4 _Outline;
			half _Extrude;
			half _DistToCamera;
			int _Alerted;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				v.vertex.xyz += _Extrude * _DistToCamera * v.normal;
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Outline;
				col.r += _Time.x;
				col.r = frac(col.r);
				
				col.b += _Time.y;
				col.b = frac(col.b);
				
				col.g += _Time.z;
				col.g = frac(col.g);
				
				return col * _Alerted;
			}
			ENDCG
		}
	}
}