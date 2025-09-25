Shader "Custom/WaterSurfaceShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _LightDir ("Light Direction", Vector) = (0.5, -1, 0, 0)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,0.5)
        _ShadowLength ("Shadow Length", Range(0,2)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _LightDir;
            float4 _ShadowColor;
            float _ShadowLength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 shadowUV : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.shadowUV = o.uv + _LightDir.xy * _ShadowLength;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, i.uv);
                
                fixed4 shadowCol = tex2D(_MainTex, i.shadowUV);
                shadowCol.rgb = _ShadowColor.rgb;
                shadowCol.a *= _ShadowColor.a;
                
                return lerp(shadowCol, mainCol, mainCol.a);
            }
            ENDCG
        }
    }
}
