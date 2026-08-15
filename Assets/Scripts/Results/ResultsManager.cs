using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The singleton instance.
        private static ResultsManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The results UI.
        public ResultsUI resultsUI;

        // The results audio.
        public ResultsAudio resultsAudio;

        // The title scene.
        public string titleScene = "TitleScene";

        // Constructor
        private ResultsManager()
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
            // If this isn't set, get the instance.
            if(resultsUI == null)
            {
                resultsUI = ResultsUI.Instance;
            }

            // If this isn't set, get the instance.
            if(resultsAudio == null)
            {
                resultsAudio = ResultsAudio.Instance;
            }

            // If the tutorials object still exists, destroy it.
            if(Tutorials.Instantiated)
            {
                Destroy(Tutorials.Instance.gameObject);
            }


            // If the data logger has been instantiated, apply the data and then destroy it.
            if (DataLogger.Instantiated)
            {
                // Gets the instance.
                DataLogger dataLogger = DataLogger.Instance;

                // Applies the results data.
                ApplyResultsData(dataLogger);

                // Destroy.
                Destroy(dataLogger.gameObject);
            }
            else
            {
                // Either apply the test data or clear the data.
                // ApplyResultsTestData();
                ClearResultsData();
            }
        }

        // Gets the instance.
        public static ResultsManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<ResultsManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Results Manager (singleton)");
                        instance = go.AddComponent<ResultsManager>();
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

        // Applies the results data using the provided data logger.
        public void ApplyResultsData(DataLogger dataLogger)
        {
            // If the data logger exists, apply the data.
            if (dataLogger != null)
            {
                resultsUI.ApplyResultsData(dataLogger);
            }
            // Data doesn't exist, so clear it.
            else
            {
                ClearResultsData();
            }
        }

        // Applies debug data to the results manager.
        public void ApplyResultsTestData()
        {
            // Creates a data logger.
            DataLogger dataLogger = DataLogger.Instance;

            // TODO: give test values.

            // Apply results data.
            ApplyResultsData(dataLogger);

            // Destroy the logger.
            Destroy(dataLogger.gameObject);
        }

        // Clears the results data.
        public void ClearResultsData()
        {
            ResultsUI.Instance.ClearResultsData();
        }

        // Goes to the title scene.
        public void ToTitleScene()
        {
            // If the loading scene canvas exists, see if the loading graphic should be used.
            if(EDU_LoadingSceneCanvas.Instantiated)
            {
                // If the loading screen is being used.
                if (EDU_LoadingSceneCanvas.Instance.IsUsingLoadingGraphic())
                {
                    EDU_LoadingSceneCanvas.Instance.LoadScene(titleScene);
                }
                else
                {
                    SceneManager.LoadScene(titleScene);
                }
            }
            // No loading screen, so load the scene normally.
            else
            {
                SceneManager.LoadScene(titleScene);
            }
        }

        // Completes the game.
        public void CompleteGame()
        {
            // Now goes to the title screen no matter what.
            
            // // The SDK has been initialized.
            // if (SystemManager.Instantiated)
            // {
            //     // Complete the game (LOL only).
            //     // LOLSDK.Instance.CompleteGame();
            // 
            //     // Just return to the tilte screen.
            //     ToTitleScene();
            // }
            // else
            // {
            //     // Commented out since the game since it now goes to the title screen no matter what.
            //     // Logs the error.
            //     // Debug.LogError("SDK NOT INITIALIZED. RETURNING TO THE TITLE SCREEN.");
            // 
            //     // Return to the main menu scene.
            //     ToTitleScene();
            // }

            // If the save system is instantiated, load the save file.
            // This is done to make sure the player can continue from a saved game...
            // Once the game goes back to the title screen.
            // Otherwise, the player would have to reset the game to go back to their last save.
            if(SaveSystem.Instantiated)
            {
                // Gets the instance.
                SaveSystem saveSystem = SaveSystem.Instance;

                // If there's no loaded data and no last save data, try loading the game.
                // This function checks for saving and loading being enabled.
                // It also checks if the save file exists.
                if(!saveSystem.HasLoadedData() && !saveSystem.HasLastSaveData())
                {
                    SaveSystem.Instance.LoadGame();
                }
            }

            // Do not return to the title scene if running through the LOL platform.
            // This is because you can't have the game get repeated in the same session.
            ToTitleScene();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}