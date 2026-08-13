using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action user block button.
    public class ActionUserBlockButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The unit remove button.
        public Button button;

        // The icon image.
        public Image blockIconImage;

        // The block active sprite.
        public Sprite blockActiveSprite;

        // The block inactive sprite.
        public Sprite blockInactiveSprite;

        // The event overlay trigger for this button.
        // public ActionEventOverlayTrigger eventOverlayTrigger;

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

            // Updates the block icon.
            UpdateBlockIcon();
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // Toggles the user's energy.
            ActionManager.Instance.playerUser.ToggleBlockingAttackEnergy();

            // Updates the block icon.
            UpdateBlockIcon();

            // // Plays the event if the stage event overlay is enabled.
            // if (ActionManager.Instance.StageEventOverlayEnabled)
            //     eventOverlayTrigger.PlayEvent();
        }

        // Updates the block icon.
        public void UpdateBlockIcon()
        {
            // Checks if the user is blocking attack energy or not.
            if (ActionManager.Instance.playerUser.IsBlockingAttackEnergy()) // Blocking
            {
                blockIconImage.sprite = blockActiveSprite;
            }
            else // Not blocking
            {
                blockIconImage.sprite = blockInactiveSprite;
            }
        }

    }
}