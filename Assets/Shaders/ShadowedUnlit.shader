Shader "Unlit/ShadowedUnlit"
{
    Properties
    {
		[Header(Rim Effect)]
		[HDR] _RimColor ("Rim Color", Color) = (1,1,1,1)
		[PowerSlider(2)] _ShadowWidth ("Shadow width", Range(0.5, 3)) = 2
    }
    SubShader
    {
        Cull Off
        Tags { "Queue"= "Transparent" "RenderType"="Transparent" "RenderPipeline" = "UniversalRenderPipeline"}
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 screenPosition : TEXCOORD2;

				float3 viewDir : TEXCOORD3;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
			sampler2D _NormalMap;
            float4 _MainTex_ST, _NormalMap_ST;
			float _NormalStrength;
			float2 _normalTiling, _normalOffset;

			float4 _RimColor, _OuterRimColor;
			float _ShadowWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _NormalMap);
				o.screenPosition = ComputeScreenPos(o.vertex);

				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//Calculate distortion, and apply background texture.
				float2 uvModifier = UnpackNormal(tex2D(_NormalMap, (i.uv2 * _normalTiling) + (_normalOffset * _Time.x))) * _NormalStrength;

				float2 textureCoord = (i.screenPosition.xy / i.screenPosition.w) + uvModifier;
				float4 col = float4(0.0f, 0.0f, 0.0f, 1.0f);


				float rim = 1.0 - abs(dot(i.viewDir, i.normal));
				rim = pow(rim, _ShadowWidth);

				col.rgb += rim * _RimColor;

                col = clamp(col, float4(0.0f, 0.0f, 0.0f, 0.0f), float4(1.0f, 1.0f, 1.0f, 1.0f));

				UNITY_APPLY_FOG(i.fogCoord, col);
                col.w =  clamp((col.r + col.b) * 5, .3f, 0.75f);
                return col;
            }
            ENDCG
        }
    }
}
