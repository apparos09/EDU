using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The script for the tutorials.
    public class Tutorials : MonoBehaviour
    {
        // The tutorial data.
        [System.Serializable]
        public class TutorialsData
        {
            // The tutorial count.
            // Changed from 21 to 20 since the final area intro tutorial was removed.
            public const int TUTORIAL_COUNT = 20; // Original: 21

            // World - 1
            public bool clearedIntroTutorial = false;

            // Action
            public bool clearedFirstActionIntroTutorial = false;
            public bool clearedFirstActionGeneratorsTutorial = false;
            public bool clearedFirstActionDefensesTutorial = false;
            public bool clearedFirstActionFirstKillTutorial = false;

            // Action Complete (World)
            // public bool clearedFirstActionCompleteTutorial = false; // Removed

            // Knowledge 
            public bool clearedFirstKnowledgeIntroTutorial = false;
            public bool clearedFirstKnowledgeVerifyTutorial = false;

            // Knowledge Complete (World)
            // public bool clearedFirstKnowledgeCompleteTutorial = false; - Removed

            // World - 2
            public bool clearedFirstAreaCompleteTutorial = false;
            // public bool clearedFinalAreaIntroTutorial = false; // Removed.

            // Natural Resources
            public bool clearedBiomassTutorial = false;
            public bool clearedGeothermalTutorial = false;
            public bool clearedHydroTutorial = false;
            public bool clearedSolarTutorial = false;
            public bool clearedWaveTutorial = false;
            public bool clearedWindTutorial = false;
            public bool clearedCoalTutorial = false;
            public bool clearedNaturalGasTutorial = false;
            public bool clearedNuclearTutorial = false;
            public bool clearedOilTutorial = false;

            // Gets the tutorials count.
            public int GetTutorialsCount()
            {
                return TUTORIAL_COUNT;
            }

            // The tutorials cleared count.
            public int GetTutorialsClearedCount()
            {
                // The cleared count.
                int clearedCount = 0;

                // // OLD
                // // World - 1
                // clearedCount += clearedIntroTutorial ? 1 : 0;
                // 
                // // Action
                // clearedCount += clearedFirstActionIntroTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionGeneratorsTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionDefensesTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionFirstKillTutorial ? 1 : 0;
                // 
                // // Action Complete (World)
                // clearedCount += clearedFirstActionCompleteTutorial ? 1 : 0;
                // 
                // // Knowledge 
                // clearedCount += clearedFirstKnowledgeIntroTutorial ? 1 : 0;
                // clearedCount += clearedFirstKnowledgeVerifyTutorial ? 1 : 0;
                // 
                // // Knowledge Complete (World)
                // clearedCount += clearedFirstKnowledgeCompleteTutorial ? 1 : 0;
                // 
                // // World - 2
                // clearedCount += clearedFirstAreaCompleteTutorial ? 1 : 0;
                // clearedCount += clearedFinalAreaIntroTutorial ? 1 : 0;
                // 
                // // Natural Resources
                // clearedCount += clearedBiomassTutorial ? 1 : 0;
                // clearedCount += clearedGeothermalTutorial ? 1 : 0;
                // clearedCount += clearedHydroTutorial ? 1 : 0;
                // clearedCount += clearedSolarTutorial ? 1 : 0;
                // clearedCount += clearedWaveTutorial ? 1 : 0;
                // clearedCount += clearedWindTutorial ? 1 : 0;
                // clearedCount += clearedCoalTutorial ? 1 : 0;
                // clearedCount += clearedNaturalGasTutorial ? 1 : 0;
                // clearedCount += clearedNuclearTutorial ? 1 : 0;
                // clearedCount += clearedOilTutorial ? 1 : 0;

                // NEW
                // The cleared tutorial array.
                bool[] clearedArr = new bool[]
                { 
                    // World - 1
                    clearedIntroTutorial,

                    // Action
                    clearedFirstActionIntroTutorial,
                    clearedFirstActionGeneratorsTutorial,
                    clearedFirstActionDefensesTutorial,
                    clearedFirstActionFirstKillTutorial,

                    // Action Complete (World)
                    // clearedFirstActionCompleteTutorial, // Removed

                    // Knowledge 
                    clearedFirstKnowledgeIntroTutorial,
                    clearedFirstKnowledgeVerifyTutorial,

                    // Knowledge Complete (World)
                    // clearedFirstKnowledgeCompleteTutorial, // Removed

                    // World - 2
                    clearedFirstAreaCompleteTutorial,
                    // clearedFinalAreaIntroTutorial, // Removed.

                    // Natural Resources
                    clearedBiomassTutorial,
                    clearedGeothermalTutorial,
                    clearedHydroTutorial,
                    clearedSolarTutorial,
                    clearedWaveTutorial,
                    clearedWindTutorial,
                    clearedCoalTutorial,
                    clearedNaturalGasTutorial,
                    clearedNuclearTutorial,
                    clearedOilTutorial
                };

                // Goes through the array.
                foreach(bool value in clearedArr)
                {
                    // If the value is true, add to the cleared count.
                    if (value)
                        clearedCount++;
                }

                // Return the cleared count.
                return clearedCount;
            }

            // Copies the tutorial data.
            public TutorialsData Copy()
            {
                // The copied data.
                TutorialsData copyData = new TutorialsData();

                // World - 1
                copyData.clearedIntroTutorial = clearedIntroTutorial;

                // Action
                copyData.clearedFirstActionIntroTutorial = clearedFirstActionIntroTutorial;
                copyData.clearedFirstActionGeneratorsTutorial = clearedFirstActionGeneratorsTutorial;
                copyData.clearedFirstActionDefensesTutorial = clearedFirstActionDefensesTutorial;
                copyData.clearedFirstActionFirstKillTutorial = clearedFirstActionFirstKillTutorial;

                // Action - Complete (World)
                // copyData.clearedFirstActionCompleteTutorial = clearedFirstActionCompleteTutorial; // Removed

                // Knowledge
                copyData.clearedFirstKnowledgeIntroTutorial = clearedFirstKnowledgeIntroTutorial;
                copyData.clearedFirstKnowledgeVerifyTutorial = clearedFirstKnowledgeVerifyTutorial;

                // Knowledge - Complete (World)
                // copyData.clearedFirstKnowledgeCompleteTutorial = clearedFirstKnowledgeCompleteTutorial; // Removed

                // World - 2
                copyData.clearedFirstAreaCompleteTutorial = clearedFirstAreaCompleteTutorial;

                // World - 3
                // copyData.clearedFinalAreaIntroTutorial = clearedFinalAreaIntroTutorial; // Removed.

                // Natural Resources
                copyData.clearedBiomassTutorial = clearedBiomassTutorial;
                copyData.clearedGeothermalTutorial = clearedGeothermalTutorial;
                copyData.clearedHydroTutorial = clearedHydroTutorial;
                copyData.clearedSolarTutorial = clearedSolarTutorial;
                copyData.clearedWaveTutorial = clearedWaveTutorial;
                copyData.clearedWindTutorial = clearedWindTutorial;

                copyData.clearedCoalTutorial = clearedCoalTutorial;
                copyData.clearedNaturalGasTutorial = clearedNaturalGasTutorial;
                copyData.clearedNuclearTutorial = clearedNuclearTutorial;
                copyData.clearedOilTutorial = clearedOilTutorial;

                // Resulting data.
                return copyData;
            }

            // Pastes the values from the provided data to this object.
            public void Paste(TutorialsData pasteData)
            {
                // World - 1
                clearedIntroTutorial = pasteData.clearedIntroTutorial;

                // Action
                clearedFirstActionIntroTutorial = pasteData.clearedFirstActionIntroTutorial;
                clearedFirstActionGeneratorsTutorial = pasteData.clearedFirstActionGeneratorsTutorial;
                clearedFirstActionDefensesTutorial = pasteData.clearedFirstActionDefensesTutorial;
                clearedFirstActionFirstKillTutorial = pasteData.clearedFirstActionFirstKillTutorial;

                // Action - Complete
                // clearedFirstActionCompleteTutorial = pasteData.clearedFirstActionCompleteTutorial; // Removed

                // Knowledge
                clearedFirstKnowledgeIntroTutorial = pasteData.clearedFirstKnowledgeIntroTutorial;
                clearedFirstKnowledgeVerifyTutorial = pasteData.clearedFirstKnowledgeVerifyTutorial;

                // Knowledge - Complete
                // clearedFirstKnowledgeCompleteTutorial = pasteData.clearedFirstKnowledgeCompleteTutorial; // Removed

                // World - 2
                clearedFirstAreaCompleteTutorial = pasteData.clearedFirstAreaCompleteTutorial;

                // World - 3
                // clearedFinalAreaIntroTutorial = pasteData.clearedFinalAreaIntroTutorial; // Removed.

                // Natural Resources
                clearedBiomassTutorial = pasteData.clearedBiomassTutorial;
                clearedGeothermalTutorial = pasteData.clearedGeothermalTutorial;
                clearedHydroTutorial = pasteData.clearedHydroTutorial;
                clearedSolarTutorial = pasteData.clearedSolarTutorial;
                clearedWaveTutorial = pasteData.clearedWaveTutorial;
                clearedWindTutorial = pasteData.clearedWindTutorial;

                clearedCoalTutorial = pasteData.clearedCoalTutorial;
                clearedNaturalGasTutorial = pasteData.clearedNaturalGasTutorial;
                clearedNuclearTutorial = pasteData.clearedNuclearTutorial;
                clearedOilTutorial = pasteData.clearedOilTutorial;
            }

        }

        // A tutorial info set, which is used to store info for a tutorial.
        public class TutorialInfo
        {
            public string title = "";
            public string titleKey = "";
            public List<Page> pages = new List<Page>();

            // Translates the title.
            public void TranslateTitle()
            {
                // If possible, translate the title.
                if(titleKey != "" && SystemManager.IsInstantiatedAndIsLanguageLoaded())
                {
                    title = SystemManager.GetLanguageTextStatic(titleKey);
                }
            }

            // Removed since the title should be saved already translated.
            // // Gets the title translated.
            // public string GetTitleTranslated()
            // {
            //     // If possible, translate the title.
            //     if(titleKey != "" && LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            //     {
            //         return LOLManager.GetLanguageTextStatic(titleKey);
            //     }
            //     // Returns the title.
            //     else
            //     {
            //         return title;
            //     }
            // }
        }

        // The singleton instance.
        private static Tutorials instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game manager.
        public GameplayManager gameManager;

        // The tutorials UI.
        public TutorialUI tutorialsUI;

        // If 'true', the tutorials object is set to not be destroyed on load in the Start().
        [Tooltip("If true, DontDestroyOnLoad(gameObject) is called in Start().")]
        public bool dontDestroyOnLoadInStart = true;

        // // If 'true', the tutorials object constantly checks for starting tutorials.
        // [Tooltip("Constant check for tutorial start.")]
        // public bool constantTutorialStartCheck = true;


        [Header("Tutorials")]

        // The tutorial data.
        private TutorialsData data = new TutorialsData();

        // Constructor
        private Tutorials()
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
                // If the data doesn't exist, create an object.
                if(data == null)
                    data = new TutorialsData();

                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the game manager object if it isn't set.
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // Gets the tutorials UI object if it exists.
            // Notably, TutorialUI checks for Tutorials as well, and will save itself...
            // To that object if this check fails.
            if (tutorialsUI == null && TutorialUI.Instantiated)
            {
                // The tutorial UI.
                tutorialsUI = TutorialUI.Instance;

                // Set the UI to use this tutorial.
                if (tutorialsUI.tutorials == null)
                    tutorialsUI.tutorials = this;
            }   

            // Don't destroy this game object.
            if(dontDestroyOnLoadInStart)
                DontDestroyOnLoad(gameObject);
        }


        // This function is called when the object is enabled and active
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Gets the instance.
        public static Tutorials Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<Tutorials>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial (singleton)");
                        instance = go.AddComponent<Tutorials>();
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

        // Called when the scene is loaded.
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // If the game manager is not set, set it.
            if (gameManager == null)
            {
                // Try to find the game manager.
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // Try to get the tutorials UI again if it has been instantiated.
            // Since the tutorial object might be going into a scene that will end up destroying it...
            // No message is printed to show that there's no TutorialUI object if TutorialUI...
            // Hasn't been instantiated.
            if (tutorialsUI == null && TutorialUI.Instantiated)
            {
                tutorialsUI = TutorialUI.Instance;
            }
        }

        // Gets the tutorials data.
        public TutorialsData Data
        {
            get { return data; }
        }

        // Gets the tutorials count.
        public int TutorialsCount
        {
            get { return TutorialsData.TUTORIAL_COUNT; }
        }

        // Gets the tutorials cleared count.
        public int TutorialsClearedCount
        {
            get { return data.GetTutorialsClearedCount(); }
        }

        // TUTORIAL FUNCTIONS
        // Checks if a tutorial is running.
        public bool IsTutorialRunning()
        {
            return tutorialsUI.IsTutorialRunning();
        }

        // Starts the tutorial.
        public void StartTutorial()
        {
            tutorialsUI.StartTutorial();
        }

        // Restarts the tutorial.
        public void RestartTutorial()
        {
            tutorialsUI.RestartTutorial();
        }

        // Ends the tutorial.
        public void EndTutorial()
        {
            tutorialsUI.EndTutorial();
        }

        // Called when a tutorial is started.
        public void OnTutorialStart()
        {
            // UI start function.
            tutorialsUI.OnTutorialStart();

            // Freeze the game.
            Time.timeScale = 0.0F;
        }

        // Called when a tutorial ends.
        public void OnTutorialEnd()
        {
            // UI end function.
            tutorialsUI.OnTutorialEnd();

            // // If the game manager hasn't been set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // Unfreeze the game if the game is not paused.
            if (!gameManager.IsGamePaused())
            {
                // If the game manager is set, check it for the time scale.
                // If it's not set, use 1.0F.
                if (gameManager != null)
                {
                    Time.timeScale = gameManager.GetGameTimeScale();
                }
                else
                {
                    Time.timeScale = 1.0F;
                }

            }

            // Ignore the current input for this frame in case the player is holding the space bar.
            // gameManager.player.IgnoreInputs(1);
        }


        // TUTORIAL DATA, INFO

        // Generates a copy of the tutorials data.
        public TutorialsData GenerateTutorialsDataCopy()
        {
            // The data.
            TutorialsData copy = data.Copy();

            return data;
        }

        // Sets the tutorials data.
        public void LoadTutorialsData(TutorialsData newData)
        {
            // If the data doesn't exist, create it.
            if (data == null)
                data = new TutorialsData();

            data.Paste(newData);
        }

        // Loads the tutorial debug data.
        public void LoadTutorialsDebugData()
        {
            // The debug data.
            TutorialsData debugData = new TutorialsData();

            // Load the debug data.
            LoadTutorialsData(debugData);
        }

        // Resets the tutorials data.
        public void ResetTutorialsData()
        {
            TutorialsData newData = new TutorialsData();
            data.Paste(newData);
        }

        // Generates tutorial info.
        // translateTitle: translates the title using the provided title key and saves it to the object if true.
        //  - The tutorial log assumes the title has already been translated.
        public TutorialInfo GenerateTutorialInfo(string title, string titleKey, ref List<Page> pages, bool translateTitle = true)
        {
            // Creates the info object.
            TutorialInfo tutorialInfo = new TutorialInfo();

            // Load the info.
            tutorialInfo.title = title;
            tutorialInfo.titleKey = titleKey;
            tutorialInfo.pages.AddRange(pages);

            // If the title should be saved as its translated form.
            if(translateTitle)
            {
                // If tranlsation is available, translate.
                if(SystemManager.IsInstantiatedAndIsLanguageLoaded())
                {
                    tutorialInfo.title = SystemManager.GetLanguageTextStatic(titleKey);
                }
            }

            // Returns the info.
            return tutorialInfo;
        }

        // Generates tutorials infos list.
        public List<TutorialInfo> GenerateTutorialInfos(bool clearedOnly)
        {
            // The resulting list.
            List<TutorialInfo> resultList = new List<TutorialInfo>();

            // World - 1
            if (!clearedOnly || (clearedOnly && Data.clearedIntroTutorial))
                resultList.Add(GetIntroTutorialInfo());

            // Action
            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionIntroTutorial))
                resultList.Add(GetFirstActionIntroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionGeneratorsTutorial))
                resultList.Add(GetFirstActionGeneratorsTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionDefensesTutorial))
                resultList.Add(GetFirstActionDefensesTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionFirstKillTutorial))
                resultList.Add(GetFirstActionFirstKillTutorialInfo());

            // // Action Complete (World) - Removed since the tutorial is no longer used.
            // if (!clearedOnly || (clearedOnly && Data.clearedFirstActionCompleteTutorial))
            //     resultList.Add(GetFirstActionCompleteTutorialInfo());

            // Knowledge 
            if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeIntroTutorial))
                resultList.Add(GetFirstKnowledgeIntroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeVerifyTutorial))
                resultList.Add(GetFirstKnowledgeVerifyTutorialInfo());

            // // Knowledge Complete (World) - Removed since the tutorial is no longer used.
            // if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeCompleteTutorial))
            //     resultList.Add(GetFirstKnowledgeCompleteTutorialInfo());

            // World - 2
            if (!clearedOnly || (clearedOnly && Data.clearedFirstAreaCompleteTutorial))
                resultList.Add(GetFirstAreaCompleteTutorialInfo());

            // // World - 3 - Removed since the tutorial is no longer used.
            // if (!clearedOnly || (clearedOnly && Data.clearedFinalAreaIntroTutorial))
            //     resultList.Add(GetFinalAreaIntroTutorialInfo());

            // Natural Resources
            if (!clearedOnly || (clearedOnly && Data.clearedBiomassTutorial))
                resultList.Add(GetBiomassTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedGeothermalTutorial))
                resultList.Add(GetGeothermalTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedHydroTutorial))
                resultList.Add(GetHydroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedSolarTutorial))
                resultList.Add(GetSolarTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedWaveTutorial))
                resultList.Add(GetWaveTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedWindTutorial))
                resultList.Add(GetWindTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedCoalTutorial))
                resultList.Add(GetCoalTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedNaturalGasTutorial))
                resultList.Add(GetNaturalGasTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedNuclearTutorial))
                resultList.Add(GetNuclearTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedOilTutorial))
                resultList.Add(GetOilTutorialInfo());

            // Return the resulting list.
            return resultList;
        }

        // TUTORIAL LOADING

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages, bool startTutorial = true)
        {
            // The gameplay manager isn't set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // Loads pages for the tutorial.
            if (gameManager != null && startTutorial) // If the game manager is set, start the tutorial.
            {
                gameManager.StartTutorial(pages);
            }
            else // No game manager, so just load the pages.
            {
                tutorialsUI.LoadPages(ref pages, false);
            }
        }


        // Load the tutorial (template)
        private void LoadTutorialTemplate(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new Page("Insert text here.")
            };

            // You can set an image to go along with a page by providing it to EDU_Page constructor.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }

        // Load test tutorial
        public void LoadTutorialTest(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("This is a test."),
                new EDU_Page("This is only a test.")
            };

            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }


        // MAIN
        // GAME INTRO
        // Loads the intro tutorial.
        public void LoadIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetIntroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the intro tutorial info into an object and returns it.
        public TutorialInfo GetIntroTutorialInfo()
        {  
            // The title and title translation key.
            string title = "Game Introduction";
            string titleKey = "trl_intro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "Welcome to the Energy Defense Unit (EDU)! This is a training simulation that teaches you about the Earth's natural resources. There are two stage types in this simulation game: action (blue) and knowledge (green). You'll get more details on both later.", "trl_intro_txt_00", tutorialsUI.textBox.stageTypesSprite),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log provides information on simulation elements you've encountered.", "trl_intro_txt_01", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. The options menu allows you to save your progress, quit the game, reread tutorials, and adjust the game's settings. When in a stage, the options menu allows you to reset the stage.", "trl_intro_txt_02", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "With all that covered, please select the available stage. Details on natural resources will be provided when relevant.", "trl_intro_txt_03"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - INTRO
        // Loads the first action stage - intro tutorial.
        public void LoadFirstActionIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionIntroTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionIntroTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionIntroTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Introduction";
            string titleKey = "trl_firstActionIntro_ttl";

            // LOL VERSION - GENERATION MODE ONLY
            // // NOTE: this is the tutorial for generation mode only. Since the LOL verison only uses generation mode...
            // // Only that text is in the language file.
            // // Create the pages list.
            // List<Page> pages = new List<Page>
            // {
            //     // Load the pages.
            //     new EDU_Page(title, titleKey, "This is an action stage! In action stages, you generate energy while stopping enemies from entering your base. Enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the left end of the field, you lose.", "trl_firstActionIntro_txt_00"),
            //     new EDU_Page(title, titleKey, "If you reach the energy generation goal, which is displayed by the bar on the right, you win. The energy generation goal bar fills automatically, but generating energy fills the bar faster.", "trl_firstActionIntro_txt_01", tutorialsUI.textBox.actionEnergyBarUserSprite),
            //     new EDU_Page(title, titleKey, "Some energy will be given periodically, but only energy you generate contributes to the energy goal. To generate energy, you use generator units, which can be found in the bottom left. The energy needed to create a unit is displayed on its unit button.", "trl_firstActionIntro_txt_02"),
            //     new EDU_Page(title, titleKey, "When a unit is selected, it'll be displayed in the bottom middle, and spots on the field will be highlighted to show where it can go. With all that covered, please place a unit on the field.", "trl_firstActionIntro_txt_03", tutorialsUI.textBox.actionUnitSelectedSprite),
            // };

            // ENHANCED VERSION - GENERATION AND DEFENSE MODE
            // NOTE: the LOL version only has one gameplay mode. For the build that has multiple gameplay mode options...
            // Use the following for setting the pages instead.
            // Create the pages list.
            // The pages are different if its generation mode or defense mode.
            List<Page> pages;
            
            // Checks the gameplay mode to see which version to use.
            switch(GameSettings.Instance.gameplayMode)
            {
                // Generation Mode
                case GameSettings.gameMode.generation:
                default:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "This is an action stage! In action stages, you generate energy while stopping enemies from entering your base. Enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the left end of the field, you lose.", "trl_firstActionIntro_gen_txt_00"),
                        new EDU_Page(title, titleKey, "If you reach the energy generation goal, which is displayed by the bar on the right, you win. The energy generation goal bar fills automatically, but generating energy fills the bar faster.", "trl_firstActionIntro_gen_txt_01", tutorialsUI.textBox.actionEnergyBarUserSprite),
                        new EDU_Page(title, titleKey, "Some energy will be given periodically, but only energy you generate contributes to the energy goal. To generate energy, you use generator units, which can be found in the bottom left. The energy needed to create a unit is displayed on its unit button.", "trl_firstActionIntro_gen_txt_02"),
                        new EDU_Page(title, titleKey, "When a unit is selected, it'll be displayed in the bottom middle, and spots on the field will be highlighted to show where it can go. With all that covered, please place a unit on the field.", "trl_firstActionIntro_txt_03", tutorialsUI.textBox.actionUnitSelectedSprite),
                    };
                    break;
            
                // Defense Mode
                // This isn't available in the LOL version, so the translation keys haven't been changed.
                case GameSettings.gameMode.defense:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "This is an action stage! In action stages, you fight off enemies that are trying to enter your base. Enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the left end of the field, you lose.", "trl_firstActionIntro_def_txt_00"),
                        new EDU_Page(title, titleKey, "If the enemy side runs out of energy, you win. The enemy side automatically loses energy as the stage progresses, but defeating an enemy makes the enemy side lose energy faster. The enemy side's energy amount is displayed by the bar on the right.", "trl_firstActionIntro_def_txt_01", tutorialsUI.textBox.actionEnergyBarEnemySprite),
                        new EDU_Page(title, titleKey, "The energy needed to create a unit is displayed on its unit button. Some energy is given periodically, but that won't be enough to defeat the enemies. To get more energy, you use generator units, which can be found in the bottom left.", "trl_firstActionIntro_def_txt_02"),
                        new EDU_Page(title, titleKey, "When a unit is selected, it'll be displayed in the bottom middle, and spots on the field will be highlighted to show where it can go. With all that covered, please place a unit on the field.", "trl_firstActionIntro_txt_03", tutorialsUI.textBox.actionUnitSelectedSprite),
                    };
                    break;
            }


            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - GENERATORS
        // Loads the first action stage - generators tutorial.
        public void LoadFirstActionGeneratorsTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionGeneratorsTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionGeneratorsTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionGeneratorsTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action generators tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionGeneratorsTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Generators";
            string titleKey = "trl_firstActionGenerators_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've placed a generator unit! Generators generate energy if their conditions are met. A generator flashes blue when it generates energy and goes dark if it cannot generate energy. Some generators also produce air pollution, which can lower your stage score.", "trl_firstActionGenerators_txt_00"),
                new EDU_Page(title, titleKey, "In the top left is the day-night indicator and in the top right is the wind indicator. The day-night indicator shows the time of day, and the wind indicator shows the current wind speed. Units affected by these elements are explained when relevant.", "trl_firstActionGenerators_txt_01", tutorialsUI.textBox.indicatorsSprite),
                new EDU_Page(title, titleKey, "In the top middle is the energy display (top value) and pollution display (bottom value). The energy display shows your current energy amount, and the pollution display shows how much air pollution you've generated in the current stage. With all that explained, let the stage continue.", "trl_firstActionGenerators_txt_02", tutorialsUI.textBox.energyAirPollutionDisplaySprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - DEFENSES
        // Loads the first action stage - defenses tutorial.
        public void LoadFirstActionDefensesTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionDefensesTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionDefensesTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionDefensesTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action defenses tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionDefensesTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Defenses";
            string titleKey = "trl_firstActionDefenses_ttl";

            // LOL VERSION - GENERATION ONLY
            // // Create the pages list.
            // List<Page> pages = new List<Page>
            // {
            //     // Load the pages.
            //     new EDU_Page(title, titleKey, "Enemies are approaching! You'll need to place defense units to defeat them, which can be found in the bottom right. Defense units come in three types: blaster, shield, and trap.", "trl_firstActionDefenses_txt_00", tutorialsUI.textBox.actionDefenseTypesSprite),
            //     new EDU_Page(title, titleKey, "Blasters (BSRs) fire projectiles at enemies, shields (SHDs) block enemies, and traps (TRPs) attack enemies that interact with them. You'll unlock more defense units as the simulation progresses. To assist you, there are lane blasters on the left side of the field.", "trl_firstActionDefenses_txt_01", tutorialsUI.textBox.actionDefenseTypesSprite),
            //     new EDU_Page(title, titleKey, "A lane blaster destroys all enemies in its row upon an enemy touching it, but it also destroys itself in the process. You cannot restore lost lane blasters, but you always get them back at the start of a stage. With all that covered, let the stage recommence!", "trl_firstActionDefenses_txt_02", tutorialsUI.textBox.actionDefenseLaneBlasterSprite),
            // };

            // ENHANCED VERSION - GENERATION AND DEFENSE MODE
            // NOTE: the LOL version only has one gameplay mode. For the build that has multiple gameplay mode options...
            // Use the following for setting the pages instead.
            // Create the pages list.
            // The pages are different if its generation mode or defense mode.
            List<Page> pages;
            
            // Checks the gameplay mode to see which version to use.
            switch(GameSettings.Instance.gameplayMode)
            {
                // Generation Mode
                case GameSettings.gameMode.generation:
                default:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "Enemies are approaching! You'll need to place defense units to defeat them, which can be found in the bottom right. Defense units come in three types: blaster, shield, and trap.", "trl_firstActionDefenses_gen_txt_00", tutorialsUI.textBox.actionDefenseTypesSprite),
                        new EDU_Page(title, titleKey, "Blasters (BSRs) fire projectiles at enemies, shields (SHDs) block enemies, and traps (TRPs) attack enemies that interact with them. You'll unlock more defense units as the simulation progresses. To assist you, there are lane blasters on the left side of the field.", "trl_firstActionDefenses_gen_txt_01", tutorialsUI.textBox.actionDefenseTypesSprite),
                        new EDU_Page(title, titleKey, "A lane blaster destroys all enemies in its row upon an enemy touching it, but it also destroys itself in the process. You cannot restore lost lane blasters, but you always get them back at the start of a stage. With all that covered, let the stage recommence!", "trl_firstActionDefenses_gen_txt_02", tutorialsUI.textBox.actionDefenseLaneBlasterSprite),
                    };
                    break;
            
                // Defense Mode
                // This isn't available in the LOL version, so the translation keys haven't been changed.
                case GameSettings.gameMode.defense:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "Enemies are approaching! You'll need to place defense units to defeat them, which can be found in the bottom right. The energy used to create units is also used to power certain unit attacks, so manage your energy well. Defense units come in three types: blaster, shield, and trap.", "trl_firstActionDefenses_def_txt_00", tutorialsUI.textBox.actionDefenseTypesSprite),
                        new EDU_Page(title, titleKey, "Blasters (BSRs) use energy to fire projectiles at enemies, shields (SHDs) block enemies, and traps (TRPs) attack enemies that interact with them. You'll unlock more defense units as the simulation progresses. To assist you, there are lane blasters on the left side of the field.", "trl_firstActionDefenses_def_txt_01", tutorialsUI.textBox.actionDefenseTypesSprite),
                        new EDU_Page(title, titleKey, "A lane blaster destroys all enemies in its row upon an enemy touching it. It uses no energy but destroys itself upon performing an attack. You cannot restore lost lane blasters, but you always get them back at the start of a stage. With all that covered, let the stage recommence!", "trl_firstActionDefenses_def_txt_02", tutorialsUI.textBox.actionDefenseLaneBlasterSprite),
                    };
                    break;
            }

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - FIRST KILL
        // Loads the first action stage - first kill tutorial.
        public void LoadFirstActionFirstKillTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionFirstKillTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionFirstKillTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionFirstKillTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action defenses tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionFirstKillTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Enemy Unit Defeated";
            string titleKey = "trl_firstActionFirstKill_ttl";

            // NOTE: the selector arrows diagram has been replaced with the the selector arrows guide diagram.
            // This is now used for both generation and defense mode since it should be clearer than the screenshot.
            // The original images are still in the game files.

            // LOL VERSION - GENERATION ONLY
            // Create the pages list.
            // List<Page> pages = new List<Page>
            // {
            //     // Load the pages.
            //     new EDU_Page(title, titleKey, "You've defeated your first enemy! When you have more units than a unit selector can display at once, rows are switched using the arrow buttons. The arrows on a unit button indicate if the unit button in the next or previous row that's within the same column is usable.", "trl_firstActionFirstKill_txt_00", tutorialsUI.textBox.actionUnitSelectorArrowsGuideSprite),
            //     new EDU_Page(title, titleKey, "On the left is the stage speed button, which switches between normal stage speed and fast stage speed. The button's icon displays the current stage speed, with 2 arrows meaning normal speed and 3 arrows meaning fast speed. With all that explained, back to the stage.", "trl_firstActionFirstKill_txt_01", tutorialsUI.textBox.stageSpeedSprite),
            // };

            // ENHANCED VERSION - GENERATION AND DEFENSE MODE
            // NOTE: the LOL version only has one gameplay mode. For the build that has multiple gameplay mode options...
            // Use the following for setting the pages instead.
            // Create the pages list.
            // The pages are different if its generation mode or defense mode.
            List<Page> pages;
            
            // Checks the gameplay mode to see which version to use.
            switch(GameSettings.Instance.gameplayMode)
            {
                // Generation Mode
                case GameSettings.gameMode.generation:
                default:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "You've defeated your first enemy! When you have more units than a unit selector can display at once, rows are switched using the arrow buttons. The arrows on a unit button indicate if the unit button in the next or previous row that's within the same column is usable.", "trl_firstActionFirstKill_txt_00", tutorialsUI.textBox.actionUnitSelectorArrowsGuideSprite),
                        new EDU_Page(title, titleKey, "On the left is the stage speed button, which switches between normal stage speed and fast stage speed. The button's icon displays the current stage speed, with 2 arrows meaning normal speed and 3 arrows meaning fast speed. With all that explained, back to the stage.", "trl_firstActionFirstKill_gen_txt_01", tutorialsUI.textBox.stageSpeedSprite),
                    };
                    break;
            
                // Defense Mode
                // This isn't available in the LOL version, so the translation keys haven't been changed.
                case GameSettings.gameMode.defense:
                    pages = new List<Page>
                    {
                        // Load the pages.
                        new EDU_Page(title, titleKey, "You've defeated your first enemy! When you have more units than a unit selector can display at once, rows are switched using the arrow buttons. The arrows on a unit button indicate if the unit button in the next or previous row that's within the same column is usable.", "trl_firstActionFirstKill_txt_00", tutorialsUI.textBox.actionUnitSelectorArrowsGuideSprite),
                        new EDU_Page(title, titleKey, "On the left are various buttons. The stage speed button switches between normal stage speed and fast stage speed. The button's icon displays the current stage speed, with 2 arrows meaning normal speed and 3 arrows meaning fast speed.", "trl_firstActionFirstKill_def_txt_01", tutorialsUI.textBox.stageSpeedSprite),
                        new EDU_Page(title, titleKey, "The unit deselect button deselects the unit you currently have selected. Your selected unit will also be deselected automatically if you no longer have the energy to create it.", "trl_firstActionFirstKill_def_txt_02", tutorialsUI.textBox.unitDeselectSprite),
                        new EDU_Page(title, titleKey, "The unit remove button toggles remove mode, which allows you to remove any of the units you've placed on the field. When remove mode is active, it's displayed in the bottom middle. To remove a field unit, select said unit while in remove mode.", "trl_firstActionFirstKill_def_txt_03", tutorialsUI.textBox.unitRemoveSprite),
                        new EDU_Page(title, titleKey, "The energy disable button toggles the energy flow to defense units. When the flow is disabled, defense units cannot use attacks that require energy. Any defense units affected will also go dark.", "trl_firstActionFirstKill_def_txt_04", tutorialsUI.textBox.energyBlockSprite),
                        new EDU_Page(title, titleKey, "The energy disable button's icon shows a connected plug when the flow is enabled and a disconnected plug when the flow is disabled. With all that explained, back to the stage.", "trl_firstActionFirstKill_def_txt_05", tutorialsUI.textBox.energyBlockSprite),
                    };
                    break;
            }

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // // FIRST ACTION COMPLETE - Removed
        // // Loads the first action stage complete tutorial.
        // public void LoadFirstActionCompleteTutorial(bool startTutorial = true)
        // {
        //     // Gets the tutorial info.
        //     TutorialInfo tutorialInfo = GetFirstActionCompleteTutorialInfo();
        // 
        //     // Sets the bool and loads the tutorial.
        //     data.clearedFirstActionCompleteTutorial = true;
        //     LoadTutorial(ref tutorialInfo.pages, startTutorial);
        // }
        // 
        // // Loads the first action complete tutorial info into an object and returns it.
        // public TutorialInfo GetFirstActionCompleteTutorialInfo()
        // {
        //     // The title and title translation key.
        //     string title = "First Action Stage Complete";
        //     string titleKey = "trl_firstActionComplete_ttl";
        // 
        //     // Create the pages list.
        //     List<Page> pages = new List<Page>
        //     {
        //         // Load the pages.
        //         new EDU_Page(title, titleKey, "You've completed your first action stage! When a stage is cleared, sometimes one or more new stages are unlocked. Some stages can be beaten before others, but all stages must be completed to finish the simulation.", "trl_firstActionComplete_txt_00"),
        //     };
        // 
        //     // Creates the info object.
        //     TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);
        // 
        //     // Returns the info.
        //     return tutorialInfo;
        // }

        // FIRST KNOWLEDGE - INTRO
        // Loads the first knowledge stage - intro tutorial.
        public void LoadFirstKnowledgeIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstKnowledgeIntroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first knowledge - intro tutorial info into an object and returns it.
        public TutorialInfo GetFirstKnowledgeIntroTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Introduction";
            string titleKey = "trl_firstKnowledgeIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is a knowledge stage! In knowledge stages, you must match statements with the resources they apply to. All statements must be matched correctly to complete the stage. Statement buttons are blue and resource buttons are green.", "trl_firstKnowledgeIntro_txt_00", tutorialsUI.textBox.knowledgeElementsSprite),
                new EDU_Page(title, titleKey, "When you have a statement or resource selected, its text is shown in the bottom middle. If you have a statement selected, it'll be connected to the next resource you select, and vice versa. When a statement and resource are connected, they both display the same number.", "trl_firstKnowledgeIntro_txt_01", tutorialsUI.textBox.knowledgeElementsSprite),
                new EDU_Page(title, titleKey, "Once connections are made, select the verify button in the bottom right to check your answers. This process locks all correct matches and disconnects all incorrect matches. The number of correct matches you have is displayed in the bottom left, which is updated every verification.", "trl_firstKnowledgeIntro_txt_02"),
                new EDU_Page(title, titleKey, "For information on resources, you can check the info log using the button in the top left. Try to beat the stage with as few verifications as possible. With all that covered, let the stage begin!", "trl_firstKnowledgeIntro_txt_03", tutorialsUI.textBox.logButtonSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST KNOWLEDGE - VERIFY
        // Loads the first knowledge stage - verify tutorial.
        public void LoadFirstKnowledgeVerifyTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstKnowledgeVerifyTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeVerifyTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first knowledge verify tutorial info into an object and returns it.
        public TutorialInfo GetFirstKnowledgeVerifyTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Verification";
            string titleKey = "trl_firstKnowledgeVerify_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've performed a connection verification! All statements and resources that were matched correctly have become locked. All statements that weren't matched correctly or weren't matched at all have been randomized.", "trl_firstKnowledgeVerify_txt_00"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // // FIRST KNOWLEDGE COMPLETE - Removed
        // // Loads the first knowledge stage complete tutorial.
        // public void LoadFirstKnowledgeCompleteTutorial(bool startTutorial = true)
        // {
        //     // Gets the tutorial info.
        //     TutorialInfo tutorialInfo = GetFirstKnowledgeCompleteTutorialInfo();
        // 
        //     // Sets the bool and loads the tutorial.
        //     data.clearedFirstKnowledgeCompleteTutorial = true;
        //     LoadTutorial(ref tutorialInfo.pages, startTutorial);
        // }
        // 
        // // Loads the first knowledge complete tutorial info into an object and returns it.
        // public TutorialInfo GetFirstKnowledgeCompleteTutorialInfo()
        // {
        //     // The title and title translation key.
        //     string title = "First Knowledge Stage Complete";
        //     string titleKey = "trl_firstKnowledgeComplete_ttl";
        // 
        //     // Create the pages list.
        //     List<Page> pages = new List<Page>
        //     {
        //         // Load the pages.
        //         new EDU_Page(title, titleKey, "You've completed your first knowledge stage! If you got an energy start bonus, it'll be displayed at the top middle of the screen. If applicable, the energy bonus will automatically be applied at the start of your next action stage.", "trl_firstKnowledgeComplete_txt_00"),
        //     };
        // 
        //     // Creates the info object.
        //     TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);
        // 
        //     // Returns the info.
        //     return tutorialInfo;
        // }

        // FIRST AREA COMPLETE
        // Loads the first area complete tutorial.
        public void LoadFirstAreaCompleteTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstAreaCompleteTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstAreaCompleteTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first area complete tutorial info into an object and returns it.
        public TutorialInfo GetFirstAreaCompleteTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Area Complete";
            string titleKey = "trl_firstAreaComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've cleared the first area! When all stages in an area are completed, you're able to move onto the next area. There are three areas total, and all areas must be cleared to complete the game. Switch areas using the world arrow buttons.", "trl_firstAreaComplete_txt_00"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // // FNAL AREA INTRO - Removed
        // // Loads the final area intro tutorial.
        // public void LoadFinalAreaIntroTutorial(bool startTutorial = true)
        // {
        //     // Gets the tutorial info.
        //     TutorialInfo tutorialInfo = GetFinalAreaIntroTutorialInfo();
        // 
        //     // Sets the bool and loads the tutorial.
        //     data.clearedFinalAreaIntroTutorial = true;
        //     LoadTutorial(ref tutorialInfo.pages, startTutorial);
        // }
        // 
        // // Loads the final area intro tutorial info into an object and returns it.
        // // NOTE: this tutorial has been removed.
        // public TutorialInfo GetFinalAreaIntroTutorialInfo()
        // {
        //     // The title and title translation key.
        //     string title = "Final Area Introduction";
        //     string titleKey = "trl_finalAreaIntro_ttl";
        // 
        //     // Create the pages list.
        //     List<Page> pages = new List<Page>
        //     {
        //         // Load the pages.
        //         new EDU_Page(title, titleKey, "This is the final area! Once you clear all the stages in this area, the simulation game is complete. Good luck!", "trl_finalAreaIntro_txt_00"),
        //     };
        // 
        //     // Creates the info object.
        //     TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);
        // 
        //     // Returns the info.
        //     return tutorialInfo;
        // }


        // NATURAL RESOURCES //
        // Returns if the requested natural resource tutorial has been cleared.
        public bool GetNaturalResourceTutorialCleared(NaturalResources.naturalResource resource)
        {
            // The result to be returned.
            bool result;

            // Checks the resource.
            switch (resource)
            {
                default:
                case NaturalResources.naturalResource.unknown:
                    result = false; // False by default.
                    break;

                case NaturalResources.naturalResource.biomass:
                    result = data.clearedBiomassTutorial;
                    break;

                case NaturalResources.naturalResource.geothermal:
                    result = data.clearedGeothermalTutorial;
                    break;

                case NaturalResources.naturalResource.hydro:
                    result = data.clearedHydroTutorial;
                    break;

                case NaturalResources.naturalResource.solar:
                    result = data.clearedSolarTutorial;
                    break;

                case NaturalResources.naturalResource.wave:
                    result = data.clearedWaveTutorial;
                    break;

                case NaturalResources.naturalResource.wind:
                    result = data.clearedWindTutorial;
                    break;

                case NaturalResources.naturalResource.coal:
                    result = data.clearedCoalTutorial;
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    result = data.clearedNaturalGasTutorial;
                    break;

                case NaturalResources.naturalResource.nuclear:
                    result = data.clearedNuclearTutorial;
                    break;

                case NaturalResources.naturalResource.oil:
                    result = data.clearedOilTutorial;
                    break;
            }

            return result;
        }

        // Sets natural resource tutorial cleared by the provided resource.
        public void SetNaturalResourceTutorialCleared(NaturalResources.naturalResource resource, bool cleared)
        {
            // Checks the resource.
            switch (resource)
            {
                // default:
                // case NaturalResources.naturalResource.unknown:
                //     // ...
                //     break;

                case NaturalResources.naturalResource.biomass:
                    data.clearedBiomassTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.geothermal:
                    data.clearedGeothermalTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.hydro:
                    data.clearedHydroTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.solar:
                    data.clearedSolarTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.wave:
                    data.clearedWaveTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.wind:
                    data.clearedWindTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.coal:
                    data.clearedCoalTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    data.clearedNaturalGasTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.nuclear:
                    data.clearedNuclearTutorial = cleared;
                    break;

                case NaturalResources.naturalResource.oil:
                    data.clearedOilTutorial = cleared;
                    break;
            }
        }

        // Gets tutorial info for the provided natural resource.
        public TutorialInfo GetNaturalResourceTutorialInfo(NaturalResources.naturalResource resource)
        {
            // The tutorial info to be returned.
            TutorialInfo tutorialInfo = null;

            // Checks the resource.
            switch (resource)
            {
                default:
                case NaturalResources.naturalResource.unknown:
                    tutorialInfo = null;
                    break;

                case NaturalResources.naturalResource.biomass:
                    tutorialInfo = GetBiomassTutorialInfo();
                    break;

                case NaturalResources.naturalResource.geothermal:
                    tutorialInfo = GetGeothermalTutorialInfo();
                    break;

                case NaturalResources.naturalResource.hydro:
                    tutorialInfo = GetHydroTutorialInfo();
                    break;

                case NaturalResources.naturalResource.solar:
                    tutorialInfo = GetSolarTutorialInfo();
                    break;

                case NaturalResources.naturalResource.wave:
                    tutorialInfo = GetWaveTutorialInfo();
                    break;

                case NaturalResources.naturalResource.wind:
                    tutorialInfo = GetWindTutorialInfo();
                    break;

                case NaturalResources.naturalResource.coal:
                    tutorialInfo = GetCoalTutorialInfo();
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    tutorialInfo = GetNaturalGasTutorialInfo();
                    break;

                case NaturalResources.naturalResource.nuclear:
                    tutorialInfo = GetNuclearTutorialInfo();
                    break;

                case NaturalResources.naturalResource.oil:
                    tutorialInfo = GetOilTutorialInfo();
                    break;
            }

            return tutorialInfo;
        }

        // BIOMASS
        // Loads the biomass tutorial.
        public void LoadBiomassTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetBiomassTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedBiomassTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the biomass tutorial info into an object and returns it.
        public TutorialInfo GetBiomassTutorialInfo()
        {
            // The title and title translation key.
            string title = "Biomass";
            string titleKey = "trl_biomass_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses biomass, which is a renewable resource that burns plant and/or waste materials for generating energy. A biomass generator can be placed anywhere on land, generates energy regularly, and produces no air pollution. It generates a moderate amount of energy at a moderate rate.", "trl_biomass_txt_00", tutorialsUI.textBox.biomassSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // GEOTHERMAL
        // Loads the geothermal tutorial.
        public void LoadGeothermalTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetGeothermalTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedGeothermalTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the geothermal tutorial info into an object and returns it.
        public TutorialInfo GetGeothermalTutorialInfo()
        {
            // The title and title translation key.
            string title = "Geothermal";
            string titleKey = "trl_geothermal_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses geothermal, a renewable resource that uses Earth's internal heat for generating energy. Geothermal generators can only be placed on spots with geothermal symbols, generate energy perpetually, and produce no air pollution. They generate high amounts of energy at a moderate rate.", "trl_geothermal_txt_00", tutorialsUI.textBox.geothermalSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // HYDRO
        // Loads the hydro tutorial.
        public void LoadHydroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetHydroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedHydroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the hydro tutorial info into an object and returns it.
        public TutorialInfo GetHydroTutorialInfo()
        {
            // The title and title translation key.
            string title = "Hydro";
            string titleKey = "trl_hydro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses hydro, a renewable resource that uses the flow of water for generating energy. Hydro generators can only be placed in rivers and cannot be placed next to other hydro generators.", "trl_hydro_txt_00", tutorialsUI.textBox.hydroSprite),
                new EDU_Page(title, titleKey, "Hydro generators produce a very small amount of air pollution and will flood the spots behind them if they run for too long. They produce a high amount of energy at a high rate.", "trl_hydro_txt_01", tutorialsUI.textBox.hydroSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // SOLAR
        // Loads the solar tutorial.
        public void LoadSolarTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetSolarTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedSolarTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the solar tutorial info into an object and returns it.
        public TutorialInfo GetSolarTutorialInfo()
        {
            // The title and title translation key.
            string title = "Solar";
            string titleKey = "trl_solar_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses solar, a renewable resource that converts sunlight into electricity. Solar generators can be placed anywhere on land, produce no air pollution, and only generate energy during the day. They produce a moderate amount of energy at a moderate rate.", "trl_solar_txt_00", tutorialsUI.textBox.solarSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // WAVE
        // Loads the wave tutorial.
        public void LoadWaveTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetWaveTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedWaveTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the wave tutorial info into an object and returns it.
        public TutorialInfo GetWaveTutorialInfo()
        {
            // The title and title translation key.
            string title = "Wave";
            string titleKey = "trl_wave_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses wave, which is a renewable resource that uses the movement of waves for generating energy. Wave generators can only be placed in the sea, produce moderate amounts of energy, and create no air pollution. The energy generation speed is determined by the wind speed.", "trl_wave_txt_00", tutorialsUI.textBox.waveSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // WIND
        // Loads the wind tutorial.
        public void LoadWindTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetWindTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedWindTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the wind tutorial info into an object and returns it.
        public TutorialInfo GetWindTutorialInfo()
        {
            // The title and title translation key.
            string title = "Wind";
            string titleKey = "trl_wind_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses wind, which is a renewable resource that uses moving air for generating energy. Wind generators can be placed anywhere on land or in water near land. They create no air pollution and produce moderate amounts of energy. The energy generation speed is determined by the wind speed.", "trl_wind_txt_00", tutorialsUI.textBox.windSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // COAL
        // Loads the coal tutorial.
        public void LoadCoalTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetCoalTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedCoalTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the coal tutorial info into an object and returns it.
        public TutorialInfo GetCoalTutorialInfo()
        {
            // The title and title translation key.
            string title = "Coal";
            string titleKey = "trl_coal_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses coal, a non-renewable fossil fuel resource that's fed to a furnace for generating energy. Coal generators can only be placed on spots with coal symbols, generate energy for a limited time, and create high amounts of air pollution. They produce high amounts of energy at a high rate.", "trl_coal_txt_00", tutorialsUI.textBox.coalSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // NATURAL GAS
        // Loads the naturalgas tutorial.
        public void LoadNaturalGasTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetNaturalGasTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedNaturalGasTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the natural gas tutorial info into an object and returns it.
        public TutorialInfo GetNaturalGasTutorialInfo()
        {
            // The title and title translation key.
            string title = "Natural Gas";
            string titleKey = "trl_naturalGas_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses natural gas, which is a non-renewable fossil fuel resource that's burned to produce gases to turn a turbine to generate energy. It is often found with coal or oil.", "trl_naturalGas_txt_00", tutorialsUI.textBox.naturalGasSprite),
                new EDU_Page(title, titleKey, "Natural gas generators generate energy for a limited time, produce moderate amounts of air pollution, and can be placed on spots with natural gas, coal, or oil symbols. They produce a high amount of energy at a moderate rate.", "trl_naturalGas_txt_01", tutorialsUI.textBox.naturalGasSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // NUCLEAR
        // Loads the nuclear tutorial.
        public void LoadNuclearTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetNuclearTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedNuclearTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the nuclear tutorial info into an object and returns it.
        public TutorialInfo GetNuclearTutorialInfo()
        {
            // The title and title translation key.
            string title = "Nuclear";
            string titleKey = "trl_nuclear_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses nuclear, a non-renewable resource that splits uranium atoms for generating energy. Nuclear generators can only be placed on spots with nuclear symbols, generate energy for a limited time, produce no air pollution, and are automatically destroyed once they run out of resources.", "trl_nuclear_txt_00", tutorialsUI.textBox.nuclearSprite),
                new EDU_Page(title, titleKey, "If a nuclear generator is destroyed by an enemy before its finished, it will leave nuclear waste behind. No units can be placed on tiles that have nuclear waste. Nuclear generators produce a very high amount of energy at a very high rate.", "trl_nuclear_txt_01", tutorialsUI.textBox.nuclearSprite),            
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // OIL
        // Loads the oil tutorial.
        public void LoadOilTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetOilTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedOilTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the oil tutorial info into an object and returns it.
        public TutorialInfo GetOilTutorialInfo()
        {
            // The title and title translation key.
            string title = "Oil";
            string titleKey = "trl_oil_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses oil, which is a non-renewable fossil fuel resource that's burned for producing energy. Oil can be found on land or at sea. Oil generators can only be placed on spots with oil symbols, generate energy for a limited time, and produce high amounts of air pollution.", "trl_oil_txt_00", tutorialsUI.textBox.oilSprite),
                new EDU_Page(title, titleKey, "Oil generators leave oil spills if they're destroyed by enemies before they've used up their resources. No units can be placed on spots that have oil spills. Oil generators generate a very high amount of energy at a moderate rate.", "trl_oil_txt_01", tutorialsUI.textBox.oilSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }
    }
}