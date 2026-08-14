using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The title UI.
    public class TitleUI : MonoBehaviour
    {
        // The singleton instance.
        private static TitleUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The manager.
        public TitleManager titleManager;

        [Header("Main Buttons")]

        // The new game button, new game (with mode) button, and continue button.
        public Button newGameButton;
        public Button newGameModeButton;
        public Button continueButton;

        // The instructions, settings, and licenses.
        public Button instructionsButton;
        public Button settingsButton;
        public Button licensesButton;

        // The quit button.
        public Button quitButton;

        [Header("Windows")]
        // The title window.
        public GameObject titleDialog;

        // The instructions, settings, and credits windows.
        public InstructionsDialog instructionsDialog;
        public GameSettingsUI settingsDialog;
        public Licenses licensesDialog;

        // The game mode dialog (unused).
        public GameModeDialog gameModeDialog;

        [Header("Other")]
        // The save text for the game.
        public TMP_Text saveText;

        // Constructor
        private TitleUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            // The manager.
            if (titleManager == null)
                titleManager = TitleManager.Instance;

            // If the platform is set to webGL, disable the quit button.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // The quit button doesn't do anything in WebGL, so turn it off.
                quitButton.interactable = false; // Disable               
            }
            else
            {
                // Set as true.
                quitButton.interactable = true;
            }

            // Makes the save text empty by default.
            saveText.text = string.Empty;

            // Save the save text as the save feedback text.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
            {
                // Set the save text for the save system if it exists.
                // if (SystemManager.Instance.saveSystem != null) // Old
                if (SaveSystem.Instantiated) // New
                {
                    // Gets the save system.
                    SaveSystem saveSystem = SystemManager.Instance.saveSystem;

                    // If using the LOL System
                    // NOTE: the data isn't loaded until the continue button is pressed.
                    // As such, the continue button is made interactable even though the save system...
                    // Has no data loaded yet.

                    // New
                    // NOTE: now only gets set interactable if saving/loading is enabled...
                    // And the save system has data to load in.

                    // continueButton.interactable = true;
                    continueButton.interactable = saveSystem.SavingLoadingEnabled && saveSystem.HasLoadedData();

                    // If there is loaded data, make the continue button interactable.
                    // If there is no loaded data, make the continue button non-interactable.
                    // continueButton.interactable = saveSystem.HasLoadedData();

                    // Sets the feedback text.
                    saveSystem.feedbackText = saveText;
                }
                else
                {
                    // The save system doesn't exist, so there's no save data to pull.
                    // As such, the continue button is turned not interactable.
                    continueButton.interactable = false;
                }

                // Removed since the quit button is now always active.
                // Old (LOL): Hide the quit button.
                // quitButton.gameObject.SetActive(false);
            }
            else
            {
                // NOTE: the continue button is disabled because the local saved data is cleared after it's used...
                // TO load the game. As such, if the player loads in a saved game using local data, then backs out...
                // Of the game, the data is gone, meaning the player cannot load that data a second time.
                // You could have it use the last save data, but the data saved to the server would always...
                // Be used instead of that if it's available, even if the last save data is ahead of the server data.
                // It's easier just to keep the continue button completely disabled.

                // Enable the continue button if the save system exists and it has data.
                // Disable it otherwise.
                // if(SaveSystem.Instantiated)
                // {
                //     continueButton.interactable = SaveSystem.Instance.HasLoadedData();
                // }
                // else
                // {
                //     continueButton.interactable = false;
                // }

                // Keep continue button disabled.
                continueButton.interactable = false;

                // Removed since the quit button is now always active.
                // OLD: Show the quit button.
                // quitButton.gameObject.SetActive(true);
            }

            // If game mode select isn't enabled, use the new game button.
            // If it is enable, use the new game mode button.
            newGameButton.gameObject.SetActive(!GameSettings.GAME_MODE_SELECT_ENABLED);
            newGameModeButton.gameObject.SetActive(GameSettings.GAME_MODE_SELECT_ENABLED);

            // Opens the title window at the start.
            OpenTitleDialog();
        }

        // Gets the instance.
        public static TitleUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TitleUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Title UI (singleton)");
                        instance = go.AddComponent<TitleUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Starts the new game.
        public void StartNewGame()
        {
            titleManager.StartNewGame();
        }

        // Continues the game.
        public void ContinueGame()
        {
            titleManager.ContinueGame();
        }


        // OPENING/CLOSING DIALOGS/WINDOWS

        // Opens the given dialog.
        public void OpenDialog(GameObject dialog, bool closeOtherDialogs)
        {
            // If 'true', close all the other dialogs.
            if(closeOtherDialogs)
            {
                CloseAllDialogs();
            }

            dialog.SetActive(true);
        }

        // Closes the given window.
        public void CloseDialog(GameObject dialog)
        {
            dialog.SetActive(false);
        }

        // Closes all dialog boxes (windows).
        public void CloseAllDialogs()
        {
            titleDialog.SetActive(false);
            instructionsDialog.gameObject.SetActive(false);
            settingsDialog.gameObject.SetActive(false);
            licensesDialog.gameObject.SetActive(false);
            gameModeDialog.gameObject.SetActive(false);
        }

        // Opens the title dialog, which closes all the other dialogs.
        public void OpenTitleDialog()
        {
            OpenDialog(titleDialog, true);
        }

        // Opens the instructions dialog.
        public void OpenInstructionsDialog()
        {
            OpenDialog(instructionsDialog.gameObject, true);
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            OpenDialog(settingsDialog.gameObject, true);
        }

        // Opens the licenses dialog.
        public void OpenLicensesDialog()
        {
            OpenDialog(licensesDialog.gameObject, true);
        }

        // Opens the game mode dialog.
        public void OpenGameModeDialog()
        {
            OpenDialog(gameModeDialog.gameObject, true);
        }

        // Other

        // Quits the game.
        public void QuitGame()
        {
            TitleManager.Instance.QuitGame();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}