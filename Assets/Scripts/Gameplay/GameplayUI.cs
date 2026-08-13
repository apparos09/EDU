using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The gameplay UI.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The tutorial UI.
        public TutorialUI tutorialUI;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the game manager isn't set, try to find it.
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // If the tutorial UI isn't set and is instantiated, get the instance.
            if(tutorialUI == null && TutorialUI.Instantiated)
            {
                tutorialUI = TutorialUI.Instance;
            }
                
            // If the tutorial UI is set.
            if(tutorialUI != null)
            {
                // The gameUI object isn't set, so give it this.
                if(tutorialUI.gameUI == null)
                {
                    tutorialUI.gameUI = this;
                }
            }
        }

        // TUTORIAL //
        // Start tutorial
        public void StartTutorial(List<Page> pages)
        {
            // TODO: see if loading multiple pages into one tutorial is fine. If it isn't, change it back.

            // Loads the pages, sets the index to 0, and closes the textbox.
            // Closes the textbox.
            // if (tutorialUI.textBox.IsBoxObjectActiveSelf())
            // {
            //     // Close the textbox.
            //     tutorialUI.textBox.Close();
            // }

            // Load the pages.
            tutorialUI.LoadPages(ref pages, true); // Clears pages.
            tutorialUI.textBox.CurrentPageIndex = 0;

            // If the text box isn't open, open it.
            if(!tutorialUI.textBox.IsBoxObjectActiveSelf())
                tutorialUI.textBox.Open();
        }

        // On Tutorial Start
        public virtual void OnTutorialStart()
        {
            // Debug.Log("GameplayUI: OnTutorialStart");
        }

        // On Tutorial End
        public virtual void OnTutorialEnd()
        {
            // Debug.Log("GameplayUI: OnTutorialEnd");
        }

        // Checks if the tutorial text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            // Checks if it's visible normally, and in the hierachy.
            return tutorialUI.textBox.IsBoxObjectActiveSelf() && tutorialUI.textBox.IsBoxObjectActiveInHierachy();
        }

        // Returns 'true' if a tutorial is active.
        public bool IsTutorialActive()
        {
            // NOTE: the text box should only be open if a tutorial is active.
            return IsTutorialTextBoxOpen();
        }

        // Checks if the tutorial is running.
        public bool IsTutorialRunning()
        {
            // Checks if the tutorial is instantiated.
            if (Tutorials.Instantiated)
            {
                // Checks if the tutorial UI is not set to null.
                if (tutorialUI != null)
                {
                    // The tutorials object has a dedicated function for seeing if it's running...
                    // You don't know why you did a seperate set up for this, but you're leaving it as is.
                    return IsTutorialTextBoxOpen();
                }
                else // No tutorial UI, so the tutorial cannot run.
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        // Adds the tutorial text box open/close callbacks.
        public void AddTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialUI.textBox.OnTextBoxOpenedAddCallback(manager.OnTutorialStart);
            tutorialUI.textBox.OnTextBoxClosedAddCallback(manager.OnTutorialEnd);
        }

        // Removes the tutorial text box open/close callbacks.
        public void RemoveTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialUI.textBox.OnTextBoxOpenedRemoveCallback(manager.OnTutorialStart);
            tutorialUI.textBox.OnTextBoxClosedRemoveCallback(manager.OnTutorialEnd);
        }

        // SCENES //
        // Goes to the title scene
        public void LoadTitleScene()
        {
            gameManager.LoadTitleScene();
        }

        // Goes to the world scene.
        public void LoadWorldScene()
        {
            gameManager.LoadWorldScene();
        }

        // Goes to the action scene.
        public void LoadActionScene()
        {
            gameManager.LoadActionScene();
        }

        // Goes to the knowledge scene.
        public void LoadKnowledgeScene()
        {
            gameManager.LoadKnowledgeScene();
        }

        // Loads the results scene.
        public void LoadResultsScene()
        {
            gameManager.LoadResultsScene();
        }

        // Quits the game.
        public void QuitGame()
        {
            gameManager.QuitGame();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}
