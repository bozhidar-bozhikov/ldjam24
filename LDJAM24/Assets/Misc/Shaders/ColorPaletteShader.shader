Shader "Unlit/ColorPaletteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorPalette ("Color Palette", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            sampler2D _ColorPalette;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 paletteCol = tex2D(_ColorPalette, i.uv);

                // Find the closest color in the palette
                col.rgb = paletteCol.rgb;

                return col;
            }
            ENDCG
        }
    }
}