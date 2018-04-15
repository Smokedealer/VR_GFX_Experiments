﻿Shader "Hidden/CustomImageShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Red ("Red part", Range(0,1)) = 0
		_Green ("Green part", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
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
            float _Red;
            float _Green;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				color.r = _Red;
				color.g = _Green;
                return color;
			}
			ENDCG
		}
	}
}
