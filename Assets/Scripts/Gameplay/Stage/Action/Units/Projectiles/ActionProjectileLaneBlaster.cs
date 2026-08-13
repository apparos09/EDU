using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action projectile for the lane blaster.
    public class ActionProjectileLaneBlaster : ActionProjectile
    {
        [Header("Projectile Lane Blaster")]

        // The enemy death cost factor for enemies destroyed by this projectile.
        // This is used to reduce the energy death cost for an enemy killed by a lane blaster.
        public float enemyDeathCostFactor = 0.10F; // Original: 0.25F

        // Called when the projectile has made contact with the target.
        public override void OnContactWithTarget(ActionUnit target)
        {
            // If the target is a unit enemy.
            if(target is ActionUnitEnemy)
            {
                // Downcast.
                ActionUnitEnemy unitEnemy = (ActionUnitEnemy)target;
                
                // Sets the energy death cost factor.
                unitEnemy.energyDeathCostFactor = enemyDeathCostFactor;

                // NOTE: this doesn't put it back to normal, but since it's a one-hit kill...
                // It should be fine.
            }

            base.OnContactWithTarget(target);
        }

    }
}
