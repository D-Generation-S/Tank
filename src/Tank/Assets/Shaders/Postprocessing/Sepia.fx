#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 sepia1 = float4(0.2, 0.05, 0.0, 1.0);
    float4 sepia2 = float4(1, 0.9, 0.5, 1.0);
    float4 pixelColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float sepiaMix = dot(float3(0.03, 0.59, 0.11), float3(pixelColor.r, pixelColor.g, pixelColor.b));
	
    
    pixelColor = lerp(pixelColor, float4(sepiaMix, sepiaMix, sepiaMix, sepiaMix), float4(0.05, 0.05, 0.05, 0.05));
    float4 sepia = lerp(sepia1, sepia2, sepiaMix);
	
    //return sepia;
    return lerp(pixelColor, sepia, 1.0);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};