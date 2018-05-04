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
				float noise = clamp(tex2D(_NoiseTex, float2(i.texcoord.x * .0025 - _Time.x * .01, .5).r), .5, 1.);
				fixed4 col = i.texcoord.y < noise ? fixed4(1., 1., 1., 1.) : fixed4(0., 0., 0., 0.);
				if((noise - i.texcoord.y) / i.texcoord.y < .8)
					col *= .5;
				return col;
			}
			ENDCG
		}
	}
}
