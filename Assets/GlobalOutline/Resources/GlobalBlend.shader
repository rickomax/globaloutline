Shader "Hidden/GlobalBlend"
{
	Properties
	{

	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _OutlineTex;

			float3 alphaBlend(float4 a, float4 b) {
				return saturate(b.xyz * b.w + a.xyz * (1.0 - b.w));
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				float4 outline = tex2D(_OutlineTex, i.uv);
				return float4(alphaBlend(color, outline), 1.0);
			}
			ENDCG
		}
	}
}
