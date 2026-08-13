using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // Plays audio when clicking a toggle.
    public class ToggleAudio : UIElementAudio
    {
        // The toggle this script is for.
        public Toggle toggle;

        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            // Moved here in case the toggle has not been set enabled before the game was closed.

            // Button not set.
            if (toggle == null)
            {
                // Tries to get the component.
                toggle = GetComponent<Toggle>();
            }

            base.Awake();
        }

        // Add OnValueChanged Delegate
        public override void AddOnValueChanged()
        {
            // If the toggle isn't set, return.
            if (toggle == null)
                return;

            // Listener for the toggle.
            toggle.onValueChanged.AddListener(delegate
            {
                OnValueChanged(toggle.isOn);
            });
        }

        // Remove OnValueChanged Delegate
        public override void RemoveOnValueChanged()
        {
            // If the toggle isn't set, return.
            if (toggle == null)
                return;

            // Remove the listener for onValueChanged if the toggle has been set.
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnValueChanged);
            }
        }


        // Called when the toggle is clicked.
        protected virtual void OnValueChanged(bool isOn)
        {
            if (IsPlaySafe())
                audioSource.PlayOneShot(audioClip);
        }
    }
}