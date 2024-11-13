Shader "Custom/FlashAndShakeShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlashColor ("Flash Color", Color) = (1,0,0,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
        _ShakeAmount ("Shake Amount", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _FlashColor;
            float _FlashAmount;
            float _ShakeAmount;
            float4 _MainTex_ST;

            // Simple random function
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            v2f vert (appdata_t v)
            {
                v2f o;

                // Apply shaking effect
                float2 shakeOffset = float2(
                    (_ShakeAmount * (rand(v.vertex.xy * 10.0) * 2.0 - 1.0)),
                    (_ShakeAmount * (rand(v.vertex.xy * 20.0) * 2.0 - 1.0))
                );

                v.vertex.xy += shakeOffset;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.texcoord) * i.color;
                texColor.rgb = lerp(texColor.rgb, _FlashColor.rgb, _FlashAmount);
                return texColor;
            }
            ENDCG
        }
    }
}
