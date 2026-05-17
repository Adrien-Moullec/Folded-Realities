Shader "Custom/Iris"
{
    Properties
    {
        _Radius ("Radius", Range(0,1)) = 1
        _Center ("Center", Vector) = (0.5,0.5,0,0)
        _Color ("Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float4 _Center;
            float4 _Color;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS =
                    TransformObjectToHClip(
                        IN.positionOS.xyz
                    );

                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag (Varyings IN)
                : SV_Target
            {
                float2 uv =
    IN.uv;

uv.x *=
    _ScreenParams.x
    / _ScreenParams.y;

float2 center =
    _Center.xy;

center.x *=
    _ScreenParams.x
    / _ScreenParams.y;

float dist =
    distance(
        uv,
        center
    );
                if (
                    dist < _Radius
                )
                {
                    discard;
                }

                return _Color;
            }

            ENDHLSL
        }
    }
}