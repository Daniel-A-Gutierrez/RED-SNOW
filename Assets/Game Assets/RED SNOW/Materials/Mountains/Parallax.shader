Shader "Sprites/Parallax"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
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

			float4 _MainTex_ST;
			float _Order;
			float _PlayerSpeed;
			sampler2D _MainTex;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				v.uv.y = 1. - v.uv.y;
				o.texcoord = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 uv = float2(i.texcoord.x - _Time.x / (_Order * 10), i.texcoord.y);
				fixed4 col = SampleSpriteTexture(uv);
				return fixed4(col.a * col.rgb / _Order, col.a);
			}
			ENDCG
		}
	}
}
