#ifndef ADDADDITIONALLIGHTS_INCLUDED
#define ADDADDITIONALLIGHTS_INCLUDED

void AddAdditionalLight_float(float Smoothness, float3 WorldPosition, float3 WorldNormal,
float3 WorldView,float MainDiffuse, float MainSpecular, float3 MainColor,
out float Diffuse, out float Specular, out float3 Color)
{
    Diffuse = MainDiffuse;
    Specular = MainSpecular;
    Color = MainColor * (MainDiffuse + MainSpecular);

    #ifndef SHADERGRAPH_PREVIEW
        int pixelLightCount = GetAdditionalLightsCount();

        for (int i = 0; i < pixelLightCount; ++i) {
            Light light = GetAdditionalLight(i, WorldPosition);
            half NdotL = saturate(dot(WorldNormal, light.direction));
            half atten = light.distanceAttenuation * light.shadowAttenuation;
            half thisDiffuse = atten * NdotL;
            half thisSpecular = LightingSpecular(thisDiffuse, light.direction, WorldNormal, WorldView, 1, Smoothness);
            Diffuse += thisDiffuse;
            Specular += thisSpecular;
            Color += light.color * (thisDiffuse + thisSpecular);
        }
        
    #endif

    half total = Diffuse + Specular;
    Color = total <= 0 ? MainColor : Color / total;
}

#endif // ADDADDITIONALLIGHTS_INCLUDED