using System.Runtime.InteropServices;
using UnityEngine;

namespace util
{
    /*
     * Sources:
     * https://www.youtube.com/watch?v=BQGTdRhGmE4
     * https://discussions.unity.com/t/screen-shake-effect/391783/4
     */

    // Shakes an object, which is primarily used for camera shake.
    public class ObjectShake : MonoBehaviour
    {
        // The maximum distance the object can move from its base position when shaking.
        [Tooltip("The maximum distance the object can move from its base position.")]
        public float distanceMax = 1.0F;

        // The object's reset lcoal position. This is the position the object takes when the shake ends.
        [Tooltip("The reset local position of the object. The object takes this position when the shake ends.")]
        public Vector3 resetLocalPosition = Vector3.zero;

        // Sets the object's local position on awake if true. If the object is already shaking, this is ignored.
        [Tooltip("Sets the object's reset local position on awake if true. If the object is already shaking, this is ignored.")]
        public bool setLocalResetPosOnAwake = true;

        // If 'true', the object constantly shakes. The timer is not updated.
        [Tooltip("Constantly shakes the object if true. When enabled, the shake timer doesn't go down.")]
        public bool constantShaking = false;

        // Gets set to 'true' if object was shoken the last frame.
        private bool updatedShaking = false;

        // If 'true', shaking is enabled. WHen shaking is disabled, no check are done.
        [Tooltip("If true, shaking is enabled. If false, shaking is disabled, meaning no checks are done.")]
        public bool shakeEnabled = true;

        [Header("Timer")]

        // The shake duration.
        [Tooltip("The shake duration of the object.")]
        public float duration = 1.0F;

        // The timer used for shaking. When the timer runs out, the object stops shaking.
        [Tooltip("The timer for how long the shake occurs for.")]
        public float shakeTimer = 0.0F;

        // If 'true;, the object uses scaled delta time. If false, the object uses unscaled delta time.
        [Tooltip("Uses scaled delta time if true, unscaled delta time if false.")]
        public bool useScaledDeltaTime = true;

        // If 'true', the time scale is ignored, meaning that the shake still happens if time scale is 0.
        // If 'false', the time scale is referenced, meaning that shakes will not occur if the time scale is 0.
        [Tooltip("If 'false', the shaking stops if the time scale is 0.")]
        public bool ignoreTimeScale = false;

        [Header("Axes")]

        // If 'true', the object does not move the given axis.
        public bool freezeX = false;
        public bool freezeY = false;
        public bool freezeZ = false;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // If the object isn't shaking, automatically set the reset position.
            if (setLocalResetPosOnAwake && !IsShakingScaled())
            {
                resetLocalPosition = transform.localPosition;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Returns 'true' if the object is shaking. This is determined by seeing if the shake timer is greater than 0.
        public bool IsShaking()
        {
            // If the shake timer is greater than 0, then  the object is shaking.
            // If there is constant shaking, always return true.
            return shakeTimer > 0 || constantShaking;
        }

        // Returns 'true' if the object is shaking when the time scale is applied.
        // If the time scale is ignored, this returns the same value as 'IsShaking'
        public bool IsShakingScaled()
        {
            // The result to be returned.
            bool result;

            // Checks if the time scale should be ignored.
            if(ignoreTimeScale)
            {
                result = IsShaking();
            }
            else
            {
                result = IsShaking() && Time.timeScale != 0.0F;
            }

            return result;
        }

        // Shakes the object.
        public void StartShaking()
        {
            shakeTimer = duration;
            resetLocalPosition = transform.localPosition;
        }

        // Sets the duration and shakes the object.
        public void StartShaking(float newDuration)
        {
            duration = newDuration;
            StartShaking();
        }

        // Stops the object from shaking.
        public void StopShaking()
        {
            shakeTimer = 0.0F;
            transform.localPosition = resetLocalPosition;

            // The shake updates have stopped.
            updatedShaking = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks if shaking is enabled.
            if(shakeEnabled)
            {
                //  If the shake should be updated.
                if (IsShakingScaled())
                {
                    // Reduce the timer if there is not constant shaking.
                    if (!constantShaking)
                    {
                        shakeTimer -= (useScaledDeltaTime) ? Time.deltaTime : Time.unscaledDeltaTime;
                    }

                    // There should be shaking.
                    if (shakeTimer > 0.0F || constantShaking)
                    {
                        // Change the local position by offsetting from the reset position.
                        Vector3 newLocalPos = resetLocalPosition + Random.insideUnitSphere * distanceMax;

                        // Checks what axes to kepe and which ones to ignore.
                        newLocalPos.x = freezeX ? resetLocalPosition.x : newLocalPos.x;
                        newLocalPos.y = freezeY ? resetLocalPosition.y : newLocalPos.y;
                        newLocalPos.z = freezeZ ? resetLocalPosition.z : newLocalPos.z;

                        // Set the new local position.
                        transform.localPosition = newLocalPos;

                        // The object has been shoken.
                        updatedShaking = true;
                    }
                    // Reset the position.
                    else
                    {
                        // Stops the shaking.
                        StopShaking();
                    }
                }
                else
                {
                    // If the shaking was updated last time, and was expected to update again...
                    // Stop the shaking.
                    // This is here to account for the constant shake parameter.
                    if (updatedShaking)
                    {
                        StopShaking();
                    }
                }
            }
        }
    }
}