Shader "Hidden/GlobalOutline"
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

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			float _OutlineSize;
			float4 _OutlineColor;
			
			float sobel(sampler2D tex, float2 uv) {
				float2 delta = _MainTex_TexelSize * _OutlineSize;
				float4 hr = float4(0, 0, 0, 0);
				float4 vt = float4(0, 0, 0, 0);
				hr += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) * -1.0;
				hr += tex2D(tex, (uv + float2(-1.0, 0.0) * delta)) *  2.0;
				hr += tex2D(tex, (uv + float2(0.0, 0.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 0.0) * delta)) * -2.0;
				hr += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2(0.0, 1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;
				vt += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  2.0;
				vt += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2(-1.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(0.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(1.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) * -1.0;
				vt += tex2D(tex, (uv + float2(0.0, 1.0) * delta)) * -2.0;
				vt += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;
				return sqrt(hr * hr + vt * vt);
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 fragment = tex2D(_MainTex, i.uv);
				float alpha;
				if (fragment.a > 0.0) {
					alpha = _OutlineColor.a * sobel(_MainTex, i.uv);
				}
				else {
					alpha = 0.0;
				}
				return float4(_OutlineColor.xyz, alpha);
			}
			ENDCG
		}
	}
}
