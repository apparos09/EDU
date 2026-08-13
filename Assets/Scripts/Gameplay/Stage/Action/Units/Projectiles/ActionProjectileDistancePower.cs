using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // A projectile that's power changes based on how far it travels.
    public class ActionProjectileDistancePower : ActionProjectile
    {
        [Header("Distance Power")]

        // If 'true', the distance power changes are used.
        [Tooltip("If true, the power changes based on the projectile's distance from its starting point.")]
        public bool useDistancePowerChange = true;

        // The starting position of the projectile.
        public Vector3 startPosition = Vector3.zero;

        // If 'true', the start position is set in the start function.
        [Tooltip("Sets the start position in the Start() function if true.")]
        public bool setStartPosOnStart = true;

        // The maximum distance the projectile travels while having its power change.
        // The distance max by default is 10 tiles (1.28 x 10 = 12.8).
        [Tooltip("The maximum distance the projectile can travel before it's power is at a fixed amount.")]
        public float attackPowerDistanceMax = ActionStage.TILE_SIZE_X_DEFAULT * 10.0F;

        // The attack power at the maximum distance.
        // The base power is the power of the projectile or shooter based on the projectile's setting.
        [Tooltip("The projectile's power when it's at or beyond its maximum distance")]
        public float attackPowerAtDistanceMax = 10.0F;

        // Colors
        // If set to 'true', the projectile changes colour based on the power.
        // This is disabled since it isn't very noticable.
        protected bool changeColor = false;

        // The lowest and highest power colours.
        protected Color lowestPowerColor = new Color(0.25F, 0.25F, 0.25F); // Dark Grey
        protected Color highestPowerColor = Color.white; // White

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set this to false so that the projectile's attack power...
            // Based on the shooter's attack power is used.
            // In other words, don't pass the shooter for the damage calculation.
            useShooterDirectly = false;

            // Sets the starting position automatically.
            if(setStartPosOnStart)
                startPosition = transform.position;

            // If the color should be changed, change it at the start.
            if (changeColor)
                CalculateAndSetColor();
        }

        // Calculates the attack power.
        public override float CalculateAttackPower(bool updateShooter)
        {
            // Gets the base power.
            float basePower = base.CalculateAttackPower(updateShooter);

            // The modified power.
            float modPower = basePower;

            // The distance from start.
            float distFromStart = Vector3.Distance(startPosition, transform.position);

            // If it's at or greater than the maximum distance, set it to the end power.
            if(distFromStart >= attackPowerDistanceMax)
            {
                // Sets as the attack power at the maximum distance.
                modPower = attackPowerAtDistanceMax;
            }
            else
            {
                // Gets the t-value and uses it to lerp the modified power.
                float t = Mathf.Clamp01(distFromStart / attackPowerDistanceMax);
                modPower = Mathf.Lerp(basePower, attackPowerAtDistanceMax, t);
            }

            // Returns the modifiedp ower.
            return modPower;
        }

        // Gets the position at the distance where the projectile's power stops changing.
        // Uses Vector3.right for the calculation.
        public Vector3 GetAttackPowerDistanceMaxPosition()
        {
            return GetAttackPowerDistanceMaxPosition(Vector3.right);
        }

        // Gets the position at the distance where the projectile's power stops changing.
        // The provided direction is used to determine what direction the projecitle is going.
        // The normalized version of the direction is used.
        public Vector3 GetAttackPowerDistanceMaxPosition(Vector3 direc)
        {
            // Start position plus the direction times the attack power distance maximum.
            // Uses Vector3.ClampMagnitude to clamp the result.
            Vector3 maxDistPos = startPosition + 
                Vector3.ClampMagnitude(direc.normalized * attackPowerDistanceMax, attackPowerDistanceMax);
            
            // Returns the result.
            return maxDistPos;
        }

        // Returns 'true' if the projectile changes colour.
        public bool ChangeColor
        {
            get { return changeColor; }
        }

        // The color of the projectile at it's lowest power.
        public Color LowestPowerColor
        {
            get { return lowestPowerColor; }
        }

        // The color of the projectile at it's highest power.
        public Color HighestPowerColor
        {
            get { return highestPowerColor; }
        }

        // Calculates the projectile's color based on its distance travelled.
        // This doesn't SET the color, only calculate what it is.
        public Color CalculateColor()
        {
            // Gets the end position.
            Vector3 endPos = GetAttackPowerDistanceMaxPosition();

            // The values added together.
            int addCount = 0;

            // The t-value to be used.
            float t = 0.0F;

            // If the x-values don't match, factor that in.
            if(startPosition.x != endPos.x)
            {
                t += Mathf.InverseLerp(startPosition.x, endPos.x, transform.position.x);
                addCount++;
            }

            // If the y-values don't match, factor that in.
            if (startPosition.y != endPos.y)
            {
                t += Mathf.InverseLerp(startPosition.y, endPos.y, transform.position.y);
                addCount++;
            }

            // If the z-values don't match, factor that in.
            if (startPosition.z != endPos.z)
            {
                t += Mathf.InverseLerp(startPosition.z, endPos.z, transform.position.z);
                addCount++;
            }

            // Average out the t-values.
            // If only the x-value changed (which is expected), nothing should happen.
            t /= addCount;

            // Clamp the t-value within (0, 1).
            t = Mathf.Clamp01(t);

            // Gets the base attack power at the attack power at the maximum distance.
            float basePower = base.CalculateAttackPower(true);
            float powerAtDistMax = attackPowerAtDistanceMax;

            // The new color.
            Color newColor;

            // If the power at the maximum distance is greater than the base power, it means the power is going up.
            // As such, lerp from lowest power color to highest power color.
            if(powerAtDistMax >= basePower)
            {
                newColor = Color.Lerp(lowestPowerColor, highestPowerColor, t);
            }
            // If the power at distance max is lower than the base power, that menas the power is going down.
            // As such, lerp from the highest power color to the lowest power color.
            else
            {
                newColor = Color.Lerp(highestPowerColor, lowestPowerColor, t);
            }

            // Returns the new color.
            return newColor;
        }

        // Calculates and changes the color.
        public void CalculateAndSetColor()
        {
            Color newColor = CalculateColor();
            spriteRenderer.color = newColor;
        }

        // Sets the color to white. It's assumed that this is the reset color, but it may not be.
        public void SetColorToWhite()
        {
            spriteRenderer.color = Color.white;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if (actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If the colour should be changed, change it.
                if (changeColor)
                {
                    // Calculates and sets the new color.
                    CalculateAndSetColor();
                }
            }
        }
    }
}