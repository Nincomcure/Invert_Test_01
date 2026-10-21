Shader "Unlit/Invert_Colors_01"
{
    Properties
    {
        [HideInInspector] _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        // Render after regular geometry and match transparent masking rules
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        // Turn off depth writing so it doesn't block depth sorting
        ZWrite Off

        // This math command forces the inversion: (1 - Destination Color) * Source Alpha
        Blend OneMinusDstColor Zero

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the sprite shape's alpha channel
                fixed4 texColor = tex2D(_MainTex, i.uv);

            // Return pure white, but use the sprite's actual alpha transparency shape
            return fixed4(1.0, 1.0, 1.0, texColor.a);
        }
        ENDCG
    }
    }
}