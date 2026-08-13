using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The stage UI.
    public abstract class StageUI : GameplayUI
    {
        [Header("Stage UI")]
        // The game will skip these overwritten functions in StageUI and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        // The stage manager.
        public StageManager stageManager;

        // The options button.
        public Button optionsButton;

        // The options dialog.
        public StageOptionsDialog optionsDialog;

        // The tutorial log dialog.
        public TutorialLog tutorialLogDialog;

        // The game settings UI.
        public GameSettingsUI settingsDialog;

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Update is called once per frame
        // protected override void Update()
        // {
        //     base.Update();
        // }
        // 
        // protected override void OnDestroy()
        // {
        //     base.OnDestroy();
        // }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finds the stage manager if it isn't set.
            if (stageManager == null)
                stageManager = FindObjectOfType<StageManager>();

            // Makes sure the settings UI is off.
            if (settingsDialog != null)
                settingsDialog.gameObject.SetActive(false);

            // Closes all the dialogs.
            CloseAllDialogs();
        }

        // DIALOGS
        // Opens the given dialog.
        public virtual void OpenDialog(GameObject dialog, bool closeOtherDialogs)
        {
            // If 'true', close all the other dialogs.
            if (closeOtherDialogs)
            {
                CloseAllDialogs();
            }

            // Activates the dialog box.
            dialog.SetActive(true);

            // Pauses the game.
            stageManager.PauseGame();
        }

        // Closes the given dialog.
        public virtual void CloseDialog(GameObject dialog)
        {
            dialog.SetActive(false);

            // If no dialogs are open, unpause the game.
            if (!IsDialogOpen())
            {
                stageManager.UnpauseGame();
            }
        }

        // Closes all dialog boxes (windows).
        public virtual void CloseAllDialogs()
        {
            // Generates a dialog list.
            List<GameObject> dialogList = GenerateDialogList();

            // Goes through each dialog.
            foreach(GameObject dialog in dialogList)
            {
                // If the dialog exist, close it.
                if(dialog != null)
                {
                    // NOTE: this doesn't use the CloseDialog() function, because that function...
                    // Checks if any dialogs are open to know if the game should be unpaused.
                    // This function does the same thing after all dialogs are closed.

                    // CloseDialog(dialog);
                    dialog.gameObject.SetActive(false);
                }
            }

            // Unpause the game since all dialogs are closed.
            stageManager.UnpauseGame();
        }

        // Generates a list of dialogs.
        public virtual List<GameObject> GenerateDialogList()
        {
            // The list to return, which is given the dialogs in this script.
            List<GameObject> dialogList = new List<GameObject>
            {
                optionsDialog.gameObject,
                tutorialLogDialog.gameObject,
                settingsDialog.gameObject
            };

            return dialogList;
        }

        // Returns true if a dialog is open.
        public virtual bool IsDialogOpen()
        {
            // Set to see if there's a dialog open.
            bool result = false;

            // Generates a list of dialogs.
            List<GameObject> dialogList = GenerateDialogList();

            // Goes through all dialogs.
            foreach(GameObject dialog in  dialogList)
            {
                // Dialog exists.
                if(dialog != null)
                {
                    // If an active dialog has been found.
                    if(dialog.activeSelf)
                    {
                        result = true;
                        break;
                    }
                }
            }

            // Return result.
            return result;
        }

        // Options
        // Opens the options dialog.
        public void OpenOptionsDialog(bool closeOtherDialogs)
        {
            OpenDialog(optionsDialog.gameObject, closeOtherDialogs);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            CloseDialog(optionsDialog.gameObject);
        }

        // Tutorial Log
        // Returns 'true' if the tutorial log dialog is open.
        public bool IsTutorialLogDialogOpen()
        {
            return tutorialLogDialog.gameObject.activeSelf;
        }

        // Opens the tutorial log  dialog.
        public void OpenTutorialLogDialogDialog(bool closeOtherDialogs)
        {
            OpenDialog(tutorialLogDialog.gameObject, closeOtherDialogs);
        }

        // Closes the tutorial log  dialog.
        public void CloseTutorialLogDialog()
        {
            CloseDialog(tutorialLogDialog.gameObject);
        }

        // Settings
        // Opens the settings dialog.
        public void OpenSettingsDialog(bool closeOtherDialogs)
        {
            OpenDialog(settingsDialog.gameObject, closeOtherDialogs);
        }

        // Closes the settings dialog.
        public void CloseSettingsDialog()
        {
            CloseDialog(settingsDialog.gameObject);
        }

        // Resets the stage by calling the stage manager.
        public virtual void ResetStage()
        {
            stageManager.ResetStage();
        }

        // Quits the stage, returning to the world scene.
        public virtual void QuitStage()
        {
            stageManager.QuitStage();
        }

        // Called to finish the stage.
        // This just calls the appropriate function in the manager.
        public virtual void FinishStage()
        {
            stageManager.FinishStage();
        }

    }
}