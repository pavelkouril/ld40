// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/AtmosphericScattering"
{
/*

atmosphere =
Kr				: 0.0025
Km				: 0.0010
ESun			: 20.0
g				: -0.950
innerRadius 	: 100
outerRadius		: 102.5
wavelength		: [0.650, 0.570, 0.475]
scaleDepth		: 0.25
mieScaleDepth	: 0.1

*/

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _CameraPosition("CameraPosition", Vector) = (1.0, 0.0, 0.0, 0.0)       // Camera position
        _LightDirection("LightDirection", Vector) = (1.0, 0.0, 0.0, 0.0)       // The direction vector to the light source

        _AtmoRadius("AtmosphereRadius", Float) = 1.01
        _EarthCenter("Center", Vector) = (0.0, 0.0, 0.0, 0.0)
        _EarthRadius("Radius", Float) = 1.0

        _NUM_DENSITY_SAMPLES("Density Samples", Float) = 8.0
        _NUM_VIEW_SAMPLES("View Samples", Float) = 8.0
        _NUM_DENSITY_SAMPLES_INT ("Density Samples Int", Int) = 8
        _NUM_VIEW_SAMPLES_INT ("View Samples Int", Int) = 8
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
                float3 vertexWorld : TEXCOORD1;
                //float3 direction : TEXCOORD1;
                //float3 c0 : TEXCOORD2;
                //float3 c1 : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            
            float4 _CameraPosition;
            float4 _LightDirection;

            float _AtmoRadius;
            float4 _EarthCenter;
            float _EarthRadius;
            float _NUM_DENSITY_SAMPLES;
            float _NUM_VIEW_SAMPLES;
            int _NUM_DENSITY_SAMPLES_INT;
            int _NUM_VIEW_SAMPLES_INT;
            
            // Intersect sphere
            float2 iSphere(float3 ro, float3 rd, float4 sph)
            {
                float3 tmp = ro - sph.xyz;

                float b = dot(rd, tmp);
                float c = dot(tmp, tmp) - sph.w * sph.w;

                float disc = b * b - c;

                if (disc < 0.0) return float2(-1e9, -1e9);

                float disc_sqrt = sqrt(disc);

                float t0 = -b - disc_sqrt;
                float t1 = -b + disc_sqrt;

                return float2(t0, t1);
            }

            // Henyey-Greenstein phase function
            float phase(float nu, float g)
            {
                return (3.0f * (1.0f - g * g) * (1.0f + nu * nu)) / (2.0f * (2.0f + g * g) * pow(1.0f + g * g - 2.0f * g * nu, 1.5f));
            }

            // density integral calculation from p0 to p1 
            // for mie and rayleight
            float2 densityOverPath(float3 p0, float3 p1, float2 prescaler)
            {
                float l = length(p1 - p0);
                float3 v = (p1 - p0) / l;

                l /= _NUM_DENSITY_SAMPLES;

                float2 density = float2(0.0f, 0.0f);
                float t = 0.0;

                for (int i = 0; i < _NUM_DENSITY_SAMPLES_INT; i++)
                {
                    float3 sp = p0 + v * (t + 0.5 * l);
                    float h = float(length(sp) - _EarthRadius);
                    density += exp(-h / prescaler);

                    t += l;
                }

                return l * density;
            }

            ///////////////////////////////////////
            // inscatter integral calculation
            float4 inscatter(float3 cam, float3 v, float3 sun)
            {
                float sunIntensity = 60.0;

                float3 betaR = float3(5.8e-4, 1.35e-3, 3.31e-3);
                float3 betaM = float3(4.0e-3, 4.0e-3, 4.0e-3);

                float3 M_4PIbetaR = 4.0f * 3.1415926535f * float3(5.8e-4, 1.35e-3, 3.31e-3);
                float3 M_4PIbetaM = 4.0f * 3.1415926535f * float3(4.0e-3, 4.0e-3, 4.0e-3);

                float heightScaleRayleight = 0.06;
                float heightScaleMie = 0.024;
                //float heightScaleRayleight = 6.0;
                //float heightScaleMie = 1.2;
                float g = -0.76;

                float4 atmoSphere = float4(_EarthCenter.xyz, _AtmoRadius);
                float4 earthSphere = float4(_EarthCenter.xyz, _EarthRadius);

                float2 t0 = iSphere(cam, v, atmoSphere);
                float2 t1 = iSphere(cam, v, earthSphere);

                //return float4(t0.x, t0.y, 0.0f, 1.0f);

                bool bNoPlanetIntersection = t1.x < 0.0 && t1.y < 0.0;

                float farPoint = bNoPlanetIntersection ? t0.y : t1.x;
                float nearPoint = t0.x > 0.0 ? t0.x : 0.0;

                float l = (farPoint - nearPoint) / _NUM_VIEW_SAMPLES;
                cam += nearPoint * v;

                float t = 0.0;

                float3 rayleight = float3(0.0f, 0.0f, 0.0f);
                float3 mie = float3(0.0f, 0.0f, 0.0f);

                float2 prescalers = float2(heightScaleRayleight, heightScaleMie);

                float2 densityPointToCam = float2(0.0f, 0.0f);

                for (int i = 0; i < _NUM_VIEW_SAMPLES_INT; i++)
                {
                    float3 sp = cam + v * (t + 0.5 * l);
                    float tc = iSphere(sp, sun, float4(_EarthCenter.xyz, _AtmoRadius)).y;

                    float3 pc = sp + tc * sun;

                    float2 densitySPCam = densityOverPath(sp, cam, prescalers);
                    float2 densities = densityOverPath(sp, pc, prescalers) + densitySPCam;
                    
                    float h = length(sp) - _EarthRadius;
                    float2 expRM = exp(-h / prescalers);

                    rayleight += expRM.x * exp(-M_4PIbetaR * densities.x);
                    mie += expRM.y * exp(-M_4PIbetaM * densities.y);

                    densityPointToCam += densitySPCam;

                    t += l;
                }

                rayleight *= l;
                mie *= l;

                float3 extinction = exp(-(M_4PIbetaR * densityPointToCam.x + M_4PIbetaM * densityPointToCam.y));

                float nu = dot(sun, -v);
                
                float3 inscatter_ = sunIntensity * (betaM * mie * phase(nu, g) + betaR * phase(nu, 0.0) * rayleight);
                return float4(inscatter_, extinction.r * float(bNoPlanetIntersection));
            }

			v2f vert (appdata v)
			{
                v2f o;

                o.vertexWorld = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0f)).xyz;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				
                return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
                float3 v3Ray = normalize(i.vertexWorld.xyz - _CameraPosition.xyz);

                float3 sun = normalize(_LightDirection.xyz);

                float4 col = inscatter(_CameraPosition.xyz, v3Ray, sun);

                if (col.w == 0.0)
                {
                    col.w = 1.0;
                }

                return float4(col.xyz, 1.0 - col.w);
			}
			ENDCG

            //Cull Front
		}
	}
}
