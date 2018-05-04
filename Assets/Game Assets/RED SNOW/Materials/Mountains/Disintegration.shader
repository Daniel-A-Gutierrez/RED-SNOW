Shader "Sprites/Disintegration"
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
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float noise = tex2D(_NoiseTex, float2(i.texcoord.x, i.texcoord.y - 5. * _Time.x)).r;
				fixed4 color = fixed4(.8, 1., 1., 1.);
				return color * clamp(i.texcoord.y, 1., noise);
			}
			ENDCG
		}
	}
}
