using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // An overlay that's used to show changes to the time of day.
    public class DayNightOverlay : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The overlay image used for the day-night overlay. Its alpha value is changed based on the time of day.
        public Image overlayImage;

        // The reset colour for the overlay.
        public Color resetColor = Color.black;

        // If true, the reset color is automatically set.
        [Tooltip("Automatically sets the reset color to the overlayImage's color in Awake.")]
        bool autoSetResetColor = true;

        // The alphas for the overlays.
        private float dayAlpha = 0.0F;
        private float nightAlpha = 0.4F; // 40%

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Automatically sets the reset color and sets its alpha to 1.
            if(autoSetResetColor)
            {
                resetColor = overlayImage.color;
                resetColor.a = 1.0F;
            }

            // Resets the overlay to make sure it's set properly.
            ResetOverlay();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // The colour the overlay is set to upon being reset.
        public Color ResetColor
        {
            get { return resetColor; }
        }

        // Returns the alpha value used when it's day.
        public float DayAlpha
        {
            get {  return dayAlpha; }
        }

        // Returns the alpha value used when it's night.
        public float NightAlpha
        {
            get { return nightAlpha; }
        }

        // Resets the overlay.
        public void ResetOverlay()
        {
            overlayImage.gameObject.SetActive(true);
            overlayImage.color = resetColor;
            overlayImage.gameObject.SetActive(false);
        }

        // Updates the overlay.
        public void UpdateOverlay(float t)
        {
            // Makes sure the overlay image is active.
            overlayImage.gameObject.SetActive(true);

            // The new color.
            Color newColor = overlayImage.color;

            // If the t-value is greater than 0.
            if (t > 0)
            {
                // Lerp the alpha values to get the new alpha, and set it to the new color.
                // Checks whether its going from day to night or night to day to know what...
                // Values to set as the start and end for the lerp.
                if(ActionManager.Instance.IsDayToNight())
                {
                    newColor.a = Mathf.Lerp(dayAlpha, nightAlpha, t);
                }
                else
                {
                    newColor.a = Mathf.Lerp(nightAlpha, dayAlpha, t);
                }


                // Set the overlay image's new color.
                // If the color won't be changing, this step is ignored.
                if (overlayImage.color != newColor)
                {
                    overlayImage.color = newColor;
                }
            }
            // The t-value is 0, so set the alpha to 1.0 and turn off the image.
            else
            {
                // Set the new color's alpha to 1.
                newColor.a = 1;

                // If the current color is different from the new color...
                // Set it to the new color.
                if (overlayImage.color != newColor)
                {
                    overlayImage.color = newColor;
                }

                // Turn off the overlay image again.
                overlayImage.gameObject.SetActive(false);
            }
        }

    }
}