using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A blaster that fires multiple shots in short succession for every instance.
    public class ActionUnitDefenseBlasterRapid : ActionUnitDefenseBlaster
    {
        [Header("Defense/Blaster/Rapid")]

        // The number of shots fired by the rapid blaster.
        // If 0, it will only shoot once.
        [Tooltip("The number of remaining shots to fire in succession for every instance. If 0 or less, it only shoots 1 shot.")]
        public int shotCount = 0;

        // The maximum amount of shots to fire.
        [Tooltip("The maximum amount of shots to fire in one instance.")]
        public int shotCountMax = 3;

        // The delay in seconds between shots when firing them rapidly.
        [Tooltip("The delay (in seconds) between shots when firing rapidly.")]
        public float shotRapidDelay = 0.125F; // 1/8 second

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the shot count to its max.
            SetShotCountToMax();
        }

        // Sets the shot count to its max.
        public void SetShotCountToMax()
        {
            shotCount = shotCountMax;
        }

        // Called when an attack has been performed.
        public override void OnUnitAttackPerformed()
        {
            base.OnUnitAttackPerformed();

            // Reduce the shot count.
            shotCount--;

            // The shots have finished.
            if(shotCount <= 0)
            {
                // Set the shot count down to max.
                SetShotCountToMax();

                // Set the attack cooldown time to its normal value.
                CalculateAndSetAttackCooldownTime();
            }
            else
            {
                // Checks if the blaster has energy for another attack.
                bool hasEnergyForAttack = false;

                // If the owner exists, see if they have energy for another shot.
                if(owner != null)
                {
                    hasEnergyForAttack = owner.HasEnergyForAttack(this);
                }

                // The owner has energy for another attack.
                if(hasEnergyForAttack)
                {
                    // Reduce the attack cooldown so that another shot will be fired.
                    // If the attack countdown is less than the rapid shot delay, don't change it.
                    if (attackCooldownTimer > shotRapidDelay)
                        attackCooldownTimer = shotRapidDelay;
                }
                // Lacks energy for another attack.
                else
                {
                    // Reset the shot count and leave the attack cooldown timer as is.
                    SetShotCountToMax();
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            
            // If the shot count is less than the shot count max...
            // And the attack cooldown timer is above the shot rapid delay...
            // Set the shot count to the max.
            // This is to fix a bug where the rapid blaster would use its regular attack cooldown time...
            // But not reset the shot count to its max.
            // NOTE: make sure the regular attack cooldown time is greater than the shot rapid delay time.
            if(shotCount < shotCountMax && attackCooldownTimer > shotRapidDelay)
            {
                SetShotCountToMax();
            }
        }
    }
}