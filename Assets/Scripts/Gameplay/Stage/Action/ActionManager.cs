using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action manager.
    public class ActionManager : StageManager
    {
        // The singleton instance.
        private static ActionManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Action")]

        // The action UI.
        public ActionUI actionUI;

        // The action audio.
        public ActionAudio actionAudio;

        // The action stage list.
        public ActionStageList actionStageList;

        // The action stage.
        public ActionStage actionStage;

        // The total amount of time the stage lasts in seconds.
        // The stage lasts 1:40 (100 seconds).
        //  - Originally, the stage was 2:00 (120 seconds).
        // Enemy units don't start spawning until 10 seconds have passed.
        // - The 10 seconds wait time is there because of changes to the enemy player's starting spawn time.
        // - It's always 10 seconds regardless of the stage difficulty. This wait time is included in the stage length.
        // Day and night are 50 seconds (0:50) each (half of the stage).
        // - Originally, day and night were 1 minute each when the stage was 2 minutes (i.e., half of stage time).
        public const float STAGE_LENGTH_MAX_SECONDS = 100.0F;

        // If 'true', the event overlay is enabled.
        // private bool stageEventOverlayEnabled = true;

        // If 'ture', stage notifications are enabled.
        private bool stageNotificationsEnabled = true;

        // The stage day timer, which is used to determine the time of day.
        // This is seperate from 'gameTime' which is the real-world time it takes the player to finish the stage.
        // TODO: loop around to day time.
        public float dayNightTimer = 0.0F;

        // The fade duration for transitioning from day to night.
        private float dayNightTransDur = 5.0F;

        // If 'true', the stage is progressing from day to night.
        // If 'false', the stage is progerssing from night to day.
        private bool dayToNight = true;

        // If 'true', the day-night cycle loops. When the end of a day-night period is reached...
        // The timer resets, and the game marks that it's going from night to day.
        // If this is 'false', the timer continues without adjusting the day-night cycle at all.
        private bool loopDayNightCycle = false;

        // If 'true', the day-night cycle is enabled.
        private bool dayNightEnabled = true;

        // The default number of wind ratings.
        public const int WIND_RATINGS_COUNT_DEFAULT = 3;

        // The wind ratings (speeds) of the stage. The times the wind speed changes is based on how many wind elements there are.
        public ActionUnit.statRating[] windRatings = new ActionUnit.statRating[WIND_RATINGS_COUNT_DEFAULT];

        // The most recent wind speed that the stage had.
        private ActionUnit.statRating recentWindRating = ActionUnit.statRating.unknown;

        // If 'true', wind is enabled.
        private bool windEnabled = true;

        // The player user.
        public ActionPlayerUser playerUser;

        // The player user defense ids, which are used to determine what defenses the player has.
        [Tooltip("The list of defense IDs for the defeneses the player is loaded with")]
        public List<int> userDefenseIds = new List<int>();

        // The player enemy.
        public ActionPlayerEnemy playerEnemy;

        // Post-processing was removed for game performance reasons.
        // The action post processor (unused).
        // public ActionPostProcessor postProcessor;

        // If 'true', the game uses post processing.
        // private bool usePostProcessing = true;

        // If 'true', the day-night visual effects are used.
        // This is a replaced to 'usePostProcessing'.
        private bool useDayNightEffects = true;

        // The fast time scale.
        public const float STAGE_SPEED_FAST_TIME_SCALE = 2.0F;

        // The slow time scale.
        public const float STAGE_SPEED_SLOW_TIME_SCALE = 0.5F;

        // Uses the stage energy start bonus.
        // This feature has been removed, so leave this as false.
        private bool useEnergyStartBonus = false;

        // If 'true', the generator info dialog is opened upon the stage being initialzied.
        private bool openGenInfoDialogOnLateStart = true;

        // If 'true', the stage start dialog is used.
        private bool useStageStartDialog = true;

        // Gets set to 'true' when the action manager has been initialized.
        protected bool actionInitialized = false;

        // Constructor
        private ActionManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the action UI is not set, find it.
            if(actionUI == null)
            {
                actionUI = ActionUI.Instance;
            }

            // Gets the action audio instance.
            if(actionAudio == null)
            {
                actionAudio = ActionAudio.Instance;
            }

            // Gets the action stage list.
            if(actionStageList == null)
            {
                actionStageList = ActionStageList.Instance;
            }

            // If the action stage is not set.
            if(actionStage == null)
            {
                actionStage = FindObjectOfType<ActionStage>();
            }

            // Tries to find the start info. The object must be active for it to be gotten.
            ActionStageStartInfo startInfo = FindObjectOfType<ActionStageStartInfo>(false);

            // Found start info, so set the default values.
            if (startInfo != null)
            {
                // Applies the start info.
                startInfo.ApplyStartInfo(this);

                // Destroys the start info object.
                Destroy(startInfo.gameObject);
            }

            // Initializes the stage.
            InitializeStage();
        }

        // Called the first frame after start.
        protected override void LateStart()
        {
            base.LateStart();

            // If the stage start dialog is being used, open it.
            if (useStageStartDialog)
            {
                actionUI.OpenStageStartDialog();
            }
            // If it isn't, close the stage start dialog.
            else
            {
                actionUI.CloseStageStartDialog();
            }

            // Opens the generator info dialog if it should be shown on start.
            // This closes the stage start dialog, but opens it back up...
            // Once the generator info dialog is closed.
            if (openGenInfoDialogOnLateStart)
            {
                // Make sure not to go back to the options dialog on close.
                actionUI.generatorInfoDialog.CloseAllDialogsOnClose = true;

                // Open the info dialog.
                actionUI.OpenGeneratorInfoDialog(true);
            }
        }

        // Gets the instance.
        public static ActionManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionManager>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Manager (singleton)");
                        instance = go.AddComponent<ActionManager>();
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

        // Initializes the knowledge stage.
        public override void InitializeStage()
        {
            // If there are no natrual resources, fill the list with the type list.
            if(naturalResources.Count <= 0)
            {
                SetNaturalResourceListToTypeList();
            }

            // No defense ids, so fill it with the default list.
            // Index 0 and index 1 aren't included since the former is null and the latter is the lane blaster.
            if(userDefenseIds.Count <= 0)
            {
                SetDefenseIdListToAllValidIds();
            }

            // Generates the map using the id number.
            actionStage.GenerateStage(idNumber);

            // Sets the player's starting energy.
            // If the data logger exists, use it to check for bonus energy.
            if(DataLogger.Instantiated)
            {
                // If the starting energy bonus should be used, set it to the value.
                if(useEnergyStartBonus)
                {
                    playerUser.energyStartBonus = DataLogger.Instance.energyStartBonus;
                }
                // The energy start bonus shouldn't be used, so make sure it's 0.
                else
                {
                    playerUser.energyStartBonus = 0.0F;
                }
            }

            // Sets the player user difficulty.
            playerUser.ApplyDifficulty(true);

            // OLD: sets the energy to the starting energy.
            // New: sets the starting energy by the game mode, and sets the starting energy.
            // This also gets called in the player user's start function, but it shouldn't be a big deal.
            // playerUser.SetEnergyToStartingEnergy();
            playerUser.SetStartingEnergyByGameMode(true);

            // Sets the generator and defense prefabs.
            playerUser.SetGeneratorPrefabsFromManager();
            playerUser.SetDefensePrefabsFromManager();

            // NOTE: this check now happens in the enemy player script.
            // If the game is in defense mode, enable the auto energy loss.
            // If the game is in generation mode, don't automatically reduce the energy.
            // playerEnemy.autoEnergyLoss = GameSettings.Instance.gameplayMode == GameSettings.gameMode.defense;

            // Applies the enemy difficulty and resets the values.
            // Also sets the spawn timer value to its starting amount rather than its max amount.
            playerEnemy.ApplyDifficulty(true);
            playerEnemy.SetSpawnTimerToStartingAmount();

            // Call the base function to mark that the stage has been initialized successfully.
            base.InitializeStage();

            // The action stage has been initialized.
            actionInitialized = true;
        }

        // Returns 'true' if the action manager has been initialized.
        public bool ActionInitialized
        {
            get { return actionInitialized; }
        }

        // Returns 'true' if the stage start dialog is being used.
        public bool IsUsingStageStartDialog()
        {
            return useStageStartDialog;
        }

        // TUTORIALS
        // Checks for tutorials.
        public override void CheckTutorials()
        {
            // Check for action specific tutorials.
            if (IsUsingTutorialsAndTutorialNotActive())
            {
                // Gets set to true when a tutorial has started.
                bool startedTutorial = false;

                // First Action Stage
                if (!startedTutorial && !tutorials.Data.clearedFirstActionIntroTutorial)
                {
                    tutorials.LoadFirstActionIntroTutorial();
                    startedTutorial = true;
                }

                // This only handles the intro tutorial and the generator tutorials.
                // Other tutorials are triggered elsewhere.
            }

            // Calls base to check for resource tutorials.
            base.CheckTutorials();
        }

        // Called when the first action intro tutorial is starting.
        public void OnFirstActionIntroTutorialStart()
        {
            // Disables elements that will be enabled by another tutorial (generators tutorial).
            if(!tutorials.Data.clearedFirstActionGeneratorsTutorial)
            {
                // If in generation mode, disable the auto-fill of the energy goal bar.
                // This is to fix an oversight that caused it to fill while enemies couldn't spawn.
                // It will be turned back on when the generation tutorial is triggered.
                if(GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation)
                {
                    // This setting is only used in generation mode, hence the conditional statement.
                    playerUser.autoAddToEnergyGenTotal = false;
                }

                // Disables the enemy player, day-night cycle and wind.
                playerEnemy.gameObject.SetActive(false);
                dayNightEnabled = false;
                windEnabled = false;

                // Disables the info log button, day-night indicator, wind indicator, and options button.
                actionUI.infoLogButton.gameObject.SetActive(false);
                actionUI.dayNightIndicator.gameObject.SetActive(false);
                actionUI.windIndicator.gameObject.SetActive(false);
                actionUI.optionsButton.gameObject.SetActive(false);
            }

            // Disables elements that will be enabled by another tutorial (defenses tutorial).
            if (!tutorials.Data.clearedFirstActionDefensesTutorial)
            {
                // Disables the defense unit selector.
                actionUI.defenseUnitSelector.gameObject.SetActive(false);

                // Disables the lane blaster parent object, which also disables the lane blasters.
                playerUser.laneBlastersParent.gameObject.SetActive(false);
            }

            // Disables elements that will be enabled by another tutorial (first kill tutorial).
            if (!tutorials.Data.clearedFirstActionFirstKillTutorial)
            {
                // Disables the speed, deselect, remove, and block button UIs.
                // The UIs include the buttons and the labels they've been given.
                // Generation Mode
                actionUI.speedButtonGenUI.gameObject.SetActive(false);
                actionUI.deselectButtonUI.gameObject.SetActive(false);
                actionUI.removeButtonUI.gameObject.SetActive(false);
                actionUI.blockButtonUI.gameObject.SetActive(false);

                // Defense Mode
                actionUI.speedButtonDefUI.gameObject.SetActive(false);

                // Disables the reset and world buttons.
                // This is to prevent the player from resetting or leaving the stage before the tutorial ends.
                actionUI.optionsDialog.resetButton.interactable = false;
                actionUI.optionsDialog.worldButton.interactable = false;
            }
        }

        // Called when the first action generator tutorial is starting.
        public void OnFirstActionGeneratorsTutorialStart()
        {
            // Enables the player user's energy generation goal bar being auto filled.
            // This was to fix an oversight with tutorials.
            if (GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation)
            {
                // This setting is only used in generation mode, hence the conditional statement.
                playerUser.autoAddToEnergyGenTotal = true;
            }

            // Enables the player enemy, the day-night system, and the wind system.
            playerEnemy.gameObject.SetActive(true);
            dayNightEnabled = true;
            windEnabled = true;

            // Enables the info log button, day-night indicator, wind indicator, and options button.
            actionUI.infoLogButton.gameObject.SetActive(true);
            actionUI.dayNightIndicator.gameObject.SetActive(true);
            actionUI.windIndicator.gameObject.SetActive(true);
            actionUI.optionsButton.gameObject.SetActive(true);
        }

        // Called when the first action defenses tutorial is starting.
        public void OnFirstActionDefensesTutorialStart()
        {
            // Activate the defense unit selector.
            actionUI.defenseUnitSelector.gameObject.SetActive(true);

            // Activates the lane blaster parent object, which also enables the lane blasters.
            playerUser.laneBlastersParent.gameObject.SetActive(true);
        }

        // Called when the first action first kill tutorial is starting.
        public void OnFirstActionFirstKillTutorialStart()
        {
            // Activates the speed button UI, deselect button UI, remove button UI, and block button UI.
            // Generation Mode
            actionUI.speedButtonGenUI.gameObject.SetActive(true);
            actionUI.deselectButtonUI.gameObject.SetActive(true);
            actionUI.removeButtonUI.gameObject.SetActive(true);
            actionUI.blockButtonUI.gameObject.SetActive(true);

            // Defense Mode
            actionUI.speedButtonDefUI.gameObject.SetActive(true);

            // Enables the reset and world buttons.
            actionUI.optionsDialog.resetButton.interactable = true;
            actionUI.optionsDialog.worldButton.interactable = true;
        }

        // DEFENSE IDS
        // Sets the defense id list to all valid ids.
        public void SetDefenseIdListToAllValidIds()
        {
            userDefenseIds.Clear();
            userDefenseIds = ActionUnitPrefabs.Instance.GenerateDefensePrefabIdList(false, false);
        }

        

        // Calculates the stage score.
        public override float CalculateStageScore()
        {
            // The points base.
            float basePoints = 500;

            // Bonus for the user winning, and their amount of kills.
            float userWonBonus = HasPlayerUserWon() ? 500.0F : 0.0F;

            // Time
            // 4 points per second.
            float timeBonus = STAGE_LENGTH_MAX_SECONDS * 4.0F * Mathf.Clamp01(1.0F - (stageTimer / STAGE_LENGTH_MAX_SECONDS));

            // Energy Remaining
            // 1 point for every 5 points of energy the player user has left.
            float energyLeftBonus = 1.0F * Mathf.Ceil(playerUser.energy / 5.0F);

            // Bounds check.
            if (energyLeftBonus < 0)
                energyLeftBonus = 0;

            // Kills
            float killBonus = playerUser.kills * 50.0F;

            // Lane Blasters
            // The bonus for the lane blasters the player has left.
            int laneBlastersLeft = 0;
            float laneBlastersBonus = 0.0F;

            // Goes through all units the player has.
            for(int i = 0; i < playerUser.createdUserUnits.Count; i++)
            {
                // Gets the user unit.
                ActionUnitUser userUnit = playerUser.createdUserUnits[i];

                // The user unit exists.
                if (userUnit != null)
                {
                    // If the unit is a lane blaster, add to the lane blaster count.
                    if(userUnit.idNumber == 1)
                    {
                        // Add to the count.
                        laneBlastersLeft++;
                    }
                }
            }

            // Give the player a bonus for every lane blaster they have left.
            // Lowered from 50 to 25, then 20 per lane blaster.
            laneBlastersBonus = 20.0F * laneBlastersLeft;

            // Pollution
            // Reduce for air pollution.
            float pollutionDeduction = 0;
            
            // If the player user has generated air pollution, calculate a reduction.
            if(playerUser.airPollution > 0)
            {
                // For every 25 points of air pollution, deduct 50 points from the score.
                pollutionDeduction = Mathf.Floor(playerUser.airPollution / 25.0F) * 50.0F;
            }

            // Calculates the bonus. If the bonus is less than 0, make it 0.
            float bonusPoints = userWonBonus + timeBonus + energyLeftBonus + killBonus + laneBlastersBonus - pollutionDeduction;

            // If the bonus is negative, set the bonus to 0.
            if (bonusPoints < 0)
                bonusPoints = 0.0F;

            // Final score - rounds up to nearest whole value.
            float finalScore = basePoints + bonusPoints;
            finalScore = Mathf.Ceil(finalScore);

            // If the final score is less than 0, make it 0.
            if (finalScore < 0.0F)
                finalScore = 0.0F;

            // Return the final score.
            return finalScore;
        }

        // // EVENT OVERLAY
        // // Returns 'true' if the action event overlay is enabled.
        // public bool StageEventOverlayEnabled
        // {
        //     get { return stageEventOverlayEnabled; }
        // }

        // NOTIFICATION
        // Returns 'true' if the stage notifications are enabled.
        public bool StageNotificationsEnabled
        {
            get { return stageNotificationsEnabled; }
        }

        // STAGE SPEED
        // Returns the stage speed, which is the game time scale.
        public float GetStageSpeed()
        {
            return GetGameTimeScale();
        }

        // Sets the stage speed using the provided factor.
        // change: -1 = slow, 0 = normal, 1 = fast
        public void SetStageSpeed(int change)
        {
            // Checks if the game is paused.
            // If the game is paused, don't update the current time scale.
            bool gamePaused = IsGamePaused();

            // Set to fast speed.
            if(change > 0)
            {
                SetGameTimeScale(STAGE_SPEED_FAST_TIME_SCALE, !gamePaused);
            }
            // Set to slow speed.
            else if(change < 0)
            {
                SetGameTimeScale(STAGE_SPEED_SLOW_TIME_SCALE, !gamePaused);
            }
            // Set to normal speed.
            else
            {
                ResetGameTimeScale(!gamePaused);
            }
        }

        // Gets the stage speed as an integer.
        public int GetStageSpeedAsInt()
        {
            // Gets the game time scale.
            float gameTimeScale = GetGameTimeScale();

            // The speed int.
            int speedInt;

            // If the stage speed is fast.
            if(gameTimeScale == STAGE_SPEED_FAST_TIME_SCALE)
            {
                speedInt = 1;
            }
            // If the stage speed is slow.
            else if(gameTimeScale == STAGE_SPEED_SLOW_TIME_SCALE)
            {
                speedInt = -1;
            }
            // Stage speed is normal or unknown.
            else
            {
                speedInt = 0;
            }

            return speedInt;
        }

        // Returns 'true' if the stage speed is normal.
        public bool IsStageSpeedNormal()
        {
            return IsGameTimeScaleNormal();
        }

        // Sets the stage speed to normal.
        public void SetStageSpeedNormal()
        {
            SetStageSpeed(0);
        }
        
        // Returns true if stage speed is fast.
        public bool IsStageSpeedFast()
        {
            return GetGameTimeScale() == STAGE_SPEED_FAST_TIME_SCALE;
        }

        // Sets the stage speed to fast.
        public void SetStageSpeedFast()
        {
            SetStageSpeed(1);
        }

        // Returns true if the stage speed slow.
        public bool IsStageSpeedSlow()
        {
            return GetGameTimeScale() == STAGE_SPEED_SLOW_TIME_SCALE;
        }

        // Gets the stage speed to slow.
        public void SetStageSpeedSlow()
        {
            SetStageSpeed(-1);
        }

        // Resets the stage speed.
        // updateButton: refreshes the speed button if true.
        public void ResetStageSpeed()
        {
            // Resets the game time scale.
            ResetGameTimeScale(false);
        }


        // DAY-NIGHT CYCLE
        // Is the day night function enabled.
        public bool IsDayNightEnabled()
        {
            return dayNightEnabled;
        }

        // Returns the stage
        public float GetDayNightTimer()
        {
            return dayNightTimer;
        }

        // Gets the stage time progress as a percentage based on how long the stage has been going.
        public float GetDayNightTimerProgress()
        {
            return dayNightTimer / STAGE_LENGTH_MAX_SECONDS;
        }

        // Returns 'true' if the day-night cycle is in the first half of the timer (< 50).
        // Check dayToNight to see if it's day to night or night to day.
        private bool IsDayNightProgressInFirstHalf()
        {
            return dayNightTimer < STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }

        // Returns 'true' if the day-night cycle is in the second half of the timer (>= 50%).
        // Check dayToNight to see if it's day to night or night to day.
        private bool IsDayNightProgressInSecondHalf()
        {
            return dayNightTimer >= STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }
        
        // Returns true if the stage is going from day to night.
        public bool IsDayToNight()
        {
            return dayToNight;
        }

        // Returns 'true' if teh stage is going from night to day.
        public bool IsNightToDay()
        {
            return !dayToNight;
        }

        // Returns 'true' if its day time. Day time is the first half of the stage.
        public bool IsDayTime()
        {
            // If the stage is transitioning from day to night, check if it's in the first half of the cycle (day).
            // If the stage is transitioning from night to day, check if it's in the second half of the cycle (day).
            return (dayToNight) ? IsDayNightProgressInFirstHalf() : IsDayNightProgressInSecondHalf();
        }

        // Returns 'true' if it's night time. Night time is the second half of the stage.
        public bool IsNightTime()
        {
            // If the stage is transitioning from day to night, check if it's in the second half of the cycle (night).
            // If the stage is transitioning from night to day, check if it's in the first half of the cycle (night).
            return (dayToNight) ? IsDayNightProgressInSecondHalf() : IsDayNightProgressInFirstHalf();
        }

        // Does the day night cycle loop? If not, it stops when it becomes night.
        public bool IsDayNightCycleLooping()
        {
            return loopDayNightCycle;
        }

        // Gets the day night transition duration.
        public float GetDayNightTransitionDuration()
        {
            return dayNightTransDur;
        }

        // Returns 'true' if the day night cycle is in transition.
        public bool InDayNightTransition()
        {
            // Calculates the end of the transition and the start of the transition.
            // Keep in mind that the day night timer counts up.
            // TODO: make this a dedicated variable?
            float transEnd = STAGE_LENGTH_MAX_SECONDS / 2.0F;
            float transStart = transEnd - dayNightTransDur;

            // Gets the result.
            bool result = dayNightTimer >= transStart && dayNightTimer <= transEnd;

            return result;
        }

        // Gets the day-night transition value.
        // If it's below the transition point, it's 0.0F. If it's above the transition point, it's 1.0F.
        public float GetDayNightTransitionValue()
        {
            // The transition end and start.
            float transEnd = STAGE_LENGTH_MAX_SECONDS / 2.0F;
            float transStart = transEnd - dayNightTransDur;

            // Calculates the result.
            float result = Mathf.InverseLerp(transStart, transEnd, dayNightTimer);

            // Clamps it into 01 space.
            result = Mathf.Clamp01(result);

            // Returns the result.
            return result;
        }


        // Updates the day-night effect.
        public void UpdateDayNightEffect()
        {
            // OLD - Post-processing is no longer being used
            // // If the post processor isn't set, do nothing.
            // if (postProcessor == null)
            //     return;
            // 
            // // Gets the day-night transition value.
            // float t = Mathf.Clamp01(GetDayNightTransitionValue());
            // 
            // // Sets t-value.
            // postProcessor.lerpT = t;

            // NEW - use the overlay.
            // Calculate the transition value.
            float t = Mathf.Clamp01(GetDayNightTransitionValue());
            actionUI.UpdateDayNightEffect(t);
        }

        // Called when the day-night timer has finished, which rolls the timer over to another day.
        public void OnDayNightTimerFinished()
        {
            // Checks if the day night cycle should be looped or not.
            if(loopDayNightCycle) // Roll over to the next day.
            {
                dayNightTimer = 0.0F; // Set timer to 0.
                dayToNight = !dayToNight; // Swap cycle.
            }
            else // Clamp the timer since day-night cycle is over.
            {
                // Stop the timer from going past the stage length.
                if (dayNightTimer > STAGE_LENGTH_MAX_SECONDS)
                    dayNightTimer = STAGE_LENGTH_MAX_SECONDS;
            }
        }

        // Resets the day night cycle.
        public void ResetDayNightCycle()
        {
            // Sets day night timer to 0 and transition (going from day to night).
            dayNightTimer = 0.0F;
            dayToNight = true;
        }
        
        // WIND
        // If true, the wind weather effect is enabled.
        public bool IsWindEnabled()
        {
            return windEnabled;
        }

        // Returns 'true' if there is any wind belowing at all.
        // If the wind isn't enabled, this always returns false.
        public bool IsWindBlowing()
        {
            bool result;

            // First checks if there is wind.
            if(windEnabled)
            {
                // Gets the current wind rating.
                ActionUnit.statRating windRating = GetCurrentWindRating();

                // Checks the rating.
                switch(windRating)
                {
                    case ActionUnit.statRating.noneMinus: // No wind blowing.
                    case ActionUnit.statRating.none:
                        result = false;
                        break;

                    default: // Wind blowing.
                        result = true;
                        break;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        // Gets the current wind rating.
        // If wind is disabled, a rating of 'none' is returned.
        public ActionUnit.statRating GetCurrentWindRating()
        {
            // The wind rating.
            ActionUnit.statRating rating;

            // Checks if the wind is enabled.
            if(windEnabled)
            {
                // If there are wind speeds.
                if(windRatings.Length > 0)
                {
                    // Gets the time increment. The wind should change based on the stat of the stage.
                    // Each wind speed goes for an equal amount of time.
                    float timeInc = STAGE_LENGTH_MAX_SECONDS / windRatings.Length;

                    // Adds to the sum until it finds a value just below the stage length.
                    float timeSum = 0.0F;

                    // If the time increment is greater than 0.
                    if(timeInc > 0)
                    {
                        // Gets the stage time.
                        float stageTime = GetStageTimer();

                        // The wind speed index.
                        int windSpeedIndex = 0;

                        // While the time sum is greater than the stage time.
                        while (timeSum < stageTime)
                        {
                            // Add the time increment to the time sum.
                            timeSum += timeInc;
                            
                            // Increase the index if the time sum...
                            // Hasn't moved into the next bracket.
                            if(timeSum < stageTime)
                                windSpeedIndex++;
                        }

                        // Clamps the wind speed index.
                        windSpeedIndex = Mathf.Clamp(windSpeedIndex, 0, windRatings.Length - 1);

                        // Gets the wind speed based on the index.
                        rating = windRatings[windSpeedIndex];

                        // TODO: make this calculation more efficient? Maybe track the index periodically.

                        // If the rating has changed, update the value and call the related function.
                        if (rating != recentWindRating)
                        {
                            recentWindRating = rating;
                            OnWindChanged();
                        }
                    }
                    else
                    {
                        // The time sum is 0, so there's no wind.
                        // This case should never be used.
                        rating = ActionUnit.statRating.none;
                    }
                }
                // No wind speeds, so no wind.
                else
                {
                    rating = ActionUnit.statRating.none;
                }
            }
            // Disabled.
            else
            {
                // The wind is disabled, so there's no wind.
                rating = ActionUnit.statRating.none;
            }

            return rating;
        }

        // Gets the current wind rating.
        // recalculate: if true, the current wind rating is calculated.
        // - If false, the recent wind rating is returned.
        public ActionUnit.statRating GetCurrentWindRating(bool recaulcate)
        {
            // If the wind rating should be recalculated, call the current wind rating function.
            if (recaulcate)
            {
                return GetCurrentWindRating();
            }
            // Don't recalculate, so get the recent wind rating.
            else
            {
                return GetRecentWindRating();
            }
        }

        // Gets the provide wind rating as a value.
        public float GetWindRatingAsValue(ActionUnit.statRating windRating)
        {
            // The stat maximum (100).
            float statMax = ActionUnit.BASE_STAT_MAXIMUM;

            // The threshold stats are compared to.
            // The stat maximum is 100.
            float threshold = statMax / 5.0F;

            // The result to return.
            float result;

            // Checks the stat rating to know the wind rating value.
            // These are fixed values based on the rating.
            switch (windRating)
            {
                case ActionUnit.statRating.maximumPlus: // 100
                case ActionUnit.statRating.maximum: // 100
                case ActionUnit.statRating.veryHigh: // 100
                    result = threshold * 5;
                    break;

                case ActionUnit.statRating.high: // 80
                    result = threshold * 4;
                    break;

                case ActionUnit.statRating.medium: // 60
                    result = threshold * 3;
                    break;

                case ActionUnit.statRating.low: // 40
                    result = threshold * 2;
                    break;

                case ActionUnit.statRating.veryLow: // 20
                    result = threshold * 1;
                    break;

                case ActionUnit.statRating.none: // 0 (No Wind)
                case ActionUnit.statRating.noneMinus:
                    result = threshold * 0;
                    break;

                case ActionUnit.statRating.unknown: // 0 (No Wind)
                default:
                    result = 0.0F;
                    break;
            }

            // Returns the rating.
            return result;
        }

        // Gets the current wind rating as a value.
        public float GetCurrentWindRatingAsValue()
        {
            return GetWindRatingAsValue(GetCurrentWindRating());
        }

        // Gets the wind rating as a percentage.
        public float GetWindRatingAsAPercentage(ActionUnit.statRating windRating)
        {
            // Gets the value and the percentage.
            float value = GetWindRatingAsValue(windRating);
            float percentage = value / ActionUnit.BASE_STAT_MAXIMUM;

            // Returns percentage.
            return percentage;
        }

        // Gets the current wind rating as a percentage.
        public float GetCurrentWindRatingAsAPercentage()
        {
            return GetWindRatingAsAPercentage(GetCurrentWindRating());
        }

        // Gets the most recent wind rating, which is saved to a variable.
        // This doesn't do a calculation to get the current wind rating, so it's more efficient.
        public ActionUnit.statRating GetRecentWindRating()
        {
            return recentWindRating;
        }

        // Called when the wind speed has changed.
        protected virtual void OnWindChanged()
        {
            // If the wind indicator isn't being automatically updated, manaully update it.
            if(!actionUI.windIndicator.autoUpdateIndicator)
            {
                actionUI.windIndicator.UpdateIndicator();
            }
        }

        // Resets the wind. This doesn't clear the wind array.
        protected virtual void ResetWind()
        {
            recentWindRating = ActionUnit.statRating.unknown;
        }

        // Updates the wind.
        protected virtual void UpdateWind()
        {
            // Gets the current wind rating, which also calculates the wind rating.
            ActionUnit.statRating windRating = GetCurrentWindRating();
        }


        // PLAYERS
        // If the stage isn't playing and the player enemy has no energy, the player user has won.
        public bool HasPlayerUserWon()
        {
            return !IsStagePlaying() && !playerEnemy.HasEnergy();
        }

        // If the stag isn't running and the player enemy has energy, the player user has lost.
        public bool HasPlayerEnemyWon()
        {
            return !IsStagePlaying() && playerEnemy.HasEnergy();
        }

        // Called on the death of the user.
        public void OnPlayerUserDeath()
        {
            OnStageOver();
        }

        // Called on the death of the enemy.
        public void OnPlayerEnemyDeath()
        {
            OnStageOver();
        }

        // Gets the stage's energy total.
        public override float GetStageEnergyTotal()
        {
            // Provides the total amount of energy generated by the user.
            return playerUser.energyGenTotal;
        }

        // Gets the stage air pollution.
        public override float GetStageAirPollution()
        {
            return playerUser.airPollution;
        }

        // STAGE OVER
        // Called when the stage is over.
        public override void OnStageOver()
        {
            base.OnStageOver();

            // Set the enemy bar to 0 if the enemy has no energy.
            // This is to make sure the enemy's energy bar is empty when the stage is over.
            if(!playerEnemy.HasEnergy())
            {
                // Force the enemy's energy bar to be 0 in case it's currently in transition.
                // It might still show that the enemy has energy left if the stage end dialog...
                // Opens and sets the time scale to 0 before the transition is done.
                actionUI.playerEnemyEnergyBar.SetValueAsPercentage(0, false);
            }

            // If the enemy attack source is playing, make sure it's stopped.
            // This is to address a glitch where the sound would keep playing because an enemy was attacking...
            // When the stage ended.
            if(actionAudio.IsEnemyAttackSourcePlaying())
                actionAudio.StopEnemyAttackSfx(true);

            // Open the end UI.
            actionUI.OpenStageEndDialog();
        }

        // Returns 'true' if the stage is complete.
        public override bool IsComplete()
        {
            // The result to be returned.
            bool result;

            // If the player enemy exists, check if it has energy.
            if(playerEnemy != null)
            {
                result = !playerEnemy.HasEnergy();
            }
            // The player doesn't exist, so consider the game completed.
            else
            {
                result = true;
            }
            
            return result;
        }

        // Resets the acion stage.
        public override void ResetStage()
        {
            base.ResetStage();

            // The audio world source and the old enabled parameter.
            AudioSource sfxWorldSource;
            bool sfxWorldEnabledTemp;

            // If the aciton audio exists, get the source and disable it.
            if(actionAudio != null)
            {
                sfxWorldSource = actionAudio.sfxWorldSource;
                sfxWorldEnabledTemp = actionAudio.sfxWorldSource.enabled;
                actionAudio.sfxWorldSource.enabled = false;
            }
            // Source couldn't be found.
            else
            {
                sfxWorldSource = null;
                sfxWorldEnabledTemp = true;
            }


            // Resets stage speed. Also refreshes the speed buttons.
            ResetStageSpeed();
            actionUI.speedButtonGen.RefreshSpeedIcon();
            actionUI.speedButtonDef.RefreshSpeedIcon();

            // Resets the day-night cycle, and the wind.
            ResetDayNightCycle();
            ResetWind();

            // Reset the players.
            playerUser.ResetPlayer();
            playerEnemy.ResetPlayer();

            // Resets the stage.
            actionStage.ResetStage();

            // Resets notifications
            actionUI.playedEnemyApproachNotif = false;

            // Updates the UI for the player and enemy.
            actionUI.UpdatePlayerUserUI();
            actionUI.UpdatePlayerEnemyUI();

            // Set the selectors for the player user to row 0.
            actionUI.generatorUnitSelector.SetRow(0);
            actionUI.defenseUnitSelector.SetRow(0);

            // If the SFX audio source exists, restore the old enabled value.
            if(sfxWorldSource != null)
            {
                sfxWorldSource.enabled = sfxWorldEnabledTemp;
            }

            // Sets that the stage is playing.
            SetStagePlaying(true);
            SetGamePaused(false);
        }

        // Called to finish the stage. TODO: implement.
        public override void FinishStage()
        {
            base.FinishStage();

            // Open the window.
            // actionUI.SetStageEndWindowActive(true);

            // Sets the energy start bonus to 0.
            if (DataLogger.Instantiated)
            {
                DataLogger.Instance.energyStartBonus = 0.0F;
            }

            // Generates a world start info object. The function gives it the data.
            WorldStartInfo startInfo = GenerateWorldStartInfo(true);

            // TODO: put this into the UI instead.
            LoadWorldScene();

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game isn't paused, adjust the stage day timer.
            if(IsStagePlayingAndGameUnpaused())
            {
                // If the day night cycle is enabled.
                if(dayNightEnabled)
                {
                    dayNightTimer += Time.deltaTime;

                    // If the stage day timer has passed the stage length...
                    // Mark that the timer has finished.
                    if (dayNightTimer >= STAGE_LENGTH_MAX_SECONDS)
                    {
                        OnDayNightTimerFinished();
                    }
                    else
                    {
                        // Post processing is no longer being used.
                        // If post processing is being used.
                        // if (usePostProcessing)

                        // If the day-night visual effects are being used.
                        if (useDayNightEffects)
                        {
                            // Update the day-night effect.
                            UpdateDayNightEffect();
                        }
                    }
                }

                // If the wind is enabled.
                if(windEnabled)
                {
                    // Call update function.
                    UpdateWind();
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