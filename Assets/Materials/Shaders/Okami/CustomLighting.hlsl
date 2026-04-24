#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile _ _SHADOWS_SOFT

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1,1,0));
    Color = 1;
    Attenuation = 1;
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);

    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
#endif
}

void MainLight_half(half3 WorldPos, out half3 Direction, out half3 Color, out half Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(half3(1,1,0));
    Color = 1;
    Attenuation = 1;
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);

    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
#endif
}

void AdditionalLight_float(float3 WorldPos, int lightID, out float3 Direction, out float3 Color, out float Attenuation)
{
    Direction = normalize(float3(1,1,0));
    Color = 0;
    Attenuation = 0;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    if (lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos, 1);

        Direction = light.direction;
        Color = light.color;
        Attenuation = light.distanceAttenuation * light.shadowAttenuation;
    }
#endif
}

void AdditionalLight_half(half3 WorldPos, int lightID, out half3 Direction, out half3 Color, out half Attenuation)
{
    Direction = normalize(half3(1,1,0));
    Color = 0;
    Attenuation = 0;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    if (lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos, 1);

        Direction = light.direction;
        Color = light.color;
        Attenuation = light.distanceAttenuation * light.shadowAttenuation;
    }
#endif
}

void AllAdditionalLights_float(
    float3 WorldPos,
    float3 WorldNormal,
    float2 CutoffThresholds,
    out float3 LightColor)
{
    LightColor = 0;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    for (int i = 0; i < lightCount; i++)
    {
        Light light = GetAdditionalLight(i, WorldPos, 1);
        float NdotL = dot(light.direction, WorldNormal);

        // Hard light band
        float lightStep = step(CutoffThresholds.x, NdotL);

        // Proper shadow usage
        float shadow = light.shadowAttenuation;

        float attenuation = light.distanceAttenuation;

        float3 color = lightStep * shadow * light.color * attenuation;

        LightColor += color;
    }
#endif
}

void AllAdditionalLights_half(
    half3 WorldPos,
    half3 WorldNormal,
    half2 CutoffThresholds,
    out half3 LightColor)
{
    LightColor = 0;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    for (int i = 0; i < lightCount; i++)
    {
        Light light = GetAdditionalLight(i, WorldPos, 1);

        half NdotL = dot(light.direction, WorldNormal);

        half intensity = smoothstep(
            CutoffThresholds.x,
            CutoffThresholds.y,
            NdotL
        );

        half attenuation = light.distanceAttenuation * light.shadowAttenuation;

        half3 color = intensity * light.color * attenuation;

        LightColor += color;
    }
#endif
}

#endif