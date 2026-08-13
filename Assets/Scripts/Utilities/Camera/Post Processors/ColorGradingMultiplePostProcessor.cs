using UnityEngine;

namespace util
{
    // Color grading post processing with multiple textures.
    public class ColorGradingMultiplePostProcessor : PostProcessor
    {
        [Header("Color Grading Multiple")]

        // Color Grade IDs
        public string colorGradeRedID = "_ColorGradeRed";
        public string colorGradeGreenID = "_ColorGradeGreen";
        public string colorGradeBlueID = "_ColorGradeBlue";

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

        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            postMaterial.SetTexture(colorGradeRedID, colorGradeRed);
            postMaterial.SetTexture(colorGradeGreenID, colorGradeGreen);
            postMaterial.SetTexture(colorGradeBlueID, colorGradeBlue);
        }
    }
}