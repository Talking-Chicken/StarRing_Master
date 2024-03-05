float2 GenerateUVs(float3 normal, float4 pos)
{
	float3 n = normalize(mul(unity_ObjectToWorld, normal).xyz);
	float3 vDirection = float3(0, 0, 1);
	if(abs(n.y) < 1.0f)
	{
		vDirection = normalize(float3(0, 1, 0) - n.y * n);
	}
	float3 uDirection = normalize(cross(n, vDirection));
	float3 worldSpace = mul(unity_ObjectToWorld, pos).xyz;
	return float2(dot(worldSpace, uDirection), dot(worldSpace, vDirection));
}

struct ev_appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 color    : COLOR;
	float2 texcoord : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct ev_v2f
{
	float4 vertex : SV_POSITION;
	float3 normal : NORMAL;
	fixed4 color    : COLOR;
	float2 texcoord : TEXCOORD0;
	#if USE_RIM
	float3 viewDir : TEXCOORD1;
	#endif
	UNITY_FOG_COORDS(1)
	#if defined(USE_SOFTPARTICLES) && defined(SOFTPARTICLES_ON)
	float4 projPos : TEXCOORD2;
	#endif
	UNITY_VERTEX_OUTPUT_STEREO
};