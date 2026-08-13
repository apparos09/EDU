using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A lane blaster.
    public class ActionUnitDefenseBlasterLane : ActionUnitDefenseBlaster
    {
        [Header("Defense/Blaster/Lane")]

        // Modifies the enemy energy death cost for enemies killed by the lane blaster if true.
        [Tooltip("Modifies the enemy's energy death cost if killed by a lane blaster projectile.")]
        public bool modifyEnemyEnergyDeathCost = true;

        // Kills unit once an attack has been performed.
        [Tooltip("Kills this unit once an attack has been performed.")]
        public bool killOnAttackPerformed = true;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Makes sure the projectile pool isn't being used...
            // Since lane blasters are destroyed automatically.
            if(killOnAttackPerformed)
            {
                useProjectilePool = false;
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Lane blasters shouldn't take damage or be able to attack by default.
            // Once a shot is triggered, the lane blaster destroys itself.
            vulnerable = false;
            attackingEnabled = false;

            // Use the shot sound effect.
            useShotSfx = true;
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Checks if the entity that triggered the laser is an enemy unit.
            ActionUnitEnemy enemyUnit;

            // Tries to get component.
            if(collision.TryGetComponent(out enemyUnit))
            {
                // Enable attacking.
                attackingEnabled = true;
            }
        }

        // Called when an attack ahs been performed.
        public override void OnUnitAttackPerformed()
        {
            base.OnUnitAttackPerformed();

            // If unit should be killed.
            if(killOnAttackPerformed)
            {
                // A shot has been fired, so kill the lane blaster.
                Kill();
            }
        }

        // Shoots the projectile.
        public override ActionProjectile Shoot()
        {
            // Fires the projectile.
            ActionProjectile projectile = base.Shoot();

            // If it is a lane blaster projectile, down cast.
            if(projectile is ActionProjectileLaneBlaster)
            {
                // Downcast.
                ActionProjectileLaneBlaster projLaneBlaster = (ActionProjectileLaneBlaster)projectile;
            
                // If the enemy death cost shouldn't be modified...
                // Set the factor to 1.0. It's already set in the prefab...
                // So it doesn't need to be changed if modifyEnemyEnergyDeathCost...
                // Is true.
                if(!modifyEnemyEnergyDeathCost)
                {
                    // Set to default (1.0F).
                    projLaneBlaster.enemyDeathCostFactor = 1.0F;
                }
            }

            return projectile;
        }

        // Called when the unit has died.
        public override void OnUnitDeath()
        {
            // Tile exists.
            if(tile != null)
            {
                // If this is the unit on the tile, mark the tile as unusable.
                if(tile.HasActionUnitUser(this))
                {
                    tile.SetTileOverlayType(ActionTile.actionTileOverlay.unusable);
                }
            }

            base.OnUnitDeath();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}