using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The language manager, which saves information on a given language.
    public class LanguageManager : MonoBehaviour
    {
        // The languages supported by the game.
        public enum language { none, english }

        // The instance of the language marker.
        private static LanguageManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The file reader for the language manager.
        public util.FileReaderLines fileReader;

        // The file path for language files.
        const string LANGUAGE_FILES_PATH = "Assets\\Resources\\Data\\Languages\\";

        // The language text.
        // The 'key' is used to determine the line set to said identifier.
        private Dictionary<string, string> langText = new Dictionary<string, string>();

        // The default langage for the language manager.
        // If set to load langauge on start, this is the language that's loaded.
        [Tooltip("The default language, which will be loaded in Start() if set to do so.")]
        public language defaultLanguage = language.english;

        // The language the game is set to.
        // This is also the default language.
        private language loadedLanguage = language.english;

        // If set to 'true', the text is translated.
        // This is set to 'false', since the init class should determine if a language should be loaded.
        public bool loadLangaugeOnStart = false;

        // The constructor
        private LanguageManager()
        {
            // ...
        }

        // Awake is called when the script is loaded.
        private void Awake()
        {
            // Instance saving.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;

                // File reader not set.
                if (fileReader == null)
                {
                    // Tries to get the component.
                    if (!TryGetComponent(out fileReader))
                    {
                        // Failed to get component, so add the component.
                        fileReader = gameObject.AddComponent<util.FileReaderLines>();
                    }
                }

                // Don't destroy the language manager on load.
                DontDestroyOnLoad(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // If a language should be loaded on start.
            if(loadLangaugeOnStart)
            {
                // Load using the starting language.
                LoadLanguage(defaultLanguage);
            }
        }

        // Returns the instance of the language marker.
        public static LanguageManager Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("Language Marker (singleton)");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<LanguageManager>();
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

        // Gets the loaded language.
        public language LoadedLanguage
        {
            get { return loadedLanguage; }
        }

        // Loads the language.
        private bool LoadLanguage(language newLanguage)
        {
            // Clear the language text.
            langText.Clear();

            // The file.
            string file = "";

            // The file path.
            // The file path is taken from a const variable since all language files...
            // Should be in the same place.
            // string filePath = "Assets\\Resources\\Data\\Languages\\";
            string filePath = LANGUAGE_FILES_PATH;

            // Set the provided language as the loaded language.
            // This will be set to none by an error check if the file cannot be found.
            loadedLanguage = newLanguage;

            // Checks which file to load.
            switch (loadedLanguage)
            {
                case language.english:
                default:
                    file = "lge_-_en.txt";
                    break;
            }

            // Set the file and the file path.
            fileReader.SetFile(file, filePath);

            // File doesn't exist, so file can't be loaded.
            if (!fileReader.FileExists())
            {
                Debug.LogError("LANGUAGE FILE MISSING. LANGUAGE LOAD FAILED.");

                loadedLanguage = language.none;
                return false;
            }

            // Read the file.
            fileReader.ReadFile();

            // Goes through each line.
            foreach (string line in fileReader.lines)
            {
                // Splits the string by tab.
                string[] str = line.Split('\t');

                // Sets the text.
                if (str.Length >= 2)
                    langText.Add(str[0], str[1]);
                else if (str.Length == 1)
                    langText.Add(str[0], string.Empty);


                // NOTE: by default, this adds quotation marks on the end of the string.
                // As such, the trims need to happen after taking the string out.
                // Note that if you intended to have quotes around a message, that would need a workaround.

                // Remove spaces, quotation marks, and other elements on the ends of the string.
                string temp = langText[str[0]];

                // Remove quotation marks on the edges (start and end).
                temp = temp.Trim('\"');

                // Remove white spaces.
                temp = temp.Trim();

                // Replace triple-elipses (…) with three periods (...). They can't be displayed for some reason.
                temp = temp.Replace("\uFFFD", "...");

                // Put temp back in lang text.
                langText[str[0]] = temp;
            }

            // Data loaded successfully.
            return true;
        }

        // Loads the English language.
        public bool LoadEnglish()
        {
            return LoadLanguage(language.english);
        }

        // Returns 'true' if there's language text.
        public bool HasLanguageText()
        {
            return langText.Count > 0;
        }

        // Returns 'true' if the language text contains the language key.
        public bool LanguageTextContainsKey(string key)
        {
            return langText.ContainsKey(key);
        }

        // Gets the language text.
        public string GetLanguageText(string key)
        {
            // If the key is in the language text list, get the text.
            if(langText.ContainsKey(key))
            {
                return langText[key];
            }
            // Key not found, so return empty string.
            else
            {
                return "";
            }
        }

        // Clears the language text and resets the language.
        public void ClearLanguageText()
        {
            langText.Clear();
            loadedLanguage = language.none;
        }

        // Translates the text using the provided key.
        // If the language file isn't loaded, then the text is marked using the noLoad colour..
        public bool TranslateText(TMP_Text text, string key, bool markIfFailed = true)
        {
            // The translation result.
            bool result = false;

            // Checks if the key is in the list.
            if (langText.ContainsKey(key) && key != string.Empty)
            {
                // Set the text.
                text.text = GetLanguageText(key);

                // Successful result.
                result = true;
            }
            else
            {
                // Since the game is only in English, this will always return false.
                if (markIfFailed)
                {
                    // MarkText(text);
                    LanguageMarker.Instance.MarkText(text);
                }

                result = false;
            }

            return result;
        }

        // Marks the provided text object using LanguageMarker.
        public void MarkText(TMP_Text text)
        {
            LanguageMarker.Instance.MarkText(text);
        }
    }
}
