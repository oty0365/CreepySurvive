Shader "Custom/WaterWaveAlpha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0,0.1)) = 0.05
        _WaveSpeed ("Wave Speed", Range(0,10)) = 2
        _WaveFrequency ("Wave Frequency", Range(0,50)) = 10
        _TintColor ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off   // 투명은 깊이 버퍼에 기록 안 하는 게 보통 좋아

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _WaveStrength;
            float _WaveSpeed;
            float _WaveFrequency;
            float4 _TintColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _WaveSpeed;

                float waveX = sin(i.uv.y * _WaveFrequency + t) * _WaveStrength;
                float waveY = cos(i.uv.x * _WaveFrequency + t) * _WaveStrength;

                float2 distortedUV = i.uv + float2(waveX, waveY);

                fixed4 col = tex2D(_MainTex, distortedUV);

                // 색 + 알파를 같이 틴트 적용
                col *= _TintColor;

                return col;
            }
            ENDCG
        }
    }
}
