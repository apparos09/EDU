using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // Adds audio to a slider.
    public class SliderAudio : UIElementAudio
    {
        // The slider this script is for.
        public Slider slider;

        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            // Moved here in case the slider has not been set enabled before the game was closed.

            // Button not set.
            if (slider == null)
            {
                // Tries to get the component.
                slider = GetComponent<Slider>();
            }

            base.Awake();
        }

        // Add OnValueChanged Delegate
        public override void AddOnValueChanged()
        {
            // If the slider isn't set, return.
            if (slider == null)
                return;

            // Listener for the tutorial toggle.
            slider.onValueChanged.AddListener(delegate
            {
                OnValueChanged(slider.value);
            });
        }

        // Remove OnValueChanged Delegate
        public override void RemoveOnValueChanged()
        {
            // If the slider isn't set, return.
            if (slider == null)
                return;

            // Remove the listener for onValueChanged if the slider has been set.
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(OnValueChanged);
            }
        }


        // Called when the slider has been changed.
        private void OnValueChanged(float value)
        {
            if (IsPlaySafe())
                audioSource.PlayOneShot(audioClip);
        }

    }
}