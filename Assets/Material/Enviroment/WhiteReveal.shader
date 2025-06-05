Shader "Custom/ObjectWhiteRevealMulti"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _RevealAmount ("Reveal Amount", Range(0, 1)) = 1.0
        _Radius ("Reveal Radius", Float) = 1.0
        _Center0 ("Center 0", Vector) = (0, 0, 0, 0)
        _Center1 ("Center 1", Vector) = (0, 0, 0, 0)
        _Center2 ("Center 2", Vector) = (0, 0, 0, 0)
        _Center3 ("Center 3", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _RevealAmount;
            float _Radius;
            float3 _Center0;
            float3 _Center1;
            float3 _Center2;
            float3 _Center3;

            Varyings vert(Attributes v)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.worldPos = worldPos;
                o.positionHCS = TransformWorldToHClip(worldPos);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float d0 = distance(i.worldPos, _Center0);
                float d1 = distance(i.worldPos, _Center1);
                float d2 = distance(i.worldPos, _Center2);
                float d3 = distance(i.worldPos, _Center3);
                float minDist = min(min(d0, d1), min(d2, d3));

                float reveal = 1.0 - smoothstep(_Radius, _Radius * 0.5, minDist);
                float blend = saturate(reveal * _RevealAmount);

                float4 col = tex2D(_MainTex, i.uv);
                float lum = dot(col.rgb, float3(0.299, 0.587, 0.114));
                float3 shadedWhite = float3(lum, lum, lum);
                float3 finalColor = lerp(col.rgb, shadedWhite, blend);

                return float4(finalColor, col.a);
            }
            ENDHLSL
        }
    }
}
