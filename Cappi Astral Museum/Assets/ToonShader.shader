Shader "Custom/ToonMultiBandOutlineNormal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)

        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0,2)) = 1

        _Bands ("Number of Bands", Range(1,5)) = 3
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.5

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.001, 0.05)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // =====================
        //  OUTLINE PASS
        // =====================
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "Always" }

            Cull Front
            ZWrite On
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _OutlineThickness;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float3 normal = normalize(v.normal);
                float3 pos = v.vertex.xyz + normal * _OutlineThickness;

                o.pos = UnityObjectToClipPos(float4(pos, 1.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // =====================
        // MAIN TOON PASS
        // =====================
        CGPROGRAM
        #pragma surface surf Toon

        sampler2D _MainTex;
        fixed4 _Color;

        sampler2D _BumpMap;
        float _BumpScale;

        float _Bands;
        float _ShadowStrength;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Normal map
            fixed3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            normal.xy *= _BumpScale;
            o.Normal = normalize(normal);
        }

        half4 LightingToon(SurfaceOutput s, half3 lightDir, half atten)
        {
            float NdotL = dot(s.Normal, lightDir);
            float light = saturate(NdotL);

            float banded = floor(light * _Bands) / _Bands;
            banded = lerp(_ShadowStrength, 1.0, banded);

            half4 color;
            color.rgb = s.Albedo * _LightColor0.rgb * banded * atten;
            color.a = s.Alpha;

            return color;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
