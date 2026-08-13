using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world options dialog.
    public class WorldOptionsDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        [Header("Buttons")]

        // The save and continue button.
        public Button saveContButton;

        // The save and quit button.
        public Button saveQuitButton;

        // The tutoria log button.
        public Button tutorialLogButton;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // Checks to see if saving can be done.
            bool canSave;

            // This is done by seeing if this is the LOL_BUILD, the SaveSystem is Instantiated...
            // And if Saving/Loading is enabled.
            // In WebGL, the game can only save if it's using LOL's systems.
            canSave = GameSettings.IS_LOL_BUILD && SaveSystem.Instantiated &&
                    WorldManager.Instance.SavingLoadingEnabled;

            // Use this version if running the game with the LOL components removed.
            // canSave = Application.platform != RuntimePlatform.WebGLPlayer &&
            //     SaveSystem.Instantiated && WorldManager.Instance.SavingLoadingEnabled;

            // Sets the interactivity of the save buttons.
            saveContButton.interactable = canSave;
            saveQuitButton.interactable = canSave;
        }

        // Saves and continues the game.
        public void SaveAndContinue()
        {
            worldUI.SaveAndContinue();
        }

        // Saves and quits the game.
        public void SaveAndQuit()
        {
            worldUI.SaveAndQuit();
        }

        // Quits the game without saving.
        public void QuitWithoutSaving()
        {
            worldUI.QuitWithoutSaving();
        }

        // Opens the tutorials dialog.
        public void OpenTutorialLogDialog()
        {
            worldUI.OpenTutorialLogDialogDialog(true);
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            worldUI.OpenSettingsDialog(true);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            worldUI.CloseOptionsDialog();
        }


    }
}