void GaussianBlur_float(UnityTexture2D Texture, float2 UV, float Blur, UnitySamplerState Sampler, out float3 OutRGB, out float OutAlpha)
{
    float weights[9] = {
        0.0625, 0.125, 0.0625,
        0.125,  0.25,  0.125,
        0.0625, 0.125, 0.0625
    };
    float4 col = float4(0.0f, 0.0f, 0.0f, 0.0f);
    float kernelSum = 0.0f;
    
    int index = 0;
    for (int x = -1; x <= 1; ++x)
    {
        for (int y = -1; y <= 1; ++y)
        {
            float2 offset = float2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
            col += Texture.Sample(Sampler, UV + offset) * weights[index];
            kernelSum += weights[index];
            index++;
        }
    }
    
    col /= kernelSum;
    OutRGB = float3(col.r, col.g, col.b);
    OutAlpha = col.a;
}