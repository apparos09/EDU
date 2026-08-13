using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A projectile that phases through targets and gets weaker with each hit.
    public class ActionProjectilePhase : ActionProjectile
    {
        [Header("Phase")]

        // The number of targets this projectile has gone through.
        public int targetHits = 0;

        // The decay of the attack power.
        // The attack power goes down by this amount every time the projectile hits something.
        [Tooltip("Decreases the attack power by the attack decay points for every hit the projectile has.")]
        public float attackPowerDecay = 15.0F;

        // The minimum value the attack power can have.
        [Tooltip("The minimum value the attack power can have.")]
        public float attackPowerMin = 5;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // This projectile shouldn't die on contact.
            dieOnContact = false;

            // The projectile's calculated attack should be used, not the shooter directly.
            useShooterDirectly = false;
        }

        // Calculates the attack power, taking into account the attack power decay.
        public override float CalculateAttackPower(bool updateShooter)
        {
            // Gets the base power.
            float basePower = base.CalculateAttackPower(updateShooter);

            // The power modified by the decay.
            float modPower = basePower;

            // Calculates the power decay.
            float powerDec = attackPowerDecay * targetHits;

            // The power decay is greater than 0, so apply it.
            if(powerDec > 0.0F)
            {
                // Reduce the modified power.
                modPower -= powerDec;
            }

            // If the modified power is less than the minimum, set it to the minimum.
            if (modPower < attackPowerMin)
            {
                modPower = attackPowerMin;
            }

            return modPower;
        }

        // Called wehn the projectile has made contact with a target.
        public override void OnContactWithTarget(ActionUnit target)
        {
            // Call the base function.
            base.OnContactWithTarget(target);

            // Increase the number of hits.
            targetHits++;
        }

        // Sets target hits back to 0.
        public void ResetTargetHits()
        {
            targetHits = 0;
        }

    }
}