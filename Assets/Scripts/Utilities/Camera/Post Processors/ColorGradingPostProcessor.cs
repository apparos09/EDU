using System;
using UnityEngine;

namespace util
{
    // Alters the colour of the image using color grading.
    // TODO: the colour grading shaders don't work. For some reason, the darkest and lightest parts of the render...
    // Don't get recoloured properly. Telling the program to ignore pure black and white from the original render...
    // Didn't fix it either. Don't use this, as it doesn't work properly.
    public class ColorGradingPostProcessor : PostProcessor
    {
        [Header("ColorGrading")]

        // ID used for setting single grade mode.
        public string singleGradeModeID = "_SingleGradeMode";

        // If 'true', a single color grade is being used.
        // IF 'false', the grade is split between RGB.
        public bool singleGradeMode = true;

        [Header("ColorGrading/Texture IDs")]
        // All in 1 color grade.
        public string colorGradeRGBID = "_ColorGradeRGB";

        // RGB
        public string colorGradeRedID = "_ColorGradeRed";
        public string colorGradeGreenID = "_ColorGradeGreen";
        public string colorGradeBlueID = "_ColorGradeBlue";

        [Header("ColorGrading/Textures")]
        // Combined color grade. Top row is red, middle row is green, and bottom row is blue.
        public Texture2D colorGradeRGB;

        // The three colour grading textures.
        public Texture2D colorGradeRed;
        public Texture2D colorGradeGreen;
        public Texture2D colorGradeBlue;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Set the textures to the material.
            SetValuesToMaterial();
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Needs to be set every frame for some reason.
            SetValuesToMaterial();

            base.OnRenderImage(source, destination);
        }

        // Better to manually set them.
        // Sets the combined color grade texture.
        protected void SetColorGradeTexture(Texture2D colorGradeRGB)
        {
            this.colorGradeRGB = colorGradeRGB;
            SetValuesToMaterial();
        }

        // Better to manually set them.
        // Sets the textures to be used for color grading.
        protected void SetColorGradeTextures(Texture2D red, Texture2D green, Texture2D blue)
        {
            // Sets the three textures.
            colorGradeRed = red;
            colorGradeGreen = green;
            colorGradeBlue = blue;

            // Sets the textures in the material.
            SetValuesToMaterial();
        }

        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Pretty sure this int (bool) doesn't need to be set every frame, but since other values...
            // Are being set, you might as well set all the values to be sure.
            // Setting the mode (single or multi) by converting the bool to an int.
            // 0 = false, 1 = true. Anything that's not 0 registers as true.
            int singleGradeModeInt = Convert.ToInt32(singleGradeMode);
            postMaterial.SetInt(singleGradeModeID, singleGradeModeInt);

            // Setting the texture seems to work, but not getting the texture.
            // Combined color grade.
            if(singleGradeMode)
            {
                postMaterial.SetTexture(colorGradeRGBID, colorGradeRGB);
            }
            else
            {
                // Sets the color grading textures. Data type sampler2D in the shader.
                postMaterial.SetTexture(colorGradeRedID, colorGradeRed);
                postMaterial.SetTexture(colorGradeGreenID, colorGradeGreen);
                postMaterial.SetTexture(colorGradeBlueID, colorGradeBlue);
            }
        }
    }
}