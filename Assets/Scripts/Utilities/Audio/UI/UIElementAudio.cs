using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The user interface audio.
    public abstract class UIElementAudio : MonoBehaviour
    {
        // THe audio for the user inputs.
        public AudioSource audioSource;

        // The audio clip for the toggle.
        public AudioClip audioClip;

        // Gets set to 'true' when the audio is ready to be used.
        // This is used to make sure audio doens't play on the first frame.
        private bool audioReady = false;

        // Gets set to 'true' when late start has been called.
        protected bool calledLateStart = false;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            // Add to the onValueChanged function.
            AddOnValueChanged();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Called on the first update frame of this object.
        protected virtual void LateStart()
        {
            // Late start has been called.
            calledLateStart = true;

            // The audio is ready to be used.
            audioReady = true;
        }

        // Add OnValueChanged Delegate
        public abstract void AddOnValueChanged();

        // Remove OnValueChanged Delegate
        public abstract void RemoveOnValueChanged();

        // Returns 'true' if it is safe to play the audio.
        // This is false if the audio source or the clip are not set.
        // It also checks audio ready to make sure audio doesn't play on the first frame (this happens with sliders).
        public virtual bool IsPlaySafe()
        {
            // Result to be returned.
            bool result;

            // Checks if the provided elements are set to play the audio.
            result = audioSource != null && audioClip != null && audioReady;

            // Returns the result.
            return result;
        }

        // Returns 'true' if the audio can play. If the audio object is unusable, this returns false.
        public virtual bool IsPlayable()
        {
            // First checks if playing is safe.
            bool result = IsPlaySafe();

            // Safe to play.
            if(result)
            {
                // Checks if the audio source is active and enabled.
                result = audioSource.isActiveAndEnabled;
            }

            return result;
        }


        // NOTE: this can vary from element to element, so it cannot be inherited.
        // Called when the UI element is triggered.
        // protected abstract void OnValueChanged();

        // Update is called once per frame
        protected virtual void Update()
        {
            // Call late start.
            if(!calledLateStart)
            {
                LateStart();
            }
        }

        // Script is destroyed.
        protected void OnDestroy()
        {
            RemoveOnValueChanged();
        }
    }
}