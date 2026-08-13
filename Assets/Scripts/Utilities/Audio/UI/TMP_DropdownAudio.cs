using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace util
{
    // Adds audio to a TMP dropdown. This code is copied from DropdownAudio, but with the regular dropdown being replaced with the TMP dropdown.
    // This is inefficient, so find a better way to optimize this, maybe.
    public class TMP_DropdownAudio : UIElementAudio
    {
        // The dropdown this script is for.
        public TMP_Dropdown dropdown;

        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            // Moved here in case the dropdown has not been set enabled before the game was closed.

            // Button not set.
            if (dropdown == null)
            {
                // Tries to get the component.
                dropdown = GetComponent<TMP_Dropdown>();
            }

            base.Awake();
        }

        // Add OnValueChanged Delegate
        public override void AddOnValueChanged()
        {
            // If the dropdown isn't set, return.
            if (dropdown == null)
                return;

            // Listener for the tutorial toggle.
            dropdown.onValueChanged.AddListener(delegate
            {
                OnValueChanged(dropdown.value);
            });
        }

        // Remove OnValueChanged Delegate
        public override void RemoveOnValueChanged()
        {
            // If the dropdown isn't set, return.
            if (dropdown == null)
                return;

            // Remove the listener for onValueChanged if the dropdown has been set.
            if (dropdown != null)
            {
                dropdown.onValueChanged.RemoveListener(OnValueChanged);
            }
        }


        // Called when the dropdown has been changed.
        private void OnValueChanged(int value)
        {
            if (IsPlaySafe())
                audioSource.PlayOneShot(audioClip);
        }

    }
}