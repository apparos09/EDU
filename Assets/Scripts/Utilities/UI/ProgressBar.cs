using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // A script for a progress bar.
    public class ProgressBar : MonoBehaviour
    {
        // The slider bar that's being animated.
        public Slider bar;

        // The bar's fill image.
        public Image fillImage;

        // Checks the bar for the min and max values instead, since that makes more sense.
        // Doing the min and max values this way also prevented the user from changing the min-max values on the bar.

        // // The minimum value.
        // private float minValue = 0.0F;
        // 
        // // The maximum value.
        // private float maxValue = 1.0F;

        // The value for the progress bar.
        // The progress bar script prevents the value from being set by the Slider script.
        private float value = 0.0F;

        // Sets the progress bar's value to the bar's starting value on awake if true.
        [Tooltip("Sets the progress value using the slider's starting value in Awake() if true.")]
        public bool setValueOnAwake = true;

        [Header("Speed")]

        // The scroll speed for the transitions.
        public float speed = 1.0F;

        // If 'true', the bar scrolls at a fixed speed.
        public bool fixedSpeed = false;

        // If 'true', time scaled delta time is used for progress bar speed.
        [Tooltip("If true, timeScaled delta time is used. If false, unscaled deltaTime is used.")]
        public bool useTimeScale = false;

        // The starting value that's used for animation.
        private float startValue = 0.0F;

        [Tooltip("If true, the bar transitions between values. If false, the bar instantly changes when switching values.")]
        // If 'true', the progress bar transitions between values. If false, the bar instantly switches between values.
        public bool useTransition = true;

        // Set to 'true' if the bar is transitioning between values.
        private bool transitioning = false;

        // The value t for interpolation.
        private float v_t = 0.0F;

        // Callbacks
        // A callback for when the progress bar's transition finishes.
        // If there is no transition, the start and end functions are called instantly.
        public delegate void TransitionCallback();

        // Callback for the transition starting.
        private TransitionCallback transitionStartCallback;

        // Callback for the transtion ending.
        private TransitionCallback transitionEndCallback;

        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            // Tries to get the slider component if it isn't set.
            if (bar == null)
                bar = GetComponent<Slider>();

            // If the bar isn't null, do starting functions.
            if(bar != null)
            {
                // Sets the value on awake if enabled.
                // This is done here in case a SetValue() function call happens in the Start() function of another object.
                // If it was, doing this without the transition would stop the effect if the first call in the object's Start()...
                // Said to use the transition.
                if(setValueOnAwake)
                    SetValueAsPercentage(bar.value / bar.maxValue, false);
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Tries to get the slider component a second time if it isn't set.
            // Originally this only happened in Start, but it's done in Awake as well...
            // To make sure the bar matches the slider's starting value if possible.
            if (bar == null)
                bar = GetComponent<Slider>();
        }

        // Getters and setters for the minimum and maximum values.
        // If the setters are used, the SetValue() function is called to adjust the progress bar.

        // The minimum value.
        public float MinValue
        {
            get { return bar.minValue; }

            set
            {
                bar.minValue = value;
                SetValue(this.value);
            }
        }

        // The maximum value.
        public float MaxValue
        {
            get { return bar.maxValue; }

            set
            {
                bar.maxValue = value;
                SetValue(this.value);
            }
        }

        // Returns the value.
        public float GetValue()
        {
            return value;
        }

        // Sets the value for the progess bar, using the member variable to determine...
        // If a transition should be used.
        public void SetValue(float newValue)
        {
            SetValue(newValue, useTransition);
        }

        // Sets the value for the progress bar.
        // The argument 'transition' determines if the new value is transitioned to, or if the change is instant.
        public void SetValue(float newValue, bool transition)
        {
            // If the minimum value is larger than the maximum value they will be swapped.
            // Since the bar min and max values are being used, this may not be necessary...
            // But the check has stil been included regardless.
            if (bar.minValue > bar.maxValue)
            {
                float temp = bar.minValue;
                bar.minValue = bar.maxValue;
                bar.maxValue = temp;
            }

            // Applies the new value.
            startValue = value;
            value = Mathf.Clamp(newValue, bar.minValue, bar.maxValue);

            // Not needed since the bar is used directly now.
            // bar.minValue = minValue;
            // bar.maxValue = maxValue;

            v_t = 0.0F;

            // If there should be a transition.
            if (transition)
            {
                // If currently transitioning, recalculate the current v_t value.
                if (transitioning)
                    v_t = Mathf.InverseLerp(startValue, value, bar.value);

                // Transitioning.
                transitioning = true;

                // Callback(s)
                // Calls OnTransitionStart(). OnTransitionEnd() will be called when the transition finishes.
                OnTransitionStart();
            }
            else
            {
                // Not transitioning.
                transitioning = false;

                // Changes the progress bar visual.
                startValue = value;
                bar.value = value;

                // Callback(s)
                // Calls start and end since there is no transition.
                OnTransitionStart();
                OnTransitionEnd();
            }
        }

        // Returns the value in a 0.0 (0%) to 1.0 (100%) range.
        public float GetValueAsPercentage()
        {
            return Mathf.InverseLerp(bar.minValue, bar.maxValue, value);
        }

        // Sets the value as a percentage, using the member variable 'useTransition' to determine...
        // If a transition should be used.
        public void SetValueAsPercentage(float newValue)
        {
            SetValueAsPercentage(newValue, useTransition);
        }

        // Sets the value as a percentage.
        // Argument 'transition' determines if a transition is used or not.
        public void SetValueAsPercentage(float newValue, bool transition)
        {
            SetValue(newValue * bar.maxValue, transition);
        }

        // Gets the value from the slider UI object.
        public float GetSliderValue()
        {
            return bar.value;
        }

        // Gets the value from the slider UI object as a percentage.
        public float GetSliderValueAsPercentage()
        {
            return Mathf.InverseLerp(bar.minValue, bar.maxValue, bar.value);
        }

        // Checks if the progress bar is transitioning to another vlaue.
        public bool IsTransitioning()
        {
            return transitioning;
        }

        // Adds a callback for the transition starting.
        public void OnTransitionStartAddCallback(TransitionCallback callback)
        {
            transitionStartCallback += callback;
        }

        // Removes a callback for the transition starting.
        public void OnTransitionStartRemoveCallback(TransitionCallback callback)
        {
            transitionStartCallback -= callback;
        }

        // Called when the transition has started.
        // This is still called even if the transition is instant.
        private void OnTransitionStart()
        {
            // Checks if there are functions to call.
            if (transitionStartCallback != null)
                transitionStartCallback();
        }

        // Adds a callback for the transition ending.
        public void OnTransitionEndAddCallback(TransitionCallback callback)
        {
            transitionEndCallback += callback;
        }

        // Removes a callback for the transition ending.
        public void OnTransitionEndRemoveCallback(TransitionCallback callback)
        {
            transitionEndCallback -= callback;
        }

        // Called when the transition has ended.
        // This is still called even if the transition is instant.
        private void OnTransitionEnd()
        {
            // Checks if there are functions to call.
            if (transitionEndCallback != null)
                transitionEndCallback();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // If the start value is not equal to the set value.
            if (transitioning)
            {
                // Increases 't' and clamps it. Variable determines of time scale should be used or not.
                v_t += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime) * speed;
                v_t = Mathf.Clamp01(v_t);

                // Checks if the bar should be moving at a fixed pace.
                if (fixedSpeed) // Fixed speed.
                {
                    // If the start value is less than the destination value then the bar is increasing.
                    if (value > startValue) // Increase
                    {
                        bar.value = Mathf.Lerp(bar.minValue, bar.maxValue, v_t);

                        // If the bar value has reached the desired value then it should stop moving.
                        if (bar.value >= value)
                        {
                            v_t = 1.0F;
                            bar.value = value;
                        }

                    }
                    else // Decrease
                    {
                        bar.value = Mathf.Lerp(bar.maxValue, bar.minValue, v_t);

                        // If the bar value has reached the desired value then it should stop moving.
                        if (bar.value <= value)
                        {
                            v_t = 1.0F;
                            bar.value = value;
                        }

                    }
                }
                else // Not moving at a fixed speed.
                {
                    bar.value = Mathf.Lerp(startValue, value, v_t);
                }

                // If the transition is complete.
                if (v_t >= 1.0F)
                {
                    v_t = 0.0F;
                    transitioning = false;

                    // Callback
                    OnTransitionEnd();
                }

            }
            else
            {
                // if the bar value does not match.
                if (bar.value != value)
                {
                    // Sets the values.
                    // Not needed since the bar is checked for the min and max directly.
                    // bar.minValue = minValue;
                    // bar.maxValue = maxValue;

                    bar.value = value;
                }
            }


        }
    }
}