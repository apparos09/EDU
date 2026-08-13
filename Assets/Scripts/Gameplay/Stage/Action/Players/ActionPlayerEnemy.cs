using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action player enemy.
    public class ActionPlayerEnemy : ActionPlayer
    {
        [Header("Enemy")]
        // The maximum amount of energy the enemy has.
        [Tooltip("The maximum amount of energy the enemy player can have.")]
        public float energyMax = 300.0F;

        // The enemy's decrementation amount, which reduces from the enemy's energy every frame.
        // When the enemy runs out of energy, the stage is over.
        private float energyDec = 1.0F;

        // Automatically loses energy at a set rate if true.
        [Tooltip("If true, the enemy player loses energy automatically as the stage progresses.")]
        public bool autoEnergyLoss = true;

        // The parent for enemy retreats.
        public GameObject enemyRetreatParent;

        [Header("Enemy/Units")]

        // The rate at which enemies are spawned.
        public float spawnRate = 1.0F;

        // The countdown timer for spawning enemies.
        public float spawnTimer = 0.0F;

        // The spawn time max.
        // This should change based on the game difficulty.
        public float spawnTimeMax = 5.0F;

        // The starting spawn timer amount.
        private float spawnTimeStart = 10.0F;

        // If 'true', the enemy's starting spawn time is a fixed amount.
        // If 'false', the starting spawn amount is the same as spawn time max.
        private bool useFixedSpawnStartTime = true;

        // The enemy spawn count minimum. This determines the minimum of enemies to spawn eachi nstance.
        [Tooltip("Minimum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMin = ENEMIES_PER_SPAWN_MIN_DEFAULT;

        // The default enemies per spawn min.
        public const int ENEMIES_PER_SPAWN_MIN_DEFAULT = 1;

        // The enemy spawn count maximum. This determines the maximum of enemies to spawn each instance.
        [Tooltip("Maximum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMax = ENEMIES_PER_SPAWN_MAX_DEFAULT;

        // The default enemies per spawn max.
        public const int ENEMIES_PER_SPAWN_MAX_DEFAULT = 5;

        // If 'true', spawning is allowed.
        private bool allowSpawns = true;

        // The list of usable enemies by their ids in the prefab list.
        // The enemy id number should match the index number.
        [Tooltip("Lists the ids for usable enemies from the prefab list..")]
        public List<int> enemyIds = new List<int>();

        // The action enemy units.
        public List<ActionUnitEnemy> spawnedEnemies = new List<ActionUnitEnemy>();

        // The enemy unit spawn limit.
        // Uncomment the line below to allow an enemy unit for every row and column tile.
        // Originally 28 (7 rows * 4 cols).
        // public const int ACTIVE_ENEMY_UNIT_LIMIT = ActionStage.MAP_ROW_COUNT_DEFAULT * ActionStage.MAP_COLUMN_COUNT_DEFAULT; 
        public const int ACTIVE_ENEMY_UNIT_LIMIT = 15; // 5 rows * 3 cols (15 / 60 = 0.25)

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player enemyis null, set it to this.
            if (actionManager.playerEnemy == null)
            {
                actionManager.playerEnemy = this;
            }

            // Sets the enemy to max.
            SetEnergyToMax();

            // Calculates and sets the energy decrement amount to its default.
            CalculateAndSetEnergyDecrementAmount();

            // Sets the spawn timer to the starting amount.
            // SetSpawnTimerToMax();
            SetSpawnTimerToStartingAmount();

            // Sets the enemy prefabs.
            // If there are no usable enemy ids, fill the list with all enemies.
            if(enemyIds.Count <= 0)
                SetEnemyPrefabsToAll(false); 
        }

        // Applies the game difficulty to the enemy.
        // resetValues: if true, reset the current values to match the current difficulty.
        //  * NOTE: if the spawn time is reset, it's reset to the max value, not the starting value.
        public void ApplyDifficulty(int difficulty, bool resetValues)
        {
            // Clamps the difficulty from 0 to 9.
            int enemyDiff = Mathf.Clamp(difficulty, 0, 9);

            // Clears the enemy ids.
            enemyIds.Clear();

            // Checks the enemy difficulty.
            switch(enemyDiff)
            {
                case 1:
                    energyMax = 920.0F;
                    spawnTimeMax = 9.00F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);

                    break;

                case 2:
                    energyMax = 930.0F;
                    spawnTimeMax = 8.95F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);

                    break;

                case 3:
                    energyMax = 940.0F;
                    spawnTimeMax = 8.90F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);
                    enemyIds.Add(2);

                    break;

                case 4:
                    energyMax = 950.0F;
                    spawnTimeMax = 8.85F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);
                    enemyIds.Add(2);

                    break;

                case 5:
                    energyMax = 960.0F;
                    spawnTimeMax = 8.80F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                case 6:
                    energyMax = 970.0F;
                    spawnTimeMax = 8.75F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 4;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                case 7:
                    energyMax = 980.0F;
                    spawnTimeMax = 8.70F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 4;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                case 8:
                    energyMax = 990.0F;
                    spawnTimeMax = 8.65F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 5;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                default: // Max/Main Difficulty
                case 0:
                case 9:
                    // Energy and spawn time.
                    energyMax = 1000.0F;
                    spawnTimeMax = 8.60F;

                    // Enemies per spawn min and max.
                    enemiesPerSpawnMin = ENEMIES_PER_SPAWN_MIN_DEFAULT;
                    enemiesPerSpawnMax = ENEMIES_PER_SPAWN_MAX_DEFAULT;

                    // Give the enemy player all the enemy units.
                    SetEnemyPrefabsToAll(false);
                    break;
            }

            // If values should be reset based on the new difficulty.
            if(resetValues)
            {
                // Sets the energy to max, sets the energy decrement value, and sets the spawn timer to max.
                SetEnergyToMax();
                CalculateAndSetEnergyDecrementAmount();
                SetSpawnTimerToMax();
            }
        }

        // Gets the difficulty from the manager and uses that to apply the settings.
        // If 'resetValues' are true, the relevant parameters are adjusted to their new defaults.
        public void ApplyDifficulty(bool resetValues)
        {
            ApplyDifficulty(ActionManager.Instance.difficulty, resetValues);
        }

        // Sets the enemy prefabs from the list
        public void SetEnemyPrefabs(List<int> newIds)
        {
            // Clears the current list.
            enemyIds.Clear();

            // Sets to the new ids.
            if (newIds != null)
                enemyIds = newIds;
        }

        // Sets the usable enemy prefabs list to include all enemies.
        public void SetEnemyPrefabsToAll(bool include0)
        {
            SetEnemyPrefabs(ActionUnitPrefabs.Instance.GenerateEnemyPrefabIdList(include0));
        }

        // Increases the energy by the provided amount.
        // The decrease function calls this function, so it doesn't need to be overrided.
        public override void IncreaseEnergy(float energyPlus)
        {
            // Call base function.
            base.IncreaseEnergy(energyPlus);

            // Clamp energy within 0 and max.
            energy = Mathf.Clamp(energy, 0, energyMax);
        }

        // Gets the energy max.
        public float GetEnergyMax()
        {
            return energyMax;
        }

        // Sets the energy max.
        public void SetEnergyMax(float energyMax)
        {
            this.energyMax = energyMax;

            // Bounds check.
            if (energyMax < 0)
                energyMax = 0.0F;
        }

        // Sets the enemy's energy to its max.
        public void SetEnergyToMax()
        {
            energy = energyMax;
        }


        // Returns the energy decrement amount.
        // This is the amount of energy lost per frame, which is multiplied by delta time.
        public float GetEnergyDecrementAmount()
        {
            return energyDec;
        }

        // Calculates the energy decrement amount per second.
        public float CalculateEnergyDecrementAmount()
        {
            // The maximum length of the stage is used...
            // To determine how much energy the enemy should lose every second.

            // Originally it was multiplied by delta time for some reason.
            // Delta time is applied when the decrement amount applied in Update.
            // It should not be part of the base value for the energy decrement.
            // float energyMax / ActionManager.STAGE_LENGTH_MAX_SECONDS * Time.deltaTime;

            float result = energyMax / ActionManager.STAGE_LENGTH_MAX_SECONDS;

            // If the result is negative, set it to 1.
            if (result < 0)
                result = 1.0F;

            return result;
        }

        // Calculates and sets the energy decrement amount per second.
        public void CalculateAndSetEnergyDecrementAmount()
        {
            energyDec = CalculateEnergyDecrementAmount();
        }

        // Resets the spawn timer.
        public void SetSpawnTimerToMax()
        {
            spawnTimer = spawnTimeMax;
        }

        // The spawn time starting value.
        public float SpawnTimeStart
        {
            get { return spawnTimeStart; }
        }

        // Sets the spawn timer to the starting amount.
        public void SetSpawnTimerToStartingAmount()
        {
            // Checks if the spawn timer at the start is the fixed amount.
            if(useFixedSpawnStartTime)
            {
                // If the max is greater than the starting amount, use the max.
                if(spawnTimeMax > spawnTimeStart)
                {
                    spawnTimer = spawnTimeMax;
                }
                // If the max amount is less than the starting amount, use the starting amount.
                else
                {
                    spawnTimer = spawnTimeStart;
                }
            }
            // If false, use the regular spawn time max.
            else
            {
                spawnTimer = spawnTimeMax;
            }
        }

        // Returns the number of spawned enemy units.
        public int GetSpawnedEnemyUnitCount()
        {
            return spawnedEnemies.Count;
        }

        // Returns 'ture' if the enemy is at or above their active enemy unit limit.
        public bool IsBelowActiveEnemyUnitLimit()
        {
            return spawnedEnemies.Count < ACTIVE_ENEMY_UNIT_LIMIT;
        }

        // Spawns enemies.
        public void SpawnEnemyUnits()
        {
            // The number of enemies in each row.
            // By default, it's length is equal to the number of rows. All indexes are filled by 0 by default.
            List<int> rowEnemyCount = new List<int>(new int[actionManager.actionStage.MapRowCount]);

            // Gets the number of spawns.
            int spawns = Random.Range(enemiesPerSpawnMin, enemiesPerSpawnMax + 1);

            // A temporary list of spawned enemies.
            List<ActionUnitEnemy> tempList = new List<ActionUnitEnemy>();

            // While spawns is greater than 0 and the enemy is below the active enemy unit limit.
            while(spawns > 0 && IsBelowActiveEnemyUnitLimit())
            {
                // Gets the index, which matches up with the ids in the enemyIds list.
                // The ids should match up with the enemy indexes in the list. It's less safe to do it this way...
                // But it's faster since a loop doesn't need to be started.
                int index = enemyIds[Random.Range(0, enemyIds.Count)];
                ActionUnitEnemy prefab = actionUnitPrefabs.GetEnemyPrefab(index);

                // Gets the prefab using the enemy id.
                // int idIndex = Random.Range(0, enemyIds.Count);
                // ActionUnitEnemy prefab = actionUnitPrefabs.GetEnemyPrefabById(enemyIds[idIndex]);


                // Prefab exists.
                if (prefab != null)
                {
                    // Creates the new enemy, making a child of this object by default.
                    ActionUnitEnemy enemyUnit = Instantiate(prefab, transform);

                    // This enemy player owns this unit.
                    enemyUnit.owner = this;

                    // Sets the parent.
                    SetActionUnitParentToUnitParent(enemyUnit);

                    // Gives the enemy unit the action manager since it won't be set...
                    // Before certain functions are used.
                    enemyUnit.actionManager = ActionManager.Instance;

                    // Gets a random row.
                    int row = Random.Range(0, actionManager.actionStage.MapRowCount);

                    // The column should be the end of the map.
                    int col = actionManager.actionStage.MapColumnCount - 1;

                    // Gives enemy the values.
                    // Gets the enemy position.
                    Vector3 enemyPos = actionManager.actionStage.ConvertMapPositionToWorldUnits(col, row);

                    // Saves the row this enemy is in and increases the row count.
                    enemyUnit.SetRow(row);
                    rowEnemyCount[row] += 1;

                    // Adjust the enemy's position by how many enemies are in that row.
                    // This moves it off-screen and prevents enemies from overlapping each other.
                    enemyPos.x += rowEnemyCount[row] * actionManager.actionStage.TileSize.x;

                    // Give enemy their position.
                    enemyUnit.transform.position = enemyPos;

                    // Mark enemy unit as belonging to this enemy and add to the list of spawned enemies.
                    enemyUnit.playerEnemy = this;
                    spawnedEnemies.Add(enemyUnit);
                }

                spawns--;
            }


            // Reset the spawn timer.
            SetSpawnTimerToMax();
        }

        // Destroys all enemy units.
        // TODO: add check to see if the death state should be used.
        public void KillAllEnemyUnits()
        {
            // Destroys all enemy units spawned by this enenmy.
            for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                // Kills the spawned enemy.
                // This function leds to another function where the enemy removes itself from the list.
                spawnedEnemies[i].Kill();
            }

            // Clears all spawned enemies.
            spawnedEnemies.Clear();
        }

        // Called when an enemy unit has been killed.
        public void OnEnemyUnitDeath(ActionUnitEnemy enemyUnit)
        {
            // If the enemy unit is in the list of spawned enemies, remove it.
            if(spawnedEnemies.Contains(enemyUnit))
                spawnedEnemies.Remove(enemyUnit);

            // TODO: have enemy run back to ship before it's destroyed.

            Destroy(enemyUnit.gameObject);
        }

        // Resets the player.
        public override void ResetPlayer()
        {
            base.ResetPlayer();

            // Kills all enemy units and enemy retreats. 
            KillAllEnemyUnits();
            EnemyRetreat.KillAllEnemyRetreats();

            // Sets the energy to max, and resets the spawn timer to the starting amount.
            SetEnergyToMax();
            // SetSpawnTimerToMax();
            SetSpawnTimerToStartingAmount();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is playing and the game is unpaused, run the enemy actions.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If the game is in defense mode.
                if(GameSettings.Instance.gameplayMode == GameSettings.gameMode.defense)
                {
                    // If energy should be automatically lost as time progresses.
                    if (autoEnergyLoss)
                    {
                        // Reduces the energy.
                        DecreaseEnergy(energyDec * Time.deltaTime);
                    }
                }

                // Bounds check.
                if (energy < 0.0F)
                    energy = 0.0F;

                // If the enemy has no energy left, the stage is over.
                if(energy <= 0.0F)
                {
                    actionManager.OnPlayerUserDeath();
                }
                // The enemy still has energy.
                else
                {
                    // If spawns are allowed.
                    if(allowSpawns)
                    {
                        spawnTimer -= Time.deltaTime;

                        // If the spawn timer has run out, generate spawns.
                        if(spawnTimer <= 0.0F)
                        {
                            spawnTimer = 0.0F;

                            // If the enemy is below the active enemy unit list limit...
                            // Spawn enemies.
                            if(IsBelowActiveEnemyUnitLimit())
                            {
                                SpawnEnemyUnits();
                            }
                        }
                    }
                    
                }

                // If stage notifications are enabled.
                if(actionManager.StageNotificationsEnabled)
                {
                    // Gets the action UI instance.
                    ActionUI actionUI = ActionUI.Instance;

                    // If the enemy approaching notification hasn't been played yet.
                    if (!actionUI.playedEnemyApproachNotif)
                    {
                        // If the spawn timer is below 3.25 seconds, play the enemy approaching notification.
                        if(spawnTimer < 3.25F)
                        {
                            actionUI.PlayEnemiesApproachingNotification();
                        }
                    }
                }
            }
        }
    }
}