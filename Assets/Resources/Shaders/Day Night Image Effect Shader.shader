Shader "Hidden/Day Night Image Effect Shader"
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

        // The night texture for the shader, which is used for applying night colours.
        // The day colours are the default colours.
        sampler2D _ColorGradeNight;

        // The lerp between the colour codes.
        fixed _LerpT;

        // Used to see if the stage is going from day to night, or vice-versa.
        // This is an integer, but it's treated like a boolean.
        int _DayToNight;

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
                // Base colour, which is also the day color.
                fixed4 col = tex2D(_MainTex, i.uv);
               
                // The new color, which is the base color by default.
                fixed4 newCol = col;

                // The night colors.
                fixed4 nightColRed = tex2D(_ColorGradeNight, fixed2(col.r, 1.0F));
                fixed4 nightColGreen = tex2D(_ColorGradeNight, fixed2(col.g, 0.5F));
                fixed4 nightColBlue = tex2D(_ColorGradeNight, fixed2(col.b, 0.0F));

                // Mixes the day colors and night colors using lerp T.
                // Checks if going from day to night or night to day.
                if(_DayToNight != 0) // True (all non-0 values register as true)
                {
                    newCol.r = lerp(col.r, nightColRed.r, _LerpT);
                    newCol.g = lerp(col.g, nightColGreen.g, _LerpT);
                    newCol.b = lerp(col.b, nightColBlue.b, _LerpT);
                }
                else // False (0)
                {
                    newCol.r = lerp(nightColRed.r, col.r, _LerpT);
                    newCol.g = lerp(nightColGreen.g, col.g, _LerpT);
                    newCol.b = lerp(nightColBlue.b, col.b, _LerpT);
                }

                // newCol.a = col.a; // Unneeded since the alpha has already been copied over.

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}