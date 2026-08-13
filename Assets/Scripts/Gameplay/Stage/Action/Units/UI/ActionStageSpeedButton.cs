using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A button used to change the action stage's speed.
    public class ActionStageSpeedButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The unit remove button.
        public Button button;

        // The icon image.
        public Image iconImage;

        // If 'true', the button speeds up the stage upon being selected.
        [Tooltip("Speeds up the stage when the button is pressed.")]
        public bool speedUp = true;

        // If 'true', the button slows down the stage upon being selected.
        [Tooltip("Slows down the stage when the button is pressed.")]
        public bool slowDown = false;

        // The event overlay trigger for this button.
        // public ActionEventOverlayTrigger eventOverlayTrigger;

        [Header("Sprites")]
        // Slow speed icon.
        public Sprite speedSlowIconSprite;

        // Normal speed icon.
        public Sprite speedNormalIconSprite;

        // Fast speed icon.
        public Sprite speedFastIconSprite;

        // Start is called before the first frame update
        void Start()
        {
            // Setting the action UI if it's null.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Setting the button if it's null.
            if (button == null)
                button = GetComponent<Button>();

            // Adds a Select() call on the button.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }

            // Refreshes the speed icon.
            RefreshSpeedIcon();
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // Changes the stage speed.
            ChangeStageSpeed();

            // // Plays the event if the stage event overlay is enabled.
            // if (ActionManager.Instance.StageEventOverlayEnabled)
            //     eventOverlayTrigger.PlayEvent();
        }

        // Sets the stage speed based on the button's settings.
        public void ChangeStageSpeed()
        {
            // Gets the instance.
            ActionManager actionManager = ActionManager.Instance;

            // Checks the enabled options to see how to handle it.
            if (speedUp && slowDown) // Both Options
            {
                // Gets the speed int.
                int speedInt = actionManager.GetStageSpeedAsInt();

                // Checks the speed integer.
                if (speedInt > 0) // Fast, so go to slow
                {
                    actionManager.SetStageSpeedSlow();
                }
                else if (speedInt < 0) // Slow, so go to normal.
                {
                    actionManager.SetStageSpeedNormal();
                }
                else // Normal, so go to fast.
                {
                    actionManager.SetStageSpeedFast();
                }
            }
            else if (speedUp && !slowDown) // Speed Up Only
            {
                // If the stage speed is normal, speed it down.
                if (actionManager.IsStageSpeedNormal())
                {
                    actionManager.SetStageSpeedFast();
                }
                // If the stage speed isn't normal, make it normal.
                else
                {
                    actionManager.SetStageSpeedNormal();
                }
            }
            else if (!speedUp && slowDown) // Slow Down Only
            {
                // If the stage speed is normal, slow it down.
                if (actionManager.IsStageSpeedNormal())
                {
                    actionManager.SetStageSpeedSlow();
                }
                // If the stage speed isn't normal, make it normal.
                else
                {
                    actionManager.SetStageSpeedNormal();
                }
            }
            else // None
            {
                // Set the stage speed to normal.
                actionManager.SetStageSpeedNormal();
            }

            // Adjust the speed icon.
            RefreshSpeedIcon();
        }

        // Refreshes the speed icon based on the time scale.
        public void RefreshSpeedIcon()
        {
            // Gets the action manager.
            ActionManager actionManager = ActionManager.Instance;

            // Checks the stage speed to see what icon to display.
            if(actionManager.IsStageSpeedFast()) // Fast
            {
                iconImage.sprite = speedFastIconSprite;
            }
            else if(actionManager.IsStageSpeedSlow()) // Slow
            {
                iconImage.sprite = speedSlowIconSprite;
            }
            else // Normal
            {
                iconImage.sprite = speedNormalIconSprite;
            }
        }

    }
}