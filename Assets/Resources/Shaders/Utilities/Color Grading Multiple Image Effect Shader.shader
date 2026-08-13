Shader "Hidden/Color Grading Multiple Image Effect Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
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

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        };

        // Three textures that represent the Red (R), Green (G), and B (Blue) channels respectively.
        // Recommended that you use textures that range from 256x256 to 1024x1024. 
        // Smaller than 256x256 doesn't represent all color codes, and bigger than 1024x1024 doesn't give any more color variance.
        sampler2D _ColorGradeRed;
        sampler2D _ColorGradeGreen;
        sampler2D _ColorGradeBlue;

    ENDCG

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
               
                // The new color.
                fixed4 newCol = col;

                // RGB components of the new color.
                // In Unity UV space, the bottom left corner is (0,0).
                // Gets the new RGB channel values from the reference textures. Each texture is one channel (R, G, B).
                fixed4 newColRed = tex2D(_ColorGradeRed, fixed2(col.r, 0.0F));
                fixed4 newColGreen = tex2D(_ColorGradeGreen, fixed2(col.g, 0.0F));
                fixed4 newColBlue = tex2D(_ColorGradeBlue, fixed2(col.b, 0.0F));
                
                // Sets new colors.
                newCol.r = newColRed.r;
                newCol.g = newColGreen.g;
                newCol.b = newColBlue.b;
                // newCol.a = col.a; // Unneeded.

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
