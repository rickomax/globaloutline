Shader "Hidden/GlobalBlur"
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
			float2 _MainTex_TexelSize;

			float4 blur(float2 uv) {
				float4 a = tex2D(_MainTex, uv) * 0.25;
				float4 b = tex2D(_MainTex, float2(uv.x + _MainTex_TexelSize.x, uv.y)) * 0.125;
				float4 c = tex2D(_MainTex, float2(uv.x - _MainTex_TexelSize.x, uv.y)) * 0.125;
				float4 d = tex2D(_MainTex, float2(uv.x, uv.y + _MainTex_TexelSize.y)) * 0.125;
				float4 e = tex2D(_MainTex, float2(uv.x, uv.y - _MainTex_TexelSize.y)) * 0.125;
				float4 f = tex2D(_MainTex, float2(uv.x + _MainTex_TexelSize.x, uv.y + _MainTex_TexelSize.y)) * 0.0625;
				float4 g = tex2D(_MainTex, float2(uv.x - _MainTex_TexelSize.x, uv.y - _MainTex_TexelSize.y)) * 0.0625;
				float4 h = tex2D(_MainTex, float2(uv.x - _MainTex_TexelSize.x, uv.y + _MainTex_TexelSize.y)) * 0.0625;
				float4 i = tex2D(_MainTex, float2(uv.x + _MainTex_TexelSize.x, uv.y - _MainTex_TexelSize.y)) * 0.0625;
				return a + b + c + d + e + f + g + h + i;
			}

			float4 frag(v2f i) : SV_Target
			{
				return blur(i.uv);
			}
			ENDCG
		}
	}
}
