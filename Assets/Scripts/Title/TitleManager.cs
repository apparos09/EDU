using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // Manages the title scene.
    public class TitleManager : MonoBehaviour
    {
        // The singleton instance.
        private static TitleManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The title screen UI.
        public TitleUI titleUI;

        // The title audio for the game.
        public TitleAudio titleAudio;

        // The scene loaded when start is selected.
        public string startScene = "WorldScene";

        // Gets set to 'true' when late start is called.
        private bool calledLateStart = false;

        // Constructor
        private TitleManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
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

                // Checks if LOL SDK has been initialized.
                GameSettings settings = GameSettings.Instance;

                // Gets an instance of the LOL manager.
                SystemManager lolManager = SystemManager.Instance;

                // Language
                // JSONNode defs = SharedState.LanguageDefs;

                // // Translate text.
                // if (defs != null)
                // {
                //     // ...
                // }
                // else
                // {
                //     // Mark all of the text.
                //     LanguageMarker marker = LanguageMarker.Instance;
                // 
                //     // ...
                // }

                // Checks for initialization
                // if (LanguageManager.Instantiated)
                if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
                {
                    // NOTE: the buttons disappear for a frame if there is no save state.
                    // It doesn't effect anything, but it's jarring visually.
                    // As such, the Update loop keeps them both on.

                    // Set up the game initializations.
                    if (titleUI.newGameButton != null && titleUI.continueButton != null)
                        lolManager.saveSystem.Initialize(titleUI.newGameButton, titleUI.continueButton);

                    // Don't disable the continue button, otherwise the save data can't be loaded.
                    // Enables/disables the continue button based on if there is loaded data or not.
                    // continueButton.interactable = lolManager.saveSystem.HasLoadedData();
                    // Continue button is left alone.

                    // Since the player can't change the tutorial settings anyway when loaded from InitScene...
                    // These are turned off just as a safety precaution. 
                    // This isn't needed since the tutorial is activated by default if going from InitScene...
                    // And can't be turned off.
                    // overrideTutorial = true;
                    // continueTutorial = true;

                    // LOLSDK.Instance.SubmitProgress();
                }
                else
                {
                    // Debug.LogError("LOL SDK NOT INITIALIZED.");
                    Debug.LogError("SYSTEM MANAGER NOT INITIALIZED.");

                    // You can save and go back to the menu, so the continue button is usable under that circumstance.
                    if (lolManager.saveSystem.HasLoadedData()) // Game has loaded data.
                    {
                        // TODO: manage tutorial content.
                    }
                    else // No loaded data.
                    {
                        // TODO: manage tutorial content.
                    }

                    // TODO: do you need this?
                    // // Have the button be turned on no matter what for testing purposes.
                    // titleUI.continueButton.interactable = true;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Finds the instance if it isn't set.
            if (titleUI == null)
            {
                titleUI = TitleUI.Instance;
            }
                
            // Finds the instance of the title audio.
            if (titleAudio == null)
            {
                titleAudio = TitleAudio.Instance;
            }

            // If the data logger exists, destroy it.
            // This is to account for going from the gameplay scenes to the title scene.
            if(DataLogger.Instantiated)
            {
                Destroy(DataLogger.Instance.gameObject);
            }

            // If the tutorials object still exists, destroy it.
            // This makes sure that the tutorials are properly reset if the user starts a new game...
            // After coming back from a saved game.
            if (Tutorials.Instantiated)
            {
                Destroy(Tutorials.Instance.gameObject);
            }
        }

        // Called on the first update frame.
        public void LateStart()
        {
            // Late start has been called.
            calledLateStart = true;

            // Makes sure the audio is adjusted to the current settings.
            // For some reason, this doesn't happen properly in the LOL harness when done in Start()...
            // By other scripts, so it's done here to make another correction.
            // Opening the settings window automatically adjusts the audio levels...
            // So doing this in the title script should be fine.
            GameSettings.Instance.AdjustAllAudioLevels();
        }

        // Gets the instance.
        public static TitleManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TitleManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Title Manager (singleton)");
                        instance = go.AddComponent<TitleManager>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Starts the game (general function for moving to the GameScene).
        public void StartGame()
        {
            // Creates the data logger and makes sure it won't be destroyed on load.
            // It will get destroyed in the results scene once all data is gotten from it.
            DataLogger dataLogger = DataLogger.Instance;
            DontDestroyOnLoad(dataLogger);

            // If the loading screen has been instnatiated.
            if (EDU_LoadingSceneCanvas.Instantiated)
            {
                // If the loading screen is being used, use it to load the scene.
                if (EDU_LoadingSceneCanvas.Instance.IsUsingLoadingGraphic())
                {
                    EDU_LoadingSceneCanvas.Instance.LoadScene(startScene);
                }
                else // The loading screen is not being used, so load the scene without it.
                {
                    SceneManager.LoadScene(startScene);
                }
            }
            else
            {
                SceneManager.LoadScene(startScene);
            }
        }

        // Starts a new game.
        public void StartNewGame()
        {
            // Clear out the loaded data and last save if the save system has been initialized.
            ClearLastSaveAndLoadedData();

            // Start the game.
            StartGame();
        }

        // Continues a saved game.
        public void ContinueGame()
        {
            // New
            // NOTE: a callback is setup onclick to load the save data.
            // Since that might happen after this function is processed...
            // Loaded data does not need to be checked for at this stage.

            //// If the user's tutorial settings should be overwritten, do so.
            //if (overrideTutorial)
            //    GameSettings.Instance.UseTutorial = continueTutorial;

            // Starts the game.
            StartGame();
        }


        // Clears out the save.
        public void ClearLastSaveAndLoadedData()
        {
            // If the save system has been instantiated, clear the save.
            if(SaveSystem.Instantiated)
            {
                SaveSystem.Instance.lastSave = null;
                SaveSystem.Instance.loadedData = null;

                titleUI.continueButton.interactable = false;
            }
        }

        // Quits the game (will not be used in LOL version).
        public void QuitGame()
        {
            Application.Quit();
        }

        // Update is called once per frame
        void Update()
        {
            // Call late start.
            if (!calledLateStart)
            {
                LateStart();
            }
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