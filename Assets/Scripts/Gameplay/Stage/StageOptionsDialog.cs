using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The stage options dialog.
    public class StageOptionsDialog : MonoBehaviour
    {
        // The stage UI.
        public StageUI stageUI;

        [Header("Buttons")]

        // The reset stage button.
        public Button resetButton;

        // The return to world button.
        public Button worldButton;

        // The tutoria log button.
        public Button tutorialLogButton;

        // The settings button.
        public Button settingsButton;

        // The back button.
        public Button backButton;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the stage UI isn't set, try to get it.
            if (stageUI == null)
                stageUI = FindObjectOfType<StageUI>();
        }

        // Resets the stage.
        public void ResetStage()
        {
            stageUI.ResetStage();
        }

        // Quits the stage, which loads the world scene.
        public void QuitStage()
        {
            stageUI.QuitStage();
        }

        // Opens the tutorials dialog.
        public void OpenTutorialLogDialog()
        {
            stageUI.OpenTutorialLogDialogDialog(true);
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            stageUI.OpenSettingsDialog(true);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            stageUI.CloseOptionsDialog();
        }
    }
}