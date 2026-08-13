using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A trap, which damages enemies that pass over it.
    public class ActionUnitDefenseTrap : ActionUnitDefense
    {      
        [Header("Defense/Trap")]

        // The number of hits that have occurred.
        public int hitCount = 0;

        // The limit on the number of hits this trap can have before it destroys itself.
        [Tooltip("The number of hits this trap can take before it destroys itself.")]
        public float hitLimit = 10;

        // If 'true', the hit limit is enabled. If false, there is no limit, so the hit limit is infinite.
        [Tooltip("Enables hit limit. If the hit limit isn't being used, the hit limit is considered infinite.")]
        public bool useHitLimit = true;

        // The amount of energy that the owner gets when a unit is damaged. By default it's 25%.
        [Tooltip("The percentage of energy taken for the damage done to an enemy (0.00 -> 0%).")]
        public float damageEnergyPercent = 0.25F;

        // If 'true', when the trap is killed, any targets on the same tile are killed as well.
        [Tooltip("If true, when this trap reaches it energy limit, all targets on the tile this unit uses are also destroyed.")]
        public bool killTargetsOnHitLimitReached = true;

        // The list of enemy targets. A unit is added to this list in trigger start and removed in trigger end.
        [Tooltip("A list of enemy targets. A target is added in trigger start and removed in trigger end.")]
        protected List<ActionUnitEnemy> enemyTargets = new List<ActionUnitEnemy>();

        [Header("Defense/Trap/Animaitons")]

        // The animation for energy generation.
        public string energyGenAnim = "Action Unit - Flash - Blue Animation";

        [Header("Defense/Trap/Audio")]

        // The explosion audio for the trap.
        public AudioClip explosionSfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the defense type is unknown, set to default value based on script.
            if (defType == defenseType.unknown)
            {
                defType = defenseType.trap;
            }

            // The defense is intangible so that enemies aren't blocked by it, and don't get stopped by it.
            tangible = false;

            // Enable the death animation.
            deathAnimationEnabled = true;
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Enemy component.
            ActionUnitEnemy enemy;

            // Gets the enemy.
            if(collision.TryGetComponent(out enemy))
            {
                // Tries to add the enemy to the list.
                TryAddEnemyTargetToList(enemy);
            }
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            // Calls base function.
            base.OnTriggerStay2D(collision);
        }

        // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);

            // Enemy component.
            ActionUnitEnemy enemy;

            // Gets the enemy.
            if (collision.TryGetComponent(out enemy))
            {
                // Try to remove the enemy to the list.
                TryRemoveEnemyTargetFromList(enemy);
            }
        }

        // Returns 'true' if the trap can attack.
        public override bool CanAttack()
        {
            // Checks if the trap can attack and if it has remaining hits.
            bool canAttack = base.CanAttack();
            bool hasHits = !HasHitLimtBeenReached();

            return canAttack && hasHits;
        }

        // Called to perform an attack.
        public override void PerformAttack()
        {
            // Attacks all the enemies.
            AttackAllEnemiesInList();

            // Calls base.
            base.PerformAttack();
        }

        // Called when an attack has been performed.
        public override void OnUnitAttackPerformed(ActionUnit target)
        {
            base.OnUnitAttackPerformed(target);
        }

        // Returns 'true' if the hit count is below the hit limit.
        public bool HasHitLimtBeenReached()
        {
            // If the hit limit is being used, check if the hit count is...
            // At or above the limit.
            // If the hit limit isn't being used, there is no limit to reach.
            if (useHitLimit)
                return hitCount >= hitLimit;
            else
                return false;
        }

        // Tries to add an enemy target to the list.
        public bool TryAddEnemyTargetToList(ActionUnitEnemy enemyUnit)
        {
            // If the enemy isn't in the list, add it.
            if(!enemyTargets.Contains(enemyUnit))
            {
                enemyTargets.Add(enemyUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Tries to remove an enemy target from the list.
        public bool TryRemoveEnemyTargetFromList(ActionUnitEnemy enemyUnit)
        {
            // If the enemy is in the list, remove it.
            if (enemyTargets.Contains(enemyUnit))
            {
                enemyTargets.Remove(enemyUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Removes enemies from the list that are null.
        // If 'nullOnly' is true, dead enemies are kept in the list.
        public void RemoveDeadAndNullEnemyTargetsFromList(bool nullOnly = false)
        {
            // Goes through all enemies.
            for(int i = enemyTargets.Count - 1; i >= 0; i--)
            {
                // If the enemy exists.
                if (enemyTargets[i] != null)
                {
                    // Removes the enemy if it's dead.
                    if (enemyTargets[i].IsDead() && !nullOnly)
                    {
                        enemyTargets.RemoveAt(i);
                    }
                }
                // Removes the enemy since it's null.
                else
                {
                    enemyTargets.RemoveAt(i);
                }
            }
        }

        // Attacks all the enemies in the list.
        public void AttackAllEnemiesInList()
        {
            // The sum of the energy gained from the attacks.
            float energyGainSum = 0.0F;

            // Goes through all the enemies in the list, back to front.
            for(int i = enemyTargets.Count - 1; i >= 0; i--)
            {
                // Checks if the enemy exists.
                if(enemyTargets[i] != null)
                {
                    // The enemy's health before and after the attack.
                    float healthBefore = enemyTargets[i].health;
                    float healthAfter = 0;

                    // The difference in health amounts, and the energy gained.
                    float healthDiff;
                    float energyGain;

                    // Attacks the enemy target.
                    enemyTargets[i].AttackUnit(this);

                    // Checks that i is still less than the energy targets count.
                    if(i < enemyTargets.Count)
                    {
                        // If the enemy still exists, get its health afterwards.
                        // If the enemy doesn't exist, assume the enemy's full health is 0.
                        healthAfter = (enemyTargets[i] != null) ? enemyTargets[i].health : 0;
                    }
                    // (i) isn't less than the enemy targets count, meaning the list has been adjusted.
                    else
                    {
                        // Assume the enemy lost all its health.
                        healthAfter = 0;
                    }

                    // Bounds check for health before.
                    if(healthBefore < 0)
                        healthBefore = 0;

                    // Bounds check for health after.
                    if(healthAfter < 0)
                        healthAfter = 0;

                    // Calculates the different in health to see how muc damage was done.
                    healthDiff = healthBefore - healthAfter;

                    // Calculates the energy gain by taking a percentage of the damage (health difference).
                    energyGain = healthDiff * damageEnergyPercent;

                    // Bounds check for energy gain.
                    if(energyGain < 0)
                        energyGain = 0;


                    // Add to the energy gain sum.
                    energyGainSum += energyGain;

                    // Adds to the hit count.
                    hitCount++;

                    // If the hit limit has been reached, stop damaging enemy targets.
                    if (HasHitLimtBeenReached())
                        break;
                }
                else
                {
                    // Remove the enemy target since it's null.
                    enemyTargets.Remove(enemyTargets[i]);
                }
            }


            // If the onwer is set and the energy gain sum is greater than 0...
            // Add it to the owner's energy.
            if(owner != null && energyGainSum > 0)
            {
                // Rounds the energy gain sum up to a whole number.
                float energyGainSumCeiled = Mathf.Ceil(energyGainSum);

                // Increase energy by the ceil rounded amount.
                owner.IncreaseEnergy(energyGainSumCeiled);

                // Play the energy generation animation.
                PlayEnergyGenerationAnimation();
            }
        }

        // Kills all the enemy targets, which also clears the list.
        public void KillAllEnemyTargets()
        {
            // Goes through all enemies.
            for (int i = enemyTargets.Count - 1; i >= 0; i--)
            {
                // If the enemy isn't null, kill it.
                if (enemyTargets[i] != null)
                {
                    enemyTargets[i].Kill();
                }
            }

            // Clear the list.
            enemyTargets.Clear();
        }

        // THe kill function.
        public override void Kill()
        {
            // If enemy targets should be killed when the hit limit has been reached...
            // And the hit limit has been reached, kill all the targets.
            if(killTargetsOnHitLimitReached && HasHitLimtBeenReached())
            {
                KillAllEnemyTargets();
            }

            // Call kill base afterwards so that the trigger exit function hasn't been called yet.
            base.Kill();
        }

        // ANIMATIONS
        // Plays the energy generation animation.
        public void PlayEnergyGenerationAnimation()
        {
            if (animator != null && energyGenAnim != "")
                animator.Play(energyGenAnim);
        }

        // AUDIO
        // Plays the explosion sound effect.
        public void PlayExplosionSfx()
        {
            if(CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(explosionSfx);
            }
        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Stage is active.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Checks if the hit limit has been reached.
                if (HasHitLimtBeenReached())
                {
                    // Hit limit reached, so kill the trap.
                    Kill();
                }
                // Still hits left.
                else
                {
                    // Calculates and sets the attack cooldown time...
                    // If the attack cooldown time is zero or less.
                    // This is done here so that the trap can damage all its targets...
                    // In its trigger function.
                    // if (attackCooldownTimer <= 0.0F)
                    // {
                    //     CalculateAndSetAttackCooldownTime();
                    // }
                }

            }
        }
    }
}