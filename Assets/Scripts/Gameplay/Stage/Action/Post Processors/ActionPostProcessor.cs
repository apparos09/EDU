using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The post processor for the action stage.
    // This is no longer used because it possibly impacted game performance significantly.
    public class ActionPostProcessor : util.PostProcessor
    {
        [Header("Action")]
        // The action UI.
        public ActionUI actionUI;

        // Color Grade Night ID
        public string colorGradeNightId = "_ColorGradeNight";

        // The colour grade for night (RGB).
        public Texture2D colorGradeNight;

        // The lerp T id.
        public string lerpTId = "_LerpT";

        // The lerp value between the day and night textures.
        public float lerpT = 0.0F;

        // A check to see if the transition is from day to night or day to day.
        [Tooltip("The id for a boolean in the shader that indicates if the stage is going from day to night or vice-versa.")]
        public string dayToNightId = "_DayToNight";

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Set the textures to the material.
            SetValuesToMaterial();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the action UI isn't set, set it.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
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
            // This takes an instance of the ActionManager because the ActionUI wasn't getting set in time.

            postMaterial.SetTexture(colorGradeNightId, colorGradeNight);
            postMaterial.SetFloat(lerpTId, lerpT);
            postMaterial.SetInteger(dayToNightId, Convert.ToInt32(ActionManager.Instance.IsDayToNight()));
        }
    }
}