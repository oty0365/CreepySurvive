Shader "Custom/TopDownWater" {
    Properties {
        _ReflectionTex ("Reflection Texture", 2D) = "black" {}
        _Distortion ("Distortion Strength", Range(0,0.1)) = 0.05
        _WaterColor ("Water Tint Color", Color) = (0.1,0.3,0.5,0.6)
        _ReflectionAlpha ("Reflection Alpha", Range(0, 1)) = 1.0
        _WaterAlpha ("Water Alpha", Range(0, 1)) = 0.6
        _NoiseScale ("Noise Scale", Range(1, 50)) = 20
        _NoiseStrength ("Noise Strength", Range(0, 0.1)) = 0.02
    }
    
    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _ReflectionTex;
            float _Distortion;
            float4 _WaterColor;
            float _ReflectionAlpha;
            float _WaterAlpha;
            float _NoiseScale;
            float _NoiseStrength;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            // 간단한 2D 노이즈 (값 노이즈)
            float hash(float2 p) {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }
            
            float noise(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }
            
            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                
                // 기본 출렁임 (전체적으로)
                uv.y += sin(_Time.y * 2 + uv.x * 10) * _Distortion;
                uv.x += cos(_Time.y * 1.5 + uv.y * 10) * _Distortion;
                
                // 국소적인 노이즈 출렁임 (자글자글)
                float n = noise(uv * _NoiseScale + _Time.y * 0.5);
                uv += (n - 0.5) * _NoiseStrength;
                
                // 렌더 텍스처에서 색상 샘플링
                fixed4 reflectionCol = tex2D(_ReflectionTex, uv);
                
                // 렌더 텍스처의 알파값 조절 (비치는 오브젝트의 투명도)
                reflectionCol.a *= _ReflectionAlpha;
                
                // 물 색상과 블렌딩
                fixed4 finalCol;
                finalCol.rgb = lerp(_WaterColor.rgb, reflectionCol.rgb, reflectionCol.a);
                
                // 최종 알파값 = 물의 기본 알파 + 반사된 오브젝트의 알파 기여도
                finalCol.a = saturate(_WaterAlpha + reflectionCol.a * 0.5);
                
                return finalCol;
            }
            ENDHLSL
        }
    }
}