using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Causes an object to float in place by interpolating its position.
    public class ObjectFloat : MonoBehaviour
    {
        // If 'true', the local position is used (recommended). If false, the regular (world) position is used.
        [Tooltip("If true, transform.localPosition is used by this class (recommended). If false, transform.position is used.")]
        public bool useLocalPosition = true;

        // The reset position of the object float.
        [Tooltip("The reset position of the object.")]
        public Vector3 resetPosition;

        // Automatically sets the reset position of the object.
        [Tooltip("If true, the reset position is automatically set in Start().")]
        public bool autoSetResetPosition = true;

        // The high position of the object.
        [Tooltip("The highest position the object reaches.")]
        public Vector3 highPosition;

        // The low position of the object.
        [Tooltip("The lowest position the object reaches.")]
        public Vector3 lowPosition;

        // If set to 'true', the high and low positions are automatically set.
        [Tooltip("Automatically sets the highest and lowest positions if true.")]
        public bool autoSetHighLowPos = true;

        // The base position offset. This is only used if the low and high positions are automatically set.
        [Tooltip("The base position offset, which is used to set the high and low positions automatically if said option is enabled.")]
        public float basePosOffset = 1.0F;

        // The speed of the animation.
        [Tooltip("The speed of the animation.")]
        public float speed = 1.0F;

        // If true, scaled delta time is used. If false, unscaled delta time is used.
        [Tooltip("Uses scaled delta time if true, unscaled delta time if false.")]
        public bool useScaledDeltaTime = true;

        // If set to 'true', floating is enabled.
        [Tooltip("If true, floating is enabled")]
        public bool floatEnabled = true;

        // The start point for the object float.
        private Vector3 startPosition;

        // The end point of the object float.
        private Vector3 endPosition;

        // The time value used for the interpolation.
        private float time = 0.0F;

        // Determines if the object is working towards the high point or the low point.
        private bool onHigh = true;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the local position as the reset position.
            if (autoSetResetPosition)
            {
                // Determines what position to use.
                if(useLocalPosition) // Local
                {
                    resetPosition = transform.localPosition;
                }
                else // World
                {
                    resetPosition = transform.position;
                }
            }

            // If the low and high positions should be automatically set.
            if (autoSetHighLowPos)
            {
                // Determines if the local position should be used or the world position.
                if(useLocalPosition)
                {
                    lowPosition = gameObject.transform.localPosition - new Vector3(0.0F, basePosOffset, 0.0F);
                    highPosition = gameObject.transform.localPosition + new Vector3(0.0F, basePosOffset, 0.0F);
                }
                else
                {
                    lowPosition = gameObject.transform.position - new Vector3(0.0F, basePosOffset, 0.0F);
                    highPosition = gameObject.transform.position + new Vector3(0.0F, basePosOffset, 0.0F);
                }    
            }

            // Resets the process to start it.
            ResetProcess();
        }

        // Eases in and out of the provided positions.
        public Vector3 EaseInOutLerp(Vector3 start, Vector3 end, float t)
        {
            // ease in-out calculation
            float newT = (t < 0.5F) ? 2 * Mathf.Pow(t, 2) : -2 * Mathf.Pow(t, 2) + 4 * t - 1;

            // Use the lerp equation.
            Vector3 result = Vector3.Lerp(start, end, newT);

            // Return the result.
            return result;
        }

        // Starting values.
        public void ResetProcess()
        {
            // The start position is the low point.
            startPosition = lowPosition;

            // End point is the high point.
            endPosition = highPosition;

            // Calculates the rough T
            {
                // The current position of the object.
                Vector3 currPos = transform.position;

                // Calculates the t-value for all three location values.
                // If the start and end are the same, the value is left at 0. 
                float xT = 0, yT = 0, zT = 0;

                float sumT = 0.0F;
                int added = 0;

                // X
                if (highPosition.x != lowPosition.x)
                {
                    xT = Mathf.InverseLerp(lowPosition.x, highPosition.x, currPos.x);
                    sumT += xT;
                    added++;
                }

                // Y
                if (highPosition.y != lowPosition.y)
                {
                    yT = Mathf.InverseLerp(lowPosition.y, highPosition.y, currPos.y);
                    sumT += yT;
                    added++;
                }

                // Z
                if (highPosition.z != lowPosition.z)
                {
                    zT = Mathf.InverseLerp(lowPosition.z, highPosition.z, currPos.z);
                    sumT += zT;
                    added++;
                }

                // Calculates the final t.
                if (added != 0) // Average out the values.
                    sumT /= added;
                else // Set it to half.
                    sumT = 0.5F;

                // Set the value.
                time = sumT;
            }

            // Set the transformation's position from the start.
            if(useLocalPosition) // Local
            {
                transform.localPosition = EaseInOutLerp(startPosition, endPosition, time);
            }
            else // World
            {
                transform.position = EaseInOutLerp(startPosition, endPosition, time);
            }
                

            // The object is going towards the high point.
            onHigh = true;
        }

        // Sets the object to the reset position.
        public void SetObjectToResetPosition()
        {
            // Sets the object to its reset position, which also resets the process.
            SetObjectToResetPosition(true);   
        }

        // Sets the object to its reset position.
        // resetProcess: if true, the process is also reset.
        public void SetObjectToResetPosition(bool resetProcess)
        {
            // Determines what position to reset.
            if (useLocalPosition)
            {
                transform.localPosition = resetPosition;
            }
            else
            {
                transform.position = resetPosition;
            }

            // If the process should be reset.
            if(resetProcess)
            {
                ResetProcess();
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If floating is enabled.
            if (floatEnabled)
            {
                // Checks if using scaled or unscaled delta time.
                float dt = useScaledDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;

                // Increments the timer.
                time += dt * speed;

                // Clamps the time value within 0-1 bounds.
                time = Mathf.Clamp01(time);

                // Change the object's position.
                if(useLocalPosition) // Local
                {
                    transform.localPosition = EaseInOutLerp(startPosition, endPosition, time);
                }
                else // World
                {
                    transform.position = EaseInOutLerp(startPosition, endPosition, time);
                }
                    

                // If the end of the line has been reached.
                if (time >= 1.0F)
                {
                    // Checks if the object is going towards the high point or low point.
                    if (onHigh) // At the high point, so now you're going to the low point.
                    {
                        onHigh = false;
                        startPosition = highPosition;
                        endPosition = lowPosition;
                    }
                    else// At the low point, so now you're going to the low point.
                    {
                        onHigh = true;
                        startPosition = lowPosition;
                        endPosition = highPosition;
                    }

                    // Reset the time.
                    time = 0.0F;
                }
            }

        }
    }
}
