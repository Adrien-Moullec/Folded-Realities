Shader "Custom/ShellTexturing_URP"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _BaseMap ("Base Map", 2D) = "white" {}
        _Noise("Noise", 2D) = "gray" {}

        _StrandDensity ("Strand Density", Float) = 1
        _StrandHeight ("Strand Height", Float) = 0
        _BaseThickness ("Base Thickness", Range(0,1)) = 1
        _TipThickness ("Tip Thickness", Range(0,1)) = 0
        _BaseDarkness ("Darkness", Range(0,1)) = 1
        _SwayPower ("Sway Power", Range(0.1,5)) = 1
        _SwayAmount ("Sway Amount", Range(0,50)) = 1
        _GravityPower ("Gravity", Range(0,50)) = 1
        _RandomHairDisplacement ("Random Hair Displacement", Range(0,0.5)) = 0.5

        [Toggle] _AlphaClip ("Alpha Clipping", Float) = 1
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "TransparentCutout" "Queue" = "AlphaTest" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            ZWrite On

            HLSLPROGRAM

            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_Noise);
            SAMPLER(sampler_Noise);

            float4 _BaseColor;

            float _Cutoff;
            float _StrandDensity;
            float _StrandHeight;
            float _TipThickness;
            float _BaseThickness;
            float _BaseDarkness;
            float _AlphaClip;
            float _SwayPower;
            float _SwayAmount;
            float _GravityPower;
            float _RandomHairDisplacement;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float RandomNumber(float Min, float Max, float2 Seed)
            {
                return lerp(
                Min,
                Max,
                frac(sin(dot(Seed, float2(12.9898, 78.233))) * 43758.5453)
                );
            }

            float2 HairCentre(float Min, float Max, float2 Seed1, float2 Seed2)
            {
                return float2(
                RandomNumber(Min, Max, Seed1),
                RandomNumber(Min, Max, Seed2)
                );
            }

            Varyings Vert(Attributes v)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(v);

                float3 worldPos = TransformObjectToWorld(v.positionOS);
                float3 normalWS = TransformObjectToWorldNormal(v.normalOS);

                worldPos += normalWS * _StrandHeight * _Cutoff;

                float3 up = float3(0,1,0);

                if (abs(dot(normalWS, up)) > 0.99)
                up = float3(1,0,0);

                float3 direction1 = normalize(cross(normalWS, up));
                float3 direction2 = normalize(cross(normalWS, direction1));

                float time = _Time.y * _SwayPower;

                float swayX = sin(time + dot(worldPos.xz, float2(10,10)));
                float swayY = cos(time + dot(worldPos.xz, float2(10,10)));

                float3 totalSway =
                (direction1 * swayX + direction2 * swayY)
                * _SwayAmount;

                float3 gravity =
                float3(0, -1, 0)
                * _Cutoff
                * _Cutoff
                * _SwayAmount
                * _GravityPower;

                worldPos += (totalSway + gravity) * _Cutoff;

                OUT.positionCS = TransformWorldToHClip(worldPos);
                OUT.uv = v.uv;

                return OUT;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                if (_AlphaClip > 0.5)
                {
                    float2 scaledUV = i.uv * _StrandDensity;

                    // Integer strand cell
                    float2 cellID = floor(scaledUV);

                    // Local UV inside the cell
                    float2 localUV = frac(scaledUV);

                    // Stable random center per strand
                    float2 centre = HairCentre(
                    0.5 - _RandomHairDisplacement,
                    0.5 + _RandomHairDisplacement,
                    cellID,
                    cellID + float2(5.2, 1.3)
                    );

                    float circleMap = distance(localUV, centre);

                    float thickness =
                    lerp(_BaseThickness, _TipThickness, _Cutoff);

                    if (circleMap > (thickness * 0.5))
                    discard;
                }

                float4 tex =
                SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv)
                * lerp(_BaseDarkness, 1, _Cutoff);

                return saturate(tex * _BaseColor);
            }

            ENDHLSL
        }
    }
}