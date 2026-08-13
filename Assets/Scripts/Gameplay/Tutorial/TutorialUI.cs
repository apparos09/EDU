using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The Tutorial UI.
    public class TutorialUI : MonoBehaviour
    {
        // The singleton instance.
        private static TutorialUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game UI.
        public GameplayUI gameUI;

        // The tutorials object.
        public Tutorials tutorials;

        // The background panel used to block other buttons.
        [Tooltip("Used to cover the screen to block raycasts (mouse/touch inputs)")]
        public Image raycastBlockPanel;

        // The tutorial text box.
        public TutorialTextBox textBox;

        // Constructor
        private TutorialUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
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
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                textBox.OnTextBoxOpenedAddCallback(OnTextBoxOpened);
                textBox.OnTextBoxClosedAddCallback(OnTextBoxClosed);
                textBox.OnTextBoxFinishedAddCallback(OnTextBoxFinished);

                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the UI instance if it's not set.
            if (gameUI == null)
            {
                gameUI = FindObjectOfType<GameplayUI>();
            }

            // Gets the tutorials object isn't set, instantiate it.
            // This doesn't check if Tutorials is instantiated since the Tutorials class doesn't use a prefab.
            if (tutorials == null)
            {
                tutorials = Tutorials.Instance;
            }

            // If the tutorials UI is not set, set it to this.
            if(tutorials.tutorialsUI == null)
            {
                tutorials.tutorialsUI = this;
            }

            // If the text box is active and there isn't a tutorial running, close it.
            if (textBox.IsBoxObjectActiveSelf() && !IsTutorialRunning())
            {
                // Closes the text box.
                CloseTextBox();
            }
        }

        // Gets the instance.
        public static TutorialUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TutorialUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial UI (singleton)");
                        instance = go.AddComponent<TutorialUI>();
                    }

                }

                // Return the instance.
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

        // Is the tutorial active?
        public bool IsTutorialRunning()
        {
            // If the textbox is visible and there are pages to read, the tutorial is running.
            return textBox.IsBoxObjectActiveSelf() && textBox.HasPages();
        }


        // Starts a tutorial.
        public void StartTutorial()
        {
            textBox.SetPage(0);
            OpenTextBox();
        }

        // Restarts the tutorial.
        public void RestartTutorial()
        {
            // Gets the pages from the text box.
            List<Page> pages = textBox.pages;

            // Ends the tutorial, sets the textbox pages, and starts the tutorial again.
            EndTutorial();
            textBox.pages = pages;
            StartTutorial();
        }

        // Ends the tutorial.
        public void EndTutorial()
        {
            // If the tutorial is running, end it.
            if (IsTutorialRunning())
            {
                // Sets to the last page and closes the text box.
                textBox.SetPage(textBox.GetPageCount() - 1);
                CloseTextBox();
            }
        }

        // Called when a tutorial is started.
        public void OnTutorialStart()
        {
            // If there is a background panel, turn it on.
            if (raycastBlockPanel != null)
                raycastBlockPanel.gameObject.SetActive(true);
        }

        // Called when a tutorail ends.
        public void OnTutorialEnd()
        {
            RefreshRaycastBlocker();
        }

        // TEXT BOX
        // Loads pages for the textbox.
        public void LoadPages(ref List<Page> pages, bool clearPages)
        {
            // If the pages should be cleared.
            if (clearPages)
                textBox.ClearPages();

            // Adds pages to the end of the text box.
            textBox.pages.AddRange(pages);

        }

        // Opens Text Box
        public void OpenTextBox()
        {
            textBox.Open();
            ActivateRaycastBlocker();
        }

        // Closes the Text Box
        public void CloseTextBox()
        {
            textBox.Close();
            DeactivateRaycastBlocker();
        }

        // Text box operations.
        // Called when the text box is opened.
        private void OnTextBoxOpened()
        {
            // These should be handled by the pages.
            // Hides the diagram by default.
            // HideDiagram();

            // The tutorial has started.
            tutorials.OnTutorialStart();
        }

        // Called when the text box is closed.
        private void OnTextBoxClosed()
        {
            // NOTE: this may not be needed.
            // // The tutorial has ended (at least for now), so allow the game to move again.
           tutorials.OnTutorialEnd();
        }

        // Called when the text box is finished.
        private void OnTextBoxFinished()
        {
            // Remove all the pages.
            textBox.ClearPages();

            // These should be handled by the pages.
            // // Clear the diagram and hides it.
            // ClearDiagram();
            // HideDiagram();

            // The tutorial has ended.
            tutorials.OnTutorialEnd();
        }

        // Returns 'true' if the text box is visible.
        public bool IsTextBoxActive()
        {
            return textBox.IsBoxObjectActiveSelf();
        }

        // Returns 'true' if the text box visual is active.
        public bool IsTextBoxVisualActive()
        {
            return textBox.IsBoxVisualActiveSelf();
        }

        // Returns 'true' if the text box and the text box visual is active.
        public bool IsTextBoxAndVisualActive()
        {
            return textBox.IsBoxObjectAndVisualActiveSelves();
        }


        // RAYCAST BLOCKER //

        // Returns 'true' if the raycast blocker is active.
        // If the raycast blocker is null, return false.
        public bool IsRaycastBlockerActive()
        {
            if (raycastBlockPanel != null)
                return raycastBlockPanel.isActiveAndEnabled;
            else
                return false;
        }

        // Sets if the raycast blocker is active or not.
        public void SetRaycastBlockerActive(bool active)
        {
            if (raycastBlockPanel != null)
                raycastBlockPanel.gameObject.SetActive(active);
        }

        // Activates the raycast blocker.
        public void ActivateRaycastBlocker()
        {
            SetRaycastBlockerActive(true);
        }

        // Deactivates the raycast blocker.
        public void DeactivateRaycastBlocker()
        {
            SetRaycastBlockerActive(false);
        }

        // Toggles the raycast blocker being active.
        public void ToggleRaycastBlockerActive()
        {
            SetRaycastBlockerActive(!IsRaycastBlockerActive());
        }

        // Refreshes the raycast blocker to make sure it's only enabled if the text box is visible.
        public void RefreshRaycastBlocker()
        {
            SetRaycastBlockerActive(IsTextBoxActive());
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