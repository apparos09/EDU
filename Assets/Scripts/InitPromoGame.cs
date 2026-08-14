using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_EDU
{
    // Initializes a promo game
    public class InitPromoGame : MonoBehaviour
    {
        // Becomes 'true' when the game has been initialized.
        // Originally public in LOL-373. Now is private with a getter.
        private bool initializedGame = false;

        void Awake()
        {
            // Unity Initialization
            Application.targetFrameRate = 30; // 30 FPS
            Application.runInBackground = false; // Don't run in the background.

            // Use the tutorial by default.
            GameSettings.Instance.UseTutorials = true;

            // Set for loading languages.
            if (GameSettings.IS_MULTI_LANGUAGE && LanguageManager.Instantiated)
            {
                LanguageManager.Instance.defaultLanguage = LanguageManager.language.english;
                LanguageManager.Instance.loadLangaugeOnStart = true;
            }

            // Set to true to show that the game has been initialized.
            initializedGame = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Makes the TMP text get marked if translation failed.
            // Change 'IS_MULTI_LANGUAGE" in game settings instead of adjusting this.
            TMP_TextTranslator.markIfFailed = true;
        }

        // Returns 'true' if the game has been initialized.
        public bool InitializedGame
        {
            get {  return initializedGame; }
        }

        // Update is called once per frame
        void Update()
        {
            // Loads the title scene.
            SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
        }
    }
}