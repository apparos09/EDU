using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Formally the 'LOL' manager.
    public class SystemManager : MonoBehaviour
    {
        // The instance of the class.
        private static SystemManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Language definition for translation (LOL only).
        //private JSONNode defs;

        // Loads in a language to be used in the game.
        public LanguageManager languageLoader;
        
        // The save system for the game.
        public SaveSystem saveSystem;
        
        // The text-to-speech object.
        public TextToSpeech textToSpeech;

        // The maximum progress points for the game.
        const int PROGRESS_MAX = WorldManager.STAGE_COUNT; // Same as the stage count.

        // private constructor so that only one accessibility object exists.
        private SystemManager()
        {
            // ...
        }

        // Awake is called when the script instance is being loaded
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

                // This object should not be destroyed.
                DontDestroyOnLoad(gameObject);

                // The LOLSDK version is the one you use.
                // It is automatically being used already, but I wanted to make a note of this...
                // Since you didn't realize you had to do it this way at the time.
                // LOLSDK.DontDestroyOnLoad(this);

                // Gets the instance if it's not set.
                if (saveSystem == null)
                    saveSystem = SaveSystem.Instance;

                // Gets the instance if it's not set.
                if (textToSpeech == null)
                    textToSpeech = TextToSpeech.Instance;
            }
       
        }

        // Start is called before the first frame update
        void Start()
        {
            //// If defs is not set, try to set it.
            //if(defs == null)
            //    defs = SharedState.LanguageDefs;

            // If the language loader isn't set, try to set it. If failed, generate a new component.
            if(languageLoader == null)
            {
                // If the language loader couldn't be found, add the component.
                if(!TryGetComponent<LanguageManager>(out languageLoader))
                {
                    languageLoader = gameObject.AddComponent<LanguageManager>();
                }
            }

            // If the save system is null but it has been instantiated, get the instance.
            if (saveSystem == null && SaveSystem.Instantiated)
                saveSystem = SaveSystem.Instance;

            // If the text-to-speech is null but it has been instantiated, get the instance.
            if (textToSpeech == null && TextToSpeech.Instantiated)
                textToSpeech = TextToSpeech.Instance;
        }

        // Returns the instance of the accessibility.
        public static SystemManager Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("LOL Manager (singleton)");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<SystemManager>();
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

        // Returns 'true' if the LOLSDK is initialized.
        public static bool IsLanguageLoaderInitialized()
        {
            // return LanguageManager.Instantiated;
            return false;
        }

        // Checks if the LOL manager is instantiated, and if the LOL SDK is initialized.
        public static bool IsInstantiatedAndIsLanguageLoaderInitialized()
        {
            return Instantiated && IsLanguageLoaderInitialized();
        }

        // NOTE: this function could be static, but the game shouldn't be operating if the LOLSDK and LOLManager...
        // Aren't both instantiated. As such, this is kept as a non-static function.
        // Gets the text from the language file.
        public string GetLanguageText(string key)
        {
            //// Gets the language definitions.
            //if(defs == null)
            //    defs = SharedState.LanguageDefs;

            //// Returns the text.
            //if (defs != null)
            //    return defs[key];
            //else
            //    return "";

            // The result to be returned.
            string result;

            // If the language loader is set, 
            if(languageLoader != null)
            {
                result = languageLoader.HasLanguageText() ? languageLoader.GetLanguageText(key) : string.Empty;
            }
            else
            {
                result = "";
            }

            return result;  
        }

        // Gets the language text. Static function version.
        public static string GetLanguageTextStatic(string key)
        {
            //// Gets the language defs.
            //JSONNode languageDefs = SharedState.LanguageDefs;

            //// Returns the text.
            //if (languageDefs != null)
            //    return languageDefs[key];
            //else
            //    return "";

            // Checks for instantiation
            if(Instantiated)
            {
                return Instance.GetLanguageText(key);
            }
            else
            {
                return "";
            }
        }

        // Returns true if text-to-speech is usable.
        public static bool IsTextToSpeechUsable()
        {
            // The LOL SDK must be initialized and text-to-speech must be instantiated.
            return IsLanguageLoaderInitialized() && TextToSpeech.Instantiated;
        }

        // Returns 'true' if text-to-speech is enabled.
        public static bool IsTextToSpeechEnabled()
        {
            // Checks game settings to see if text-to-speech is enabled.
            if (GameSettings.Instantiated)
                return GameSettings.Instance.UseTextToSpeech;
            else
                return false;
        }

        // Returns 'true' if text-to-speech is usable and enabled.
        public static bool IsTextToSpeechUsableAndEnabled()
        {
            return IsTextToSpeechUsable() && IsTextToSpeechEnabled();
        }

        // Speaks the text.
        public void SpeakText(string key)
        {
            textToSpeech.SpeakText(key);
        }   
        

        // Submits progress for the game.
        // The value overrides the last progress value submitted, and must not go over the max.
        // NOTE: the value will be REPLACED, not added to.
        public void SubmitProgress(int score, int currentProgress)
        {
            //// SDK not initialized.
            //if(!LanguageManager.Instantiated)
            //{
            //    Debug.LogWarning("The SDK is not initialized. No data was submitted.");
            //    return;
            //}

            //// Clamps the current progress.
            //currentProgress = Mathf.Clamp(currentProgress, 0, PROGRESS_MAX);

            //// Submit the progress.
            //LOLSDK.Instance.SubmitProgress(score, currentProgress, PROGRESS_MAX);

            // NOTE: this function is a holdover from the original. It no longer does anything.
        }

        // Submits progress to show that the game is complete.
        public void SubmitProgressComplete(int score)
        {
            // Submits the final score.
           SubmitProgress(score, PROGRESS_MAX);
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

