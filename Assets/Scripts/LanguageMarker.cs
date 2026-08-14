using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // A marker used to mark text that is not loaded from the language file.
    public class LanguageMarker : MonoBehaviour
    {
        // the instance of the language marker.
        private static LanguageMarker instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The color used for marking text that wasn't repalced with language file content. 
        [HideInInspector]
        public Color noLoadColor = Color.red;

        // If the text colour should be changed.
        // TODO: set this to 'false' when creating the promo build.
        // This should really have been a regular variable that could be edited.
        public const bool CHANGE_TEXT_COLOR = GameSettings.IS_MULTI_LANGUAGE;

        // The constructor
        private LanguageMarker()
        {
            // ...
        }

        // Awake is called when the script is loaded.
        private void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Returns the instance of the language marker.
        public static LanguageMarker Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("Language Marker (singleton)");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<LanguageMarker>();
                }

                // returns the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Marks the provided text object.
        public void MarkText(Text text)
        {
            // Added this to avoid triggering an unreachable code warning.
            bool changeColor = CHANGE_TEXT_COLOR;

            // If the text color should be changed.
            if (changeColor)
                text.color = noLoadColor;
        }

        // Marks the provided TMP text object.
        public void MarkText(TMP_Text text)
        {
            // Added this to avoid triggering an unreachable code warning.
            bool changeColor = CHANGE_TEXT_COLOR;

            // If the text color should be changed.
            if (changeColor)
                text.color = noLoadColor;
        }

        // Translates the text using the provided key.
        // If the language file isn't loaded, then the text is marked using the noLoad colour.
        public bool TranslateText(Text text, string key, bool markIfFailed)
        {
            // Checks if the SDK has been initialized. 
            if (SystemManager.Instantiated)
            {
                text.text = SystemManager.Instance.GetLanguageText(key);
                return true;
            }
            else
            {

                // If the text should be marked if failed.
                if (markIfFailed)
                    MarkText(text);

                return false;
            }

        }

        // Translates the TMP text using the provided key.
        // If the language file isn't loaded, then the text is marked using the noLoad colour.
        public bool TranslateText(TMP_Text text, string key, bool markIfFailed)
        {
            // Checks if the SDK has been initialized. 
            if(SystemManager.Instantiated)
            {
                text.text = SystemManager.Instance.GetLanguageText(key);
                return true;
            }
            else
            {
                // // Prints an error message if the translation failed.
                // if(Application.isEditor)
                // {
                //     Debug.LogError("The LOL SDK has not been initialized. Translation failed.");
                // }

                // If the text should be marked if failed.
                if (markIfFailed)
                    MarkText(text);

                return false;
            }

        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}