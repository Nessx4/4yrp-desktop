// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/BasicCG"
{
	Properties
	{
		_MainColor ("Main Color", Color)= (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Opaque"
		}
		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct v2f
			{
				float3 worldPos : TEXCOORD0;
				half3 worldNormal : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
			
			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);

				o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;

				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}

			fixed4 _MainColor;
			
			fixed4 frag (v2f i) : SV_Target
			{
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				half3 worldRefl = reflect(-worldViewDir, i.worldNormal);

				half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
				half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);

				fixed4 c = 0;
				c = fixed4(skyColor * 0.5, 1.0) + _MainColor * 0.5;
				c.a = 0.25;
				return c;
			}

			ENDCG
		}
	}
}
