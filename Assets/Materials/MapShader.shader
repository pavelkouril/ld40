// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/MapShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Unpack("Unpack", Range(0.0, 1.0)) = 1.0
        _AspectX("AspectX", Float) = 1.0
        _AspectY("AspectY", Float) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

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
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Unpack;
                float _AspectX;
                float _AspectY;

                float4 Slerp(float4 p0, float4 p1, float t)
                {
                    float dotp = dot(normalize(p0), normalize(p1));
                    if ((dotp > 0.9999) || (dotp < -0.9999))
                    {
                        if (t <= 0.5)
                            return p0;
                        return p1;
                    }
                    float theta = acos(dotp * 3.14159f / 180.0f);
                    float4 P = ((p0*sin((1 - t)*theta) + p1*sin(t*theta)) / sin(theta));
                    P.w = 1.0f;
                    return P;
                }

                v2f vert(appdata v)
                {
                    v2f o;

                    float4 vertexSphere = v.vertex;
                    float4 vertexUV = float4(_AspectX * (v.uv.x * 2.0f - 1.0f), 2.0f, _AspectY * (v.uv.y * 2.0f - 1.0f), 1.0f);

                    float4 vertex = Slerp(vertexSphere, vertexUV, _Unpack);

                    o.vertex = UnityObjectToClipPos(vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG

            Cull Back
        }
        }
}
