using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A day-night indicator, which is used for a visual that shows the timee of day.
    public class DayNightIndicator : MonoBehaviour
    {
        // The action ui.
        public ActionUI actionUI;

        // The display image.
        public Image displayImage;

        // The day-to-night sprite.
        public Sprite dayToNightSprite;

        // The night-to-day sprite.
        public Sprite nightToDaySprite;

        [Header("Tracker")]

        // The tracker used to show the time.
        public Image trackerImage;

        // The start position of the tracker.
        public GameObject trackerStartPos;

        // The end position of the tracker.
        public GameObject trackerEndPos;

        [Header("Update")]

        // NOTE: the day and night colors will likely be taken out once a proper graphic is created.
        // The day and night colors.
        // private Color dayColor = Color.yellow;
        // private Color nightColor = Color.blue;

        // If the wind indicator should be updated automatically.
        public bool autoUpdateIndicator = true;

        // If the indicator is enabled, update it every frame.
        public bool indicatorEnabled = true;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // NOTE: since the game doesn't take advantage of night-to-day...
            // The game doesn't automatically change the display.
            // In other words, the day-to-night display is the one that's always used.
        }

        // Automatically sets the display image based on if the stage is going from day to night or night to day.
        public void AutoSetDisplayImage()
        {
            // Gets the instance.
            ActionManager actionManager = ActionManager.Instance;

            // Checks if going from night to day.
            // Day to night is the default image.
            if(actionManager.IsNightToDay())
            {
                SetDisplayToNightToDay();
            }
            else
            {
                SetDisplayToDayToNight();
            }
        }

        // Returns 'true' if transitioning from day to night, which is checked using the sprite.
        public bool IsDisplayDayToNight()
        {
            return displayImage.sprite == dayToNightSprite;
        }

        // Sets the display to go from day to night.
        public void SetDisplayToDayToNight()
        {
            displayImage.sprite = dayToNightSprite;
        }

        // Returns 'true' if transition from night to day, which is checked using the sprite.
        public bool IsDisplayNightToDay()
        {
            return displayImage.sprite == nightToDaySprite;
        }

        // Sets the display to go from night to day.
        public void SetDisplayToNightToDay()
        {
            displayImage.sprite = nightToDaySprite;
        }

        // Updates the indicator.
        public void UpdateIndicator()
        {
            // Gets the action manager.
            ActionManager actionManager = ActionManager.Instance;

            // If the stage is progressing, update the indicator.
            if (actionManager.IsStagePlayingAndGameUnpaused())
            {
                // The t-value for going from day to night.
                float t = actionManager.GetDayNightTimerProgress();

                // Gets the tracker position using lerp.
                Vector3 trackerPos = Vector3.Lerp(trackerStartPos.transform.position, trackerEndPos.transform.position, t);

                // Sets the tracker's position.
                trackerImage.transform.position = trackerPos;

                // Lerps between the two colours.
                // displayImage.color = Color.Lerp(dayColor, nightColor, t);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the indicator is enabled and day night is enabled.
            if (indicatorEnabled && ActionManager.Instance.IsDayNightEnabled())
            {
                // If the indicator should be automatically updated, call the function.
                if (autoUpdateIndicator)
                    UpdateIndicator();
            }
            
        }
    }
}
