using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The stage manager.
    public abstract class StageManager : GameplayManager
    {
        // The game will skip these overwritten functions in StageManager and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        [Header("Stage")]

        // The stage user interface.
        public StageUI stageUI;

        // The stage audio.
        public StageAudio stageAudio;

        // The timer for the stage. This is scaled with delta time since it's tied to game events.
        // This is effected by the time scale and is reset if the stage starts over.
        [Tooltip("The stage timer, which is used for some stage events. This is effected by time scale and reset if the stage starts over.")]
        public float stageTimer = 0.0F;

        // The stage's ID number.
        public int idNumber = 0;

        // The world stage index.
        public int worldStageIndex = -1;

        // The stage's difficulty.
        public int difficulty = 0;

        // The stage's BGM number. If the BGM number is out of bounds, it's clamped. 0 = no BGM.
        [Tooltip("The BGM to be played. Value is clamped if outside of bounds. 0 = no BGM.")]
        public int bgmNumber = 0;

        // If 'true', the BGM is restarted if the stage is reset.
        private bool restartStageBgmOnReset = true;

        // The natural resources that will be used.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // Gets set to 'true' if the stage is initialized.
        protected bool stageInitialized = false;

        // Gets set to 'true' when the stage is active/running.
        private bool stagePlaying = false;

        // NOTE: this is not called in the Start() function of StageManager() since some other things...
        // Need to be initialized first.

        // Gets set to 'true' when tutorials have been checked. Checks for tutorials in Update().
        [Tooltip("Becomes true when tutorials have been checked. Set to false to check for tutorials in Update().")]
        public bool checkedTutorials = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finds the stage ui if it isn't set.
            if (stageUI == null)
            {
                stageUI = FindObjectOfType<StageUI>();
            }

            // If the stage audio isn't set, try to find it.
            if (stageAudio == null)
            {
                stageAudio = FindObjectOfType<StageAudio>();
            }

            // Will be set to false in late start to check tutorials.
            checkedTutorials = true;
        }

        // Late start.
        protected override void LateStart()
        {
            base.LateStart();

            // Check for tutorials.
            checkedTutorials = false;
        }

        // Initializes the stage.
        public virtual void InitializeStage()
        {
            // Plays the BGM.
            PlayStageBgm();

            // Stage is initialized.
            stageInitialized = true;

            // Stage is playing.
            stagePlaying = true;
        }

        // Returns 'true' if the stage has been initialized.
        public bool StageInitialized
        {
            get { return stageInitialized; }
        }

        // AUDIO //
        // Returns 'true' if one of the stage BGMs are playing.
        public virtual bool IsStageBgmSet()
        {
            bool result;

            // If the stage audio isn't null.
            if (stageAudio != null)
            {
                // If either BGMs are set, return true.
                result = stageAudio.IsStageBgm01Set() || stageAudio.IsStageBgm02Set();
            }
            else
            {
                result = false;
            }

            return result;
        }

        // Returns 'true' if a stage BGM is currently playing.
        public virtual bool IsStageBgmPlaying()
        {
            bool result;

            // Checks if stage audio exists.
            if (stageAudio != null)
            {
                // Returns 'true' if a stage BGM is set and the BGM source is playing.
                result = IsStageBgmSet() && stageAudio.bgmSource.isPlaying;
            }
            else
            {
                result = false;
            }

            return result;

        }

        // Plays the BGM.
        public void PlayStageBgm()
        {
            // Stage audio is set.
            if (stageAudio != null)
            {
                // Clamps the BGM number.
                bgmNumber = Mathf.Clamp(bgmNumber, 0, stageAudio.GetStageBgmCount());

                // Plays the stage BGM. Note that this might be overwritten.
                stageAudio.PlayStageBgm(bgmNumber);
            }
        }

        // Stops the BGM.
        public void StopStageBgm()
        {
            // Stage audio is set.
            if(stageAudio != null)
            {
                stageAudio.StopBackgroundMusic();
            }
        }

        // Restarts the stage BGM.
        public void RestartStageBgm()
        {
            // The stage audio is set.
            if(stageAudio != null)
            {
                // Stops and plays the BGM so that the BGM is set.
                StopStageBgm();
                PlayStageBgm();
            }
        }

        // Plays the stage results BGM.
        public void PlayStageResultsBgm()
        {
            // Stage audio is set.
            if(stageAudio != null)
            {
                // Play the results BGM.
                stageAudio.PlayStageResultsBgm();
            }
        }

        // TUTORIALS //
        // Called when a tutorial is ended.
        public override void OnTutorialEnd()
        {
            // Check for tutorials.
            checkedTutorials = false;
        }

        // Checks for tutorials.
        public virtual void CheckTutorials()
        {
            // Uses tutorials and tutorial isn't active.
            if (IsUsingTutorials() && !IsTutorialActive())
            {
                // Gets set to true when a tutorial has started.
                bool startedTutorial = false;

                // Biomass
                if(!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.biomass) && 
                    !tutorials.Data.clearedBiomassTutorial)
                {
                    tutorials.LoadBiomassTutorial();
                    startedTutorial = true;
                }

                // Geothermal
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.geothermal) && 
                    !tutorials.Data.clearedGeothermalTutorial)
                {
                    tutorials.LoadGeothermalTutorial();
                    startedTutorial = true;
                }

                // Hydro
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.hydro) &&
                    !tutorials.Data.clearedHydroTutorial)
                {
                    tutorials.LoadHydroTutorial();
                    startedTutorial = true;
                }

                // Solar
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.solar) &&
                    !tutorials.Data.clearedSolarTutorial)
                {
                    tutorials.LoadSolarTutorial();
                    startedTutorial = true;
                }

                // Wave
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.wave) &&
                    !tutorials.Data.clearedWaveTutorial)
                {
                    tutorials.LoadWaveTutorial();
                    startedTutorial = true;
                }

                // Wind
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.wind) &&
                    !tutorials.Data.clearedWindTutorial)
                {
                    tutorials.LoadWindTutorial();
                    startedTutorial = true;
                }

                // Coal
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.coal) &&
                    !tutorials.Data.clearedCoalTutorial)
                {
                    tutorials.LoadCoalTutorial();
                    startedTutorial = true;
                }

                // Natural Gas
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.naturalGas) &&
                    !tutorials.Data.clearedNaturalGasTutorial)
                {
                    tutorials.LoadNaturalGasTutorial();
                    startedTutorial = true;
                }

                // Nuclear
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.nuclear) &&
                    !tutorials.Data.clearedNuclearTutorial)
                {
                    tutorials.LoadNuclearTutorial();
                    startedTutorial = true;
                }

                // Oil
                if (!startedTutorial && naturalResources.Contains(NaturalResources.naturalResource.oil) &&
                    !tutorials.Data.clearedOilTutorial)
                {
                    tutorials.LoadOilTutorial();
                    startedTutorial = true;
                }

            }

            // Tutorials have been checked.
            checkedTutorials = true;
        }


        // NATURAL RESOURCES //
        // Sets the natural resource list to the resource type list.
        public virtual void SetNaturalResourceListToTypeList(bool includeUnknown = false)
        {
            naturalResources.Clear();
            naturalResources = NaturalResources.GenerateNaturalResourceTypeList(includeUnknown);
        }

        // STAGE //
        // Returns 'true' if the stage is playing.
        public bool IsStagePlaying()
        {
            return stagePlaying;
        }

        // Sets if the stage is playing or not.
        public void SetStagePlaying(bool stagePlaying)
        {
            this.stagePlaying = stagePlaying;
        }

        // Returns 'true' if the stage is playing and the game is unpaused.
        public bool IsStagePlayingAndGameUnpaused()
        {
            return IsStagePlaying() && !IsGamePaused();
        }

        // Gets the stage type.
        public WorldStage.stageType GetStageType()
        {
            // Checks the manager to see what type of stage it is.
            if(this is ActionManager)
            {
                return WorldStage.stageType.action;
            }
            else if(this is KnowledgeManager)
            {
                return WorldStage.stageType.knowledge;
            }
            else
            {
                return WorldStage.stageType.unknown;
            }
        }

        // Returns the stage time.
        public virtual float GetStageTimer()
        {
            return stageTimer;
        }

        // Resets the game timer and stage timer.
        public void ResetGameTimerAndStageTimer()
        {
            ResetGameTimer();
            ResetStageTimer();
        }

        // Resets the stage timer.
        // NOTE: this doesn't reset gameTimer. The stage timer is effected by time scale, but the game timer is not.
        // If you want to reset the game timer, call ResetStageTimer().
        public virtual void ResetStageTimer()
        {
            stageTimer = 0.0F;
        }

        // Returns the stage score.
        public virtual float GetStageScore()
        {
            // Returns the game score.
            return gameScore;
        }

        // Calculates the stage score.
        public abstract float CalculateStageScore();

        // Calculates and sets the game score.
        public void CalculateAndSetGameScore()
        {
            gameScore = CalculateStageScore();
        }

        // Resets the stage score.
        public void ResetStageScore()
        {
            gameScore = 0;
        }

        // Gets the stage energy total.
        public abstract float GetStageEnergyTotal();

        // Gets the stage's air pollution amount, which only pertains to the action stage.
        public virtual float GetStageAirPollution()
        {
            return 0.0F;
        }

        // Returns 'true' if the stage is complete.
        public abstract bool IsComplete();

        // Called when the stage is over.
        public virtual void OnStageOver()
        {
            // Stage no longer playing.
            SetStagePlaying(false);

            // The game timer isn't stopped since it's an overall timer for the scene.

            // Reset the game time scale.
            ResetGameTimeScale(true);

            // Calculates the game score.
            CalculateAndSetGameScore();

            // Play the stage results BGM if there's BGM playing.
            if(stageAudio != null)
            {
                // If the stage was set to have music, play the results BGM.
                // If the stage has no music, the BGM source isn't playing.
                // If it isn't playing, don't play the results BGM.
                if(stageAudio.bgmSource.isPlaying)
                {
                    PlayStageResultsBgm();
                }
            }
        }

        // Resets the stage.
        public virtual void ResetStage()
        {
            // Resets the time and the score.
            ResetGameTimerAndStageTimer();
            ResetStageScore();

            // Call reset stage on stage audio.
            stageAudio.ResetStage();

            // If the stage BGM should be restarted.
            if(restartStageBgmOnReset)
            {
                RestartStageBgm();
            }
            else
            {
                // Checks if the stage BGM is playing.
                // If it isn't, restart the BGM.
                if(!IsStageBgmPlaying())
                {
                    RestartStageBgm();
                }
            }

        }

        // Quits the stage, returning the world area.
        public virtual void QuitStage()
        {
            // Generates the world start info, making that it shouldn't be destroyed on load.
            WorldStartInfo wsi = GenerateWorldStartInfo(true);

            // Loads the world scene.
            LoadWorldScene();
        }

        // Finishes the stage.
        public virtual void FinishStage()
        {
            // DON'T RESET THE GAME TIMER OR THE STAGE TIMER
            // If the stage is complete, these values will be checked to get the stage time...
            // For results reasons.

            // Returns the game's time scale to normal.
            ResetGameTimeScale(true);

            // Stop running the game timer so that the time is measured accurately.
            PauseGameTimer();

            // Stage is no longer playing.
            SetStagePlaying(false);
        }

        // Generates the stage data.
        public virtual WorldStage.WorldStageData GenerateStageData(bool complete)
        {
            WorldStage.WorldStageData data = new WorldStage.WorldStageData();

            // World stage index, id number, and stage type.
            data.worldStageIndex = worldStageIndex;
            data.idNumber = idNumber;
            data.stageType = GetStageType();

            // Time, score, energy total, and air pollution.
            data.time = GetStageTimer();
            data.score = GetStageScore();
            data.energyTotal = GetStageEnergyTotal();
            data.airPollution = GetStageAirPollution();

            // Complete parameter.
            data.complete = complete;


            // Returns the data.
            return data;
        }

        // Generates a world start info object.
        // dontDestroyOnLoad: marks the object as not being destroyed on load if true.
        public WorldStartInfo GenerateWorldStartInfo(bool dontDestroyOnLoad)
        {
            // Creates a start info object and adds the world info component.
            GameObject startInfoObject = new GameObject("World Start Info");
            WorldStartInfo startInfo = startInfoObject.AddComponent<WorldStartInfo>();

            // Applies the start info from this manager.
            startInfo.SetStartInfo(this);

            // If the object shouldn't be destroyed on load, mark the object as such.
            if(dontDestroyOnLoad)
            {
                DontDestroyOnLoad(startInfo.gameObject);
            }

            return startInfo;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused...
            if(IsStagePlayingAndGameUnpaused())
            {
                // stageTimer += Time.deltaTime;
                stageTimer += Time.unscaledDeltaTime; // Now uses unscaled delta time.
            }

            // If tutorials need to be checked and the game isn't loading, check for tutorials.
            // Originally checked if the game was not paused, but that was taken out since...
            // Tutorials may be played when the game is in a paused state.
            // if (!checkedTutorials && !IsGamePaused() && !IsLoading()) // Old
            if (!checkedTutorials && !IsLoading()) // New
            {
                CheckTutorials();
            }
        }
    }
}