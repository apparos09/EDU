using UnityEngine;

namespace util
{
    // Color grading post processing with a single texture.
    public class ColorGradingSinglePostProcessor : PostProcessor
    {
        [Header("Color Grading Single")]

        // Color Grade ID
        public string colorGradeRGBID = "_ColorGradeRGB";

        // Combined color grade. Top row is red, middle row is green, and bottom row is blue.
        public Texture2D colorGradeRGB;

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

        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            postMaterial.SetTexture(colorGradeRGBID, colorGradeRGB);
        }
    }
}