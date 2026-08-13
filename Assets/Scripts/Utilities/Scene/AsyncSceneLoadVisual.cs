using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // the asynchronous scene loader visual
    public class AsyncSceneLoadVisual : MonoBehaviour
    {
        // Scene loader.
        public Slider slider;

        // Load operation.
        public AsyncSceneLoader loader;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Returns 'true' if the slider is at its max value.
        public bool IsSliderAtMaxValue()
        {
            // Checks if the slider exists for the value to be checked.
            if(slider != null)
            {
                return slider.value == slider.maxValue;
            }
            else
            {
                return false;
            }
        }

        // Called when the slider has reached its max value.
        public virtual void OnSliderReachedMaxValue()
        {
            // ...
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // Checks if the loader is loading.
            if (loader.IsLoading)
            {
                // Changes the slider.
                if(slider != null)
                {
                    slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, loader.GetProgressLoading());
                    
                    // If the slider is at max, call the appropriate function.
                    if(IsSliderAtMaxValue())
                    {
                        OnSliderReachedMaxValue();
                    }
                }
            }
        }
    }
}