using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RM_EDU
{
    // The manager for the world scene.
    public class WorldManager : GameplayManager
    {
        // the instance of the class.
        private static WorldManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage count.
        // Build V03: this has been changed from 11 to 9 to reflect the reduction in stages.
        // Build V04: the stage count has been changed from 9 to 8 to reflect the reduction in stages.
        //  - The game needs a minimum of 8 progress points, so it can't go lower than this.
        public const int STAGE_COUNT = 8;

        // If saving and loading is enabled.
        // NOTE: set this to true when you have the tutorial set up.
        private bool savingLoadingEnabled = true;

        // If 'true', auto saving is enabled.
        private bool autoSavingEnabled = true;

        // If 'true', the game auto saves when the game ends.
        // If false, the game doesn't save before the game ends.
        // NOTE: the game doesn't save when the game ends as per a new LOL requirement.
        private bool autoSaveOnGameEnd = false;

        // If 'true', auto saving happens in late start.
        // If false, it happens in InitializeWorld().
        private bool autoSaveInLateStart = true;

        [Header("World")]

        // The world UI.
        public WorldUI worldUI;

        // The world audio.
        public WorldAudio worldAudio;

        // The world camera.
        public WorldCamera worldCamera;

        // If 'true', the camera moves gradually from one area to the next.
        // If 'false', the camera moves instantly from one area to the next.
        private bool instantCameraMovement = true;

        // The areas.
        public List<WorldArea> areas = new List<WorldArea>();

        // The current area.
        private int currAreaIndex = 0;

        // If 'true', the state of the game effects the area buttons.
        // If 'false', the area buttons remain on at all times.
        private bool effectAreaButtons = true;

        // Refreshes the area buttons in a given set of wait frames.
        // Use this to delay the area buttons being refreshed to fix an error...
        // Where the area buttons aren't set properly.
        [HideInInspector]
        public int refreshAreaButtonsWaitFrames = 0;

        // The world stages.
        public List<WorldStage> stages = new List<WorldStage>();

        // If no stages have been completed, disable the info log.
        private bool disableInfoLogIfNoStagesComplete = true;

        // The default defense ids that the player has.
        [Tooltip("The defense ids that are set by default.")]
        public List<int> defaultDefenseIds = new List<int>();

        // The name of generation game mode, and its translation key.
        private string gameModeGenName = "Generation";
        private string gameModeGenNameKey = "kwd_generation";
        private bool gameModeGenTranslated = false;

        // The name of defense game mode, and its translation key.
        private string gameModeDefName = "Defense";
        private string gameModeDefNameKey = "kwd_defense";
        private bool gameModeDefTranslated = false;

        // Set to 'true' if the world has been initialized.
        protected bool worldInitialized = false;

        // Gets set to 'true' when tutorials have been checked. Checks for tutorials in Update().
        [Tooltip("Becomes true when tutorials have been checked. Set to false to check for tutorials in Update().")]
        public bool checkedTutorials = false;

        // If 'true', complete is called in the update loop. This is to fix a problem where the loading screen...
        // Wasn't triggering the game end state the first time.
        [Tooltip("Calls the CompleteGame() function in Update() to end the game.")]
        public bool callCompleteGameInLateUpdate = false; 

        [Header("World/Events")]

        // The game complete event of the world manager.
        public GameCompleteEvent gameCompleteEvent;

        // Constructor
        private WorldManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();

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
        protected override void Start()
        {
            base.Start();

            // Gets the world UI instance.
            if (worldUI == null)
            {
                worldUI = WorldUI.Instance;
            }

            // Gets the world audio instance.
            if(worldAudio == null)
            {
                worldAudio = WorldAudio.Instance;
            }

            // If the world camera isn't set, try to get the world camera...
            // From the main camera.
            if(worldCamera == null)
            {
                worldCamera = Camera.main.GetComponent<WorldCamera>();
            }

            // If the game complete event isn't set, try to grab the component.
            if(gameCompleteEvent == null)
            {
                gameCompleteEvent = GetComponent<GameCompleteEvent>();
            }

            // Finds all the stages in the scene if the list has none.
            if (areas.Count <= 0)
            {
                areas.Clear();
                areas.AddRange(FindObjectsOfType<WorldArea>());
            }

            // Finds all the stages in the scene if the list has none.
            if(stages.Count <= 0)
            {
                stages.Clear();
                stages.AddRange(FindObjectsOfType<WorldStage>());
            }

            // Initializes the world.
            InitializeWorld();

            // Will be made false in late start to make sure everything else is set.
            checkedTutorials = true;
        }

        // The late start function.
        protected override void LateStart()
        {
            base.LateStart();

            // NOTE: was causing the current area to reset to area 0.
            // Refreshes the current world area.
            // This also refreshes the area arrow buttons if that setting is on.
            // RefreshCurrentWorldArea();
            worldUI.RefreshWorldAreaButtons();

            // If the info log button should be disabled if no stages are complete.
            if(disableInfoLogIfNoStagesComplete)
            {
                // If a stage has been completed, make the button interactable.
                // If a stage hasn't been complete,d make the button non-interactable.
                worldUI.infoLogButton.interactable = HasCompletedAStage();
            }

            // If saving and auto saving is enabled, and the game should auto save in late start...
            // Save the game.
            if(IsSavingAndAutoSavingEnabled() && autoSaveInLateStart)
            {
                // If autoSaveOnGameEnd is true, save no matter what.
                // If autoSaveOnGameEnd is false, only save if not all stages are complete.
                // If a save should be performed.
                if(autoSaveOnGameEnd || (!autoSaveOnGameEnd && !AllStagesComplete()))
                {
                    // Gets the save system.
                    SaveSystem saveSystem = SaveSystem.Instance;

                    // Auto save the game.
                    SaveGame();

                    // If the save system is set, that means it's been instantiated.
                    // Clears the loaded data so that it doesn't take priority over the data logger.
                    // This is a holdover from the auto save call in Initialize(), but it's also done here...
                    // To be safe.
                    if (saveSystem != null)
                        saveSystem.ClearLoadedAndLastSaveData();
                }
            }

            // Check for some tutorials.
            checkedTutorials = false;
        }

        // Gets the instance.
        public static WorldManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<WorldManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldManager (singleton)");
                        instance = go.AddComponent<WorldManager>();
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

        // Initializes the world.
        public void InitializeWorld()
        {
            // Gets the data logger instance.
            // The data logger should already be set.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // The save system.
            // If instantiated, get the instance. If not instantiated, set to null.
            SaveSystem saveSystem = SaveSystem.Instantiated ? SaveSystem.Instance : null;

            // Set to false by default. Only set to true if returning from a completed stage.
            autoSaveInLateStart = false;

            // Loading Data
            // Gets set to true if save data was loaded.
            bool saveDataLoaded = false;

            // The index for the area the player should be in for the world.
            // This is used to fix an issue where the camera isn't in the right area...
            // When the player comes back from a saved game or a world stage.
            int initAreaIndex = 0;

            // If there's save data to load in, try to load it in.
            if(saveSystem != null)
            {
                // If the save system has loaded data, load the game.
                if(saveSystem.HasLoadedData())
                {
                    // Gets the world area index from the loaded data.
                    // This is done here since the loaded data is cleared after...
                    // It's applied to the game in the function below.
                    int loadedDataAreaIndex = saveSystem.loadedData.currentAreaIndex;

                    // Loads the game.
                    saveDataLoaded = LoadGame(true);

                    // If save data was loaded, set the init area index from the saved data.
                    if(saveDataLoaded)
                    {
                        initAreaIndex = loadedDataAreaIndex;
                    }

                    // Calls again in case the function was unsuccessful.
                    saveSystem.ClearLoadedAndLastSaveData();
                }
            }

            // If save data was loaded in, don't load the data from the data logger.
            if(saveDataLoaded)
            {
                // Save all the world datas since they've now been added from the save manager.
                dataLogger.SaveWorldStageDatas(this);
            }
            // Save data not loaded, so use the data logger.
            else
            {
                // Applies the data logger's world datas to the world.
                dataLogger.ApplyWorldStageDatasToWorld(this);
            }
            
            // Tries to find the start info.
            WorldStartInfo startInfo = FindObjectOfType<WorldStartInfo>();

            // If start info was found, apply the data and destroy the info object.
            if (startInfo != null)
            {
                // Apply the start info.
                startInfo.ApplyStartInfo(this);

                // If coming from the stage, get that as the area index.
                // If there's also save data, this overrides the value...
                // Gotten from the save data.
                if(startInfo.fromStage)
                {
                    // Gets the world stage's area index.
                    initAreaIndex = stages[startInfo.worldStageIndex].GetWorldStageAreaIndex();
                }

                // If saving/loading is enabled and auto saving is enabled, do an auto save.
                // if(savingLoadingEnabled && autoSavingEnabled) // Now a dedicated function.
                if (IsSavingAndAutoSavingEnabled())
                {
                    // If the player returned from a stage, and that stage was completed, auto save.
                    if (startInfo.fromStage && startInfo.stageCompleted)
                    {
                        // If the stage was completed and the save system is available, auto save.
                        // Auto saving only happens when coming back from a completed stage.
                        autoSaveInLateStart = startInfo.stageCompleted;

                        // Since auto saving is happening in late start, don't do it here.
                        // // If the game won't auto save in late start, do it here.
                        // if (!autoSaveInLateStart)
                        // {
                        //     // If you uncomment the code below, make sure not to auto save again in late start.
                        //     // autoSaveInLateStart = false;
                        //     // Auto save the game.
                        //     SaveGame();                            
                        // }

                        // If the save system is set, that means it's been instantiated.
                        // Clears the loaded data so that it doesn't take priority over the data logger.
                        if (saveSystem != null)
                            saveSystem.ClearLoadedAndLastSaveData();
                    }
                }

                // Check for tutorials in Late Start.
                // // If tutorials are being used.
                // if (IsUsingTutorials())
                // {
                //     // Set to true if tutorials should be checked.
                //     bool checkTutorials = tutorials.Data.clearedFirstActionCompleteTutorial || 
                //         tutorials.Data.clearedFirstKnowledgeCompleteTutorial;
                // 
                //     // If the start info came from a stage, and the stage was completed.
                //     if (checkTutorials && startInfo.fromStage && startInfo.stageCompleted)
                //     {
                //         // Gets the return stage.
                //         WorldStage returnStage = GetWorldStage(startInfo.worldStageIndex);
                // 
                //         // Gets the stage type.
                //         switch(returnStage.GetStageType())
                //         {
                //             case WorldStage.stageType.action:
                //                 
                //                 // Trigger action stage complete tutorial.
                //                 if(!tutorials.Data.clearedFirstActionCompleteTutorial)
                //                 {
                //                     tutorials.LoadFirstActionCompleteTutorial();
                //                 }
                //                 break;
                // 
                //             case WorldStage.stageType.knowledge:
                //                 // Trigger knowledge stage complete tutorial.
                //                 if (!tutorials.Data.clearedFirstKnowledgeCompleteTutorial)
                //                 {
                //                     tutorials.LoadFirstKnowledgeCompleteTutorial();
                //                 }
                // 
                //                 break;
                //         }
                //     }
                // }

                // Destroy the start info.
                Destroy(startInfo.gameObject);
            }
            else
            {
                // If save data wasn't loaded, and no start info was used...
                // Go to the first area.
                if(!saveDataLoaded)
                {
                    // The current area index and the init area index.
                    currAreaIndex = 0;
                    initAreaIndex = 0;

                    // Sets the current area to be the first one.
                    SetCurrentWorldArea(currAreaIndex);
                }

                // If area buttons should be effected.
                if(effectAreaButtons)
                {
                    // Makes sure the world UI's world manager is set.
                    if (worldUI.worldManager == null)
                        worldUI.worldManager = this;

                    // Refreshes the world area buttons.
                    worldUI.RefreshWorldAreaButtons();
                }
            }

            // Gives the data logger the default defense ids if there are ones.
            if (dataLogger.defenseIds.Count <= 0 && defaultDefenseIds.Count > 0)
            {
                // Removes duplicates and sorts the list.
                List<int> tempList = defaultDefenseIds.Distinct().ToList();
                tempList.Sort();

                // Adds the default defense untis to the list.
                dataLogger.defenseIds.Clear();
                dataLogger.defenseIds.AddRange(tempList);
            }

            // Calculates and sets the game score.
            CalculateAndSetGameScore();

            // Save the game score to the data logger.
            dataLogger.gameScore = gameScore;

            // Updates the game mode display.
            worldUI.UpdateGameModeDisplay();

            // Updates the energy start bonus display.
            worldUI.UpdateEnergyStartBonusDisplay();

            // Submits progress for the game.
            SubmitProgress();

            // Sets the current world area using the init area index.
            // This is to fix a problem where the camera wasn't in the right area...
            // Upon loading a saved game or returning from a world stage.
            SetCurrentWorldArea(initAreaIndex);

            // Refreshes the current world area.
            // This happens in late start.
            // RefreshCurrentWorldArea();

            // Checks that the world audio is set for playing music.
            if(worldAudio != null)
            {
                // If all stages are complete, don't play the world BGM.
                // The game complete dialog will be opened, which will trigger the results BGM.
                if (AllStagesComplete())
                {
                    // If the world bgm is playing, stop the BGM so that it isn't heard...
                    // Before switching over the game results BGM.
                    if (worldAudio.IsPlayingWorldBgm())
                    {
                        worldAudio.StopBackgroundMusic();
                    }
                }
                // Not all stages are complete, so play the world BGM.
                else
                {
                    worldAudio.PlayWorldBgm();
                }
            }

            // The world has been intialized.
            worldInitialized = true;
        }

        // Returns 'true' if the world has been initialized.
        public bool WorldInitialized
        {
            get { return worldInitialized; }
        }

        // GAME MODE TEXT
        // Gets the game mode text.
        public string GetGameModeTextTranslated()
        {
            // Gets the generation mode and the related value.
            bool genMode = GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation;
            string value = genMode ? GetGameModeGenerationTextTranslated() : GetGameModeDefenseTextTranslated();

            return value;
        }

        // Gets the generation mode text translated.
        public string GetGameModeGenerationTextTranslated()
        {
            // Not already translated.
            if(!gameModeGenTranslated)
            {
                // If available, translate text.
                if(SystemManager.IsInstantiatedAndIsLanguageLoaded())
                {
                    gameModeGenName = SystemManager.GetLanguageTextStatic(gameModeGenNameKey);
                }

                // Text now translated.
                gameModeGenTranslated = true;
            }

            // Return gen mode name.
            return gameModeGenName;
        }

        // Gets the defense mode text translated.
        public string GetGameModeDefenseTextTranslated()
        {
            // Not already translated.
            if (!gameModeDefTranslated)
            {
                // If available, translate text.
                if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
                {
                    gameModeDefName = SystemManager.GetLanguageTextStatic(gameModeDefNameKey);
                }

                // Text now translated.
                gameModeDefTranslated = true;
            }

            // Return def mode name.
            return gameModeDefName;
        }

        // SAVING/LOADING
        // Generates the save data for the game.
        public EDU_GameData GenerateSaveData()
        {
            // The game data to return.
            EDU_GameData data = new EDU_GameData();

            // If the data logger isn't set, grab the instance.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // Saves the gameplay mode.
            data.gameMode = GameSettings.Instance.gameplayMode;

            // Gets the game score, game time, game energy, and game air pollution.
            data.gameScore = dataLogger.gameScore;
            data.gameTime = dataLogger.gameTimer;
            data.gameEnergyTotal = CalculateEnergyTotal();
            data.gameAirPollution = CalculateAirPollutionTotal();

            // Saves the current area index.
            data.currentAreaIndex = GetCurrentWorldAreaIndex();


            // World Stages
            // Gets the world stage data for every stage.
            for (int i = 0; i < data.worldStageDatas.Length && i < stages.Count; i++)
            {
                // The stage exists.
                if (stages[i] != null)
                {
                    data.worldStageDatas[i] = stages[i].GenerateWorldStageData();
                }
                // The stage doesn't exist.
                else
                {
                    data.worldStageDatas[i] = null;
                }
            }

            // Natural Resources
            // Sets the natural resource as used or unused.
            for(int i = 0; i < data.usedResources.Length; i++)
            {
                // The resource number.
                NaturalResources.naturalResource resNum = (NaturalResources.naturalResource)i;

                // Checks the used resources list in the data logger to see if it contains the resource.
                // If it does, mark that the resource has been used. If it doesn't, mark that it hasn't been used.
                data.usedResources[i] = dataLogger.usedResources.Contains(resNum) ? true : false;
            }

            // Defense Ids
            // Sets the defense ids locked or unlocked.
            for (int i = 0; i < data.defenseIds.Length; i++)
            {
                // The value (i) matches up with the defense id. The game checks if the defense id...
                // Is in the data logger's defense ids list. If it is, this defense is unlocked.
                // If it isn't, it's locked.
                data.defenseIds[i] = dataLogger.defenseIds.Contains(i) ? true : false;
            }

            // Starting Energy Bonus
            data.energyStartBonus = dataLogger.energyStartBonus;

            // Tutorial parameter.
            data.useTutorial = GameSettings.Instance.UseTutorials;

            // Generates the tutorial data.
            data.tutorialData = tutorials.GenerateTutorialsDataCopy();

            // Saves if the game is complete.
            data.complete = IsGameComplete();

            // The data is valid.
            data.valid = true;

            // Returns the result.
            return data;
        }

        // Checks if saving and loading is allowed.
        public bool SavingLoadingEnabled
        {
            get 
            { 
                return savingLoadingEnabled; 
            }
        }

        // Sets if the game should be auto saving.
        public bool AutoSavingEnabled
        {
            get
            {
                return autoSavingEnabled;
            }
        }

        // Returns true if saving and auto saving are enabled.
        public bool IsSavingAndAutoSavingEnabled()
        {
            return savingLoadingEnabled && autoSavingEnabled;
        }

        // The game already mentions if an improper save is made...
        // So checking if the game can save beforehand is unnecessary.
        // // Returns 'true' if the game can save.
        // public bool CanSave()
        // {
        //     return SaveSystem.Instantiated && savingLoadingEnabled;
        // }

        // Saves the data for the game.
        public bool SaveGame()
        {
            // First checks if saving/loading is enabled.
            if(!savingLoadingEnabled)
            {
                Debug.LogError("Saving and loading data is disabled. Save failed.");
                return false;
            }

            // Not needed since the save system is checke for anyway.
            // // Checks if the LOL Manager and the LOLSDK exists.
            // if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            // {
            //     Debug.LogError("The LOLSDK has not been initialized.");
            //     return false;
            // }

            // The save system hasn't bene instantiated.
            if(!SaveSystem.Instantiated)
            {
                Debug.LogError("The save system has not been instantiated.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSystem = SaveSystem.Instance;

            // Sets the world manager for the save system.
            saveSystem.worldManager = this;

            // Save the game.
            bool result = saveSystem.SaveGame();

            // Returns the result.
            return result;
        }

        // Saves and continues the game.
        public bool SaveAndContinue()
        {
            // Saves the game.
            bool result = SaveGame();

            // Closes all the dialogs.
            worldUI.CloseAllDialogs();

            // Returns the result.
            return result;
        }

        // Saves and quits the game.
        public bool SaveAndQuit()
        {
            // Saves the game.
            bool result = SaveGame();

            // Closes all the dialogs.
            worldUI.CloseAllDialogs();

            // Loads the title screen.
            LoadTitleScene();

            // Returns the result.
            return result;
        }

        // Quit the game without saving.
        public void QuitWithoutSaving()
        {
            // Closes all the dialogs.
            worldUI.CloseAllDialogs();

            // New
            // If the save system exists, make sure a game is loaded...
            // So that the player can continue from the title screen.
            if (SaveSystem.Instantiated)
            {
                // Gets the instance.
                SaveSystem saveSystem = SaveSystem.Instance;

                // If there's no loaded data and no last save data, try loading the game.
                // This function checks for saving and loading being enabled.
                // It also checks if the save file exists.
                if (!saveSystem.HasLoadedData() && !saveSystem.HasLastSaveData())
                {
                    SaveSystem.Instance.LoadGame();
                }
            }

            // Loads the title scene.
            LoadTitleScene();
        }

        // Loads data, and return a 'bool' to show it was successful.
        // clearDataAfterLoad: if true, the loaded data is cleared from the save system after it's loaded.
        //  - This only happens if the data was loaded properly.
        public bool LoadGame(bool clearDataAfterLoad)
        {
            // Checks if saving/loading is enabled.
            if(!savingLoadingEnabled)
            {
                Debug.LogError("Saving and loading is disabled. Load failed.");
                return false;
            }

            // Cheks if the save system has been instantiated.
            if (!SaveSystem.Instantiated)
            {
                Debug.LogError("The save system hasn't been instantiated. Load failed.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSystem = SaveSystem.Instance;

            // Sets the world manager for the save system.
            saveSystem.worldManager = this;

            // Checks if the save system has loaded data.
            if (!saveSystem.HasLoadedData())
            {
                Debug.LogWarning("No data was found to load.");
                return false;
            }

            // Gets the loaded data.
            EDU_GameData data = saveSystem.loadedData;

            // Checks for validity.
            if(!data.valid)
            {
                Debug.LogError("The loaded data was invalid. Load failed.");
                return false;
            }

            // Checks if the loaded data is complete.
            if(data.complete)
            {
                Debug.LogWarning("The loaded data is for a completed game. Load failed.");
                return false;
            }

            // Load the data.
            // Gets the data logger if it's not set.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // Sets the gameplay mode.
            GameSettings.Instance.gameplayMode = data.gameMode;

            // Gets the game score, game time, and game energy.
            dataLogger.gameScore = data.gameScore;
            dataLogger.gameTimer = data.gameTime;

            // TODO: maybe store the energy total and air pollution somewhere? Right now they're summed from the stages.

            // World Stages
            // Gets the world stage data for every stage.
            for (int i = 0; i < data.worldStageDatas.Length && i < stages.Count; i++)
            {
                // The current data.
                WorldStage.WorldStageData currData = data.worldStageDatas[i];

                // If the world stage data exists and the stage exists, apply the world data.
                if (currData != null && stages[i] != null)
                {
                    // If the id numbers don't match, make a warning.
                    if(currData.idNumber != stages[i].idNumber)
                    {
                        Debug.LogWarning("The id number of the loaded data doesn't match the stage it's giving the data to.");
                    }

                    // Applies the world stage data to the stage.
                    stages[i].ApplyWorldStageData(data.worldStageDatas[i]);
                }
            }

            // Natural Resources
            // Clear the used resources.
            dataLogger.usedResources.Clear();

            // Sets which natural resources have been used.
            for (int i = 0; i < data.usedResources.Length; i++)
            {
                // If the resource has been used.
                if(data.usedResources[i])
                {
                    // Converts the resource number.
                    NaturalResources.naturalResource resNum = (NaturalResources.naturalResource)i;
                
                    // If the used resources list doesn't contain this resource, add it.
                    if(!dataLogger.usedResources.Contains(resNum))
                    {
                        dataLogger.usedResources.Add(resNum);
                    }
                }
            }

            // Optimizes the used natural resources list.
            dataLogger.OptimizeUsedNaturalResourcesList();

            // Defense Ids
            // Clears the data logger.
            dataLogger.defenseIds.Clear();

            // Unlocks defense units based on the active values in the data's id list.
            for (int i = 0; i < data.defenseIds.Length; i++)
            {
                // If the defense id is marked as unlocked.
                if (data.defenseIds[i])
                {
                    // If the data logger's defense ids list doesn't contain the id...
                    // Add it, as it is unlocked.
                    if(!dataLogger.defenseIds.Contains(i))
                    {
                        dataLogger.defenseIds.Add(i);
                    }
                }
            }

            // Optimize the data logger's defense units id list.
            dataLogger.OptimizeActionDefenseUnitsList();

            // Saves the current area index.
            SetCurrentWorldArea(data.currentAreaIndex);

            // Energy start bonus.
            dataLogger.energyStartBonus = data.energyStartBonus;

            // Tutorial parameter.
            GameSettings.Instance.UseTutorials = data.useTutorial;

            // Loads the tutorials data.
            tutorials.LoadTutorialsData(data.tutorialData);

            // Complete and Valid parameters were already checked.

            // If the data should be cleared after it's been loaded...
            if(clearDataAfterLoad)
            {
                // Clear the loaded and last saved data.
                saveSystem.ClearLoadedAndLastSaveData();
            }

            // Data set successfully.
            return true;
        }

        // TUTORIALS //
        // Called when a tutorial is ended.
        public override void OnTutorialEnd()
        {
            base.OnTutorialEnd();

            // Check for new tutorials in the next update.
            checkedTutorials = false;
        }

        // Checks if any tutorials need to be activated.
        public void CheckTutorials()
        {
            // If tutorials are being used and a tutorial isn't active.
            if (IsUsingTutorialsAndTutorialNotActive())
            {
                // Gets set to true when a tutorial has started.
                bool startedTutorial = false;

                // Test tutorial.
                // tutorials.LoadTutorialTest();

                // If the intro tutorial hasn't been cleared, trigger it.
                if (!startedTutorial && !tutorials.Data.clearedIntroTutorial)
                {
                    tutorials.LoadIntroTutorial();
                    startedTutorial = true;
                }

                // This tutorial has been removed.
                // // Checks if the player has cleared their first action stage.
                // if(!startedTutorial && !tutorials.Data.clearedFirstActionCompleteTutorial)
                // {
                //     // Checks all stages.
                //     for(int i = 0; i < stages.Count; i++)
                //     {
                //         // Stage exists.
                //         if (stages[i] != null)
                //         {
                //             // Is an action stage.
                //             if (stages[i].GetStageType() == WorldStage.stageType.action)
                //             {
                //                 // If the stage is complete.
                //                 if (stages[i].IsComplete())
                //                 {
                //                     tutorials.LoadFirstActionCompleteTutorial();
                //                     startedTutorial = true;
                //                     break;
                //                 }
                //             }
                //         }
                //     }
                // }

                // This tutorial has been removed.
                // // Checks if the player has cleared their first knowledge stage.
                // if (!startedTutorial && !tutorials.Data.clearedFirstKnowledgeCompleteTutorial)
                // {
                //     // Checks all stages.
                //     for (int i = 0; i < stages.Count; i++)
                //     {
                //         // Stage exists.
                //         if (stages[i] != null)
                //         {
                //             // Is a knowledge stage.
                //             if (stages[i].GetStageType() == WorldStage.stageType.knowledge)
                //             {
                //                 // If the stage is complete.
                //                 if (stages[i].IsComplete())
                //                 {
                //                     tutorials.LoadFirstKnowledgeCompleteTutorial();
                //                     startedTutorial = true;
                //                     break;
                //                 }
                //             }
                //         }
                //     }
                // }

                // Checks if the player has cleared the first area.
                if (!startedTutorial && !tutorials.Data.clearedFirstAreaCompleteTutorial)
                {
                    // There are areas.
                    if(areas.Count > 0)
                    {
                        // Area 1 (index 0) exists.
                        if (areas[0] != null)
                        {
                            // If the first area is complete.
                            if (areas[0].IsComplete())
                            {
                                tutorials.LoadFirstAreaCompleteTutorial();
                                startedTutorial = true;
                            }
                        }
                    }
                }

                // Removed since the final area intro tutorial is no longer used.
                // // Checks if the player has started the final area.
                // if (!startedTutorial && !tutorials.Data.clearedFinalAreaIntroTutorial)
                // {
                //     // If the current area is the final area, run the final area tutorial.
                //     if (areas.IndexOf(GetCurrentWorldArea()) == areas.Count - 1)
                //     {
                //         tutorials.LoadFinalAreaIntroTutorial();
                //         startedTutorial = true;
                //     }
                // }
            }

            // Tutorials have been checked.
            checkedTutorials = true;
        }

        // WORLD //

        // Calculates the game score.
        public float CalculateGameScore()
        {
            float totalScore = 0.0F;

            // Goes through all stages.
            for (int i = 0; i < stages.Count; i++)
            {
                // Stage exists.
                if (stages[i] != null)
                {
                    // Adds the score to the data logger.
                    totalScore += stages[i].score;
                }
            }

            return totalScore;
        }

        // Calculates the game score as an int.
        public int CalculateGameScoreAsInt()
        {
            float scoreFloat = CalculateGameScore();
            int scoreInt = Mathf.CeilToInt(scoreFloat);

            return scoreInt;
        }

        // Calculates and sets the game score.
        public void CalculateAndSetGameScore()
        {
            float score = CalculateGameScore();
            gameScore = score;
        }

        // Calculates the energy total.
        public float CalculateEnergyTotal()
        {
            // The energy total to return.
            float energyTotal = 0;

            // Goes through all stages to get the energy total.
            foreach(WorldStage stage in stages)
            {
                // If the stage exists, add to the energy total.
                if(stage != null)
                {
                    energyTotal += stage.energyTotal;
                }
            }

            return energyTotal;
        }

        // Calculates the air pollution total.
        public float CalculateAirPollutionTotal()
        {
            // The air pollution total to return.
            float airPollutionTotal = 0;

            // Goes through all stages to get the air pollution total.
            foreach (WorldStage stage in stages)
            {
                // If the stage exists, add to the air pollution.
                if (stage != null)
                {
                    airPollutionTotal += stage.airPollution;
                }
            }

            return airPollutionTotal;
        }


        // AREA //
        // Gets the world area at the provided index.
        public WorldArea GetWorldArea(int index)
        {
            // Index validity check.
            if (index >= 0 && index < areas.Count)
            {
                return areas[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the index of the current world area.
        public int GetWorldAreaIndex()
        {
            // Gets the area.
            WorldArea currArea = GetCurrentWorldArea();

            // Area is in list.
            if (areas.Contains(currArea))
            {
                // Return index.
                return areas.IndexOf(currArea);
            }
            else
            {
                // Return -1 since not in list.
                return -1;
            }
        }

        // Gets the world area count.
        public int GetWorldAreaCount()
        {
            return areas.Count;
        }

        // Gets the current world area.
        public WorldArea GetCurrentWorldArea()
        {
            return GetWorldArea(currAreaIndex);
        }

        // Sets the current world area.
        public void SetCurrentWorldArea(int index)
        {
            // Index validity check.
            if (index >= 0 && index < areas.Count)
            {
                // Set the current area index.
                currAreaIndex = index;
            }

            // Sets the current world area.
            SetCurrentWorldArea(areas[currAreaIndex]);
        }

        // Sets the current world area.
        public void SetCurrentWorldArea(WorldArea currArea)
        {
            // TODO: enable/disable areas?

            // If the current area exists, move the camera towards it.
            if (currArea != null)
            {
                // Move camera.
                worldCamera.Move(currArea, instantCameraMovement);

                // This is now handled by a function call later in this function.
                // // If the state of the world should effect the area buttons.
                // if(effectAreaButtons)
                // {
                //     // Current area index, prev area index, and next area index.
                //     int currAreaIndex = GetCurrentWorldAreaIndex();
                //     int prevAreaIndex = currAreaIndex - 1;
                //     int nextAreaIndex = currAreaIndex + 1;
                // 
                //     // Turn on both area buttons to start.
                //     worldUI.prevAreaButton.interactable = true;
                //     worldUI.nextAreaButton.interactable = true;
                // 
                //     // Previous
                //     // If negative, make the prev button non-interactable.
                //     if (prevAreaIndex < 0)
                //     {
                //         worldUI.prevAreaButton.interactable = false;
                //     }
                //     else
                //     {
                //         // Set button interactable if the previous area has been cleared.
                //         worldUI.prevAreaButton.interactable = areas[prevAreaIndex].areaCompleteEvent.cleared;
                //     }
                // 
                //     // Next
                //     // If the current area is completed, the next area must be open.
                //     if (nextAreaIndex < areas.Count)
                //     {
                //         worldUI.nextAreaButton.interactable = currArea.areaCompleteEvent.cleared;
                //     }
                //     // If this is the last area in the list, there is no next area to go to.
                //     else
                //     {
                //         worldUI.nextAreaButton.interactable = false;
                //     }
                // }

            }

            // Refreshes the world area buttons.
            if(effectAreaButtons)
                worldUI.RefreshWorldAreaButtons();

            // Check the tutorials now that the player has moved to a new area.
            checkedTutorials = false;
        }

        // Refreshes setting the current world area.
        public void RefreshCurrentWorldArea()
        {
            SetCurrentWorldArea(GetCurrentWorldArea());
        }

        // Gets the current world area index.
        public int GetCurrentWorldAreaIndex()
        {
            return currAreaIndex;
        }

        // Returns 'true' if this is the first world area.
        public bool IsCurrentWorldAreaFirstArea()
        {
            return currAreaIndex == 0;
        }

        // Returns 'true' if this is the last world area.
        public bool IsCurrentWorldAreaFinalArea()
        {
            return currAreaIndex == areas.Count - 1;
        }

        // Called when the world camera has finished moving to the current area.
        public void OnWorldCameraInCurrentArea()
        {
            // Removed since the final area intro tutorial is no longer used.
            // // If any of the area-specific tutorials haven't been cleared...
            // // Check for them.
            // if(!tutorials.Data.clearedFirstAreaCompleteTutorial || 
            //     !tutorials.Data.clearedFinalAreaIntroTutorial)
            // {
            //     checkedTutorials = false;
            // }
        }

        // Goes to the previous world area.
        // If 'wrapAround' is true, the game loops around to the other area if it underflows.
        public void PreviousWorldArea(bool wrapAround)
        {
            // Gets the potential index of the previous area.
            int index = currAreaIndex - 1;

            // If the index is less than 0, either wrap around or stay at 0.
            if(index < 0)
                index = (wrapAround) ? areas.Count - 1 : 0;

            // Set the new area.
            SetCurrentWorldArea(index);
        }

        // Goes to the previous world area, not allowing wrap arounds.
        public void PreviousWorldArea()
        {
            PreviousWorldArea(false);
        }

        // Goes to the next world area.
        // If 'wrapAround' is true, the game loops around to the other area if it overflows.
        public void NextWorldArea(bool wrapAround)
        {
            // Gets the potential index of the next area.
            int index = currAreaIndex + 1;

            // If the index is greater than or equal to the area count, either wrap around or stay at 0.
            if (index >= areas.Count)
                index = (wrapAround) ? 0 : areas.Count - 1;

            // Set the new area.
            SetCurrentWorldArea(index);
        }

        // Goes to the next world area, not allowing wrap arounds.
        public void NextWorldArea()
        {
            NextWorldArea(false);
        }

        // If area buttons should be effected by the state of the world.
        public bool EffectAreaButtons
        {
            get { return effectAreaButtons; }
        }


        // STAGE //
        // Gets a world stage by its index.
        public WorldStage GetWorldStage(int index)
        {
            // Index validity check.
            if (index >= 0 && index < stages.Count)
            {
                return stages[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the index of the stage in the manager's list. Returns "-1" if not in list.
        public int GetWorldStageIndex(WorldStage worldStage)
        {
            // Checks if in list.
            if (stages.Contains(worldStage))
            {
                return stages.IndexOf(worldStage);
            }
            // Not in list.
            else
            {
                return -1;
            }
        }

        // Gets the world area this stage is in.
        public WorldArea GetWorldStageArea(WorldStage worldStage)
        {
            // The area the stage is in.
            WorldArea stageArea = null;

            // Goes through each area.
            foreach(WorldArea area in areas)
            {
                // Goes through each stage.
                foreach(WorldStage areaStage in area.stages)
                {
                    // If the area stage is eqal to the world stage...
                    // This is the area the stage is in.
                    if(areaStage == worldStage)
                    {
                        stageArea = area;
                        break;
                    }
                }

                // Stage area found, so break.
                if(stageArea != null)
                {
                    break;
                }
            }

            // Returns the stage area.
            return stageArea;
        }

        
        // Starts the provided stage.
        public void StartStage(WorldStage worldStage)
        {
            // Adds the natural resources from the world stage to used list.
            dataLogger.AddUsedNaturalResources(worldStage.naturalResources);

            // A temporary object that will be used for start info.
            GameObject startInfoObject = null;

            // Checks the stage type to see what kind of scene to load.
            if (worldStage is WorldActionStage)
            {
                // Creates a temporary object and add the action stage start info.
                startInfoObject = new GameObject("Action Stage Start Info");
                ActionStageStartInfo assi = startInfoObject.AddComponent<ActionStageStartInfo>();

                // Sets the start info data.
                assi.SetStartInfo(worldStage);

                // Don't destroy the temporary object. It will be destroyed in the action scene.
                DontDestroyOnLoad(startInfoObject);

                // Load the scene.
                LoadActionScene();
            }
            else if(worldStage is WorldKnowledgeStage)
            {
                // Creates a temporary object and add the knowledge stage start info.
                startInfoObject = new GameObject("Knowledge Stage Start Info");
                KnowledgeStageStartInfo kssi = startInfoObject.AddComponent<KnowledgeStageStartInfo>();

                // Sets the start info data
                kssi.SetStartInfo(worldStage);

                // Don't destroy the temporary object. It will be destroyed in the knowledge scene.
                DontDestroyOnLoad(startInfoObject);

                // Load the scene.
                LoadKnowledgeScene();
            }
            else
            {
                Debug.LogError("No destination scene could be determined.");
                return;
            }
        }

        // Returns 'true' if the player has completed at least 1 stage.
        public bool HasCompletedAStage()
        {
            // Goes through all stages.
            foreach(WorldStage stage in stages)
            {
                // Stage exists.
                if(stage != null)
                {
                    // If a stage has been completed, return true.
                    if(stage.IsComplete())
                    {
                        return true;
                    }
                }
            }

            // No stages complete.
            return false;
        }


        // PROGRESS, COMPLETE
        // Gets the game progress.
        public int GetGameProgress()
        {
            // Progress
            int progress = 0;

            // Increases progress for every defeated challenger.
            for (int i = 0; i < stages.Count; i++)
            {
                // If the stage is compelte, add it to the progress..
                if (stages[i].IsComplete())
                    progress++;
            }

            // Returns the progress.
            return progress;
        }

        // Gets the game progress as a percentage.
        // If the percentage can't be calculated, -1 is returned.
        public float GetGameProgressAsPercentage()
        {
            // Gets the number of cleared stages.
            int clearedCount = GetGameProgress();
            float progress = 0;

            // Calculates the amount of progress that has been made.
            if (stages.Count > 0)
            {
                progress = (float)clearedCount / stages.Count;
            }
            else // Count is unknown, so just set it to -1.
            {
                progress = -1;
            }

            // Returns the progress.
            return progress;
        }

        // Submits the current game progress.
        public void SubmitProgress()
        {
            // If the LOLManager and the SDK have both been initialized, submit the score and game progress.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
                SystemManager.Instance.SubmitProgress(CalculateGameScoreAsInt(), GetGameProgress());
        }

        // Submits the game progress complete.
        public void SubmitProgressComplete()
        {
            // Submits the game score and progress complete.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
                SystemManager.Instance.SubmitProgressComplete(CalculateGameScoreAsInt());
        }

        // Checks if the game is complete.
        public bool IsGameComplete()
        {
            // Result to be returned.
            bool result;

            // Checks that the game complete event is set.
            if(gameCompleteEvent != null)
            {
                // No need to call the function since you already checked that the event is set.
                // result = IsGameCompleteEventComplete();
                result = gameCompleteEvent.cleared;
            }
            // No event, so manaully check.
            else
            {
                // Checks if all stages are complete.
                result = AllStagesComplete();
            }

            return result;
        }

        // Returns 'true' if game complete event is complete.
        public bool IsGameCompleteEventComplete()
        {
            // If the game complete event is set, check that.
            // If it isn't, return false.
            bool result = (gameCompleteEvent != null) ? gameCompleteEvent.cleared : false;
            return result;
        }

        // Returns 'true' if all stages are marked as complete.
        public bool AllStagesComplete()
        {
            // True by default.
            bool result = true;

            // Goes through all the stages. If a not complete stage is found, result is false.
            for (int i = 0; i < stages.Count; i++)
            {
                // Stage exists.
                if (stages[i] != null)
                {
                    // Stage isn't complete, so the game isn't complete.
                    if (!stages[i].IsComplete())
                    {
                        result = false;
                        break;
                    }
                }
            }

            // Returns the results.
            return result;
        }

        // Called when the game has been completed.
        public void CompleteGame()
        {
            // Save all the world datas for the results scene.
            dataLogger.SaveWorldStageDatas(this);

            // Submit progress complete.
            SubmitProgressComplete();

            // If the game should auto save on game end...
            // Of it the game shouldn't auto save in game end, but not all stages are complete...
            // Save the game on complete.
            // CompleteGame() should only be called once all stages are complete anyway...
            // But this check is still here.
            if(autoSaveOnGameEnd || (!autoSaveOnGameEnd && !AllStagesComplete()))
            {
                // Save the game one last time.
                SaveGame();
            }

            // Go to the results scene.
            LoadResultsScene();

            // Complete has been called, so don't call it again.
            callCompleteGameInLateUpdate = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Checks that the game isn't paused.
            if(!IsGamePaused())
            {
                // If there are wait frames, reduce them.
                if(refreshAreaButtonsWaitFrames > 0)
                {
                    // Down by 1 frame.
                    refreshAreaButtonsWaitFrames--;

                    // No more wait frames, so refresh the buttons.
                    // This is here in case game events end up happening out of order...
                    // And cause the area buttons not to be set properly.
                    if(refreshAreaButtonsWaitFrames <= 0)
                    {
                        refreshAreaButtonsWaitFrames = 0;
                        worldUI.RefreshWorldAreaButtons();
                    }
                }
            }

            // If tutorials need to be checked and the game isn't loading...
            // Check for tutorials.
            if (!checkedTutorials && !IsLoading())
            {
                CheckTutorials();
            }
        }

        // LateUpdate is called every frame, if this behaviour is enabled.
        private void LateUpdate()
        {
            // If the game isn't paused.
            if (!IsGamePaused())
            {
                // Calls that the game is complete.
                // Doesn't work in main update.
                if (callCompleteGameInLateUpdate)
                {
                    CompleteGame();
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }

            base.OnDestroy();
        }
    }
}