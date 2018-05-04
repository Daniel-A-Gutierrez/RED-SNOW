Shader "Sprites/ProcMt"
{
	Properties
	{
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Order ("Order", Int) = 1
	}
	SubShader
	{
		Tags { 
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"CanUseSpriteAtlas"="False"
			"PreviewType"="Plane" 
		}

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
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
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			float4 _NoiseTex_ST;
			float _Order;
			float _PlayerSpeed;
			sampler2D _NoiseTex;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.texcoord.y = 1. - o.texcoord.y;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float noise_low = clamp(tex2D(_NoiseTex, float2(i.texcoord.x * .0025 - _Time.x * .01, .5).r), .5, 1.);
				float noise_mid = .5 * clamp(tex2D(_NoiseTex, float2(i.texcoord.x * .025 - _Time.x * .1, .5).r), .5, 1.);
				float noise_high = .05 * clamp(tex2D(_NoiseTex, float2(i.texcoord.x * .25 - _Time.x, .5).r), .5, 1.);
				float noise = (noise_low + noise_mid + noise_high) * .7;
				fixed4 col = fixed4(0., 0., 0., 0.);
				float ipol = smoothstep(i.texcoord.y / noise, 0., .5);
				return pow(lerp(col, ipol, .6), .5);
			}
			ENDCG
		}
	}
}
