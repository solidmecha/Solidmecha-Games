Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_sub ("Sub",Float)=1
	}
	SubShader
	{

ZTest Always 
Cull Off 
ZWrite Off
Fog { Mode off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

					sampler2D _MainTex;
					float4 _MainTex_TexelSize;
					float _sub;

		struct Input {
			float2 uv_MainTex;
		};

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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			


			fixed4 frag (v2f i) : SV_Target
			{
			        fixed2 pix=fixed2(16.0,16.0);
					fixed2 uv = round(i.uv*pix)/pix;
half s = 1.0/pix;
 
float tl = tex2D(_MainTex, uv + fixed2(-s, -s)).r; // Top Left
float cl = tex2D(_MainTex, uv + fixed2(-s, 0)).r; // Centre Left
float bl = tex2D(_MainTex, uv + fixed2(-s, +s)).r; // Bottom Left
 
float tc = tex2D(_MainTex, uv + fixed2(-0, -s)).r; // Top Centre
float cc = tex2D(_MainTex, uv + fixed2(0, 0)).r; // Centre Centre
float bc = tex2D(_MainTex, uv + fixed2(0, +s)).r; // Bottom Centre
 
float tr = tex2D(_MainTex, uv + fixed2(+s, -s)).r; // Top Right
float cr = tex2D(_MainTex, uv + fixed2(+s, 0)).r; // Centre Right
float br = tex2D(_MainTex, uv + fixed2(+s, +s)).r; // Bottom Right

int count = tl + cl + bl + tc + bc + tr + cr + br;
 
if (count < 2 || count > 3)
	return float4(0, 0, 0, 1);
// Life
if (count == 3)
	return float4(1, 1, 1, 1);
// Stay
return cc;
			}
			ENDCG
		}
	}
}
