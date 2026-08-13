using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action stage optiosn dialog.
    public class ActionStageOptionsDialog : StageOptionsDialog
    {
        [Header("Action")]

        // The action UI.
        public ActionUI actionUI;

        // The generator info button.
        public Button generatorInfoButton;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the action UI instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Handled in the dedicated function for opening the info dialog.
            // Since this dialog is being opened as a sub dialog...
            // Don't close all dialogs.
            // actionUI.generatorInfoDialog.closeAllDialogsOnClose = false;
        }

        // Opens the generator info dialog.
        public void OpenGeneratorInfoDialog()
        {
            // Make sure it returns to the options dialog.
            // This checks if the value is true since there's UI assets are adjusted...
            // When this value is changed.
            if(actionUI.generatorInfoDialog.CloseAllDialogsOnClose)
            {
                actionUI.generatorInfoDialog.CloseAllDialogsOnClose = false;
            }

            // Open the generator info dialog.
            actionUI.OpenGeneratorInfoDialog(true);
        }
    }
}