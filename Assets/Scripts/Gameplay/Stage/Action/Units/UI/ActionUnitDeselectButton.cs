using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The deselect button, which is used to deselect the currently selected unit.
    public class ActionUnitDeselectButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The unit remove button.
        public Button button;

        // The icon image.
        public Image iconImage;

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
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // Gets the player user.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // Player user exists.
            if(playerUser != null)
            {
                // Clears the selected unit.
                playerUser.ClearSelectedActionUnitPrefab();
            }

            // // Plays the event if the stage event overlay is enabled.
            // if (ActionManager.Instance.StageEventOverlayEnabled)
            //     eventOverlayTrigger.PlayEvent();
        }
    }
}