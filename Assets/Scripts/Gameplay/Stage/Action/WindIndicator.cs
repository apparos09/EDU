using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // Indicates the strength of the wind.
    public class WindIndicator : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // NOTE: this is a temporary display.

        // The display image.
        public Image displayImage;

        // The bar used to show the wind speed.
        public ProgressBar speedBar;

        // Colours - unused to simplify the bar.
        // The minimum and maximum colors for wind.
        // private Color windMinColor = Color.blue;
        // private Color windMaxColor = Color.red;

        // Color for if the wind is not enabled.
        // private Color windDisabledColor = Color.grey;

        // If the wind indicator should be updated automatically.
        public bool autoUpdateIndicator = true;

        // If the indicator is enabled, update it every frame.
        public bool indicatorEnabled = true;

        //
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Updates the indicator.
        public void UpdateIndicator()
        {
            // Gets the instance.
            ActionManager actionManager = ActionManager.Instance;

            // If the wind is enabled.
            if (actionManager.IsWindEnabled())
            {
                // Gets the wind rating as a percentage.
                float t = actionManager.GetCurrentWindRatingAsAPercentage();

                // Adjusts the bar based on the provided color.
                speedBar.SetValueAsPercentage(t);

                // Changes the bar color.
                // speedBar.fillImage.color = Color.Lerp(windMinColor, windMaxColor, t);
            }
            else
            {
                // // Sets the color for the wind being disabled.
                // speedBar.fillImage.color = windDisabledColor;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the indicator is enabled.
            if (indicatorEnabled && ActionManager.Instance.IsWindEnabled())
            {
                // If the indicator should be automatically updated, call the function.
                if (autoUpdateIndicator)
                    UpdateIndicator();
            }
        }
    }
}