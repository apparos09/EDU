using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A generator unit, which is used to generate power.
    public class ActionUnitGenerator : ActionUnitUser
    {
        // Used to determine what time of day the generator can be used.
        public enum timeConstraint { allDay, dayOnly, nightOnly };

        [Header("Generator")]

        // The resource this generator uses.
        public NaturalResources.naturalResource resource;

        // The notes for the generator.
        [Tooltip("The notes, which will be shown in the generator info window.")]
        public List<string> genNotes;

        // The translation key for the generator notes.
        [Tooltip("The translation key for the generator notes.")]
        public string genNotesKey = "";

        // Gets set to 'true' if the notes have been translated.
        private bool genNotesTranslated = false;

        // Saves the notes translated upon the first translation call if true.
        private bool saveGenNotesTranslated = true;

        // The energy generation timer. This is set to the energyGenerationSpeed when counting down.
        [Tooltip("The timer for generating energy. When the timer hits 0, energy is generated.")]
        public float energyGenerationTimer = 0.0F;

        // The usage of the generator based on the day.
        [Tooltip("The time of day where the generator can generate energy.")]
        public timeConstraint timeUsage = timeConstraint.allDay;

        // If 'true', the generator can only generate energy when there's wind.
        [Tooltip("If true, the generator uses wind to generate energy.")]
        public bool useWindToGenEnergy = false;

        // If 'true', the generator uses energy cycles when generating energy.
        [Tooltip("If true, the generator can only generate energy a fixed number of times (cycles).")]
        public bool useEnergyCycles = false;

        // The total amount of energy generated.
        [Tooltip("The total amount of energy generated.")]
        public float energyGenerationTotal = 0.0F;

        // The energy generaiton limit.
        [Tooltip("The limit on how much energy can be generated.")]
        public float energyGenerationLimit = -1.0F;

        // If 'true', the generator can only generate a certain amount of energy.
        [Tooltip("Is true if the generator has an energy generation limit.")]
        public bool useEnergyGenLimit = false;

        [Header("Generator/Animations")]

        // The animation for energy generation.
        public string energyGenAnim = "Action Unit - Flash - Blue Animation";

        // The energy generation clip.
        public AnimationClip energyGenClip;

        // A timer that's used to wait for the energy generation animation to finish.
        // This is done to see if the usable/unsuable animation should be played.
        protected float energyGenAnimWaitTimer = 0.0F;

        // Checks if the generator is marked as usable (true) or unusable (false).
        // Used for checking animations.
        protected bool markedUsable = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set the generation timer to max.
            SetEnergyGenerationTimerToMax();

            // Generates shouldn't attack anything.
            attackingEnabled = false;

            // if animations are enabled.
            if(AnimationsEnabled)
            {
                // If energy can be generated, play the usable animation.
                if(CanGenerateEnergy())
                {
                    PlayUsableAnimation();
                }
                // If energy can't be generated, play the unusable animation.
                else
                {
                    PlayUnusableAnimation();
                }
            }

            // If using tutorials and tutorial not active, run applicable tutorial.
            if(actionManager.IsUsingTutorialsAndTutorialNotActive())
            {
                // Runs the generator tutorial.
                if(!actionManager.tutorials.Data.clearedFirstActionGeneratorsTutorial)
                {
                    actionManager.tutorials.LoadFirstActionGeneratorsTutorial();
                }
            }
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.generator;
        }

        // Gets the display name for the unit's card.
        public override string GetUnitCardDisplayName()
        {
            return GetNaturalResourceNameAbbreviation();
        }

        // Gets the natural resource name.
        public string GetNaturalResourceName()
        {
            return NaturalResources.GetNaturalResourceName(resource);
        }

        // Gets the natural resource name key.
        public static string GetNaturalResourceNameKey(NaturalResources.naturalResource resource)
        {
            return NaturalResources.GetNaturalResourceNameKey(resource);
        }

        // Gets the natural resource name abbreviation.
        public string GetNaturalResourceNameAbbreviation()
        {
            return NaturalResources.GetNaturalResourceNameAbbreviation(resource);
        }

        // Gets the natural resource name key.
        public static string GetNaturalResourceNameAbbreviationKey(NaturalResources.naturalResource resource)
        {
            return NaturalResources.GetNaturalResourceNameAbbreviationKey(resource);
        }

        // Gets the generator notes.
        public List<string> GetGeneratorNotes()
        {
            return genNotes;
        }

        // Returns the generator notes key.
        public string GetGeneratorNotesKey()
        {
            return genNotesKey;
        }

        // Gets the generator notes translated.
        public List<string> GetGeneratorNotesTranslated()
        {
            // The result to be returned.
            List<string> result;

            // If the notes translation is being saved and has been saved.
            if (saveGenNotesTranslated && genNotesTranslated)
            {
                result = new List<string>(genNotes);
            }
            else
            {
                // LOL SDK Initialized and the key is set, so get the translated text.
                if (LanguageManager.IsLanguageLoaderInitialized() && genNotesKey != "")
                {
                    // Create a new list.
                    result = new List<string>();

                    // Generates the keys.
                    List<string> notesKeys = GenerateNotesKeys();

                    // Translate each page with the applicable key.
                    for (int i = 0; i < notesKeys.Count; i++)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(notesKeys[i]));
                    }

                    // If the notes translation should be saved...
                    // Override generatorNotes and mark that the translation has been saved.
                    if (genNotesTranslated)
                    {
                        // Clear the notes list and add the result.
                        genNotes.Clear();
                        genNotes.AddRange(result);

                        // Mark that the notes have been translated.
                        genNotesTranslated = true;
                    }
                }
                else
                {
                    // Create a new list with the provided values.
                    result = new List<string>(genNotes);
                }
            }

            return result;
        }

        // Generates the transation keys for the notes.
        public List<string> GenerateNotesKeys()
        {
            // The list of keys to return.
            List<string> genNoteKeys = new List<string>();

            // Goes through all the genNotes pages.
            for (int i = 0; i < genNotes.Count; i++)
            {
                genNoteKeys.Add(genNotesKey + "_" + i.ToString("D2"));
            }

            // Returns the notes keys.
            return genNoteKeys;
        }


        // Returns 'true' if the generator is marked usable.
        public bool MarkedUsable
        {
            get { return markedUsable; }
        }

        // Gets the maximum of the energy generation timer.
        public virtual float GetEnergyGenerationTimerMax()
        {
            // The value to be returned.
            float value;

            // If the speed is 0, set the value to 0.
            if(energyGenerationSpeed == 0)
            {
                value = 0.0F;
            }
            else
            {
                // The current energy generation speed.
                float currEnergyGenSpeed;

                // If uses wind to generate energy, set the speed based on that.
                if(useWindToGenEnergy)
                {
                    // Originally, the value ranged from 0 to the stat maximum.
                    // Now, it ranges from (max/2) to max.

                    // The speed minimum. // Before
                    // float speedMin = 0.0F; // Use this if the stat range should be [0, max].
                    float speedMin = BASE_STAT_MAXIMUM / 2; // After

                    // The incrementer for the speed. The higher the speed, the shorter the time.
                    // The lowest maximum is BASE_STAT_MAXIMUM / 2.
                    // The highest maximum is BASE_STAT_MAXIMUM.
                    float speedInc = (BASE_STAT_MAXIMUM - speedMin) / 5;

                    // Checks the wind rating to see the speed.
                    // The recent wind rating should match the current wind rating...
                    // So it shouldn't need to be recalculated.
                    switch (actionManager.GetCurrentWindRating(false))
                    {
                        // NOTE: the function CanGenerateEnergy() should prevent the generator from...
                        // Reaching the default, unknown, none, and nonMinus functions...
                        // To set the energy generation speed.
                        default:
                        case statRating.unknown:
                            // Leave it as the default since it's unknown.
                            currEnergyGenSpeed = energyGenerationSpeed; // Default
                            break;

                        case statRating.noneMinus:
                        case statRating.none:
                            // Sets the current energy speed to 0 since there's no wind.
                            currEnergyGenSpeed = 0.0F;
                            break;

                        case statRating.veryLow:
                            currEnergyGenSpeed = speedMin + speedInc * 1;
                            break;

                        case statRating.low:
                            currEnergyGenSpeed = speedMin + speedInc * 2;
                            break;

                        case statRating.medium:
                            currEnergyGenSpeed = speedMin + speedInc * 3;
                            break;

                        case statRating.high:
                            currEnergyGenSpeed = speedMin + speedInc * 4;
                            break;

                        case statRating.veryHigh:
                        case statRating.maximum:
                        case statRating.maximumPlus:
                            currEnergyGenSpeed = speedMin + speedInc * 5;
                            break;
                    }

                    // Makes sure the speed value is clamped between 0 and the maximum.
                    // This was commented out since it shouldn't be necessary.
                    // currEnergyGenSpeed = Mathf.Clamp(currEnergyGenSpeed, 0, BASE_STAT_MAXIMUM);
                }
                // Fixed generation time max.
                else
                {
                    currEnergyGenSpeed = energyGenerationSpeed;
                }

                // Calculates the value based on the current energy generation speed.
                // Calculation: 1 sec + 6.25 secs * ((maxSpeed - Abs(speed)) / maxSpeed)
                //  - Maximum wait time of 7.25 seconds for each instance of energy generation.
                value = 1.0F + 6.25F * ((BASE_STAT_MAXIMUM - Mathf.Abs(currEnergyGenSpeed)) / BASE_STAT_MAXIMUM);

                // Bounds check.
                if (value < 0)
                    value = 0.0F;
            }

            // Returns the value.
            return value;
        }

        // Sets the energy generation timer to max.
        public virtual void SetEnergyGenerationTimerToMax()
        {
            energyGenerationTimer = GetEnergyGenerationTimerMax();
        }

        // Returns 'true' if the generator can generate energy.
        public bool CanGenerateEnergy()
        {
            // TIME OF DAY
            // Gets set to 'true' if the generator can operate at the current time.
            bool validTime = true;

            // If the day usage is not all day, check if the time of day is right.
            if (timeUsage != timeConstraint.allDay)
            {
                // Checks the time of day to see if the generator is usable.
                switch (timeUsage)
                {
                    case timeConstraint.dayOnly:
                        validTime = actionManager.IsDayTime();
                        break;

                    case timeConstraint.nightOnly:
                        validTime = actionManager.IsNightTime();
                        break;
                }
            }


            // WIND
            // Gets set to 'true' if the wind check was valid.
            // If the generator doesn't use wind, this will always be true.
            bool validWind = true;

            // Checks if wind shoudl be used to generate energy.
            if (useWindToGenEnergy)
            {
                validWind = actionManager.IsWindBlowing();
            }


            // ENERGY CYCLES
            // Checks if there are energy cycles to use (if applicable).
            bool hasCycles = true;

            // Uses energy cycles.
            if(useEnergyCycles)
            {
                // If this resourcce has energy cycles to use.
                if(NaturalResources.UsesEnergyCycles(resource))
                {
                    // Tile exists.
                    if(tile != null)
                    {
                        // Returns 'true' if there are energy cycles left, false otherwise.
                        hasCycles = tile.HasEnergyCycles(resource);
                    }
                    else
                    {
                        Debug.LogWarning("Tile not found when searching for energy cycles.");
                    }
                }
                else
                {
                    Debug.LogWarning("There are no energy cycles assigned to this resource type. Assuming infinite cycles can be used.");
                }
            }

            // ENERGY GENERATION LIMIT
            // Gets set to 'true' if the energy generation limit has been reached.
            bool energyGenLimitReached = false;

            // If the energy gen limit is being used.
            if (useEnergyGenLimit)
            {
                energyGenLimitReached = energyGenerationTotal >= energyGenerationLimit;
            }

            // Calculates the result.
            bool result = validTime && validWind && hasCycles && !energyGenLimitReached;

            // Returns the result.
            return result;
        }

        // Generates the energy and resets the generation timer.
        public virtual float GenerateEnergy()
        {
            // The energy being generated.
            float energy = CalculateEnergyGenerationAmount();

            // Add to the energy generation total.
            energyGenerationTotal += energy;

            // If energy cycles are being used.
            if(useEnergyCycles)
            {
                // If the tile exists, reduce the energy cycle amount by 1.
                if (tile != null)
                    tile.DecreaseEnergyCyclesByResource(resource, 1);
            }

            // Sets the timer to max.
            SetEnergyGenerationTimerToMax();

            // If the energy generation limit has been reached...
            // Call the appropriate function.
            if (HasEnergyGenerationLimitBeenReached())
            {
                OnEnergyGenerationLimitReached();
            }

            // If animations are available and enabled, play animations.
            if(IsAnimationsAvailableAndEnabled())
            {
                // Play the energy generation animation.
                PlayEnergyGenerationAnimation();

                // If the energy clip isn't null, set the wait timer to the clip's length...
                // Divided by animator speed, plus some extra time to be safe.
                if(energyGenClip != null)
                {
                    energyGenAnimWaitTimer = energyGenClip.length / animator.speed + 0.5F;
                }
                else
                {
                    // Do nothing.
                    energyGenAnimWaitTimer = 0.0F;
                }
            }

            return energy;
        }

        // Has an energy generation limit.
        public bool HasEnergyGenerationLimit()
        {
            return useEnergyGenLimit;
        }

        // Returns 'true' if the energy generation limit has been reached.
        public bool HasEnergyGenerationLimitBeenReached()
        {
            return useEnergyGenLimit && energyGenerationTotal >= energyGenerationLimit;
        }

        // Called when the energy generation limit has been reached.
        // This is only called at the time of the limit being reached.
        public virtual void OnEnergyGenerationLimitReached()
        {
            // ...
        }

        // Generates air pollution for one instance of this generator.
        public float GenerateAirPollution()
        {
            // Generates the value and rounds up to the nearest whole number.
            float value = Mathf.Ceil(12.5F * (Mathf.Abs(airPollution) / BASE_STAT_MAXIMUM));

            // Set to 0 if negative.
            if (value < 0)
                value = 0;

            // Return result.
            return value;

        }

        // KILL / DEATH
        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            base.OnUnitDeath();
        }

        // ANIMATIONS //
        // Called when an overlay animation has started.
        // public void OnOverlayAnimationStart()
        // {
        //     // ...
        // }
        // 
        // // Called when an overlay animation has ended.
        // public void OnOverlayAnimationEnd()
        // {
        // 
        // }

        // Plays the energy generation animation.
        public void PlayEnergyGenerationAnimation()
        {  
            if(animator != null && energyGenAnim != "")
                animator.Play(energyGenAnim);
        }

        // Plays the usable animation.
        public void PlayUsableAnimation()
        {
            unitAnimations.PlayUsableAnimation();
            markedUsable = true;
        }

        // Plays the unusable animation.
        public void PlayUnusableAnimation()
        {
            unitAnimations.PlayUnusableAnimation();
            markedUsable = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If energy can be generated, run the timer for generating energy.
                if (CanGenerateEnergy())
                {
                    // Reduce timer.
                    energyGenerationTimer -= Time.deltaTime;

                    // Time to generate new energy.
                    if (energyGenerationTimer <= 0.0F)
                    {
                        // Set timer to 0.
                        energyGenerationTimer = 0.0F;

                        // The new energy and pollution that was created.
                        float newEnergy = GenerateEnergy();
                        float newAirPoll = GenerateAirPollution();

                        // Increase the energy of the owner.
                        if (owner != null)
                        {
                            owner.IncreaseEnergy(newEnergy);
                            owner.IncreaseAirPollution(newAirPoll);
                        }
                    }
                }
                else
                {
                    // Keep the timer at max if no energy can be generated.
                    energyGenerationTimer = GetEnergyGenerationTimerMax();

                    // If the wait timer is greater than 0, reduce timer.
                    if(energyGenAnimWaitTimer > 0)
                    {
                        energyGenAnimWaitTimer -= Time.deltaTime;

                        // Wait timer is 0 or less.
                        if (energyGenAnimWaitTimer <= 0)
                        {
                            energyGenAnimWaitTimer = 0.0F;

                            // If the generator can generate energy, so play usable animation.
                            if(CanGenerateEnergy())
                            {
                                PlayUsableAnimation();
                            }
                            // Can't generate energy, so play unusable animation.
                            else
                            {
                                PlayUnusableAnimation();
                            }
                        }
                    }
                    else
                    {
                        // If the generator is currently marked as usable even though it can't generate energy...
                        // Play the unusuable animation, which will mark it as unusuable.
                        if(markedUsable)
                        {
                            PlayUnusableAnimation();
                        }
                    }
                }
            }
        }

    }
}