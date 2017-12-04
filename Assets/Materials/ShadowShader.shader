// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/ShadowShader" 
{
	Properties
	{
		_ShadowTexture("_ShadowTexture", 2D) = "black" {}
		_RandomTexture("_RandomTexture", 2D) = "white" {}
        _DiffuseMap("_DiffuseMap", 2D) = "white" {}
        _NormalsMap("_NormalsMap", 2D) = "white" {}
        _LightDir("_LightDir", Vector) = (0.0, 1.0, 0.0, 0.0)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 projCoord : TEXCOORD0;
				float4 zCoord : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float3 lightDir: TEXCOORD5;
                float2 uv : TEXCOORD4;
			};

			sampler2D _ShadowTexture;
            sampler2D _RandomTexture;
            sampler2D _DiffuseMap;
            sampler2D _NormalsMap;
			float4x4 _ShadowViewMatrix;
			float4x4 _ShadowProjectionMatrix;
			float4x4 _ShadowBiasMatrix;
			float _LightSize;
			float _Offset;
			float _Bias;
			float _NoiseScale;
			float _ShadowSize;
			int _FilterWidth;
            float4 _LightDir;

			v2f vert(appdata v)
			{
				float4x4 projectionMatrix = mul(_ShadowBiasMatrix, mul(_ShadowProjectionMatrix, _ShadowViewMatrix));

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
				o.projCoord = mul(projectionMatrix, mul(unity_ObjectToWorld, v.vertex));
				o.zCoord = mul(_ShadowViewMatrix, mul(unity_ObjectToWorld, v.vertex));
                o.lightDir = _LightDir.xyz;
				return o;
			}
			
			float EstimatePenumbraSize(float receiver, float2 projCoord)
			{
				float avgDepth = 0.0;
				float numBlockers = 0.0;
				for (int i = -_FilterWidth; i <= _FilterWidth; i++)
				{
					for (int j = -_FilterWidth; j <= _FilterWidth; j++)
					{
						float2 offset = float2(i, j) * _LightSize * _Offset;
						float depthSample = tex2D(_ShadowTexture, projCoord + offset).x;
						if (depthSample < receiver)
						{
							avgDepth += depthSample;
							numBlockers += 1.0;
						}
					}
				}

				avgDepth /= numBlockers;

				return max((receiver - avgDepth) * _LightSize / avgDepth, 0.0);
			}
			
			float Filter(float receiver, float2 projCoord, float penumbraSize, float3 randomizer)
			{
				float shadow = 0.0;
				float shadowSamples = 0.0;

				for (int i = -_FilterWidth; i <= _FilterWidth; i++)
				{
					for (int j = -_FilterWidth; j <= _FilterWidth; j++)
					{
						float2 offset = float2(i, j) * penumbraSize * _Offset;
						float3 rand = (tex2D(_RandomTexture, randomizer.yz + float2(i, j) * randomizer.xy) * 2.0 - 1.0) * penumbraSize * _Offset * _NoiseScale;
						float depthSample = tex2D(_ShadowTexture, projCoord + offset + rand.xy).x;
						shadow += depthSample < receiver ? 0.0 : 1.0;
						shadowSamples += 1.0;
					}
				}

				return shadow / shadowSamples;
			}
			
			float4 frag(v2f i) : SV_Target
			{
                float3 normal = normalize(i.normal);

                float3 lightDir = normalize(i.lightDir);

                float specular = pow(max(dot(reflect(lightDir, normal), normal), 0.0), 16.0);
                float light = max(dot(normal, -lightDir), 0.0f) + specular * 4.0f;
                
                float3 diffuse = tex2D(_DiffuseMap, i.uv).xyz;
                
				float lightDepth = -i.zCoord.z;
				float2 projCoord = i.projCoord.xy / i.projCoord.w;

                float l = 1.0 - pow(length(projCoord * 2.0 - 1.0), 5.0);
                l = clamp(l, 0.0, 1.0);

				float tc0 = clamp(frac(projCoord.x * _ShadowSize), 0.0, 1.0);

				float penumbra = EstimatePenumbraSize(lightDepth - _Bias, projCoord);
				float shadow = Filter(lightDepth - _Bias, projCoord, penumbra, i.vertex.xyz * 0.01);
                shadow *= light;
                shadow *= l;

				return float4(diffuse * shadow, 1.0);
			}
			ENDCG
		}
	}
}