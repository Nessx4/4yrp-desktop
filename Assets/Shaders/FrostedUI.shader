Shader "UI/FrostedUI"
{
	Properties
	{
		_Color("Color", Color) = (0, 0, 0, 0)
		_Radius ("Radius", Range(0, 255)) = 1
	}

	Category
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }

		SubShader
		{
			GrabPass
			{
				Tags { "LightMode" = "Always" }
			}

			Pass
			{
				Tags { "LightMode" = "Always" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};	

				struct v2f
				{
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif

					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}

				sampler2D _GrabTexture;
				float4 _GrabTexture_TexelSize;
				float _Radius;
				float4 _Color;

				float4 grabPixel(v2f i, float kernelx, float kernely)
				{
					return tex2Dproj(_GrabTexture, 
						UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * 
						kernelx, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)));
				}

				half4 frag(v2f i) : COLOR
				{
					half4 sum = half4(0, 0, 0, 0);

					sum += grabPixel(i, 0.0, 0.0);
					int measurements = 1;

					// Do a Guassian blur. 
					for(float range = 0.1f; range <= _Radius; range += 0.1f)
					{
						sum += grabPixel(i, range, range);
						sum += grabPixel(i, -range, range);
						sum += grabPixel(i, range, -range);
						sum += grabPixel(i, -range, -range);
						measurements += 4;
					}

					half4 col = sum / measurements;
					col = lerp(col, _Color, _Color.a);
					return col;
				}

				ENDCG
			}

			GrabPass
			{
				Tags { "LightMode" = "Always" }
			}

			Pass
			{
				Tags { "LightMode" = "Always" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};	

				struct v2f
				{
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif

					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}

				sampler2D _GrabTexture;
				float4 _GrabTexture_TexelSize;
				float _Radius;

				float4 grabPixel(v2f i, float kernelx, float kernely)
				{
					return tex2Dproj(_GrabTexture,
						UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x *
							kernelx, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)));
				}

				half4 frag(v2f i) : COLOR
				{
					half4 sum = half4(0, 0, 0, 0);
					float radius = 1.41421356237f * _Radius;

					sum += grabPixel(i, 0.0, 0.0);
					int measurements = 1;

					// Do a Guassian blur with 
					for(float range = 1.41421356237f; range <= _Radius * 1.41; range += 1.41421356237f)
					{
						sum += grabPixel(i, range, range);
						sum += grabPixel(i, -range, range);
						sum += grabPixel(i, range, -range);
						sum += grabPixel(i, -range, -range);
						measurements += 4;
					}

					return sum / measurements;
				}

				ENDCG
			}
		}
	}
}
