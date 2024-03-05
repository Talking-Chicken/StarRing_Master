﻿Shader "FX/Volumetric Smooth (Deprecated)" {
Properties {
    _Color ("Tint", Color) = (1,1,1,1)
	_Noise ("Noise Texture", 2D) = "white" {}
	_NoiseScale ("Noise Scale", Float) = 1.0
	_NoiseScrollX ("Noise Scroll X", float) = 0.1
	_NoiseScrollY ("Noise Scroll X", float) = 0.1
	_InvFade("Soft Edges Factor", Range(0.01,3.0)) = 1.0
	_RimPower( "Rim Power", Range( 0.01, 10.0 )) = 3.0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

	Cull Front
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
			#pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "easyVolumetric.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
                UNITY_FOG_COORDS(1)
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };
			
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

			sampler2D _Noise;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			float4 _CameraDepthTexture_TexelSize;
			float _NoiseScale;
			float _NoiseScrollX;
			float _NoiseScrollY;
			float _InvFade;
			float _RimPower;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif

				o.texcoord.xy = GenerateUVs(v.normal, v.vertex) * _NoiseScale;
				o.texcoord.x += _NoiseScrollX * _Time.y;
				o.texcoord.y += _NoiseScrollY * _Time.y;

				float3 worldSpace = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - worldSpace);
				o.normal = normalize( mul( float4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
				
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
				#endif
				
				fixed4 noise = tex2D(_Noise, i.texcoord);
                fixed4 col = _Color * i.color * noise.a;
				
				//Rim
				half rim = 1.0 - saturate(dot(i.normal, i.viewDir));
				col.a *= pow(rim, _RimPower);
				
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDCG
    }
}

}
