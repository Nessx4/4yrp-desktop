Shader "4gam-pat/GridGuide"
{
	Properties
	{
		_MainTex ("Image Texture", 2D) = "white" {}
		_GridTex("Grid Texture", 2D) = "white" {}
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
			
			uniform sampler2D _MainTex;
			uniform sampler2D _GridTex;

			uniform float4 _OffsetAndScale;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 img = tex2D(_MainTex, i.uv);

				float2 offset = _OffsetAndScale.xy;
				float2 scale = _OffsetAndScale.zw;

				fixed4 grd = tex2D(_GridTex, i.uv * scale + offset);
				return img * grd;
				//return fixed4(offset, 0, 1);
			}
			ENDCG
		}
	}
}
