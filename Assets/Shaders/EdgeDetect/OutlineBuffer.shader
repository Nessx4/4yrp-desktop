Shader "EdgeDetect/OutlineBuffer"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] _PixelSnap ("Pixel snap", Float) = 0
	}
	SubShader
	{
		Tags 
		{ 
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True" 
		}
		Cull [_Culling]
		Lighting Off

		//Pass
		//{
			CGPROGRAM
			
			#pragma surface surf Lambert vertex:vert nofog noshadow noambient nolightmap novertexlights noshadowmask nometa
			#pragma multi_compile _ _PIXELSNAP_ON
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;
			float _OutlineAlphaCutoff;

			struct Input
			{
				float2 uv_MainTex;
			};
			
			void vert(inout appdata_full v, out Input o)
			{
				#if defined(_PIXELSNAP_ON)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif

				UNITY_INITIALISE_OUTPUT(Input, o);
			}

			void surf(Input in, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, in.uv_MainTex);

				if(c.a < _outlineAlphaCutoff) discard;

				float alpha = c.a * 99999999;

				o.Albedo = _Color * alpha;
				o.Alpha = alpha;
				o.Emission = o.Albedo;
			}

			ENDCG
		//}
	}

	Fallback "Transparent/VertexLit"
}
