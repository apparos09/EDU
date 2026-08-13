using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The tutorial log, which allows the player to view previous tutorials.
    public class TutorialLog : MonoBehaviour
    {
        // The gameplay UI.
        public GameplayUI gameUI;

        // The tutorial infos index.
        // This is used to determine what entry buttons to display.
        protected int tutorialInfosIndex = 0;

        // The tutorial infos.
        protected List<Tutorials.TutorialInfo> tutorialInfos = new List<Tutorials.TutorialInfo>();

        // Marks that the tutorial infos have been loaded.
        private bool tutorialInfosLoaded = false;

        // The current tutorial info.
        protected Tutorials.TutorialInfo currTutorialInfo = null;

        [Header("Tutorials")]

        // The tutorial log entry buttons.
        public List<TutorialLogEntryButton> tutorialLogEntryButtons = new List<TutorialLogEntryButton>();

        // The previous tutorials page button.
        public Button prevTutorialsPageButton;

        // The next tutorial pages button.
        public Button nextTutorialsPageButton;

        // The tutorials page text.
        public TMP_Text tutorialsPageText;

        [Header("Selected Tutorial Section")]

        // The tutorial title.
        public TMP_Text tutorialTitle;

        // The tutorial image display.
        public GameObject tutorialImageDisplay;

        // The tutorial image.
        public Image tutorialImage;

        // The display sprite used when a page doesn't have a display image.
        public Sprite displaySpriteNone;

        // If 'true', display sprite - none is used.
        // If 'false', when there's no display sprite, the display object is turned off.
        private bool useDisplaySpriteNone = true;

        // The tutorial text.
        public TMP_Text tutorialText;

        // The tutorial page index.
        protected int currTutorialInfoPageIndex = 0;

        // The previous tutorial text page.
        public Button prevTutorialTextPageButton;

        // The next tutorial text page.
        public Button nextTutorialTextPageButton;

        // The tutorial page text.
        public TMP_Text tutorialTextPageText;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // By default, the tutorial info is cleared.
            ClearCurrentTutorialInfo();
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the game UI isn't set, set it.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>();

            // Loads the tutorials info.
            if(!tutorialInfosLoaded)
            {
                LoadTutorialsInfo(false);
            }
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // Tries to reload the tutorials info if they haven't been updated.
            if(!tutorialInfosLoaded)
            {
                LoadTutorialsInfo(false);
            }
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            // Marks this as false so that a reload happens on enable.
            tutorialInfosLoaded = false;

            // Clear the current tutorial info.
            // This is consistent with what happens in the info log.
            ClearCurrentTutorialInfo();
        }

        // Loads the tutorial info.
        // forceLoad: if true, a load is forced. If false, a load only happens if the cleared tutorial count has changed.
        public void LoadTutorialsInfo(bool forceLoad = true)
        {
            // Tutorials are instantiated.
            if (Tutorials.Instantiated)
            {
                // Marks that elements should be loaded.
                bool load = true;

                // Gets the instance.
                Tutorials tutorials = Tutorials.Instance;

                // If the load shouldn't be forced, check if a load is needed.
                if (!forceLoad)
                {
                    // If the counts match and the count is greater than 0, don't reload.
                    if (tutorialInfos.Count == tutorials.TutorialsClearedCount && tutorialInfos.Count > 0)
                    {
                        load = false;
                    }
                }

                // If elements should be load.
                if(load)
                {
                    // Clears the infos.
                    tutorialInfos.Clear();

                    // Clear the current tutorial info.
                    ClearCurrentTutorialInfo();

                    // If tutorials are being used, only load tutorials that have been cleared.
                    // If no tutorials are being used, load all tutorials.
                    bool clearedOnly = (GameSettings.Instantiated) ? GameSettings.Instance.UseTutorials : false;

                    // Loads the cleared tutorials.
                    tutorialInfos = tutorials.GenerateTutorialInfos(clearedOnly);
                }

                // Sets the current tutorial info index to 0 and updates the buttons.
                tutorialInfosIndex = 0;
                UpdateTutorialInfoEntryButtons();
            }
            else
            {
                // If the tutorials object doesn't exist, don't show anything.

                // Clears the tutorial infos.
                tutorialInfos.Clear();

                // Clears the tutorial info by default.
                ClearCurrentTutorialInfo();
            }

            // Mark as updated.
            tutorialInfosLoaded = true;
        }

        // Refreshes the tutorials info by doing a force load.
        public void RefreshTutorialsInfo()
        {
            LoadTutorialsInfo(true);
        }

        // TUTORIAL INFO ENTRY BUTTONS
        // Sets the current tutorial info entry buttons.
        public void UpdateTutorialInfoEntryButtons()
        {
            // The index of the information being shown.
            int infoIndex = tutorialInfosIndex;

            // Goes through all buttons.
            foreach(TutorialLogEntryButton entryButton in tutorialLogEntryButtons)
            {
                // The info index is within bounds.
                if(infoIndex >= 0 && infoIndex < tutorialInfos.Count)
                {
                    // Apply the entry info from the index.
                    entryButton.ApplyEntryInfo(tutorialInfos[infoIndex]);
                }
                // Not in bounds, so clear button.
                else
                {
                    entryButton.ClearEntryInfo();
                }

                // Increase index.
                infoIndex++;
            }

            // Gets the entry buttons page number and page count.
            int pageNumber = GetTutorialInfoEntryButtonsPageNumber();
            int pageCount = GetTutorialInfoEntryButtonsPageCount();
            tutorialsPageText.text = pageNumber.ToString() + "/" + pageCount.ToString();

            // Checks if there are multiple pages.
            // Used to make page buttons interactable or non-interactable.
            bool multPages = tutorialInfos.Count > tutorialLogEntryButtons.Count;
            prevTutorialsPageButton.interactable = multPages;
            nextTutorialsPageButton.interactable= multPages;
        }

        // Clear the tutorial info entry buttons.
        public void ClearTutorialInfoEntryButtons()
        {
            // Goes through all buttons.
            foreach (TutorialLogEntryButton entryButton in tutorialLogEntryButtons)
            {
                entryButton.ClearEntryInfo();
            }

            // Clear the entry button page text.
            tutorialsPageText.text = "-";
        }

        // Goes to the previous tutorial info.
        public void PreviousTutorialInfoEntryButtons()
        {
            // Reduce index by button count.
            tutorialInfosIndex -= tutorialLogEntryButtons.Count;

            // Bounds check for negative.
            if (tutorialInfosIndex < 0)
            {
                // OLD: this method would cause the last page to have an inconsistent...
                // Amount of buttons set, so this has been changed.
                // // Reduce the count by the number of log entry buttons.
                // tutorialInfosIndex = tutorialInfos.Count - tutorialLogEntryButtons.Count;
                // 
                // // If the index is now negative, try to position it...
                // // To show the last entry page to keep pages consistent.
                // if (tutorialInfosIndex < 0)
                // {
                //     tutorialInfosIndex = 0;
                // }

                // NEW: uses modulus to calculate the page number.
                // If the index is now negative, try to position it...
                // To show the last entry page to keep pages consistent.

                // Gets the remainder from a modulus division operation.
                int remainder = tutorialInfos.Count % tutorialLogEntryButtons.Count;

                // The new index that will be set.
                int newIndex;

                // If the remainder is 0 or less, just subtract the entry button count from the tutorials info count.
                // This happens if tutorialInfos.Count can be perfectly divided by tutorialLogEntryButtons.Count.
                // Such a calculation leaves no remainder, thus remainder = 0.
                if (remainder <= 0)
                {
                    // Reduce the count by the number of log entry buttons.
                    newIndex = tutorialInfos.Count - tutorialLogEntryButtons.Count;
                }
                // Remainder isn't 0, so subtract the remainder from the tutorials infos.
                else
                {
                    // Calculates the new index by subtracting the remainder...
                    // From the tutorials info count.
                    newIndex = tutorialInfos.Count - remainder;
                }

                // If the new index is out of bounds, set it to 0.
                // Out of bounds: lass than 0, or greater than or equal to count.
                if (newIndex < 0 || newIndex >= tutorialInfos.Count)
                {
                    newIndex = 0;
                }

                // Set to new index.
                tutorialInfosIndex = newIndex;
            }

            // Updates the entry buttons.
            UpdateTutorialInfoEntryButtons();
        }

        // Goes to the next tutorial info.
        public void NextTutorialInfoEntryButtons()
        {
            // Increase index by button count.
            tutorialInfosIndex += tutorialLogEntryButtons.Count;

            // Bounds check for positive.
            if (tutorialInfosIndex >= tutorialInfos.Count)
            {
                tutorialInfosIndex = 0;
            }

            // Updates the entry buttons.
            UpdateTutorialInfoEntryButtons();
        }

        // Gets the tutorial info entry buttons page.
        public int GetTutorialInfoEntryButtonsPageNumber()
        {
            // If there are entry buttons, return the page number.
            if(tutorialLogEntryButtons.Count > 0)
            {
                return Mathf.CeilToInt(
                    (float)(tutorialInfosIndex + tutorialLogEntryButtons.Count) /
                    tutorialLogEntryButtons.Count);
            }
            // No entry butotns, so return 0.
            else
            {
                return 0;
            }
        }

        // Gets the tutorial info entry buttons page count.
        public int GetTutorialInfoEntryButtonsPageCount()
        {
            // If there are entry buttons, do the calculation.
            if(tutorialLogEntryButtons.Count > 0)
            {
                return Mathf.CeilToInt((float)tutorialInfos.Count / tutorialLogEntryButtons.Count);
            }
            // No buttons, so return 0.
            else
            {
                return 0;
            }
        }

        // CURRENT TUTORIAL INFO
        // Returns 'true' if the tutorial info is set.
        public bool IsCurrentTutorialInfoSet()
        {
            return currTutorialInfo != null;
        }

        // Sets the tutorial info using the provided index.
        // NOTE: this doesn't override the saved index, since that's used for the entry buttons.
        public void SetCurrentTutorialInfo(int tutorialInfoIndex)
        {
            // Clamps the index.
            int indexClamped = Mathf.Clamp(tutorialInfoIndex, 0, tutorialInfos.Count - 1);

            // Index is valid, so load entry.
            if(indexClamped >= 0 && indexClamped < tutorialInfos.Count)
            {
                // Sets the current info.
                currTutorialInfo = tutorialInfos[indexClamped];

                // Sets page to 0, then updates the current tutorial info.
                currTutorialInfoPageIndex = 0;
                UpdateCurrentTutorialInfo();
            }
            // Invalid index, so clear entry.
            else
            {
                // Clears the info.
                ClearCurrentTutorialInfo();
            }
        }

        // Set the current tutorial info using info.
        public void SetCurrentTutorialInfo(Tutorials.TutorialInfo newInfo)
        {
            // Sets the info.
            currTutorialInfo = newInfo;

            // If the current info isn't null, update the info.
            if (currTutorialInfo != null)
            {
                currTutorialInfoPageIndex = 0; // Reset page.
                UpdateCurrentTutorialInfo();
            }
            // Info is now null, so clear the current info.
            else
            {
                ClearCurrentTutorialInfo();
            }
        }

        // Set the current tutorial info using an entry button.
        public void SetCurrentTutorialInfo(TutorialLogEntryButton entryButton)
        {
            SetCurrentTutorialInfo(entryButton.entryInfo);
        }

        // CURRENT TUTORIAL INFO PAGE
        // Sets the tutorial info page.
        public void SetCurrentTutorialInfoPage(int newPageIndex)
        {
            // Sets the current page index if the current tutorial info is set.
            if(currTutorialInfo != null)
            {
                currTutorialInfoPageIndex = Mathf.Clamp(newPageIndex, 0, currTutorialInfo.pages.Count - 1);
            }
            // Set to 0 since the value is null.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Updates the current tutorial info page.
            UpdateCurrentTutorialInfo();
        }

        // Goes to the previous tutorial info page.
        public void PreviousCurrentTutorialInfoPage()
        {
            // Reduce page index.
            currTutorialInfoPageIndex--;

            // If the current tutorial info is set.
            if (currTutorialInfo != null)
            {
                // Bounds check for going below 0.
                if (currTutorialInfoPageIndex < 0)
                {
                    currTutorialInfoPageIndex = currTutorialInfo.pages.Count - 1;
                }
            }
            // Not set.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Sets the tutorial page.
            SetCurrentTutorialInfoPage(currTutorialInfoPageIndex);
        }

        // Goes to the next tutorial info page.
        public void NextCurrentTutorialInfoPage()
        {
            // Increase page index.
            currTutorialInfoPageIndex++;

            // If the current tutorial info is set,
            if(currTutorialInfo != null)
            {
                // If the page is at or above the count, set it to null.
                if(currTutorialInfoPageIndex >= currTutorialInfo.pages.Count)
                {
                    currTutorialInfoPageIndex = 0;
                }
            }
            // Not set.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Sets the tutorial page.
            SetCurrentTutorialInfoPage(currTutorialInfoPageIndex);
        }

        // TUTORIAL INFO DISPLAY
        // Updates the tutorial info.
        public void UpdateCurrentTutorialInfo()
        {
            // No current info, so clear display and return.
            if(currTutorialInfo == null)
            {
                ClearCurrentTutorialInfo();
                return;
            }

            // If the page index is negative or above the page count, clear display and return. 
            if(currTutorialInfoPageIndex < 0 || currTutorialInfoPageIndex >= currTutorialInfo.pages.Count)
            {
                ClearCurrentTutorialInfo();
                return;
            }

            // Gets the page.
            EDU_Page page = (EDU_Page)currTutorialInfo.pages[currTutorialInfoPageIndex];

            // Set the text.
            tutorialTitle.text = page.title;
            tutorialText.text = page.text;
            tutorialTextPageText.text = (currTutorialInfoPageIndex + 1).ToString() + "/" + currTutorialInfo.pages.Count.ToString();

            // Set the display image.
            tutorialImageDisplay.gameObject.SetActive(true);
            tutorialImage.sprite = page.displaySprite;

            // If the tutorial image sprite is null, turn the object off.
            if(tutorialImage.sprite == null)
            {
                // If display sprite none should be used, set it.
                if(useDisplaySpriteNone)
                    tutorialImage.sprite = displaySpriteNone;

                // If the sprite is still null, hide the image display.
                if(tutorialImage.sprite == null)
                    tutorialImageDisplay.gameObject.SetActive(false);
            }                

            // If text-to-speech is usable and enabled, read the page text.
            if (page.textSpeakKey != "" && LanguageManager.IsTextToSpeechUsableAndEnabled())
            {
                // Read the text.
                SpeakText(page.textSpeakKey, false);
            }

            // Set the text page buttons interactable if there's more than 1 page.
            bool multPages = currTutorialInfo.pages.Count > 1;
            prevTutorialTextPageButton.interactable = multPages;
            nextTutorialTextPageButton.interactable = multPages;
        }

        // Clears the tutorial info.
        public void ClearCurrentTutorialInfo()
        {
            // Set the current tutorial info to null and reset the indexes.
            currTutorialInfo = null;
            tutorialInfosIndex = 0;
            currTutorialInfoPageIndex = 0;

            // Clear the text.
            tutorialTitle.text = "-";
            tutorialText.text = "-";
            tutorialTextPageText.text = "-";

            // Clear the info display image.
            tutorialImageDisplay.gameObject.SetActive(true);
            tutorialImage.sprite = null;
            tutorialImageDisplay.SetActive(false);

            // Disable page buttons.
            prevTutorialTextPageButton.interactable = false;
            nextTutorialTextPageButton.interactable = false;

        }

        // Clears the tutorial log entry buttons and the tutorial info.
        public void ClearTutoriaLogEntryButtonsAndTutorialInfo()
        {
            ClearCurrentTutorialInfo();
            ClearTutorialInfoEntryButtons();
        }

        // TEXT-TO-SPEECH
        // Speaks text if Text-to-Speech is usable and enabled.
        // checkUsable: if 'true', this function checks if the TTS is usable.
        public void SpeakText(string key, bool checkTTSUsable = true)
        {
            // If TTS should be checked for it being usable.
            if(checkTTSUsable)
            {
                // If text-to-speech is usable and enabled, speak the text.
                if (key != "" && LanguageManager.IsTextToSpeechUsableAndEnabled())
                {
                    LanguageManager.Instance.SpeakText(key);
                }
            }
            // Don't check usable.
            else
            {
                LanguageManager.Instance.SpeakText(key);
            }

        }

    }
}