using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Attach to an interactable object in the UI and call the SpeakText() function.
    public class SpeakTextUI : MonoBehaviour
    {
        // Speaks text using the provided key.
        public void SpeakText(string key)
        {
            // Checks if the instances exist and if the key is set.
            if (GameSettings.Instantiated && SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized() && key != "")
            {
                // Gets the instances.
                GameSettings gameSettings = GameSettings.Instance;
                SystemManager lolManager = SystemManager.Instance;

                // Checks if TTS should be used.
                if (gameSettings.UseTextToSpeech)
                {
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }
    }
}
