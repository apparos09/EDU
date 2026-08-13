using UnityEngine;

namespace RM_EDU
{
    // The action enemy unit.
    public class ActionUnitEnemy : ActionUnit
    {
        [Header("Enemy")]
        // The enemy player.
        public ActionPlayerEnemy playerEnemy;

        // The energy death cost factor. This can be changed based on what killed the enemy.
        // This is basically only for the lane blaster, and is there for balancing reasons.
        [HideInInspector]
        public float energyDeathCostFactor = 1.0F;

        // The row the action unit is in.
        private int row = -1;

        // The enemy's movement direction.
        // Enemies go from left to right.
        public Vector3 movementDirec = Vector3.left;

        // If 'true', the enemy moves.
        private bool movementEnabled = true;

        // The enemy attack object.
        // This is used as part of animations.
        public EnemyAttack enemyAttack;

        // If set to 'true', the enemy can move and attack at the same time.
        protected bool moveAndAttack = false;

        // Gets set to 'true' when a target within range has been found.
        // NOTE: pretty sure this doesn't do anything anymore since enemies now trigger attacks on contact.
        private bool targetInRange = false;

        // The most recent unit being targeted.
        private ActionUnit currTarget;

        // If 'true', the enemy unit checks if it's at the end of the map on the left.
        // Since this happens every frame, there should be some tile that's used to enable this function...
        // Upon the enemy unit coming into contact with it.
        protected bool checkForMapLeftBound = false;

        // The prefab for the enemy retreat, which is spawned when the enemy dies.
        public EnemyRetreat enemyRetreatPrefab;

        // If 'true', the action unit enemy uses the enemy attack.
        public bool useEnemyAttack = true;

        // If true, the enemy retreat is used. If false, the enemy dies like normal.
        [Tooltip("If true, the enemy retreats to their ship when they're destroyed.")]
        public bool useEnemyRetreat = true;

        [Header("Enemy/Sprites")]

        // The neutral sprite for the enemy, which is used when it's moving.
        public Sprite neutralSprite;

        // The attack sprite for the enemy, which is used when it's attacking.
        public Sprite attackSprite;

        // If 'true', the sprites are manually changed based on the current state of the enemy.
        [Tooltip("Changes the enemy's sprite based on the current enemy state.")]
        public bool changeSprites = true;

        [Header("Enemy/Animations")]

        // The spawn animation for the unit.
        public string unitSpawnAnim = "Action Unit Enemy - Spawn Animation";

        // If 'true', the spawn animation is used.
        private bool useSpawnAnim = true;

        // The spawn animation for the unit.
        public string unitDamagedAnim = "Action Unit Enemy - Damage Animation";

        // If 'true', the damage animation is played when the enemy takes damage.
        protected bool useDamagedAnim = true;

        [Header("Enemy/Audio")]

        // The attack sound effect.
        public AudioClip unitAttackSfx;

        // The damaged sound effect.
        public AudioClip unitDamagedSfx;

        // Uses the damaged sound effect if true.
        protected bool useDamagedSfx = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player enemy is null, but the owner is set.
            if(playerEnemy == null && owner != null)
            {
                // If the owner is an action player enemy, downcast it and save it to player enemy.
                if(owner is ActionPlayerEnemy)
                    playerEnemy = (ActionPlayerEnemy)owner;
            }

            // Gets the enemy attack in the children.
            if (enemyAttack == null)
            {
                enemyAttack = GetComponentInChildren<EnemyAttack>();
            }

            // Turn off the enemy attack.
            if(enemyAttack != null)
            {
                // Set enemy attack to use this enemy.
                if (enemyAttack.unitEnemy == null)
                    enemyAttack.unitEnemy = this;

                enemyAttack.gameObject.SetActive(false);
            }


            // If the stage has no metal tiles, check for the map left bound every frame.
            // The metal tiles tell the enemy to check for the end of the map.
            // If there are no metal tiles, this trigger will never be set by them..
            // Since the enemy doesn't know when it's close to the map end without them...
            // It just checks every frame.
            if (!actionManager.actionStage.HasMetalTiles)
                checkForMapLeftBound = true;

            // If animations are enabled and the spawn animation should be played.
            if(AnimationsEnabled && useSpawnAnim)
            {
                PlayUnitSpawnAnimation();
            }

