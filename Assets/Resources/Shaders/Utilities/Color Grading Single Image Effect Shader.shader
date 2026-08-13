Shader "Hidden/Color Grading Single Image Effect Shader"
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

        // A texture that has the RGB channels seperated.
        // Recommended that you use a texture that ranges from 256x256 to 1024x1024. 
        // Smaller than 256x256 doesn't represent all color codes, and bigger than 1024x1024 doesn't give any more color variance.
        sampler2D _ColorGradeRGB;

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
                // Gets the new RGB channel values from the reference image.
                // Red is the top row, Green is the middle row, and Blue is the bottom row.
                fixed4 newColRed = tex2D(_ColorGradeRGB, fixed2(col.r, 1.0F));
                fixed4 newColGreen = tex2D(_ColorGradeRGB, fixed2(col.g, 0.5F));
                fixed4 newColBlue = tex2D(_ColorGradeRGB, fixed2(col.b, 0.0F));
                
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