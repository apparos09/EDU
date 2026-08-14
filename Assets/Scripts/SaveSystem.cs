using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using util;
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

        // Becomes 'true' when the save system is initialized.
        private bool initialized = false;

        // The game data.
        // The last game save. This is only for testing purposes.
        public EDU_GameData lastSave;

        // The data that was loaded.
        public EDU_GameData loadedData;

        // New
        // The file reader.
        public FileReaderBytes fileReader = null;

        // The world manager for the game, which has the save information.
        public WorldManager worldManager;

        // LOL - AutoSave //
        // Added from the ExampleCookingGame. Used for feedback from autosaves.
        WaitForSecondsRealtime feedbackTimer = new WaitForSecondsRealtime(2); // Switched to real-time seconds.
        Coroutine feedbackMethod;
        public TMP_Text feedbackText;

        // The string shown when having feedback.
        private string feedbackString = "Saving Data";

        // New
        // The default saving data.
        private string FEEDBACK_STRING_DEFAULT = "Saving Data";

        // The string key for the feedback.
        private const string FEEDBACK_STRING_KEY = "sve_msg_savingGame";

        // Becomes 'true' when a save is in progress.
        private bool saveInProgress = false;

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
                // If the save system hasn't been initialized, initialize it.
                if (!initialized)
                    Initialize();

                // New
                // If saving and loading is enabled, but the game is in WebGL, disable saving and loading.
                if (savingLoadingEnabled && Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    savingLoadingEnabled = false;
                }

                // Don't destroy the save system on load.
                DontDestroyOnLoad(gameObject);

                // Instance has been made.
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

        // Returns true if the save system has been initialized.
        public bool Initialized
        {
            get { return initialized; }
        }

        // Old, Removed.
        // // Set save and load operations.
        // public void Initialize(Button newGameButton, Button continueButton)
        // {
        //     // Makes the continue button disappear if there is no data to load. 
        //     //Helper.StateButtonInitialize<EDU_GameData>(newGameButton, continueButton, OnLoadData);
        // }

        // Set save and load operations.
        public void Initialize()
        {
            // The result.
            bool result;

            // Checks if the file reader exists.
            if (fileReader == null)
            {
                // Tries to grab component.
                if (!TryGetComponent<FileReaderBytes>(out fileReader))
                {
                    // Add component.
                    fileReader = gameObject.AddComponent<FileReaderBytes>();
                }
            }

            fileReader.filePath = "Assets\\Resources\\Data\\Saves\\";
            fileReader.fileName = "save.dat";

            // Checks if the file exists.
            result = fileReader.FileExists();

            // If the file exists, the save system checks if it's empty.
            if (result)
            {
                // If the file is empty, delete the file.
                bool empty = fileReader.IsFileEmpty();

                // If empty, delete the file.
                if (empty)
                {
                    fileReader.DeleteFile();
                }
                else // Not empty, so try to load the game.
                {
                    LoadGame();
                }

            }

            // Save system has been initialized.
            initialized = true;
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


        // Converts an object to bytes (requires seralizable object) and returns it.
        static public byte[] SerializeObject(object data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, data); // Serialize the data for them emory stream.
            return ms.ToArray();
        }

        // Deserialize the provided object, converting it to an object and returning it.
        static public object DeserializeObject(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            ms.Write(data, 0, data.Length); // Write data.
            ms.Seek(0, 0); // Return to start of data.

            return bf.Deserialize(ms); // return content
        }

        // Checks if a save is in progress.
        public bool IsSaveInProgress()
        {
            return saveInProgress;
        }

        // Saves data. Defaults to asynchronous save.
        public bool SaveGame()
        {
            return SaveGame(true);
        }

        // Saves data.
        public bool SaveGame(bool async)
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

                    // New
                    // The system manager and language manager are instantiated.
                    if (LanguageManager.Instantiated)
                    {
                        // Gets the value.
                        string value = LanguageManager.Instance.GetLanguageText(FEEDBACK_STRING_KEY);

                        // The values don't match, so set the feedback string.
                        if (feedbackString != value)
                            feedbackString = value;

                    }
                }

                // From LOL version. Removed.
                // Send the save state.
                //LOLSDK.Instance.SaveState(savedData);

                // New
                // Checks if save/load should be allowed.
                if (savingLoadingEnabled)
                {
                    // Save to a file.
                    if (async) // Asynchronous save.
                    {
                        success = SaveToFileAsync(savedData);
                    }
                    else // Synchronous save.
                    {
                        success = SaveToFile(savedData);
                    }
                }
                else
                {
                    success = false;
                }

                success = true;
            }
            else // Not initialized.
            {
                Debug.LogError("The SDK has not been initialized. Improper save made.");
                success = false;
            }

            return success;
        }

        // Save the information to a file.
        private bool SaveToFile(EDU_GameData data)
        {
            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Will generate the file if it doesn't exist.
            // // Checks that the file exists.
            // if (!fileReader.FileExists())
            //     return false;

            // Seralize the data.
            byte[] dataArr = SerializeObject(data);

            // Data did not serialize properly.
            if (dataArr.Length == 0)
                return false;

            // Save started.
            saveInProgress = true;

            // Write to the file.
            File.WriteAllBytes(file, dataArr);

            // Save finished.
            saveInProgress = false;

            // Data written successfully.
            return true;
        }

        // Saves the game asynchronously.
        public bool SaveToFileAsync(EDU_GameData data)
        {
            // Checks if the feedback method exists.
            if (feedbackMethod == null)
            {
                feedbackMethod = StartCoroutine(SaveToFileAsyncCourtine(data));
                return true;
            }
            else
            {
                Debug.LogWarning("Save already in progress.");
                return false;
            }
        }


        // From LOL version. Unused.
        // // Called for saving the result.
        // private void OnSaveResult(bool success)
        // {
        //     if (!success)
        //     {
        //         Debug.LogWarning("Saving not successful");
        //         return;
        //     }
        // 
        //     if (feedbackMethod != null)
        //         StopCoroutine(feedbackMethod);
        // 
        // 
        // 
        //     // ...Auto Saving Complete
        //     feedbackMethod = StartCoroutine(Feedback(feedbackString));
        // }
        // 
        // // Feedback while result is saving.
        // IEnumerator Feedback(string text)
        // {
        //     // Only updates the text that the feedback text was set.
        //     if (feedbackText != null)
        //     {
        //         feedbackText.text = text;
        //         feedbackText.gameObject.SetActive(true);
        //     }
        //         
        // 
        //     yield return feedbackTimer;
        // 
        //     // Only updates the content if the feedback text has been set.
        //     if (feedbackText != null)
        //     {
        //         feedbackText.text = string.Empty;
        //         feedbackText.gameObject.SetActive(false);
        //     }
        //         
        // 
        //     // nullifies the feedback method.
        //     feedbackMethod = null;
        // }

        // Refreshes the feedback string.
        public void RefreshFeedbackString()
        {
            //// The language manager.
            //LanguageManager lm = LanguageManager.Instance;

            //// If the language should be translated.
            //if (lm.TranslateAndLanguageSet())
            //{
            //    feedbackString = LanguageManager.Instance.GetLanguageText(FEEDBACK_STRING_KEY);
            //}
            //else
            //{
            //    feedbackString = "Saving Game...";
            //}

            feedbackString = FEEDBACK_STRING_DEFAULT;
        }

        // Refreshes the feedback text.
        public void RefreshFeedbackText()
        {
            // If the text exists.
            if (feedbackText != null)
            {
                // Checks if a save is in progress.
                if (saveInProgress)
                    feedbackText.text = feedbackString;
                else
                    feedbackText.text = string.Empty;
            }
        }

        // Save the information to a file asynchronously (cannot return anything).
        private IEnumerator SaveToFileAsyncCourtine(EDU_GameData data)
        {
            // Save started.
            saveInProgress = true;

            // Show saving text.
            RefreshFeedbackText();

            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Seralize the data.
            byte[] dataArr = SerializeObject(data);

            // Yield return before file wrting begins.
            yield return null;

            // Show saving text in case scene has changed.
            RefreshFeedbackText();

            // Opens the file in the file stream.
            FileStream fs = File.OpenWrite(file);

            // NOTE: this is pretty scuffed, but because of the way it's set up I don't really have a better option.
            // File.WriteAsync would probably be better.

            // Ver. 1
            // // The number of bytes to write, and the offset.
            // int count = 32;
            // int offset = 0;

            // // While there's still bytes to write.
            // while(offset < dataArr.Length)
            // {
            //     // If the count exceeds the amount of remaining bytes, adjust it.
            //     if (offset + count > dataArr.Length)
            //         count = dataArr.Length - offset;
            // 
            //     fs.Write(dataArr, offset, count);
            // 
            //     // Increase the offset.
            //     offset += count;
            // 
            //     // Run other operations.
            //     // yield return null;
            // 
            //     // Pause the courtine for 2 seconds.
            //     yield return feedbackTimer;
            // }

            // Ver. 2 - write the data and suspend for the amount of time set to feedbackTimer.
            fs.Write(dataArr, 0, dataArr.Length);
            yield return feedbackTimer;

            // Show saving text in case scene has changed.
            RefreshFeedbackText();

            // Close the file stream.
            fs.Close();

            // Save finished.
            saveInProgress = false;

            // Hide feedback text now that the save is done.
            RefreshFeedbackText();

            // Save is complete, so set the method to null.
            if (feedbackMethod != null)
                feedbackMethod = null;
        }

        // The gameplay manager now checks if there is loadedData. If so, then it will load in the data when the game starts.
        // Loads a saved game. This returns 'false' if there was no data.
        public bool LoadGame()
        {
            // Loading a save is not allowed, so return false.
            if (!savingLoadingEnabled)
                return false;

            // The result of loading the save data.
            bool success;

            // The file doesn't exist.
            if (!fileReader.FileExists())
            {
                return false;
            }

            // Loads the file.
            loadedData = LoadFromFile();

            // The data has been loaded successfully.
            success = loadedData != null;

            return success;
        }

        // Loads information from a file.
        private EDU_GameData LoadFromFile()
        {
            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Checks that the file exists.
            if (!fileReader.FileExists())
                return null;

            // Read from the file.
            byte[] dataArr = File.ReadAllBytes(file);

            // Data did not serialize properly.
            if (dataArr.Length == 0)
                return null;

            // Deseralize the data.
            object data = DeserializeObject(dataArr);

            // Convert to loaded data.
            EDU_GameData loadData = (EDU_GameData)(data);

            // Return loaded data.
            return loadData;
        }

        // From LOL verison. Unused.
        // // Called to load data from the server.
        // private void OnLoadData(EDU_GameData loadedGameData)
        // {
        //     // Overrides serialized state data or continues with editor serialized values.
        //     if (loadedGameData != null)
        //     {
        //         loadedData = loadedGameData;
        //     }
        //     else // No game data found.
        //     {
        //         // Changed from error to warning since starting a new game always triggers this.
        //         // Debug.LogError("No game data found.");
        //         Debug.Log("No game data found.");
        //         loadedData = null;
        //         return;
        //     }
        // 
        //     // TODO: save data for game loading.
        //     // TODO: why do I check this here? What purpose does this serve.
        //     //if (!IsWorldManagerSet())
        //     //{
        //     //    Debug.LogError("Game gameManager not found.");
        //     //    return;
        //     //}
        // 
        //     // TODO: this automatically loads the game if the continue button is pressed.
        //     // If there is no data to load, the button is gone. 
        //     // You should move the buttons around to accomidate for this.
        //     // LoadGame();
        // }

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
            // Old
            // lastSave = null;
            // loadedData = null;

            // New - doesn't delete the save file.
            ClearLoadedAndLastSaveData(false);
        }

        // Clears out the last save and the loaded data object. Also deletes the file.
        public void ClearLoadedAndLastSaveData(bool deleteFile)
        {
            lastSave = null;
            loadedData = null;

            // If the file should be deleted.
            if (deleteFile)
            {
                // If the file exists, delete it.
                if (fileReader.FileExists())
                {
                    // Checks if a meta file exists so that that can be deleted too.
                    string meta = fileReader.GetFileWithPath() + ".meta";

                    // Delete the main file.
                    fileReader.DeleteFile();

                    // If the meta file exists, delete it.
                    if (File.Exists(meta))
                        File.Delete(meta);
                }
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