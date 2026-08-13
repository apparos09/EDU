using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The gameplay manager.
    public class GameplayManager : MonoBehaviour
    {
        // The game UI.
        public GameplayUI gameUI;

        // The game audio.
        public GameAudio gameAudio;

        // The game speed.
        // This is only used for the Action Stages.
        private float gameTimeScale = 1.0F;

        // The timer for the game.
        // NOTE: the data logger is used to track the overall game time.
        // The variable gameTimer is used to track time in the specific scene.
        public float gameTimer = 0;

        // If 'true', the game run timer is run.
        // This is seperate from the "pause" function so that time scale is uneffected.
        [Tooltip("If true, the game timer is running. Stopping the game timer from running doesn't change the time scale.")]
        public bool runGameTimer = true;

        // The game score.
        public float gameScore = 0;

        // Pauses the timer if true.
        [Tooltip("If true, the game is paused, which changes the time scale to 0.")]
        private bool gamePaused = false;

        // The mouse touch object. // Unneeded.
        // public MouseTouchInput mouseTouch;

        // The tutorials object.
        public Tutorials tutorials;

        // Returns 'true' if tutorials enabled.
        private bool tutorialsEnabled = true; // Set to false when testing other functions.

        // The title scene.
        public string titleScene = "TitleScene";

        // The world scene.
        public string worldScene = "WorldScene";

        // The action scene.
        public string actionScene = "ActionScene";

        // The knowledge scene.
        public string knowledgeScene = "KnowledgeScene";

        // The results scene.
        public string resultsScene = "ResultsScene";

        // If 'true', the loading screen is enabled.
        public bool loadingScreenEnabled = true;

        // The data logger.
        public DataLogger dataLogger;

        // If 'true', late start has been called.
        protected bool calledLateStart = false;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // Makes sure the game isn't paused and resets the game time scale to 1.0.
            UnpauseGame();
            ResetGameTimeScale(true);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the game UI isn't set, try to find it.
            if (gameUI == null)
            {
                gameUI = FindObjectOfType<GameplayUI>();
            }

            // If the game audio isn't set, try to find it.
            if(gameAudio == null)
            {
                gameAudio = FindObjectOfType<GameAudio>();
            }
                

            // If the data logger isn't set, get the instance.
            if (dataLogger == null)
            {
                // Checks if the data logger is already instantiated.
                bool alreadyInstantiated = DataLogger.Instantiated;

                // Grabs the instance.
                dataLogger = DataLogger.Instance;

                // If the object wasn't already instanted, set it to not being destroyed on load.
                if(!alreadyInstantiated)
                {
                    // If the data logger wasn't already instantiated, set it to not be destroyed on load.
                    DontDestroyOnLoad(dataLogger.gameObject);
                }
            }

            // Sets the tutorials object.
            if (tutorials == null)
            {
                tutorials = Tutorials.Instance;
            }

            // If the tutorials object doesn't have a game manager, give it this.
            if (tutorials.gameManager == null)
            {
                tutorials.gameManager = this;
            }
            

            // If the gameUI and tutorials is set, check for the tutorial text box.
            if (gameUI != null && tutorials != null)
            {
                // IF the tutorial UI isn't set.
                if(gameUI.tutorialUI == null)
                {
                    // The tutorial UI has been instantiated, so get the instance.
                    if (TutorialUI.Instantiated)
                    {
                        gameUI.tutorialUI = TutorialUI.Instance;
                    }
                }

                // If the gameUI has the tutorial UI.
                if (gameUI.tutorialUI != null)
                {
                    // If the tutorial text box has been set...
                    if (gameUI.tutorialUI.textBox != null)
                    {
                        // Adds the callback from the tutorial text box.
                        // You probably don't need to remove them.
                        gameUI.AddTutorialTextBoxCallbacks(this);
                    }
                }
            }
        }

        // A function called the first time Update(0 is called, which happens after Start() was called.
        protected virtual void LateStart()
        {
            calledLateStart = true;
        }

        // If 'true', late start has been called.
        public bool CalledLateStart
        {
            get { return calledLateStart; }
        }

        // GAME TIME
        // Gets the game timer.
        public float GetGameTimer()
        {
            return gameTimer;
        }

        // Resets the game timer.
        public void ResetGameTimer()
        {
            gameTimer = 0.0F;
        }

        // GAME TIME SCALE AND PAUSING //
        // Gets the game time scale.
        public float GetGameTimeScale()
        {
            return gameTimeScale;
        }

        // Sets the game time scale.
        public void SetGameTimeScale(float value, bool setCurrentTimeScale)
        {
            gameTimeScale = value;

            // If the current time scale should be set.
            if(setCurrentTimeScale)
            {
                Time.timeScale = gameTimeScale;
            }
        }

        // Resets the time scale.
        // setCurrentTimeScale: sets the current time scale to the reset game time scale if true.
        public void ResetGameTimeScale(bool setCurrentTimeScale)
        {
            SetGameTimeScale(1.0F, setCurrentTimeScale); 
        }

        // Sets the current time scale to game time scale.
        public void SetCurrentTimeScaleToGameTimeScale()
        {
            Time.timeScale = gameTimeScale;
        }

        // Returns 'true' if the game time scale is 1.0.
        public bool IsGameTimeScaleNormal()
        {
            return gameTimeScale == 1.0F;
        }

        // Returns 'true' if the game timer is running.
        public bool IsGameTimerRunning()
        {
            return runGameTimer;
        }

        // Set if the game timer should be running.
        public void SetGameTimerRunning(bool running)
        {
            runGameTimer = running;
        }

        // Pauses the game timer.
        public void PauseGameTimer()
        {
            SetGameTimerRunning(false);
        }

        // Unpauses the game timer.
        public void UnpauseGameTimer()
        {
            SetGameTimerRunning(true);
        }

        // Toggle the game timer running.
        public void ToggleGameTimerRunning()
        {
            runGameTimer = !runGameTimer;
        }

        // Returns 'true' if the game is paused.
        public bool IsGamePaused()
        {
            return gamePaused;
        }

        // Sets if the game should be paused.
        public virtual void SetGamePaused(bool paused)
        {
            gamePaused = paused;

            // If the game is paused.
            if (gamePaused)
            {
                // NOTE: this does not change the time scale.
                Time.timeScale = 0.0F;
            }
            else // If the game is not paused.
            {
                // If the tutorial is not running, set the time scale to 1.0F.
                if (!IsTutorialRunning())
                {
                    // Gets the game time scale.
                    float gts = GetGameTimeScale();

                    // If the time scale is 0, reset the time scale.
                    if (gts == 0)
                    {
                        ResetGameTimeScale(false);
                        gts = GetGameTimeScale();
                    }

                    // Set the game time scale.
                    Time.timeScale = gts;
                }
            }
        }

        // Pauses the game.
        public virtual void PauseGame()
        {
            SetGamePaused(true);
        }

        // Unpauses the game.
        public virtual void UnpauseGame()
        {
            SetGamePaused(false);
        }

        // Toggles if the game is paused or not.
        public virtual void TogglePausedGame()
        {
            SetGamePaused(!gamePaused);
        }

        // TUTORIAL //
        // Checks if the game is using the tutorial.
        public bool IsUsingTutorials()
        {
            // The result.
            bool result;

            // If the tutorial UI isn't instantiated, the tutorial isn't being used.
            // The tutorial UI uses a prefab, so it can't be instantiated on its own.
            if(!TutorialUI.Instantiated)
            {
                Debug.LogWarning("The TutorialUI hasn't been instantiated.");
                return false;
            }

            // If the tutorials object is null, instantiate it.
            if (tutorials == null)
                tutorials = Tutorials.Instance;

            // Checks if tutorials are enabled at all.
            if(tutorialsEnabled)
            {
                // If the game settings is instantiated, check if tutorials are being used.
                if (GameSettings.Instantiated)
                {
                    // Returns 'true' if the game settings is set to use the tutorials...
                    // And the tutorials are enabled.
                    result = GameSettings.Instance.UseTutorials;
                }
                else
                {
                    // Game settings not instantiated, so return false by default.
                    result = false;
                }

                // NOTE: if testing tutorials, uncomment line below to ignore result.
                // return true;
            }
            // Tutorials not enabled, so return false automatically.
            else
            {
                result = false;
            }

            return result;
        }

        // Set if the tutorial will be used.
        public void SetUsingTutorials(bool value)
        {
            GameSettings.Instance.UseTutorials = value;
        }

        // Returns 'true' if a tutorial is active.
        public bool IsTutorialActive()
        {
            return gameUI.IsTutorialActive();
        }

        // Returns 'true' if tutorials are being used a tutorial is not active.
        public bool IsUsingTutorialsAndTutorialNotActive()
        {
            return IsUsingTutorials() && !IsTutorialActive();
        }

        // Checks if the text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            return gameUI.IsTutorialTextBoxOpen();
        }

        // Checks if the tutorial is running.
        public bool IsTutorialRunning()
        {
            // Check this function.
            return gameUI.IsTutorialRunning();

        }

        // Starts a tutorial using the provided pages.
        public virtual void StartTutorial(List<Page> pages)
        {
            gameUI.StartTutorial(pages);
        }

        // Called when a tutorial is started.
        public virtual void OnTutorialStart()
        {
            gameUI.OnTutorialStart();
        }

        // Called when a tutorial is ended.
        public virtual void OnTutorialEnd()
        {
            gameUI.OnTutorialEnd();
        }

        // LOADING SCREEN
        // Returns 'true' if the loading screen is available.
        public bool IsLoadingScreenAvailable()
        {
            return EDU_LoadingSceneCanvas.Instantiated;
        }

        // Returns 'true' if the loading screen is enabled.
        public bool IsLoadingScreenEnabled()
        {
            return loadingScreenEnabled;
        }

        // Returns 'true'if loading screen is available and enabled.
        public bool IsLoadingScreenAvailableAndEnabled()
        {
            return EDU_LoadingSceneCanvas.Instantiated && loadingScreenEnabled;
        }

        // Returns true if the game is loading.
        public bool IsLoading()
        {
            // If the loading screen is disabled, it's not loading.
            if(loadingScreenEnabled)
            {
                // Checks if the loading screen canvas exists.
                if(EDU_LoadingSceneCanvas.Instantiated)
                {
                    // Returns 'true' if the loading graphic is currently being used.
                    return EDU_LoadingSceneCanvas.Instance.loadingGraphic.IsLoading;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // SCENES
        // Loads the scene and cheks the loading screen member variable to see if the loading screen should be used.
        public virtual void LoadScene(string scene)
        {
            LoadScene(scene, loadingScreenEnabled);
        }

        // Loads a scene, which checks if a loading screen should be used.
        public virtual void LoadScene(string scene, bool useLoadingScreen)
        {
            // Unpauses the game and resets the game time scale...
            // So that the next scene isn't frozen once loaded.
            // This also makes sure the loading screen animation works.
            UnpauseGame();
            ResetGameTimeScale(true);

            // If the loading screen should be used and the loading scene is available.
            if(useLoadingScreen && IsLoadingScreenAvailableAndEnabled())
            {
                // Gets the instance.
                EDU_LoadingSceneCanvas loadingSceneCanvas = EDU_LoadingSceneCanvas.Instance;

                // Checks if the loading graphic is being used.
                // If not, use a normal scene load.
                if(loadingSceneCanvas.IsUsingLoadingGraphic())
                {
                    // Loading Screen
                    loadingSceneCanvas.LoadScene(scene);
                }
                else
                {
                    // Normal
                    SceneManager.LoadScene(scene);
                }
            }
            else
            {
                // Normal scene load.
                SceneManager.LoadScene(scene);
            }
        }

        // Loads the title scene and checks a member variable for using the loading screen.
        public void LoadTitleScene()
        {
            LoadTitleScene(loadingScreenEnabled);
        }

        // Loads the title scene.
        public void LoadTitleScene(bool useLoadingScreen)
        {
            // If the data logger exists, destroy it before returning to the tile screen.
            if (DataLogger.Instantiated)
                Destroy(DataLogger.Instance.gameObject);

            // Loads the title screen.
            LoadScene(titleScene, useLoadingScreen);
        }

        // Loads the world scene and checks a member function for using the loading screen.
        public virtual void LoadWorldScene()
        {
            LoadWorldScene(loadingScreenEnabled);
        }

        // Goes to the world scene.
        public virtual void LoadWorldScene(bool useLoadingScreen)
        {
            LoadScene(worldScene, useLoadingScreen);
        }

        // Loads the action scene and checks a member function for using the loading screen.
        public virtual void LoadActionScene()
        {
            LoadActionScene(loadingScreenEnabled);
        }

        // Goes to the action scene.
        public virtual void LoadActionScene(bool useLoadingScreen)
        {
            LoadScene(actionScene, useLoadingScreen);
        }

        // Loads the knowledge scene. Uses a loading screen based on a class variable.
        public virtual void LoadKnowledgeScene()
        {
            LoadKnowledgeScene(loadingScreenEnabled);
        }

        // Goes to the knowledge scene and has the user set if the loading screen should be used.
        public virtual void LoadKnowledgeScene(bool useLoadingScreen)
        {
            LoadScene(knowledgeScene, useLoadingScreen);
        }

        // Loads the results scene.
        public virtual void LoadResultsScene()
        {
            LoadResultsScene(loadingScreenEnabled);
        }

        // Loads the results scene and uses argument to determine if the loading screen should be used.
        public virtual void LoadResultsScene(bool useLoadingScreen)
        {
            LoadScene(resultsScene, useLoadingScreen);
        }

        // Quits the game.
        public virtual void QuitGame()
        {
            Application.Quit();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // If late start hasn't been called, call it.
            if(!calledLateStart)
                LateStart();

            // The game isn't paused, add to the game time.
            if (runGameTimer && !IsGamePaused())
            {
                gameTimer += Time.unscaledDeltaTime;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // Unpauses the game and resets the game time scale.
            UnpauseGame();
            ResetGameTimeScale(true);
        }
    }
}