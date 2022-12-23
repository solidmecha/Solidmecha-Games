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
			fixed2 _Pixels= fixed2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
					fixed2 uv = round(i.uv * _Pixels);
if(uv.y%2==0)
	return float4(1,0,0,1);
else 
	return float4(0,1,0,1);
			}
			ENDCG
		}
	}
}
