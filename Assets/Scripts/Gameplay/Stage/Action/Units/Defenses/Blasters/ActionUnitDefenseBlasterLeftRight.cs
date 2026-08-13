using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A action unit blaster that shoots from the front (right) and the back (left).
    public class ActionUnitDefenseBlasterLeftRight : ActionUnitDefenseBlaster
    {
        [Header("Defense/Blaster/Left-Right")]

        // The projectile start pos in the parent class is for firing forward (right).
        // The starting position of the projectile if shooting towards the left side. 
        [Tooltip("Starting position used when firing a projectile to the left (back).")]
        public GameObject projectileStartPosLeft;

        // The offset of the projectile if it's being fired left.
        [Tooltip("Offset used when firing a projecitle to the left (back).")]
        public Vector3 projectileStartPosLeftOffset = Vector3.zero;

        // If 'true', the offset for the left side is flipped.
        [Tooltip("Flips the offset for the left starting position if true.")]
        public bool flipOffsetLeftX = false;

        // Has a target to the left (back) and a target to the right (front).
        private bool hasTargetLeft = false;
        private bool hasTargetRight = false;

        [Header("Blaster/Left-Right/Animations")]
        public string shootBothAnim = "Action Unit Defense - Left-Right Blaster - Shoot - Both Animation";
        public string shootLeftAnim = "Action Unit Defense - Left-Right Blaster - Shoot - Left Animation";
        public string shootRightAnim = "Action Unit Defense - Left-Right Blaster - Shoot - Right Animation";

        // Returns 'true' if the defense unit has a target to attack.
        // SInce this is a front-back blaster, it checks the left and right side.
        public override bool HasTarget()
        {
            // Result to be returned.
            bool hasTarget = false;

            // Sets both to false. Will set to true if targets are found.
            hasTargetLeft = false;
            hasTargetRight = false;

            // On a tile.
            if (tile != null)
            {
                // Gets the tile's row.
                int row = tile.GetMapRowPosition();

                // If the row is valid.
                if (actionManager.actionStage.ValidMapRow(row))
                {
                    // Checks for targets to the left and right.
                    hasTargetLeft = actionManager.actionStage.IsEnemyInRowLeftOfPosition(row, transform.position, true, false);
                    hasTargetRight = actionManager.actionStage.IsEnemyInRowRightOfPosition(row, transform.position, true, false);
                    
                    // There's a target if there is one to the left or right.
                    hasTarget = hasTargetLeft || hasTargetRight;
                }
            }
            else
            {
                // Not on a tile, so a target cannot be searched for.
            }
            
            // Return the result.
            return hasTarget;
        }

        // Performs an attack.
        public override void PerformAttack()
        {
            // If there are targets are both sides, so shooting twice.
            // The game prioritizes shooting right if it can only shoot one direction.
            bool shootTwice = hasTargetLeft && hasTargetRight;

            // Performs an attack.
            base.PerformAttack();

            // If shooting twice.
            if(shootTwice)
            {
                // Gets set to 'true' if the owner has energy for a second shot.
                bool hasEnergyForShot2 = true;

                // Checks owner's energy.
                if (owner != null)
                    hasEnergyForShot2 = owner.HasEnergyForAttack(this);

                // If the owner has energy for shot 2.
                if (hasEnergyForShot2)
                {
                    // Projectiles go right by default, so the existing projectile stays going right.
                    ActionProjectile rightProj = firedProjectiles[firedProjectiles.Count - 1];

                    // Gets the left projectile. This function also puts iti n the list.
                    ActionProjectile leftProj = Shoot();

                    // Projectile start points and directions.
                    rightProj.transform.position = CalculateProjectileStartingPositionRight();
                    rightProj.moveDirec = Vector2.right;

                    leftProj.transform.position = CalculateProjectileStartingPositionLeft();
                    leftProj.moveDirec = Vector2.left;

                    // The owner exists.
                    if(owner != null)
                    {
                        // Reduce the owner's energy by the attack cost since two projectiles have been fired.
                        // The energy cost for the first shot fired has already been applied.
                        owner.DecreaseEnergy(CalculateAttackEnergyCost(attackEnergyCost));
                    }

                    // If the shoot animation should be used, use the shoot both animation.
                    if(useShootAnim)
                    {
                        PlayShootBothAnimation();
                    }
                }
                else
                {
                    // If the shoot animation should be used, use the shoot right animation.
                    // Since shooting right takes priority, it can be assumed that the blaster shot right.
                    if (useShootAnim)
                    {
                        PlayShootRightAnimation();
                    }
                }
            }
            else
            {
                // Only fired one shot, so see where it should go.

                // Gets the most recently fired projectile.
                ActionProjectile projectile = firedProjectiles[firedProjectiles.Count - 1];

                // The x-direction of the projectile.
                int direcX = 0;

                // If the target to the right, fire right.
                if(hasTargetRight)
                {
                    direcX = 1;
                }
                // If the target is to the left, fire left.
                else if(hasTargetLeft)
                {
                    direcX = -1;
                }
                // Unknown direction for the target. Since right is the default direction, fire right.
                else
                {
                    direcX = 1;
                }

                // Checks the direction to send the projectile.
                switch(direcX)
                {
                    case -1: // Left
                        projectile.transform.position = CalculateProjectileStartingPositionLeft();
                        projectile.moveDirec = Vector2.left;

                        // Play the shoot left animation.
                        if (useShootAnim)
                            PlayShootLeftAnimation();

                        break;

                    default: // Right
                    case 1:
                        projectile.transform.position = CalculateProjectileStartingPositionRight();
                        projectile.moveDirec = Vector2.right;

                        // Play the shoot right animation.
                        if (useShootAnim)
                            PlayShootRightAnimation();

                        break;
                }
            }

        }

        // Calculates and returns the projectile's starting position if firing from the back (left).
        public virtual Vector3 CalculateProjectileStartingPositionLeft()
        {
            // The start position to be returned.
            Vector3 startPos;

            // The offset to be applied (left side).
            Vector3 offset = projectileStartPosLeftOffset;

            // If the offset should be flipped on the x-axis, flip it.
            if (flipOffsetLeftX)
                offset.x = -offset.x;

            // If the starting position object for the left exists, use that.
            if (projectileStartPosLeft != null)
            {
                startPos = projectileStartPosLeft.transform.position + offset;
            }
            // Use the base transform position plus the offset.
            else
            {
                startPos = transform.position + offset;
            }
                
            // Return the starting position.
            return startPos;
        }

        // Uses the main proejctile starting position function since right is the default direction.
        public virtual Vector3 CalculateProjectileStartingPositionRight()
        {
            return CalculateProjectileStartingPosition();
        }

        // ANIMATION
        // Play animation for shooting out of both sides.
        public void PlayShootBothAnimation()
        {
            // Animator is set and so is the shoot both animation.
            if (animator != null && shootBothAnim != "")
            {
                animator.Play(shootBothAnim);
            }
        }

        // Play animation for shooting left.
        public void PlayShootLeftAnimation()
        {
            // Animator is set and so is the shoot left animation.
            if (animator != null && shootLeftAnim != "")
            {
                animator.Play(shootLeftAnim);
            }
        }

        // Play animation for shooting right.
        public void PlayShootRightAnimation()
        {
            // Animator is set and so is the shoot right animation.
            if (animator != null && shootRightAnim != "")
            {
                animator.Play(shootRightAnim);
            }
        }
    }
}
