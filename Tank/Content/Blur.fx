#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

sampler TextureSampler : register(s0);
float2 MousePosition = { 0.0, 0.0 };
float GameTime = 0;

float4 Pixelate(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float PixelSize = 10.;
	float dx = PixelSize*(1. / 512.);
	float dy = PixelSize*(1. / 512.);
	float2 coord = float2(dx*floor(texCoord.x / dx), dy*floor(texCoord.y / dy));

	return tex2D(TextureSampler, coord);
}

float4 RadialBlur(float2 texCoord : TEXCOORD0) : COLOR
{
	float BlurStart = 1.0f;
	float BlurWidth = -0.1;
	int nsamples = 10;
	texCoord -= MousePosition;  float4 c = 0;
	for (int i = 0; i <nsamples; i++)
	{
		float scale = BlurStart + BlurWidth*(i / (float)(nsamples - 1));
		c += tex2D(TextureSampler, texCoord * scale + MousePosition);
	}
	c /= nsamples;
	return c;
}

float4 Druken(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float Gametime = GameTime / 50;
	texCoord.y = texCoord.y + (sin((texCoord.y + Gametime) * 50)*0.01);
	texCoord.x = texCoord.x + (cos((texCoord.x + Gametime) * 50)*0.01);
	tex = tex2D(TextureSampler, texCoord.xy);
	return tex*color;
}

technique BasicColorDrawing
{
	pass P0
	{
		//VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL Druken();
	}

	pass P1
	{
		//VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL Pixelate();
	}
};