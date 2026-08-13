using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action unit defense blaster.
    public class ActionUnitDefenseBlaster : ActionUnitDefense
    {
        [Header("Defense/Blaster")]

        // The projectile prefab.
        public ActionProjectile projectilePrefab;

        // A game object used to position the projectile when it's first shot.
        // If this is null, the shooter's object is used.
        [Tooltip("The starting position of the projectile as an object. If null, the shooter's position is the starting position.")]
        public GameObject projectileStartPos;

        // The offset of the starting position of a fired projectile.
        // This is applied to projectileStartPos's position if that object isn't null.
        [Tooltip("The offset of the starting position of a fired projectile.")]
        public Vector3 projectileStartPosOffset = Vector3.zero;

        // If 'true', the offset is flipped on the x-axis.
        [Tooltip("Flips the offset's x-value when applied for getting the projectile's position if true.")]
        public bool flipOffsetX = false;

        // The projectiles fired by this unit.
        public List<ActionProjectile> firedProjectiles = new List<ActionProjectile>();

        [Header("Defense/Blaster/Animation")]

        // The shoot animation for the blaster.
        public string shootAnim = "Action Unit Defense - Standard Blaster - Shoot Animation";

        // If 'true', the shoot animation is used.
        [Tooltip("Uses the shoot animation if set to true.")]
        public bool useShootAnim = true;

        [Header("Defense/Blaster/Audio")]
        // The blaster shot sfx.
        public AudioClip shotSfx;

        // If 'true', the shot sound effect is used.
        // By default, this sound effect isn't used.
        protected bool useShotSfx = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the defense type is unknown, set to default value based on script.
            if (defType == defenseType.unknown)
            {
                defType = defenseType.blaster;
            }
        }

        // Performs an attack.
        public override void PerformAttack()
        {
            Shoot();
            base.PerformAttack();
        }

        // Called when an attack ahs been performed.
        public override void OnUnitAttackPerformed()
        {
            base.OnUnitAttackPerformed();

            // If the shot sound effect should be used, play it.
            if(useShotSfx)
            {
                // Play the shot sound effect.
                PlayShotSfx();
            }
        }

        // Calculates and returns the projectile's starting position.
        public virtual Vector3 CalculateProjectileStartingPosition()
        {
            // The start position to be returned.
            Vector3 startPos;

            // The offset to be applied.
            Vector3 offset = projectileStartPosOffset;

            // If the offset should be flipped on the x-axis, flip it.
            if (flipOffsetX)
                offset.x = -offset.x;

            // If the projecile start position exists, use that plus the offset.
            if (projectileStartPos != null)
            {
                startPos = projectileStartPos.transform.transform.position + offset;
            }
            // No projectile start position object, so use shooter's transform plus offset.
            else
            {

                startPos = transform.position + offset;
            }

            // Returns the starting position.
            return startPos;
        }

        // Shoots a projectile. Returns the projectile if it's been instantiated correctly.
        public virtual ActionProjectile Shoot()
        {
            // The projectile to be returned.
            ActionProjectile projectile = null;

            // If the projectile pool is being used, try getting a projectile from it.
            if(useProjectilePool)
            {
                // The projectile should be activated if one is returned from the pool.
                projectile = GetProjectileFromPool(true);
            }

            // Projectile prefab exists and the projectile is null (didn't get object from pool).
            if (projectilePrefab != null && projectile == null)
            {
                projectile = Instantiate(projectilePrefab);
            }

            // If the projectile exists, set its values.
            // The projectile exists if it was gotten from the pool or instantiated from a prefab.
            if(projectile != null)
            {
                // Sets the projectile's parent.
                if (owner != null)
                {
                    // If the owner's projectile parent is set, use that.
                    // If the owner has no projectile parent, use the owner's transform.
                    // This doesn't use the blaster's parent in case the blaster gets destroyed.
                    projectile.transform.parent = owner.unitProjectileParent != null ? owner.unitProjectileParent.transform : owner.transform;
                }

                // The starting position.
                Vector3 startPos = CalculateProjectileStartingPosition();

                // Sets the starting position.
                projectile.transform.position = startPos;

                // Sets the owner, shooter and move direction.
                projectile.owner = owner;
                projectile.shooterUnit = this;
                projectile.moveDirec = Vector2.right;

                // Updates the shooter values in the projectile.
                projectile.UpdateShooterAttackValues();

                // Add to list of fired projectiles.
                firedProjectiles.Add(projectile);

                // Uses the shoot animation if true.
                if (useShootAnim)
                {
                    PlayShootAnimation();
                }
            }

            return projectile;
        }

        // Called when a projectile has been killed.
        public void OnProjectileKilled(ActionProjectile projectile)
        {
            // If the fired projectile list includes this projectile, remove it.
            if(firedProjectiles.Contains(projectile))
            {
                firedProjectiles.Remove(projectile);
            }
        }

        // ANIMATION //
        // Plays the shoot animation.
        public void PlayShootAnimation()
        {
            // Animator is set and so is the shoot animation.
            if(animator != null && shootAnim != "")
            {
                animator.Play(shootAnim);
            }
        }

        // AUDIO //
        // Plays the shot sound effect.
        public void PlayShotSfx()
        {
            if (CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(shotSfx);
            }
        }
    }
}