Shader "4gam-pat/EditorGridGuide"
{
	Properties
	{
		_MainTex ("Image Texture", 2D) = "white" {}
		_GridTex ("Grid Texture", 2D)  = "white" {}
	}
	SubShader
	{
		// This is a post-process shader - don't cull or modify z-buffer.
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

			uniform sampler2D _MainTex;
			uniform sampler2D _GridTex;
			uniform float4 _OffsetAndScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Sample the screen buffer.
				fixed4 screen = tex2D(_MainTex, i.uv);

				float2 offset = _OffsetAndScale.xy;
				float2 scale  = _OffsetAndScale.zw;

				// Sample the grid based on camera position and ortho size.
				fixed4 grid   = tex2D(_GridTex, i.uv * scale + offset);
				return grid * screen;
			}
			ENDCG
		}
	}
}
