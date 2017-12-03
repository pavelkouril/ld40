Shader "Hidden/Custom/AtmosphericScattering"
{
    HLSLINCLUDE

    #include "../../PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    float _Blend;

    float4 _CameraPosition;
    float4 _CameraForward;
    float4 _CameraUp;
    float4 _CameraRight;
    float4 _LightDirection;
    float _Width;
    float _Height;
    float _Fov;
    float _AtmoRadius;
    float4 _EarthCenter;
    float _EarthRadius;
    float _NUM_DENSITY_SAMPLES;
    float _NUM_VIEW_SAMPLES;
    float _Editor;

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

        for (float i = 0.0f; i < _NUM_DENSITY_SAMPLES; i += 1.0f)
        {
            float3 sp = p0 + v * (t + 0.5 * l);
            float h = float(length(sp) - _EarthRadius);
            density += exp(-h / prescaler);

            t += l;
        }

        return l * density;
    }

    // inscatter integral calculation
    float4 inscatter(float3 cam, float3 v, float3 sun)
    {
        float sunIntensity = 120.0;

        float3 betaR = float3(5.8e-4, 1.35e-3, 3.31e-3);
        float3 betaM = float3(4.0e-3, 4.0e-3, 4.0e-3);

        float3 M_4PIbetaR = 4.0f * 3.1415926535f * float3(5.8e-4, 1.35e-3, 3.31e-3);
        float3 M_4PIbetaM = 4.0f * 3.1415926535f * float3(4.0e-3, 4.0e-3, 4.0e-3);

        float heightScaleRayleight = 0.06;
        float heightScaleMie = 0.012;
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

        float4 tmp = float4(0.0f, 0.0f, 0.0f, 1.0f);

        for (float i = 0.0f; i < _NUM_VIEW_SAMPLES; i += 1.0f)
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

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        if (_Editor == 0.0f)
        {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        }
        else
        {
            float3 ro = _CameraPosition.xyz;

            float2 coord = i.texcoord.xy;

            float imageAspectRatio = _Width / _Height;
            float Px = (2.0f * (((coord.x * _Width) + 0.5f) / _Width) - 1.0f) * tan(_Fov / 2.0f * 3.1415f / 180.0f) * imageAspectRatio;
            float Py = (1.0f - 2.0f * (((coord.y * _Height) + 0.5f) / _Height)) * tan(_Fov / 2.0f * 3.1415f / 180.0f);
            float3 tmp = normalize(float3(Px, Py, -1) - float3(0.0f, 0.0f, 0.0f));
            float3 rd = tmp.x * _CameraRight - tmp.y * _CameraUp - tmp.z * _CameraForward;

            float3 sun = normalize(-_LightDirection.xyz);

            float4 col = inscatter(ro, rd, sun);
            col.x = max(col.x, 0.0f);
            col.y = max(col.y, 0.0f);
            col.z = max(col.z, 0.0f);

            return float4(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz + col.xyz, 1.0f);

            //return float4(col.w, col.w, col.w, col.w);

            /*if (col.w > 0.0f)
            {
                float lum = 0.299f * col.x + 0.587f * col.y + 0.114f * col.z;
                //return float4(lum, lum, lum, 1.0f);
                return float4(lerp(col.xyz, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz, lum), 1.0f);
            }
            else
            {
                float lum = 0.299f * col.x + 0.587f * col.y + 0.114f * col.z;
                lum *= 32.0f;
                lum = clamp(lum, 0.0f, 1.0f);
                return float4(lum * col.xyz + (1.0f - lum) * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz, 1.0f);
            }*/
        }

        /*if (col.w > 0.0f)
        {
            return float4(col.xyz, 1.0f);
        }
        else
        {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        }*/

        /*float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
        color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
        return color;*/
    }

        ENDHLSL

        SubShader
    {
        Cull Off ZWrite Off ZTest Always

            Pass
        {
            HLSLPROGRAM

#pragma vertex VertDefault
#pragma fragment Frag

            ENDHLSL
        }
    }
}