            // If using tutorials and tutorial not active, run applicable tutorial.
            if (actionManager.IsUsingTutorialsAndTutorialNotActive())
            {
                // Runs the defenses tutorial.
                if (!actionManager.tutorials.Data.clearedFirstActionDefensesTutorial)
                {
                    actionManager.tutorials.LoadFirstActionDefensesTutorial();
                }
            }
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            // Checks if colliding with a tile.
            ActionTile tile;

            // Tries to get the tile component.
            if (collision.TryGetComponent(out tile))
            {
                // If this is a metal tile, the enemy should be approaching the end of the map.
                // As such, start checking if at the end of the map every frame.
                if (tile.GetTileType() == ActionTile.actionTile.metal)
                {
                    checkForMapLeftBound = true;
                }
            }

            // Checks if colliding with a player user unit.
            ActionUnitUser userUnit;

            // Tries to get the component.
            if (collision.TryGetComponent(out userUnit))
            {
                // IF the user unit is tangible, it stop act as a obstruction...
                // And it can be attacked.
                if(userUnit.tangible)
                {
                    // Sets that a target is in range, and saves the current target.
                    targetInRange = true;
                    currTarget = userUnit;

                    // If the enemy can attack.
                    if (CanAttack())
                    {
                        AttackUserUnit(userUnit);
                    }
                }              
            }
        }

