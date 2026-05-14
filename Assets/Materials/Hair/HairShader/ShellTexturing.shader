Shader "Custom/ShellTexturing"
{
    Properties
    {
        /// Base Maps
        _BaseColor ("Color", Color) = (1,1,1,1)
        _BaseMap ("Base Map", 2D) = "white" {}
        
        ///Strand Settings
        _StrandHeight ("Strand Height", float) = 0
        _StrandDensity ("Strand Density", float) = 0
        _BaseThickness ("Base Thickness", Range(0,1)) = 1
        _TipThickness ("Tip Thickness", Range(0,1)) = 0
        _BaseDarkness ("Darkness", Range(0,1)) = 1

        ///Physics
        _SwayPower ("Sway Power", Range(0.1,5)) = 0
        _SwayAmount ("Sway Amount", float) = 0
        _GravityPower ("Gravity", Range(0,50)) = 0
        _RandomHairDisplacement ("Random Hair Displacement", Range(0,0.5)) = 0.5

        //Base Values
        [Toggle] _AlphaClip ("Alpha Clipping", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="HDRenderPipeline"
            "RenderType"="Transparent"
            "Queue"="AlphaTest"
        }
        Pass {
            
            Name "ForwardUnlit"
            //Tags { "LightMode"="ForwardOnly" }
            Cull Off
            ZWrite On

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma shader_feature_local _ALPHATEST_ON

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

            ///Base Maps
            sampler2D _BaseMap;
            float4 _BaseColor;

            ///Strand Settings
            float _StrandDensity;
            float _StrandHeight;
            float _TipThickness;
            float _BaseThickness;
            float _BaseDarkness;

            ///Physics
            float _SwayPower;
            float _SwayAmount;
            float _GravityPower;
            float _RandomHairDisplacement;
            
            ///Base Values
            float _AlphaClip;
            float _Cutoff;

            struct appdata
            {
                float3 positionOS : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            //Get a random number - This function was taken from the official unity documentation for random number node equivilent code in HLSL
            float RandomNumber(float Min, float Max, float2 Seed) {
                return lerp(Min, Max, frac(sin(dot(Seed, float2(12.9898, 78.233)))*43758.5453));
            }

            //Get a Random Float2
            float2 RandomHairCentre(float Min, float Max, float2 Seed1, float2 Seed2) {
                return float2(RandomNumber(Min, Max, Seed1),RandomNumber(Min, Max, Seed2));
            }

            v2f Vert(appdata v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                
                //World and absolute positions to calculate normals
                float3 worldPos = TransformObjectToWorld(v.positionOS);
                float3 absoluteWS = GetAbsolutePositionWS(worldPos);
                absoluteWS += v.normal * _StrandHeight;

                //Get perpendicular direction
                float3 up = float3(0,1,0);
                if (abs(dot(v.normal, up)) > 0.99)
                    up = float3(1,0,0);

                //Base values
                float3 direction1 = lerp(v.normal, normalize(cross(v.normal, up)), _Cutoff);
                float3 direction2 = lerp(v.normal, normalize(cross(v.normal, direction1)), _Cutoff);
                float time = _Time.y * _SwayPower;
            
                //Swinging maths
                float swayX = sin(time + dot(absoluteWS.xz, float2(10,10))) * _Cutoff;
                float swayY = cos(time + dot(absoluteWS.xz, float2(10,10))) * _Cutoff;            
                float3 totalSway = ((direction1 * swayX) + (direction2 * swayY)) * _SwayAmount / 100;
            
                //Final Motion
                float3 gravity = (float3(0, -1, 0) * _Cutoff * _Cutoff * _GravityPower) / 1000;
                absoluteWS += (totalSway + gravity);
            
                //Output
                OUT.positionCS = TransformWorldToHClip(GetCameraRelativePositionWS(absoluteWS));
                OUT.uv0 = v.uv0;
                return OUT;
            }

            float4 Frag(v2f i) : SV_Target
            {
                if (_AlphaClip) {
                    //Base strand values
                    float2 uv = i.uv0 * _StrandDensity; //Tiling and offset
                    float2 id = floor(uv); //Create float2 IDs for each cell
                    float2 fraction = frac(uv); //Get the 0->1 of each cell
                    
                    //Get a map of randomly dispersed circles using each cell ID as a randomness seed
                    float circleMap = distance(fraction, RandomHairCentre(0.5-_RandomHairDisplacement, 0.5+_RandomHairDisplacement, id + float2(1,1), id + float2(2,2)));

                    //Calculate the width of the circle and discard by what the current _Cutoff is and the input thickness of the Tip and Base.
                    float thickness = lerp(_BaseThickness, _TipThickness, _Cutoff) + 0.001;
                    if((circleMap > (thickness * 0.5f)))
                        discard;
                }
                
                //Final texture output
                float4 tex = tex2D( _BaseMap, i.uv0) * lerp(_BaseDarkness,1,_Cutoff);
                return saturate (tex * _BaseColor);
            }
            ENDHLSL
        }
    }
}
