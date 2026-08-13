using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A blaster that shoots three shots at once, each going in a different one.
    public class ActionUnitDefenseBlasterTriple : ActionUnitDefenseBlaster
    {
        // The number of projectiles that will be fired.
        // If this number is 0 or negative, 1 projectile is fired.
        public const int PROJECTILE_COUNT = 3;

        // Performs an attack.
        public override void PerformAttack()
        {
            // Calls the base function to generate the first shot.
            base.PerformAttack();

            // Gets the first shot from the projectile list.
            ActionProjectile shot1 = firedProjectiles[firedProjectiles.Count - 1];

            // Gets the second and third shot.
            // The additional shots don't take extra energy.
            ActionProjectile shot2 = Shoot();
            ActionProjectile shot3 = Shoot();

            // Changes the angles of the second and third shot.
            shot2.moveDirec = new Vector2(1.0F, 1.0F); // Diagonal-Up
            shot3.moveDirec = new Vector2(1.0F, -1.0F); // Diagonal-Down
        }

    }
}