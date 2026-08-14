using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // Speaks text on enable.
    public class SpeakTextOnEnable : MonoBehaviour
    {
        // The speak key to be used.
        public string key = "";

        // If 'true', the text is spoken in the start function.
        public bool speakTextOnStart = true;

        // Gets set to 'true' when the start function has been cleared.
        private bool startCleared = false;

        // Start is called before the first frame update
        void Start()
        {
            // If the text should be spoken on start.
            if (speakTextOnStart)
                SpeakText();

            // The start function has been cleared.
            startCleared = true;
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            // Start has been cleared, so speak the text.
            if(startCleared)
            {
                SpeakText();
            }
        }

        // Returns 'true' if the start function has been cleared.
        public bool StartCleared
        {
            get { return startCleared; }
        }

        // Speaks text using the provided key.
        public void SpeakText()
        {
            // Checks if the instances exist: LOL SDK, Text-to-Speech, and GameSettings.
            // Also checks that the key is set.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaded() && TextToSpeech.Instantiated && 
                GameSettings.Instantiated && key != "")
            {
                // Gets the instances.
                GameSettings gameSettings = GameSettings.Instance;
                SystemManager lolManager = SystemManager.Instance;

                // Checks if TTS should be used.
                if (gameSettings.UseTextToSpeech)
                {
                    // Grabs the LOL Manager to trigger text-to-speech.
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }
    }
}