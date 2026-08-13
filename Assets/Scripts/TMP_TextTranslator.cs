using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The text translator for TMP objects.
    // Translates a TMP_Text object's text using the provided key.
    public class TMP_TextTranslator : MonoBehaviour
    {
        // The text object.
        public TMP_Text text;

        // The translation key.
        public string key = "";

        // Marks text if the language file is not loaded.
        public static bool markIfFailed = true;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the text mesh pro component from the object this script is attached to.
            if (text == null)
                text = GetComponent<TMP_Text>();

            // Gets set if the text should be marked for a failed translation.
            bool markText;

            // If the SDK is initialized, the text is set, and the key is set.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized() && text != null && key != "")
            {
                text.text = SystemManager.Instance.GetLanguageText(key);

                // Text is blank, so mark the text to show that the translation failed.
                if(text.text == "")
                {
                    markText = true;
                }
                else
                {
                    markText = false;
                }
            }
            else
            {
                // Mark the text.
                markText = true;
            }

            // If the text should be marked, and text marking is enabled.
            if(markText && markIfFailed)
            {
                LanguageMarker.Instance.MarkText(text);
            }
        }

        // Speaks out the provided text.
        public void SpeakText()
        {
            // Checks if the TTS is set up, if the TTS is active, and if the key string exists.
            if (SystemManager.Instantiated && GameSettings.Instance.UseTextToSpeech && key != string.Empty)
            {
                // Read out the text.
                SystemManager.Instance.textToSpeech.SpeakText(key);
            }
        }
    }
}