        // NAMES, TYPES, INFO //
        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.enemy;
        } 

        // Enemy names aren't displayed anywhere.
        // Gets the enemy type name.
        public static string GetEnemyTypeName()
        {
            // Checks for the LOL SDK.
            if (LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return LanguageManager.GetLanguageTextStatic(GetEnemyTypeNameKey());
            }
            else
            {
                return "Enemy";
            }
        }
        
        // Gets the enemy type name key.
        public static string GetEnemyTypeNameKey()
        {
            return "emy_nme";
        }
        
        // Gets the enemy name abbreviation.
        public static string GetEnemyTypeNameAbbreviation()
        {
            // Checks for the LOL SDK.
            if(LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return LanguageManager.GetLanguageTextStatic(GetEnemyTypeNameAbbreviationKey());
            }
            else
            {
                return "EMY";
            }
        }
        
        // Gets the enemy name abbreviation key.
        public static string GetEnemyTypeNameAbbreviationKey()
        {
            return "emy_nme_abv";
        }

        // MOVEMENT //
        // Returns the row the enemy is in.
        public int GetRow()
        {
            return row;
        }

        // Sets the row the enemy is in.
        public void SetRow(int newRow)
        {
            // Gets the row count.
            int rowCountMax = ActionManager.Instance.actionStage.rowEnemyUnits.Count;

            // Tries to remove the enemy unit from its row list.
            TryRemoveFromRowList();

            // Set the new row.
            row = newRow;

            // Tries to add to the row list.
            TryAddToRowList();
        }

        // Returns 'true' if the enemy unit is in a row list.
        public bool InRowList()
        {
            bool result = false;

            // If the row index is valid.
            if (row >= 0 && row < actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    result = actionManager.actionStage.rowEnemyUnits[row].Contains(this);
                }
            }

            return result;
        }

        // Tries to add to the set row list. Returns 'true' if it was added. Won't be added if already in list.
        public bool TryAddToRowList()
        {
            // Gets set to 'true' if added in the list. Won't add if already in list.
            bool added = false;

            // New row is valid.
            if (row >= 0 && row <= actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    // If the list doesn't contain this enemy unit, add it.
                    if (!actionManager.actionStage.rowEnemyUnits[row].Contains(this))
                    {
                        actionManager.actionStage.rowEnemyUnits[row].Add(this);
                        added = true;
                    }
                }
            }
            // Row is not valid.
            else
            {
                Debug.LogWarning("There is no row " + row.ToString() + " in the row lists for the stage map.");
            }

            return added;
        }

        // Tries to add this to the provided row list. Returns 'true' if it was added. Won't be added if already in list.
        public void TryAddToRowList(int newRow)
        {
            SetRow(newRow);
        }

        // Tries removing this unti enemy from the row list its in.
        // Returns 'true' if it was successfully removed.
        // Returns 'false' if the row didn't exist or if the row didn't contain this enemy.
        public bool TryRemoveFromRowList()
        {
            // Checks if the enemy was removed.
            bool removed = false;

            // If the row index is valid.
            if (row >= 0 && row < actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    // If the list contains this enemy unit, remove it.
                    if (actionManager.actionStage.rowEnemyUnits[row].Contains(this))
                    {
                        actionManager.actionStage.rowEnemyUnits[row].Remove(this);

                        // Enemy unit removed.
                        removed = true;
                    }
                }
            }

            return removed;
        }

        // Cancels out the velocity, setting it to 0.
        // If 'checkVelocity' is true, a check is done to see if the velocity isn't 0 first. If it is, do nothing.
        // If 'checkVelocity' is false, the change happens regardless.
        public void CancelVelocity(bool checkVelocity = true)
        {
            // If the velocity should be checked for it not being zero.
            if(checkVelocity)
            {
                if (rigidbody.velocity != Vector2.zero)
                    rigidbody.velocity = Vector2.zero;
            }
            // Do it regardless.
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

        // TILE //
        // Returns true if the entity can use the tile.
        public override bool UsableTile(ActionTile tile)
        {
            // Enemies units can share a tile, and are not locked to a single tile.
            return true;
        }

        // ATTACK //
        // Returns 'true' if there's a target within range.
        public bool IsTargetInRange()
        {
            return targetInRange;
        }

        // Returns 'true' if the enemy has a target.
        public bool HasTarget()
        {
            return currTarget != null;
        }

        // Returns 'true' if the enemy attack has a target.
        public bool HasEnemyAttackTarget()
        {
            // If set, check enemy attack. If null, return false.
            if (enemyAttack != null)
                return enemyAttack.HasTarget();
            else
                return false;
        }

        // Returns 'true' if the enemy can move and attack at the same time.
        public bool CanMoveAndAttack()
        {
            return moveAndAttack;
        }

        // If the enemy is attacking, returns true.
        public bool IsAttacking()
        {
            // If the current target is set, then the enemy is attacking.
            bool result = currTarget != null;

            // NOTE: checking enemy attack shouldn't be needed.
            // // If enemy attack is being used and it's set.
            // if (useEnemyAttack && enemyAttack != null)
            // {
            //     // There's a current target, the enemy attack is active and has a target.
            //     result = currTarget != null && IsEnemyAttackActive() && enemyAttack.HasTarget();
            // }
            // // Only check if the target is in range.
            // else
            // {
            //     result = currTarget != null;
            // }
            
            // Returns the result.
            return result;
        }

        // Returns 'true' if the enemy attack object is active.
        public bool IsEnemyAttackActive()
        {
            // If the enemy attack exists, check it as active and enabled.
            if (enemyAttack != null)
            {
                return enemyAttack.isActiveAndEnabled;
            }
            else
            {
                return false;
            }
        }

        // Activates the enemy attack and targets the provided unit.
        public void ActivateEnemyAttack(ActionUnit targetUnit)
        {
            enemyAttack.gameObject.SetActive(true);
            enemyAttack.SetTarget(targetUnit);
        }

        // Deactivates the enemy attack.
        public void DeactivateEnemyAttack()
        {
            enemyAttack.gameObject.SetActive(true);
            enemyAttack.ClearTarget(true);
            enemyAttack.gameObject.SetActive(false);
        }

        // Attacks the provivided user unit.
        public void AttackUserUnit(ActionUnitUser target)
        {
            // If the enemy attack is being used and the enemy attack isn't active.
            if (useEnemyAttack && !IsEnemyAttackActive())
            {
                ActivateEnemyAttack(target);
            }

            // The target exists, so attack.
            if(target != null)
            {
                target.AttackUnit(this);
            }
        }

        // Called when the unit has been damaged.
        public override void OnUnitDamaged(float damage)
        {
            base.OnUnitDamaged(damage);

            // If the damage animation should be used.
            if(useDamagedAnim)
            {
                // If some damage was done, play the animation.
                if (damage > 0)
                {
                    // Plays the unit damage animation.
                    PlayUnitDamagedAnimation();
                }
            }

            // If the damaged SFX should be used.
            if (useDamagedSfx)
            {
                // If some damage was done, play the SFX.
                if(damage > 0)
                {
                    PlayUnitDamagedSfx();
                }
            }
        }

        // Called when the unit has been attacked.
        public override void OnUnitAttacked(ActionUnit attacker)
        {
            base.OnUnitAttacked(attacker);

            // NOTE: moved to OnUnitDamaged since some forms of damage don't...
            // End up coming from an atacker.

            // // But you probably don't need to account for that.
            // // If the damaged sound effect should be used, use it.
            // if(useDamagedSfx)
            // {
            //     PlayUnitDamagedSfx();
            // }
        }


        // SPEED //

        // Calculates the movement speed with the provided stat factor.
        public static float CalculateMovementSpeed(float statFactor, float movementSpeed)
        {
            switch(GameSettings.Instance.gameplayMode)
            {
                case GameSettings.gameMode.generation: // Generation
                    return movementSpeed / 100.0F * 0.840F * statFactor;

                case GameSettings.gameMode.defense: // Defense
                default:
                    return movementSpeed / 100.0F * 0.820F * statFactor;
            }
        }

        // Calculates movement speed with a stat factor of 1.
        public static float CalculateMovementSpeed(float movementSpeed)
        {
            return CalculateMovementSpeed(1, movementSpeed);
        }

        // Calculates the movement speed using the set values.
        public float CalculateMovementSpeed()
        {
            return CalculateMovementSpeed(statFactor, movementSpeed);
        }

        // KILL / DEATH //
        // Kills the unit.
        public override void Kill()
        {
            // Enemy retreat has been moved from 'Kill()' to 'OnUnitDeath()'

            // If enemy retreat is being used and the enemy retreat prefab is set.
            if(useEnemyRetreat && enemyRetreatPrefab != null)
            {
                // Save the death cost and set the death cost to 0.
                // This is so that the base Kill() function doesn't reduce the enemy's energy.
                // The enemy's energy reduction will happen when the enemy retreat...
                // Returns to the enemy ship.

                // Gets the death cost and temporarily stores it.
                float tempDeathCost = energyDeathCost;
                energyDeathCost = 0;

                // Creates the retreat enemy.
                EnemyRetreat retreat = Instantiate(enemyRetreatPrefab);

                // If the player enemy exists.
                if(playerEnemy != null)
                {
                    // If the player enemy has an enemy retreat parent, use that.
                    // If it doesn't use the player enemy as the parent.
                    retreat.transform.parent = playerEnemy.enemyRetreatParent != null ? 
                        playerEnemy.enemyRetreatParent.transform : playerEnemy.transform;
                }

                // Sets the retreat's position.
                retreat.transform.position = transform.position;

                // Sets the energy death cost and the death cost factor.
                retreat.energyDeathCost = tempDeathCost;
                retreat.energyDeathCostFactor = energyDeathCostFactor;

                // Calls the base kill function.
                base.Kill();

                // Restores the death cost to normal.
                energyDeathCost = tempDeathCost;
            }
            else
            {
                // Saves the current energy death cost temporarily.
                float tempDeathCost = energyDeathCost;

                // Apply the energy death cost factor.
                energyDeathCost *= energyDeathCostFactor;

                // Just call base kill.
                base.Kill();

                // Restore the energy death cost back to normal.
                energyDeathCost = tempDeathCost;
            }
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            base.OnUnitDeath();

            // If attached to a player enemy.
            if(playerEnemy != null)
            {
                // Calls the related function.
                playerEnemy.OnEnemyUnitDeath(this);
            }

            // Tries to remove the unit from its row list.
            // This automatically checks if the row list contains this.
            TryRemoveFromRowList();

            // If using tutorials and tutorial not active, run applicable tutorial.
            if (actionManager.IsUsingTutorialsAndTutorialNotActive())
            {
                // Runs the first kill tutorial.
                if (!actionManager.tutorials.Data.clearedFirstActionFirstKillTutorial)
                {
                    actionManager.tutorials.LoadFirstActionFirstKillTutorial();
                }
            }
        }


        // ANIMATIONS
        // Plays the spawn animation.
        public void PlayUnitSpawnAnimation()
        {
            // Plays the spawn animation if the animator and animation are set.
            if (animator != null && unitSpawnAnim != "")
            {
                // Could use the UnitAnimations script, but this is more direct.
                animator.Play(unitSpawnAnim);
            }
        }

        // Plays the unit damage animation.
        public void PlayUnitDamagedAnimation()
        {
            // Plays the damage animation if the animator and animation are set.
            if (animator != null && unitDamagedAnim != "")
            {
                // Could use the UnitAnimations script, but this is more direct.
                animator.Play(unitDamagedAnim);
            }
        }


        // AUDIO
        // Plays the attack sound effect.
        public void PlayUnitAttackSfx()
        {
            // If audio can be played.
            if(CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(unitAttackSfx);
            }
        }

        // Plays the damaged sfx.
        public void PlayUnitDamagedSfx()
        {
            // If the damaged SFX can be used, play it.
            if (CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(unitDamagedSfx);
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Set to true if the entity tried to perform an attack.
                bool triedAttack = false;

                // Set to true if the entity has moved.
                // bool moved = false;


                // If the enemy has a target to attack.
                if(IsTargetInRange())
                {
                    // Cancel out the velocity.
                    CancelVelocity();

                    // An attack was attempted, so set this to true.
                    // This doesn't mean an attakc happened, just that one was attempted.
                    triedAttack = true;
                }

                // If the enemy should use movement.
                if (movementEnabled)
                {
                    // If the enemy can move and attack...
                    // Or if the enemy can't move and attack at the same time, but has attacked.
                    if(moveAndAttack || (!moveAndAttack && !triedAttack))
                    {
                        // Calculates the move speed.
                        float moveSpeedAdjusted = CalculateMovementSpeed();

                        // Moves the enemy unit. The enemy shoud move at a fixed speed.
                        // Old - Uses translate function.
                        // transform.Translate(movementDirec.normalized * moveSpeedAdjusted * Time.deltaTime);

                        // New - adds force and clamps it to the move speed.
                        rigidbody.AddForce(movementDirec.normalized * moveSpeedAdjusted * Time.deltaTime, ForceMode2D.Impulse);
                        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, moveSpeedAdjusted);

                        // The enemy moved.
                        // moved = true;
                    }
                    else
                    {
                        // Cancel out the velocity.
                        CancelVelocity();
                    }
                }
                else
                {
                    // If the rigidbody's velocity is not 0, make it 0.
                    CancelVelocity();
                }

                // If sprites should be changed.
                if(changeSprites)
                {
                    // Checks if the enemy is attacking by seeing if there's a target in range.
                    bool attacking = IsAttacking();

                    // NOTE: this doesn't check if the sprites being changed have been set to proper values.
                    // Just set the default sprite to all values if an enemy doesn't have a sprite that...
                    // Aligns with a specific value.

                    // If the sprite rendere is set to the netural sprite and an attack was tried...
                    // Switch to the attack sprite.
                    if(spriteRenderer.sprite == neutralSprite && attacking)
                    {
                        spriteRenderer.sprite = attackSprite;
                    }
                    // If an attack wasn't tried and the attack sprite is currently being used...
                    // Switch to the netural sprite.
                    else if(spriteRenderer.sprite == attackSprite && !attacking)
                    {
                        spriteRenderer.sprite = neutralSprite;
                    }
                }

                // Sets to false. This will keep getting set to true as long as there's a target in range.
                targetInRange = false;

                // If the current target is set.
                if (currTarget != null)
                {
                    // If the current target isn't active and enabled, set it to null.
                    if(!currTarget.isActiveAndEnabled)
                    {
                        currTarget = null;
                    }
                }

                // If the enemy should check for the end of the map on the left side.
                if (checkForMapLeftBound)
                {
                    // Gets the reference vector.
                    Vector3 refVec = actionManager.actionStage.GetMapWorldPositionReferenceVector(transform.position);

                    // Outside the map on the left side.
                    if (refVec.x < 0.0F)
                    {
                        // The player has died.
                        actionManager.OnPlayerUserDeath();
                    }
                }
                
            }
            // If the stage isn't playing, no actions should be performed.
            else if(!actionManager.IsStagePlaying())
            {
                // If the rigidbody's velocity is not 0, make it 0.
                CancelVelocity();
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Tries to remove this object from the row list.
            TryRemoveFromRowList();
        }
    }
}