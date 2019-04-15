Shader "Hidden/GlobalOutlineReplacement"
{
	Properties
	{

	}
	SubShader
	{
		Tags{ "RenderType" = "Overlay" }
		Cull Off ZWrite Off ZTest Off
		Blend One One
		Pass
		{
			Stencil
			{
				Ref 128
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			int _GlobalOutline;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			float alphaClip(float4 col) {
				if (col.a > 0.1) {
					return 1.0;
				}
				return 0.0;
			}

			void frag(v2f i, out float4 color: SV_TARGET)
			{
				float4 fragment = tex2D(_MainTex, i.uv);
				float alpha = alphaClip(fragment);
				color = alpha * _GlobalOutline;
			}
			ENDCG
		}
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		Cull Off ZWrite On ZTest LEqual
		Blend One One
		Pass
		{
			Stencil
			{
				Ref 128
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float depth : TEXCOORD1;
			};

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			int _GlobalOutline;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.depth = o.pos.z / o.pos.w;
				return o;
			}

			float alphaClip(float4 col) {
				if (col.a > 0.1) {
					return 1.0;
				}
				return 0.0;
			}

			void frag(v2f i, out float4 color: SV_TARGET, out float depth : SV_DEPTH)
			{
				float4 fragment = tex2D(_MainTex, i.uv);
				float alpha = alphaClip(fragment);
				color = alpha * _GlobalOutline;
				depth.r = alpha * i.depth;
			}
			ENDCG
		}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Cull Off ZWrite On ZTest LEqual
		Blend One One
		Pass
		{
			Stencil
			{
				Ref 128
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float depth : TEXCOORD1;
			};

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			int _GlobalOutline;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.depth = o.pos.z / o.pos.w;
				return o;
			}

			float alphaClip(float4 col) {
				if (col.a > 0.1) {
					return 1.0;
				}
				return 0.0;
			}

			void frag(v2f i, out float4 color: SV_TARGET, out float depth : SV_DEPTH)
			{
				float4 fragment = tex2D(_MainTex, i.uv);
				float alpha = alphaClip(fragment);
				color = alpha * _GlobalOutline;
				depth.r = alpha * i.depth;
			}
			ENDCG
		}
	}
}
