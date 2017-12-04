// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthShader" 
{
	Properties
	{
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 position : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.position = mul(UNITY_MATRIX_MV, v.vertex);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float depth = -i.position.z;
				return float4(depth, 0.0, 0.0, 1.0);
			}
			ENDCG
		}
	}
}
