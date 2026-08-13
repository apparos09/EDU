using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // Action Unit Generator - Wave
    public class ActionUnitGeneratorWave : ActionUnitGenerator
    {
        // The strength of the waves, which effects the wave heights and speed.
        public enum waveStrength { none, veryWeak, weak, medium, strong, veryStrong }

        [Header("Generator/Wave")]

        // The current wave strength.
        public waveStrength currWaveStrength = waveStrength.none;

        // The floating object script for this wave.
        public ObjectFloat objectFloat = null;

        // If 'true', floating is enabled.
        private bool floatingEnabled = true;

        // The highest position on the y-axis (min, max).
        // HIGHEST_Y_MAX is 0.16 to stay within the tile bounds.
        public const float OBJECT_FLOAT_HIGHEST_Y_MIN = 0.08F;
        public const float OBJECT_FLOAT_HIGHEST_Y_MAX = 0.12F; // At or Below +0.16F
        
        // The lowest position on the y-axis (min, max).
        // LOWEST_Y_MIN is -0.24 to stay within the tile bounds.
        public const float OBJECT_FLOAT_LOWEST_Y_MIN = -0.20F; // At or Below -0.24F
        public const float OBJECT_FLOAT_LOWEST_Y_MAX = -0.16F;

        // Changes the float highest and lowest positions if true.
        private bool changeFloatHighLowPos = true;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to wave.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.wave;
            }

            // Uses the wind to generate energy.
            if (!useWindToGenEnergy)
                useWindToGenEnergy = true;

            // Updates the object floating values.
            UpdateObjectFloatValues(currWaveStrength);
        }

        // Converts the provided stat rating to a wave strength object.
        public static waveStrength ConvertStatRatingToWaveStrength(statRating rating)
        {
            // The value to return.
            waveStrength value;

            // Checks what value to use for the rating.
            switch (rating)
            {
                default:
                case statRating.unknown:
                case statRating.noneMinus:
                case statRating.none:
                    value = waveStrength.none;
                    break;

                case statRating.veryLow:
                    value = waveStrength.veryWeak;
                    break;

                case statRating.low:
                    value = waveStrength.weak;
                    break;

                case statRating.medium:
                    value = waveStrength.medium;
                    break;

                case statRating.high:
                    value = waveStrength.strong;
                    break;

                case statRating.veryHigh:
                case statRating.maximum:
                case statRating.maximumPlus:
                    value = waveStrength.veryStrong;
                    break;
            }

            return value;
        }

        // Sets the current wave strength.
        public void SetCurrentWaveStrength(waveStrength waveStrength)
        {
            // Override current wave strength.
            currWaveStrength = waveStrength;

            // If object floating is being used, update the values.
            if (floatingEnabled)
            {
                UpdateObjectFloatValues(currWaveStrength);
            }
        }

        // Updates the object float script using the provided wave strength.
        public void UpdateObjectFloatValues(waveStrength waveStrength)
        {
            // If the floating is set to none, disable floating.
            // If false, enable floating.
            objectFloat.floatEnabled = waveStrength != waveStrength.none;

            // The t-value used for lerping between the max and minimum values...
            // For the highest and lowest floating positions.
            float heightT;

            // The speed of the floating.
            float speed;

            // Checks the wave strength to see what values to set for object float.
            switch (waveStrength)
            {
                default:
                case waveStrength.none:
                    // Floating will be disabled, so set the height and speed to default values.
                    objectFloat.SetObjectToResetPosition(true);
                    heightT = 1.0F;
                    speed = 1.0F;
                    break;

                case waveStrength.veryWeak:
                    heightT = 0.0F;
                    speed = 0.5F;
                    break;

                case waveStrength.weak:
                    heightT = 0.25F;
                    speed = 0.625F;
                    break;

                case waveStrength.medium:
                    heightT = 0.5F;
                    speed = 0.75F;
                    break;

                case waveStrength.strong:
                    heightT = 0.75F;
                    speed = 0.875F;
                    break;

                case waveStrength.veryStrong:
                    heightT = 1.0F;
                    speed = 1.0F;
                    break;
            }

            // Gets the high and low y-values.
            // The maximum value is 1.0 for the highest y-position, and the minimum value is...
            // 1.0 for the lowest y-position.
            // They're inverted to reflect the position and negative directions.
            float highY = Mathf.Lerp(OBJECT_FLOAT_HIGHEST_Y_MIN, OBJECT_FLOAT_HIGHEST_Y_MAX, heightT);
            float lowY = Mathf.Lerp(OBJECT_FLOAT_LOWEST_Y_MAX, OBJECT_FLOAT_LOWEST_Y_MIN, heightT);

            // Sets the low and high positions.
            // Positions should be effected.
            if(changeFloatHighLowPos)
            {
                objectFloat.lowPosition.y = lowY;
                objectFloat.highPosition.y = highY;
            }
            // Position shouldn't be effected.
            else
            {
                objectFloat.lowPosition.y = OBJECT_FLOAT_LOWEST_Y_MIN;
                objectFloat.highPosition.y = OBJECT_FLOAT_HIGHEST_Y_MAX;
            }

            // Sets the float speed.
            objectFloat.speed = speed;
        }

        // Updates the object float script using the current wave strength.
        public void UpdateObjectFloatValues()
        {
            UpdateObjectFloatValues(currWaveStrength);
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        protected override void Update()
        {
            base.Update();

            // Gets the current wind rating (don't recalculate) and converts it to the spin speed.
            statRating windRating = actionManager.GetCurrentWindRating(false);
            waveStrength ratingStrength = ConvertStatRatingToWaveStrength(windRating);

            // If the converted wave strength doesn't match the current wave rating, change it.
            if (currWaveStrength != ratingStrength)
            {
                // Set the current wave strength.
                SetCurrentWaveStrength(ratingStrength);
            }
        }

    }
}