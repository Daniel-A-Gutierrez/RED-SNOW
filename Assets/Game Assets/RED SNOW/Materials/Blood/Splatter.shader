Shader "2D/Splatter"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Seed ("Random Seed", Range(0,1)) = 0.
		_StartTime("Start Time", Float) = 0.
	}
	SubShader
	{
		Tags 
		{ 
			"Queue" = "Transparent"
			"RenderType"="Transparent" 
		}

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
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

			sampler2D _MainTex, _NoiseTex;
			float _Seed, _StartTime;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.b > .5)
					col = fixed4(0.,0.,0.,0.);
				float thres = .9 - .9 * clamp((_Time.x - _StartTime) * 10., 0., 1.); 
				float noise = tex2D(_NoiseTex, i.uv + _Seed).r;
				col = noise > thres ? col : fixed4(0., 0., 0., 0.);
				return col;
			}
			ENDCG
		}
	}
}
