using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // The audio for clicking a button.
    public class ButtonAudio : UIElementAudio
    {
        // The button this script is for.
        public Button button;

        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            // Moved here in case the button has not been set enabled before the game was closed.

            // Button not set.
            if (button == null)
            {
                // Tries to get the button.
                button = gameObject.GetComponent<Button>();
            }

            base.Awake();
        }

        // Add OnValueChanged Delegate
        public override void AddOnValueChanged()
        {
            AddOnClick();
        }

        // Add OnClick Delegate
        public void AddOnClick()
        {
            // If the button has been set.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    OnClick();
                });
            }
        }

        // Remove OnValueChanged Delegate
        public override void RemoveOnValueChanged()
        {
            RemoveOnClick();
        }

        // Remove OnClick Delegate
        public void RemoveOnClick()
        {
            // Remove the listener for onClick if the button has been set.
            if (button != null)
            {
                button.onClick.RemoveListener(OnClick);
            }
        }

        // Called when the button is clicked.
        private void OnClick()
        {
            // If both the audio source and the audio clip are set, play the audio.
            if (IsPlaySafe())
                audioSource.PlayOneShot(audioClip);
        }

    }
}