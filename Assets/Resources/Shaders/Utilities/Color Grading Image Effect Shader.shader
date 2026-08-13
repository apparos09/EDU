Shader "Hidden/Color Grading Image Effect Shader"
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
        }

        // Specifies if using one colour grade or not.
        // This is an integer, but it's converted from a boolean value.
        int _SingleGradeMode;

        // A combined RGB color grade, and color grades for the individual RGB components.
        // Recommended that you use a texture that ranges from 256x256 to 1024x1024. 
        // Smaller than 256x256 doesn't represent all color codes, and bigger than 1024x1024 doesn't give any more color variance.
        sampler2D _ColorGradeRGB;
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
                fixed4 newColRed;
                fixed4 newColGreen;
                fixed4 newColBlue;

                // In Unity UV space, the bottom left corner is (0,0).

                // Gets the new colors.
                // 0 = false, anything else is true.
                if(_SingleGradeMode != 0) // Single grade mode. One texture that contains seperated RGB channels.
                {
                    // Red is the top row, Green is the middle row, and Blue is the bottom row.
                    newColRed = tex2D(_ColorGradeRGB, fixed2(col.r, 1.0F));
                    newColGreen = tex2D(_ColorGradeRGB, fixed2(col.g, 0.5F));
                    newColBlue = tex2D(_ColorGradeRGB, fixed2(col.b, 0.0F));
                }
                else // Multi-grade mode. Each RGB channel is a seperate texture.
                {
                    // Each image is one color channel (R, G, B).
                    newColRed = tex2D(_ColorGradeRed, fixed2(col.r, 0.0F));
                    newColGreen = tex2D(_ColorGradeGreen, fixed2(col.g, 0.0F));
                    newColBlue = tex2D(_ColorGradeBlue, fixed2(col.b, 0.0F));
                }
                
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
