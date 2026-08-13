using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_EDU
{
    // Initializes a promo game
    public class InitPromoGame : MonoBehaviour
    {
        // Becomes 'true' when the game has been initialized.
        [HideInInspector]
        public bool initializedGame = false;

        void Awake()
        {
            // Unity Initialization
            Application.targetFrameRate = 30; // 30 FPS
            Application.runInBackground = false; // Don't run in the background.

            // Use the tutorial by default.
            GameSettings.Instance.UseTutorials = true;

            // Set to true to show that the game has been initialized.
            initializedGame = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Change 'IS_LOL_BUILD" in game settings instead of adjusting this.
            TMP_TextTranslator.markIfFailed = true;
        }

        // Update is called once per frame
        void Update()
        {
            // Loads the title scene.
            SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
        }
    }
}