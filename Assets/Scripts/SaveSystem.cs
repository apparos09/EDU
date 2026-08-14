using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // The save system for the game.
    [System.Serializable]
    public class EDU_GameData
    {
        // Shows if the game data is valid.
        public bool valid = false;

        // Gets set to 'true' if the game was completed.
        public bool complete = false;

        // The game mode of the saved game.
        public GameSettings.gameMode gameMode;

        // The game time
        public float gameTime = 0;

        // The player's overall score.
        public float gameScore = 0;

        // The total amount of energy generated for the game.
        public float gameEnergyTotal = 0;

        // The total amount of air pollution for the game.
        public float gameAirPollution = 0;

        // The current area index.
        public int currentAreaIndex = 0;

        // The stage datas.
        public WorldStage.WorldStageData[] worldStageDatas = new WorldStage.WorldStageData[WorldManager.STAGE_COUNT];

        // The used resources. If the bool is true, then the resource has been used. If false, ith asn't been used.
        public bool[] usedResources = new bool[NaturalResources.NATURAL_RESOURCE_COUNT];

        // The defense ids. A true value means the id that lines up with the index is unlocked. False menas locked.
        public bool[] defenseIds = new bool[ActionUnitDefense.DEFENSE_ID_COUNT];

        // The starting energy bonus, which is used for action stages.
        public float energyStartBonus = 0.0F;

        // To avoid problems, the tutorial parameter cannot be changed for a saved game.
        public bool useTutorial = true;

        // Tutorial Clears
        public Tutorials.TutorialsData tutorialData;
    }

    // Used to save the game.
    public class SaveSystem : MonoBehaviour
    {
        // The instance of Save System
        private static SaveSystem instance;

        // Becomes 'true' when the save system is instanced.
        private static bool instanced = false;

        // The game data.
        // The last game save. This is only for testing purposes.
        public EDU_GameData lastSave;

        // The data that was loaded.
        public EDU_GameData loadedData;

        // The world manager for the game, which has the save information.
        public WorldManager worldManager;

        // LOL - AutoSave //
        // Added from the ExampleCookingGame. Used for feedback from autosaves.
        WaitForSecondsRealtime feedbackTimer = new WaitForSecondsRealtime(2); // Switched to real-time seconds.
        Coroutine feedbackMethod;
        public TMP_Text feedbackText;

        // The string shown when having feedback.
        private string feedbackString = "Saving Data";

        // The string key for the feedback.
        private const string FEEDBACK_STRING_KEY = "sve_msg_savingGame";

        // Other
        // Determines if saving and loading is enabled.
        private bool savingLoadingEnabled = true;

        // Private constructor so that only one save system object exists.
        private SaveSystem()
        {
            // ...
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
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
                // The initialization happens in the start function.
                // Initialize the save system.
                // Initialize();

                // New
                // If saving and loading is enabled, but the game is in WebGL, disable saving and loading.
                if(savingLoadingEnabled && Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    savingLoadingEnabled = false;
                }

                // Don't destroy the save system on load.
                DontDestroyOnLoad(gameObject);

                instanced = true;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            // Original from LOL
            //// Sets the save result to the instance.
            //LOLSDK.Instance.SaveResultReceived += OnSaveResult;

            //// Gets the language definition.
            //JSONNode defs = SharedState.LanguageDefs;

            //// Sets the save complete text.
            //if (defs != null)
            //    feedbackString = defs[FEEDBACK_STRING_KEY];

            // New
            // If language translation is active, translate the string.
            if(SystemManager.IsInstantiatedAndIsLanguageLoaded())
            {
                feedbackString = SystemManager.GetLanguageTextStatic(FEEDBACK_STRING_KEY);
            }
        }

        // Gets the instance.
        public static SaveSystem Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<SaveSystem>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Save System (singleton)");
                        instance = go.AddComponent<SaveSystem>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Set save and load operations.
        public void Initialize(Button newGameButton, Button continueButton)
        {
            // Makes the continue button disappear if there is no data to load. 
            //Helper.StateButtonInitialize<EDU_GameData>(newGameButton, continueButton, OnLoadData);
        }


        // Saving Loading Enabled
        public bool SavingLoadingEnabled
        {
            get
            {
                return savingLoadingEnabled;
            }

            set
            {
                savingLoadingEnabled = value;
            }
        }


        // Checks if the world manager has been set.
        private bool IsWorldManagerSet()
        {
            // Tries to set the world manager if it isn't saved, but it has been instantiated.
            if (worldManager == null && WorldManager.Instantiated)
            {
                worldManager = WorldManager.Instance;
            }

            // Checks if the world manager has been set and returns the result.
            if (worldManager == null)
            {
                Debug.LogWarning("The World Manager couldn't be found.");
                return false;
            }

            return true;
        }

        // Returns 'true' if the game has last save data.
        public bool HasLastSaveData()
        {
            return lastSave != null;
        }

        // Sets the last bit of saved data to the loaded data object.
        public void SetLastSaveAsLoadedData()
        {
            loadedData = lastSave;
        }

        // Clears the last save data.
        public void ClearLastSaveData()
        {
            lastSave = null;
        }

        // Saves data.
        public bool SaveGame()
        {
            // The game manager does not exist if false.
            if (!IsWorldManagerSet())
            {
                Debug.LogWarning("The WorldManager couldn't be found.");
                return false;
            }

            // Determines if saving wa a success.
            bool success = false;

            // Generates the save data.
            EDU_GameData savedData = worldManager.GenerateSaveData();

            // Stores the most recent save.
            lastSave = savedData;

            // Sets the last save as the loaded data.
            SetLastSaveAsLoadedData();

            // If the instance has been initialized.
            if (SystemManager.Instantiated)
            {
                // Makes sure that the feedback string is set.
                if (FEEDBACK_STRING_KEY != string.Empty)
                {
                    // LOL Version
                    // Gets the language definition.
                    //JSONNode defs = SharedState.LanguageDefs;

                    //// Sets the feedback string if it wasn't already set.
                    //if (feedbackString != defs[FEEDBACK_STRING_KEY])
                    //    feedbackString = defs[FEEDBACK_STRING_KEY];

                    // The system manager and language manager are instantiated.
                    if(LanguageManager.Instantiated)
                    {
                        // Gets the value.
                        string value = LanguageManager.Instance.GetLanguageText(FEEDBACK_STRING_KEY);

                        // The values don't match, so set the feedback string.
                        if(feedbackString != value)
                            feedbackString = value;

                    }
                }

                // From LOL version.
                // Send the save state.
                //LOLSDK.Instance.SaveState(savedData);

                success = true;
            }
            else // Not initialized.
            {
                Debug.LogError("The SDK has not been initialized. Improper save made.");
                success = false;
            }

            return success;
        }

        // Called for saving the result.
        private void OnSaveResult(bool success)
        {
            if (!success)
            {
                Debug.LogWarning("Saving not successful");
                return;
            }

            if (feedbackMethod != null)
                StopCoroutine(feedbackMethod);



            // ...Auto Saving Complete
            feedbackMethod = StartCoroutine(Feedback(feedbackString));
        }

        // Feedback while result is saving.
        IEnumerator Feedback(string text)
        {
            // Only updates the text that the feedback text was set.
            if (feedbackText != null)
            {
                feedbackText.text = text;
                feedbackText.gameObject.SetActive(true);
            }
                

            yield return feedbackTimer;

            // Only updates the content if the feedback text has been set.
            if (feedbackText != null)
            {
                feedbackText.text = string.Empty;
                feedbackText.gameObject.SetActive(false);
            }
                

            // nullifies the feedback method.
            feedbackMethod = null;
        }

        // Checks if the game has loaded data.
        // checkValid: if 'true', the data is checked for validity. If the data is invalid, this returns false.
        public bool HasLoadedData(bool checkValid = true)
        {
            // Used to see if the data is available.
            bool result;

            // Checks to see if the data exists.
            if (loadedData != null) // Exists.
            {
                // Checks to see if the data is valid.
                // If validity isn't being checked, return true regardless.
                result = (checkValid) ? loadedData.valid : true;
            }
            else // No data.
            {
                // Not readable.
                result = false;
            }

            // Returns the result.
            return result;
        }

        // Removes the loaded data.
        public void ClearLoadedData()
        {
            loadedData = null;
        }

        // Clears out the last save and the loaded data object.
        public void ClearLoadedAndLastSaveData()
        {
            lastSave = null;
            loadedData = null;
        }

        // The gameplay manager now checks if there is loadedData. If so, then it will load in the data when the game starts.
        // // Loads a saved game. This returns 'false' if there was no data.
        // public bool LoadGame()
        // {
        //     // No loaded data.
        //     if(loadedData == null)
        //     {
        //         Debug.LogWarning("There is no saved game.");
        //         return false;
        //     }
        // 
        //     // TODO: load the game data.
        // 
        //     return true;
        // }

        // Called to load data from the server.
        private void OnLoadData(EDU_GameData loadedGameData)
        {
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedGameData != null)
            {
                loadedData = loadedGameData;
            }
            else // No game data found.
            {
                // Changed from error to warning since starting a new game always triggers this.
                // Debug.LogError("No game data found.");
                Debug.Log("No game data found.");
                loadedData = null;
                return;
            }

            // TODO: save data for game loading.
            // TODO: why do I check this here? What purpose does this serve.
            //if (!IsWorldManagerSet())
            //{
            //    Debug.LogError("Game gameManager not found.");
            //    return;
            //}

            // TODO: this automatically loads the game if the continue button is pressed.
            // If there is no data to load, the button is gone. 
            // You should move the buttons around to accomidate for this.
            // LoadGame();
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