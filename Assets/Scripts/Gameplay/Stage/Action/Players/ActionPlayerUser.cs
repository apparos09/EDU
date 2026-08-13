using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RM_EDU
{
    // The user action player.
    public class ActionPlayerUser : ActionPlayer
    {
        // The modes the player user can be in.
        // Select: the player can select action units and place them on the map.
        // Remove: the player can remove action units they've placed on the map.
        public enum playerUserMode { select, remove };

        [Header("User")]

        // The energy the player user starts with.
        public float energyStart = 200.0F;

        // The bonus energy the player gets on start.
        public float energyStartBonus = 0.0F;

        // If 'true', the starting energy bonus is applied.
        // If the game is in generation mode, the energy start bonus will always be 0.
        private bool applyEnergyBonus = true;

        // The energy the player had on the previous update.
        // Used to see if the player's energy has changed between updates.
        private float prevUpdateEnergy = -1.0F;

        // The energy auto generation timer.
        [Tooltip("The countdown timer for the player to automatically generate energy.")]
        public float energyAutoGenTimer = 0.0F;

        // The amount of time it takes for the player to generate energy.
        [Tooltip("The maximum time for the player to automatically generate energy.")]
        public float energyAutoGenTimerMax = 12.0F;

        // The amount of energy that's generated for every instance.
        [Tooltip("The amount of energy the player automatically generates for every instance.")]
        public float energyAutoGenAmount = 120.0F;

        // If 'true', the player automatically generates energy.
        private bool energyAutoGenEnabled = true;

        [Header("User/Energy Gen Total")]
        // The total amount of energy generated.
        [Tooltip("The energy the user has generated since the start of the stage.")]
        public float energyGenTotal = 0.0F;

        // The energy goal for the user. This only applies in generation mode.
        [Tooltip("The energy generation goal. This is only used for generation mode.")]
        public float energyGenGoal = 10000.0F;

        // If 'true', the energy auto generated amount is added to the energy total.
        [Tooltip("If 'true', the energy auto generated amount is added to the energy total.")]
        public bool addEnergyAutoGenToTotal = false;

        // The automatic energy generation total increment.
        private float energyGenTotalInc = 0.0F;

        // If true, the energy generation total is automatically incremented.
        // The increment amount is based on the maximum stage length.
        public bool autoAddToEnergyGenTotal = true;

        [Header("User/Units")]

        // The generator prefabs the player can use.
        public List<ActionUnitGenerator> generatorPrefabs = new List<ActionUnitGenerator>();

        // The defense prefabs the player can use.
        public List<ActionUnitDefense> defensePrefabs = new List<ActionUnitDefense>();

        // The lane blasters parent.
        public GameObject laneBlastersParent;

        // The lane blaster prefab.
        public ActionUnitDefenseBlasterLane laneBlasterPrefab;

        // The id of the lane blaster.
        public const int LANE_BLASTER_ID = 1;

        // The player mode of the player.
        private playerUserMode userMode = playerUserMode.select;

        // The unit prefab the action player user has selected.
        // TODO: make private.
        public ActionUnit selectedUnitPrefab;

        // A list of the units created by this player.
        // Only user units can be placed in this list.
        public List<ActionUnitUser> createdUserUnits = new List<ActionUnitUser>();

        [Header("User/Audio")]

        // Sound effect played when a unit is created.
        public AudioClip unitCreatedSfx;

        // Sound effect played when a unit is removed.
        public AudioClip unitRemovedSfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player user is null, set it to this.
            if(actionManager.playerUser == null)
            {
                actionManager.playerUser = this;
            }

            // If the lane blaster is null, set the prefab using the lane blaster id.
            if(laneBlasterPrefab == null)
            {
                laneBlasterPrefab = (ActionUnitDefenseBlasterLane)actionUnitPrefabs.GetDefensePrefab(LANE_BLASTER_ID);
            }

            // Sets the starting energy by the game mode, and sets the user's energy.
            SetStartingEnergyByGameMode(true);

            // Sets the energy auto gen timer max and amount by the game mode.
            // Also sets the energy auto generation timer.
            SetEnergyAutoGenTimerMaxAndAmountByGameMode(true);

            // Calculates and sets the energy generation total inc amount to its default.
            CalculateAndSetEnergyGenerationTotalIncrementAmount();
        }

        // Applies the game difficulty to the user.
        // resetValues: if true, reset the current values to match the current difficulty.
        public void ApplyDifficulty(int difficulty, bool resetValues)
        {
            // The user difficulty.
            int userDiff = Mathf.Clamp(difficulty, 0, 9);

            // Checks the user difficulty.
            switch (userDiff)
            {
                case 1:
                    energyGenGoal = 10000;
                    break;

                case 2:
                    energyGenGoal = 15000;
                    break;

                case 3:
                    energyGenGoal = 20000;
                    break;

                case 4:
                    energyGenGoal = 25000;
                    break;

                case 5:
                    energyGenGoal = 30000;
                    break;

                case 6:
                    energyGenGoal = 35000;
                    break;

                case 7:
                    energyGenGoal = 40000;
                    break;

                case 8:
                    energyGenGoal = 45000;
                    break;

                default: // Max/Main Difficulty
                case 0:
                case 9:
                    energyGenGoal = 50000;
                    break;
            }
            // If values should be reset based on the new difficulty.
            if (resetValues)
            {
                // Sets the energy to its starting value and the energy total gen increment amount.
                // SetStartingEnergyByGameMode(true); // Not needed since the game mode should still be the same.
                SetEnergyToStartingEnergy();
                CalculateAndSetEnergyGenerationTotalIncrementAmount();
            }

        }

        // Gets the difficulty from the manager and uses that to apply the settings.
        // If 'resetValues' are true, the relevant parameters are adjusted to their new defaults.
        public void ApplyDifficulty(bool resetValues)
        {
            ApplyDifficulty(ActionManager.Instance.difficulty, resetValues);
        }


        // ENERGY //
        // Returns 'true' if the player has starting energy.
        public bool HasEnergyStart()
        {
            return energyStart > 0;
        }

        // Returns 'true' if the player user has an energy start bonus.
        public bool HasEnergyStartBonus()
        {
            return energyStartBonus > 0;
        }

        // Sets the starting energy by the game mode.
        // setEnergyToStartingEnergy: if true, the user's energy is set to the starting energy.
        public void SetStartingEnergyByGameMode(bool setEnergyToStartingEnergy)
        {
            // Sets the starting energy based on the game mode.
            switch (GameSettings.Instance.gameplayMode)
            {
                case GameSettings.gameMode.generation:
                    energyStart = 150.0F;
                    break;

                case GameSettings.gameMode.defense:
                default:
                    energyStart = 200.0F;
                    break;
            }

            // If 'true', the energy is set to the starting energy.
            if(setEnergyToStartingEnergy)
            {
                SetEnergyToStartingEnergy();
            }
        }

        // Sets the user's energy to their starting energy.
        // applyBonus: determines if the energy start bonus should be applied as well.
        public void SetEnergyToStartingEnergy(bool applyBonus)
        {
            // If the energy bonus should be applied, apply it.
            if (applyBonus)
            {
                energy = energyStart + energyStartBonus;
            }
            // Sets energy to starting energy without bonus.
            else
            {
                energy = energyStart;
            }
        }

        // Sets the user's energy to their starting energy plus the bonus energy.
        public void SetEnergyToStartingEnergy()
        {
            SetEnergyToStartingEnergy(applyEnergyBonus);
        }


        // Adds the starting energy to the user's energy.
        public void AddStartingEnergyToEnergy()
        {
            energy += energyStart;
        }

        // Adds the starting energy and bonus energy to current energy.
        public void AddStartingAndBonusEnergyToEnergy()
        {
            energy += energyStart + energyStartBonus;
        }

        // Returns 'true' if the energy auto generates.
        public bool IsEnergyAutoGenerating()
        {
            return energyAutoGenEnabled;
        }

        // Resets the energy generation total, setting it to 0.
        public void ResetEnergyGenerationTotal()
        {
            energyGenTotal = 0;
        }

        // Modifies the energy by adding the provided amount.
        public override void IncreaseEnergy(float energyPlus)
        {
            // Increase the energy and add to the energy total.
            IncreaseEnergy(energyPlus, true);
        }

        // Modifies the energy by adding the provided amount.
        public void IncreaseEnergy(float energyPlus, bool addToEnergyGenTotal)
        {
            // If the value should be added to the energy gen total...
            // And there's a value to add, add it.
            if(addToEnergyGenTotal && energyPlus >= 0)
            {
                // Checks the game mode to see what to do.
                // Generation mode, so boost the energy amount being added to the total.
                if(GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation)
                {
                    energyGenTotal += energyPlus * 4.5F;
                }
                // Defense mode, so just add the energy itself.
                else
                {
                    energyGenTotal += energyPlus;
                }
            }

            // Calls the base function to increase the energy.
            base.IncreaseEnergy(energyPlus);
        }

        // Sets the energy auto gen timer and amount by the game mode.
        public void SetEnergyAutoGenTimerMaxAndAmountByGameMode(bool resetTimer)
        {
            // Sets the starting energy based on the game mode.
            switch (GameSettings.Instance.gameplayMode)
            {
                case GameSettings.gameMode.generation:
                    energyAutoGenTimerMax = 12.5F;
                    energyAutoGenAmount = 100.0F;
                    break;

                case GameSettings.gameMode.defense:
                default:
                    energyAutoGenTimerMax = 12.0F;
                    energyAutoGenAmount = 120.0F;
                    break;
            }

            // If the timer should be reset.
            if(resetTimer)
            {
                // Resets the energy auto generation timer.
                ResetEnergyAutoGenerationTimer();
            }
        }

        // Gets the energy auto generation timer max.
        public float GetEnergyAutoGenerationTimerMax()
        {
            return energyAutoGenTimerMax;
        }

        // Gets the energy auto generation timer.
        public float GetEnergyAutoGenerationTimer()
        {
            return energyAutoGenTimer;
        }

        // Resets the energy auto generation timer.
        public void ResetEnergyAutoGenerationTimer()
        {
            energyAutoGenTimer = energyAutoGenTimerMax;
        }

        // Gets the amount of energy generated automatically per instance.
        public float GetEnergyAutoGenerationAmount()
        {
            return energyAutoGenAmount;
        }

        // Called when the energy amount for the user has changed.
        public void OnEnergyChanged()
        {
            // NOTE: the energy display text isn't updated here, it's updated as part of...
            // The general player UI update. That function already checks to make sure that...
            // The text isn't updated if the text hasn't actually change, which is meant to...
            // Help with optimization.

            // Refreshes the unit buttons.
            actionManager.actionUI.RefreshUnitButtonsInteractable();

            // Sets previous update saved energy to this energy.
            prevUpdateEnergy = energy;
        }

        // ENERGY GENERATION TOTAL
        // Calculates the energy generation total increment amount per second.
        public float CalculateEnergyGenerationTotalIncrementAmount()
        {
            // The maximum length of the stage is used to determine...
            // How much energy the energy total should be increased by every second.
            float result = energyGenGoal / ActionManager.STAGE_LENGTH_MAX_SECONDS;

            // If the result is negative, set it to 1.
            if (result < 0)
                result = 1.0F;

            return result;
        }

        // Calculates and sets the energy decrement amount per second.
        public void CalculateAndSetEnergyGenerationTotalIncrementAmount()
        {
            energyGenTotalInc = CalculateEnergyGenerationTotalIncrementAmount();
        }

        // Returns 'true' if the energy generation total goal has been reached.
        public bool IsEnergyGenerationTotalGoalReached()
        {
            return energyGenTotal >= energyGenGoal;
        }

        // UNIT PREFABS //
        // Sets the generator prefabs from the provided resources.
        public void SetGeneratorPrefabs(List<NaturalResources.naturalResource> resources)
        {
            // The unit list.
            List<ActionUnit> unitList = new List<ActionUnit>();

            // Clears the current list.
            generatorPrefabs.Clear();

            // Gets the prefabs for the provided resources.
            foreach (NaturalResources.naturalResource resource in resources)
            {
                // Gets the prefab.
                // Goes by the resource instead of the index to match with the defense verison of this function.
                // ActionUnitGenerator generatorPrefab = ActionUnitPrefabs.Instance.GetGeneratorPrefab((int)resource);
                ActionUnitGenerator generatorPrefab = ActionUnitPrefabs.Instance.GetGeneratorPrefabByResource(resource);

                // Puts the prefab in the generator list and the new list.
                generatorPrefabs.Add(generatorPrefab);
                unitList.Add(generatorPrefab);
            }

            // Adds the unit list to the generator unit selector.
            ActionUI.Instance.generatorUnitSelector.SetActionUnitPrefabs(unitList);
        }

        // Sets the generator prefabs from the manager.
        public void SetGeneratorPrefabsFromManager()
        {
            SetGeneratorPrefabs(ActionManager.Instance.naturalResources);
        }

        // Sets the defense unit prefabs using a list of ids. The defense ids should line up with the indexes.
        public void SetDefensePrefabs(List<int> defenseIds)
        {
            // The unit list.
            List<ActionUnit> unitList = new List<ActionUnit>();

            // Clears the current list.
            defensePrefabs.Clear();

            // Gets the prefabs for all the provided defenses.
            foreach (int defenseId in defenseIds)
            {
                // Gets the prefab.
                // The ID should match up with the index of the defense prefab.
                ActionUnitDefense defensePrefab = ActionUnitPrefabs.Instance.GetDefensePrefab(defenseId);

                // Puts the prefab in the defense list and the new unit list.
                defensePrefabs.Add(defensePrefab);
                unitList.Add(defensePrefab);
            }

            // Adds the unit list to the defense unit selector.
            ActionUI.Instance.defenseUnitSelector.SetActionUnitPrefabs(unitList);
        }

        // Sets the defense prefabs from the manager.
        public void SetDefensePrefabsFromManager()
        {
            SetDefensePrefabs(ActionManager.Instance.userDefenseIds);
        }

        // MODES
        // Returns the user mode.
        public playerUserMode GetPlayerUserMode()
        {
            return userMode;
        }

        // Sets the player user mode.
        public void SetPlayerUserMode(playerUserMode newMode)
        {
            userMode = newMode;

            // Update UI based on what mode the user is in.
            switch(userMode)
            {
                case playerUserMode.select:
                    ActionUI.Instance.SetSelectedUnitInfoToSelect();
                    break;

                case playerUserMode.remove:
                    ActionUI.Instance.SetSelectedUnitInfoToRemove(); // Set UI to remove.
                    ActionManager.Instance.actionStage.UnhighlightAllTiles(); // Makes sure all tiles are unhighlighted.
                    break;
            }
        }

        // Returns true if the user is in select mode.
        public bool InSelectMode()
        {
            return userMode == playerUserMode.select;
        }

        // Sets the player to select mode.
        public void EnableSelectMode()
        {
            SetPlayerUserMode(playerUserMode.select);
        }

        // Returns true if the user is in remove mode.
        public bool InRemoveMode()
        {
            return userMode == playerUserMode.remove;
        }

        // Sets to remove mode.
        public void EnableRemoveMode()
        {
            SetPlayerUserMode(playerUserMode.remove);
        }

        // Disables remove mode.
        public void DisableRemoveMode()
        {
            // Clears the selected prefab and changes to select mode.
            ClearSelectedActionUnitPrefab();
        }

        // SELECT / INSTANTIATE
        // Returns 'true' if the player is selecting a unit.
        public bool IsSelectingActionUnitPrefab()
        {
            return selectedUnitPrefab != null;
        }

        // Returns 'true' if the user selecting a unit prefab of a certain type.
        public bool IsSelectingActionUnitTypePrefab(ActionUnit.unitType type)
        {
            if (selectedUnitPrefab != null)
                return selectedUnitPrefab.GetUnitType() == type;
            else
                return false;
        }

        // Returns true if the user is selecting a generator unit prefab.
        public bool IsSelectingGeneratorUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.generator);
        }

        // Returns true if the user is selecting a defense unit prefab.
        public bool IsSelectingDefenseUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.defense);
        }

        // Returns true if the user is selecting a enemy unit prefab.
        public bool IsSelectingEnemyUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.enemy);
        }

        // Gets the unit the player has selected.
        public ActionUnit GetSelectedActionUnitPrefab()
        {
            return selectedUnitPrefab;
        }

        // Sets the prefab the player has selected. This should be a prefab, not an actual object.
        public void SetSelectedActionUnitPrefab(ActionUnit unitPrefab)
        {
            // Set to select mode.
            EnableSelectMode();

            // Set prefab.
            selectedUnitPrefab = unitPrefab;

            // Sets the info.
            ActionUI.Instance.SetSelectedUnitInfo(unitPrefab);

            // Highlights tiles if tile highlighting is enabled.
            ActionManager.Instance.actionStage.OnPlayerUserSelectedUnit();
        }

        // Gets the type of the selected action unit prefab.
        // If not selecting a prefab, it returns unknown.
        public ActionUnit.unitType GetSelectedActionUnitPrefabType()
        {
            if (selectedUnitPrefab != null)
                return selectedUnitPrefab.GetUnitType();
            else
                return ActionUnit.unitType.unknown;
        }

        // Clears the prefab the player has selected.
        public void ClearSelectedActionUnitPrefab()
        {
            // Set to select mode.
            EnableSelectMode();

            // Clear selected prefab.
            selectedUnitPrefab = null;

            // Clears the info.
            ActionUI.Instance.ClearSelectedUnitInfo();

            // Unhighlight tiles if highlighting is enabled.
            ActionManager.Instance.actionStage.OnPlayerUserClearedSelectedUnit();
        }

        // Returns 'true' if the selected action unit can use the tile.
        public bool CanSelectedActionUnitUseTile(ActionTile tile)
        {
            // Checks if their is a selected unit prefab.
            if(selectedUnitPrefab != null)
            {
                return selectedUnitPrefab.UsableTile(tile);
            }
            else
            {
                return false;
            }
        }

        // Instantiates an action unit on the provided tile.
        public ActionUnit InstantiateActionUnit(ActionUnit unitPrefab, ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            // No prefab so return null.
            if(unitPrefab == null)
            {
                return null;
            }

            // Instantiate the unit.
            ActionUnit newUnit = Instantiate(unitPrefab);

            // Sets the parent.
            SetActionUnitParentToUnitParent(newUnit);

            // Downcasts to a user unit.
            ActionUnitUser userUnit = (newUnit is ActionUnitUser) ? (newUnit as ActionUnitUser) : null;

            // If the user unit isn't equal to null, that means the conversion was successful.
            // As such, do functions specific to this unit.
            if(newUnit != null)
            {
                // If the user should be the owner of this unit.
                if(setAsOwner)
                {
                    newUnit.owner = this;
                }

                // If the tile exists, place the unit on it.
                if (tile != null)
                {
                    // If the tile doesn't have an action user unit, give it the generated one.
                    if (!tile.HasActionUnitUser())
                    {
                        // Sets the action tile to have this unit.
                        tile.SetActionUnitUser(userUnit);
                    }

                    // If the new unit is a user unit.
                    if (newUnit is ActionUnitUser)
                    {
                        // Set to tile position using function.
                        (newUnit as ActionUnitUser).SetPositionToTilePosition(tile);
                    }
                    // Not user unit.
                    else
                    {
                        // Set to tile position.
                        newUnit.transform.position = tile.transform.position;
                    }
                }               
            }

            // Adds to the created user units list if it exists.
            if (userUnit != null)
            {
                createdUserUnits.Add(userUnit);
            }

            // If the energy cost should be applied.
            if(applyEnergyCost)
            {
                // Reduce energy.
                energy -= newUnit.energyCreationCost;

                // Bounds check.
                if (energy < 0.0F)
                    energy = 0.0F;

                // If the selected prefab exists.
                if (selectedUnitPrefab != null)
                {
                    // If the player user doesn't have energy to use their currently selected unit prefab...
                    // Clear the selection.
                    if (!HasEnergyToCreateActionUnit(selectedUnitPrefab))
                    {
                        ClearSelectedActionUnitPrefab();
                    }
                }
            }

            // If the new unit isn't null, play the created sound.
            if(newUnit != null)
            {
                PlayUnitCreatedSfx();
            }

            // Returns the new unit.
            return newUnit;
        }

        // Instantiates the selected action unit without putting it on a tile.
        public ActionUnit InstantiateSelectedActionUnit(bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateSelectedActionUnit(null, setAsOwner, applyEnergyCost);
        }

        // Instantiates the selected unit and places it on the provided tile.
        public ActionUnit InstantiateSelectedActionUnit(ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateActionUnit(selectedUnitPrefab, tile, setAsOwner, applyEnergyCost);
        }

        // Instantiates a lane blaster.
        public ActionUnit InstantiateLaneBlaster(bool setAsOwner = true, bool applyEnergyCost = true)
        {
            // Converts the action unit to a lane blaster.
            ActionUnitDefenseBlasterLane laneBlaster = (ActionUnitDefenseBlasterLane)
                InstantiateActionUnit(laneBlasterPrefab, null, setAsOwner, applyEnergyCost);

            // Sets the lane blaster parent to the the lane blaster specific parent.
            SetLaneBlasterParentToLaneBlastersParent(laneBlaster);

            // Returns the lane blaster.
            return laneBlaster;
        }

        // Instantiates a lane blaster on the provided tile.
        public ActionUnit InstantiateLaneBlaster(ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            // Converts the action unit to a lane blaster.
            ActionUnitDefenseBlasterLane laneBlaster = (ActionUnitDefenseBlasterLane)
                InstantiateActionUnit(laneBlasterPrefab, tile, setAsOwner, applyEnergyCost);

            // Sets the lane blaster parent to the the lane blaster specific parent.
            SetLaneBlasterParentToLaneBlastersParent(laneBlaster);
            
            // Returns the lane blaster.
            return laneBlaster;
        }

        // Sets the provided lane blaster parent to the user parent.
        public void SetLaneBlasterParentToLaneBlastersParent(ActionUnitDefenseBlasterLane laneBlaster)
        {
            // If the lane blasters parent is set, set it as the lane blaster parent.
            if(laneBlastersParent != null)
            {
                laneBlaster.transform.parent = laneBlastersParent.transform;
            }
        }

        // Tries to remove the user unit from the created units list.
        public bool TryRemoveCreatedUnitFromList(ActionUnitUser userUnit)
        {
            // If it's in the list, remove it.
            if(createdUserUnits.Contains(userUnit))
            {
                createdUserUnits.Remove(userUnit);
                return true;
            }
            // Not in list.
            else
            {
                return false;
            }
        }

        // Kills all user units.
        public void KillAllUserUnits()
        {
            // Goes through all saved units.
            for (int i = createdUserUnits.Count - 1; i >= 0; i--)
            {
                // Kill the enemy.
                if (createdUserUnits[i] != null)
                    createdUserUnits[i].OnUnitDeath();

            }

            // Clears the list.
            createdUserUnits.Clear();
        }

        // AUDIO // 
        // Play the unit created sound effect.
        public void PlayUnitCreatedSfx()
        {
            // Audio is enabled and the action audio is instantiated.
            if(audioEnabled && ActionAudio.Instantiated)
            {
                ActionAudio.Instance.PlaySoundEffectWorld(unitCreatedSfx);
            }
        }

        // Play the unit removed sound effect.
        public void PlayUnitRemovedSfx()
        {
            // Audio is enabled and the action audio is instantiated.
            if (audioEnabled && ActionAudio.Instantiated)
            {
                ActionAudio.Instance.PlaySoundEffectWorld(unitRemovedSfx);
            }
        }

        // RESET //
        // Resets the player.
        public override void ResetPlayer()
        {
            base.ResetPlayer();

            // Kills all the user units and action proejctiles.
            KillAllUserUnits();
            ActionProjectile.KillAllActionProjectiles();

            // Resets the energy, the auto gen timer, and the energy generation total.
            SetEnergyToStartingEnergy();
            ResetEnergyAutoGenerationTimer();
            ResetEnergyGenerationTotal();

            // Clears the selected prefab.
            ClearSelectedActionUnitPrefab(); // Also sets to "select" mode.

            // Makes sure the player user isn't blocking their attack energy.
            // Also updates the block button's icon to reflect this.
            UnblockAttackEnergy();
            UpdateBlockButtonIcon();
        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is playing and unpaused.
            if (actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If energy should be generated automatically.
                if (energyAutoGenEnabled)
                {
                    // Reduces timer.
                    energyAutoGenTimer -= Time.deltaTime;

                    // If the auto gen timer is less than or equal to 0, add energy.
                    if(energyAutoGenTimer <= 0.0F)
                    {
                        // Sets the timer to 0 to stop it from going negative.
                        energyAutoGenTimer = 0.0F;

                        // Adds energy.
                        // 'addEnergyAutoGenToTotal' determines if this energy addition...
                        // Is counted in the energy total.
                        IncreaseEnergy(energyAutoGenAmount, addEnergyAutoGenToTotal);

                        // Set timer to max.
                        ResetEnergyAutoGenerationTimer();
                    }
                }

                // If the player's energy has changed.
                if(prevUpdateEnergy != energy)
                {
                    OnEnergyChanged();
                }

                // If in generation mode, see if the energy generation total has been reached.
                if(GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation)
                {
                    // If the energy generation total should automatically be added to.
                    if (autoAddToEnergyGenTotal)
                    {
                        energyGenTotal += energyGenTotalInc * Time.deltaTime;
                    }

                    // If the energy generation total is at or above the goal...
                    // The player has won.
                    if (energyGenTotal >= energyGenGoal)
                    {
                        // Sets the player's energy at 0.
                        actionManager.playerEnemy.energy = 0.0F;
                    }

                    // If the goal has been reached, set the enemy energy amount to 0.
                    // This triggers the enemy to lose.
                    if(energyGenTotal >= energyGenGoal)
                    {
                        actionManager.playerEnemy.energy = 0.0F;
                    }
                }
            }
                

        }
    }
}
