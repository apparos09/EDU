using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace util
{
    // The class for the text box for the game.
    public class TextBox : MonoBehaviour
    {
        // [Header("Text Box")]

        // The text box object to be opened/closed.
        [Tooltip("The box object that's opened/closed. By default, it's set to gameObject.")]
        public GameObject boxObject;

        // The text box visual to be shown/hidden. By default it's the same as box object.
        [Tooltip("The box visual that can be shown/hidden. By default, it's the same as boxObject.")]
        public GameObject boxVisual;

        [Header("Text Box/Text Settings")]

        // The title of the text box.
        // NOTE: the box may not have a title, so make sure you always check that this object exists first.
        [Tooltip("The title of the text box, which updates to match each page. This can be left null if page titles aren't being used.")]
        public TMP_Text boxTitle;

        // The text in the text box.
        [Tooltip("The box text, which is set using the provided pages.")]
        public TMP_Text boxText;

        // The text for showing the text page number.
        [Tooltip("The box page number, which shows the current page number. Will be ignored if left null.")]
        public TMP_Text boxPageNumberText;

        // If true, the page number is shown as a fraction (page number/page total).
        // If false, the page number is shown as a whole number (page number).
        [Tooltip("If true, the page number is shown as a fraction ((page index + 1)/page count). If false, the page number is shown as a whole number (page index + 1).")]
        public bool showPageNumberAsFraction = true;

        // List of pages for the text box.
        public List<Page> pages = new List<Page>();

        // The current page index.
        private int currPageIndex = -1;

        // If enabled, the program will automatically go to the next page once it is loaded.
        [Tooltip("Goes to the next page automatically if true.")]
        public bool autoNextEnabled = false;

        // The max time it takes for a box to automatically turn to the next page.
        [Tooltip("The maximum time for going to the next page automatically.")]
        public float autoNextTimerMax = 5.0F;

        // The timer for automatically going to the next page.
        [Tooltip("The timer for automatically going to the next page.")]
        public float autoNextTimer = 0.0F;

        // Extra time added when TTS is enabled.
        [Tooltip("The amount of extra time added to the text box auto next page timer, which is meant to account for TTS.")]
        public float ttsExtraTime = 1.0F;

        // Allows for extra time to be added to the timer when TTS is active.
        [Tooltip("If true, extra time is added to the auto next timer to give more time for reading TTS.")]
        public bool addTtsExtraTime = true;

        // Set to 'true' to pause the timer.
        [Tooltip("Pauses the auto next page timer.")]
        public bool autoNextTimerPaused = false;

        // Uses scaled delta time for all time based functions if true. Uses unscaled delta time if false.
        [Tooltip("If true, scaled delta time is used for all time-based text box functions. If false, unscaledDeltaTime is used.")]
        public bool useScaledDeltaTime = true;

        // Closes the text box when the end of the pages has been reached.
        [Tooltip("Closes the text box once it reaches the end (i.e., there are no more pages next).")]
        public bool closeOnEnd = true;

        [Header("Text Box/Controls")]

        // The previous page button.
        public Button prevPageButton;

        // The next page button.
        public Button nextPageButton;

        // If set to 'true', the back button gets disabled if the textbox is on the first page.
        public bool autoDisablePrevButtonOnFirstPage = false;

        // Animation clips were taken out, since animation is done entirely by char loading.

        [Header("Text Box/Animation")]
        // If 'true', all the shown is shown at once. If false, the text is shown letter by letter.
        public bool instantText = true;

        // If 'true, the text controls are enabled once the text box animation ends.
        [Tooltip("If true, the text box controls are enabled when all text is loaded. Related variables may take priority when it comes to enabling text box controls.")]
        public bool enableTextControlsOnAnimEnd = true;

        // Allows the user to skip the text loading animation if this is set to 'true'.
        [Tooltip("If true, the user can skip to the next page before all the text is loaded. The back skip still works by default unless enableAnimationBackSkip is set to false.")]
        public bool enableAnimationSkip = true;

        // Allows the user to skip the text loading animation if they're going back a page.
        [Tooltip("If true, the user can go back a page before all the text is loaded. This only applies if 'enableAnimationSkip' is false.")]
        public bool enableAnimationBackSkip = true;

        // If set to 'true', the controls are hidden when the text is loading.
        [Tooltip("If true, controls are disabled when text is loading. The back button won't be disabled unless enableAnimationBackSkip is set to 'false'.")]
        public bool DisableControlsIfAnimSkipDisabled = true;

        // A queue of text for progressive character loading.
        private Queue<char> charQueue = new Queue<char>();

        // The timer for loading in a new char.
        private float charTimer = 0.0F;

        // The amount of characters loaded per interation.
        [Tooltip("How many characters are loaded per instance.")]
        public int charsPerLoad = 1;

        // The speed that the text is shown on the screen. This is ignored if the text is instantly shown.
        [Tooltip("The text/character loading speed. The higher the value, the faster the text. If <= 0, characters are loaded every frame.")]
        public float textSpeed = 0.0F;

        // Becomes 'true' when characters are loading up.
        private bool loadingChars = false;

        // CALLBACKS
        // a callback for when all the text has been gone through.
        public delegate void TextBoxCallback();

        // Callback for the textbox being opened.
        private TextBoxCallback openedCallback;

        // Callback for the textbox being closed.
        private TextBoxCallback closedCallback;

        // Callback for the textbox finishing.
        private TextBoxCallback finishedCallback;


        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            // ...
        }


        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Sets the box object to the game object if null.
            if (boxObject == null)
                boxObject = gameObject;

            // Sets the box visual to box object if null.
            // By default, it's the same as box object.
            if (boxVisual == null)
                boxVisual = boxObject;

            // Set this to the max by default.
            SetAutoNextTimerToMax();
        }

        // The current page index.
        public int CurrentPageIndex
        {
            get
            {
                return currPageIndex;
            }

            set
            {
                SetPage(value);
            }
        }


        // Returns the current page.
        public Page CurrentPage
        {
            get
            {
                if (currPageIndex >= 0 && currPageIndex < pages.Count)
                    return pages[currPageIndex];
                else
                    return null;
            }
        }

        // Checks to see if the current page index is in bounds.
        public bool ValidCurrentPageIndex()
        {
            return currPageIndex >= 0 && currPageIndex < pages.Count;
        }

        // Adds a callback for when the textbox opens.
        public void OnTextBoxOpenedAddCallback(TextBoxCallback callback)
        {
            openedCallback += callback;
        }

        // Removes a callback for when the textbox opens.
        public void OnTextBoxOpenedRemoveCallback(TextBoxCallback callback)
        {
            openedCallback -= callback;
        }

        // Shows the textbox.
        public void Open()
        {
            // Makes sure the visual is active (may be same as box object).
            boxVisual.SetActive(true);

            // Activate the box object.
            boxObject.SetActive(true);
            

            // Calls the callbacks for opening the textbox.
            if (openedCallback != null)
                openedCallback();

            // Reset the timer.
            SetAutoNextTimerToMax();
        }

        // Adds a callback for when the textbox is closed.
        public void OnTextBoxClosedAddCallback(TextBoxCallback callback)
        {
            closedCallback += callback;
        }

        // Removes a callback for when the textbox is closed.
        public void OnTextBoxClosedRemoveCallback(TextBoxCallback callback)
        {
            closedCallback -= callback;
        }

        // Hides the textbox.
        public void Close()
        {
            // Makes sure the box object visual is active by default.
            // If this is the same as box object, it'll be turned off anyway.
            boxVisual.SetActive(true);

            // Turns off the box object.
            boxObject.SetActive(false);

            // Calls the callbacks for closing the textbox.
            if (closedCallback != null)
                closedCallback();
        }

        // Shows the textbox. This does NOT call the Open callbacks.
        public void Show()
        {
            // Activate the box visual (may be same as object box).
            // boxObject.SetActive(true); // Old
            boxVisual.SetActive(true); // New

            // Reset the auto next timer.
            SetAutoNextTimerToMax();
        }

        // Hides the textbox. This does Not call the Close callbacks.
        public void Hide()
        {
            // Deactivate the box visual (may be same as box object).
            // boxObject.SetActive(false); // Old
            boxVisual.SetActive(false); // New
        }

        // Returns 'true' if the box object is active.
        public bool IsBoxObjectActiveSelf()
        {
            return boxObject.activeSelf;
        }

        // Returns 'true' if the box object is active in the hierachy.
        public bool IsBoxObjectActiveInHierachy()
        {
            return boxObject.activeInHierarchy;
        }

        // Checks if the box visual is active.
        public bool IsBoxVisualActiveSelf()
        {
            // return boxObject.activeSelf; // Old
            return boxVisual.activeSelf;
        }

        // Checks if the textbox is visible in the hierarchy.
        public bool IsBoxVisualActiveInHierachy()
        {
            // return boxObject.activeInHierarchy; // Old
            return boxVisual.activeInHierarchy;
        }

        // Returns 'true' if activeSelf for box object and box visual return true.
        public bool IsBoxObjectAndVisualActiveSelves()
        {
            return boxObject.activeSelf && boxVisual.activeSelf;
        }

        // Returns 'true' if activeInHierachy for box object and box visual return true.
        public bool IsBoxObjectAndVisualActiveInHierachies()
        {
            return boxObject.activeInHierarchy && boxVisual.activeInHierarchy;
        }

        // Returns 'true' if the text box has pages.
        public bool HasPages()
        {
            return pages.Count > 0;
        }

        // Gets the page count.
        public int GetPageCount()
        {
            return pages.Count;
        }

        // Changes the page index.
        public void SetPage(int index)
        {
            SetTextBoxText(index, false);
        }

        // Moves onto the next page.
        public void NextPage()
        {
            // Increases the index, and sets the text.
            SetTextBoxText(currPageIndex + 1);
        }

        // Moves onto the next page.
        // wrapAround: if true, the text box loops around to the beginning if there is no next page.
        public void NextPage(bool wrapAround)
        {
            // Increase the index.
            int newIndex = currPageIndex + 1;

            // Checks if the text box should wrap around.
            if (wrapAround)
            {
                // Out of bounds, so wrap around to 0.
                if (newIndex >= pages.Count)
                    newIndex = 0;
            }

            // Sets the new index.
            SetTextBoxText(newIndex);
        }

        // Returns to the previous page.
        public void PreviousPage()
        {
            // If the animation should be skipped when going back, and only when going back.
            if (enableAnimationBackSkip && !enableAnimationSkip)
            {
                // Finishes the page instead of finishing it.
                SetTextBoxText(currPageIndex - 1, false);
            }
            else // Standard
            {
                // TODO: maybe have the back button skip the text always instead of loading up the rest of the page?

                // Decreases the index, and sets the text.
                // SetTextBoxText(currPageIndex - 1);
                SetTextBoxText(currPageIndex - 1);
            }
        }

        // Returns to the previous page.
        // wrapAround: if true, the textbox arounds around to the beginning if there is no previous page.
        public void PreviousPage(bool wrapAround)
        {
            // Gets the new index.
            int newIndex = currPageIndex - 1;

            // If the text box should wrap around.
            if(wrapAround)
            {
                // Out of bounds, so wrap around to last page.
                if (newIndex < 0)
                    newIndex = pages.Count - 1;
            }

            // If the animation should be skipped when going back, and only when going back.
            if (enableAnimationBackSkip && !enableAnimationSkip)
            {
                // Finishes the page instead of finishing it.
                SetTextBoxText(newIndex, false);
            }
            else // Standard
            {
                // TODO: maybe have the back button skip the text always instead of loading up the rest of the page?
                SetTextBoxText(newIndex);
            }
        }

        // Called when the page has changed.
        // This is called even if the current page was set to the value it already had.
        // This is called only if the page has changed. If you want to call a function everytime...
        // The text box text has changed (e.g., characters aren't all being loaded in at once), call OnTextBoxTextChanged().
        // newPageIndex: the new page index, which is now the current index.
        //  - This may differ from the new page index that was provided if said index was out of bounds.
        public virtual void OnPageChanged(Page newPage, int newPageIndex)
        {
            // Updates the text box page number text.
            UpdatePageNumberText();
        }

        // CONTROLS //

        // Enables/disables the textbox controls.
        public void SetTextBoxControls(bool interactable)
        {
            // Enables/disables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = interactable;

            // Enables/disables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = interactable;


            // // Checks if auto next is enabled.
            // if(autoNext)
            // {
            //     // Pause the auto next timer
            //     if (stopAutoTimerOnDisable && !interactable)
            //         autoNextTimerPaused = true;
            // }
        }

        // Enables the text box controls.
        public void EnableTextBoxControls()
        {
            SetTextBoxControls(true);
        }

        // Disables the text box controls.
        // If 'stopAutoTimer' is true, the text box's auto page turn timer is stopped.
        public void DisableTextBoxControls()
        {
            SetTextBoxControls(false);
        }

        // Enables the previous button.
        public void EnablePreviousPageButton()
        {
            // Enables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = true;
        }

        // Disables the previous button.
        public void DisablePreviousPageButton()
        {
            // Disables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = false;
        }

        // Disables the previous page button if on the first page.
        public void DisablePreviousPageButtonOnFirstPage()
        {
            // Checks for the previous page button being set.
            if (prevPageButton != null)
            {
                // Checks if the button should be enabled.
                bool enableButton = currPageIndex != 0;

                // Change the button interaction setting if it doesn't match.
                if (prevPageButton.interactable != enableButton)
                    prevPageButton.interactable = enableButton;
            }
        }

        // Enables the next button.
        public void EnableNextPageButton()
        {
            // Enables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = true;
        }

        // Disables the previous button.
        public void DisableNextPageButton()
        {
            // Disables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = false;
        }


        // SET PAGES //

        // Adds a page to the end of the pages list.
        public void AddPage(Page page)
        {
            pages.Add(page);
        }

        // Inserts a page at the provided index.
        public void InsertPage(Page page, int index)
        {
            // If the current page index is greater than the provided index...
            // Increase it by '1' so that it matches up with the current page.
            if (currPageIndex >= index)
                currPageIndex++;

            pages.Insert(index, page);
        }

        // Inserts a page after the current page.
        public void InsertAfterCurrentPage(Page page)
        {
            // Checks for validity. If this check fails, the page is added to the end of the list.
            if(currPageIndex >= 0 && currPageIndex < pages.Count)
            {
                pages.Insert(currPageIndex + 1, page);
            }
            else
            {
                pages.Add(page);
            }
        }

        // Replaces the pages from the list with the new pages.
        public void ReplacePages(List<Page> newPages)
        {
            // Replaces the pages and sets the page.
            pages.Clear();

            // Pages to add.
            if(newPages.Count > 0)
            {
                pages = new List<Page>(newPages);

                // Sets the page.
                SetPage(0);
            }
            else
            {
                // No current pages.
                currPageIndex = -1;
            }
            
        }

        // Replaces the pages from the list with the new pages.
        public void ReplacePages(List<string> newPages)
        {
            // Clears the page list.
            pages.Clear();

            // Replaces the pages and sets the page.
            foreach(string text in newPages)
            {
                pages.Add(new Page(text));
            }

            // Sets the current page.
            SetPage(0);
        }

        // Sets the text that's on the text box.
        // fnishPage: determines if the text should be finished, or if it should skip to the next page.
        private void SetTextBoxText(int nextPageIndex, bool finishPage = true)
        {
            // If text is still being loaded just sub in the rest and stop loading in new characters.
            if (loadingChars)
            {
                // If the animation skip has been enabled.
                if(enableAnimationSkip)
                {
                    // No longer loading characters.
                    loadingChars = false;

                    // If the text should be finished, or if it should just skip to the next page.
                    // If there is no next page then the rest of the text is loaded regardless of 'finishPage's value.
                    // TODO: maybe make it so that a callback is called when the dialog box is finished.

                    if (finishPage || !(nextPageIndex >= 0 && nextPageIndex < pages.Count)) // Finish loading the page.
                    {
                        // Updates the box text if the object is set.
                        // This likely isn't necessary, since presumably a page's title...
                        // Won't be changed. However, this is still here to make sure...
                        // That the box title is being updated properly.
                        if (boxTitle != null)
                            boxTitle.text = pages[currPageIndex].title;

                        // Finishes the text instead of replacing the page.
                        boxText.text = pages[currPageIndex].text;
                        charQueue.Clear();
                        charTimer = 0.0F;
                        return;
                    }
                }
                // Skipping is not allowed, so just leave the function.
                else
                {
                    // If the page should be finished first, leave the function so that the page can be completed.
                    if(finishPage)
                        return;
                }   
            }

            // There's no next page, so don't change the text.
            if (nextPageIndex >= pages.Count || nextPageIndex < 0)
            {
                // The textbox is about to be closed, so call the 'on page closed' callback for the last page.
                // There were some index out of bounds errors here, and I don't know why.
                if(currPageIndex >= 0 && currPageIndex < pages.Count)
                    pages[currPageIndex].OnPageClosed();

                // The text has all been displayed, so call the callbacks.
                if (nextPageIndex >= pages.Count)
                    OnTextBoxFinished();

                return;
            }


            // Clears out existing title.
            if (boxTitle != null)
                boxTitle.text = string.Empty;

            // Clears out the existing text.
            boxText.text = string.Empty;

            // Calls the function for the page that's being closed.
            // If the new index is the same as the current index, don't call the OnPageClosed() function.
            // It will just treat the page as never closing at all.
            if(currPageIndex >= 0 && currPageIndex < pages.Count && currPageIndex != nextPageIndex)
                pages[currPageIndex].OnPageClosed();

            // Sets the new page index.
            currPageIndex = nextPageIndex;

            // Bounds correction for the page.
            // Current Page Index is set to -1 if there are no pages.
            if (pages.Count > 0)
            {
                currPageIndex = Mathf.Clamp(currPageIndex, 0, pages.Count - 1);
            }
            else
            {
                currPageIndex = -1;
                return;
            }


            // Calls the 'open' function on the new page.
            pages[currPageIndex].OnPageOpened();


            // If the previous button should be automatically disabled on the first page, try to disable it.
            if(autoDisablePrevButtonOnFirstPage)
                DisablePreviousPageButtonOnFirstPage();


            // A bounds check is done again to make sure that the pages weren't cleared in a callback.
            // This was to address an error that was being encountered.
            if (currPageIndex >= 0 && currPageIndex < pages.Count)
            {
                // Sets the box title if the object is set.
                // This is always loaded instantly, while the box text might be loaded...
                // Instantly or gradually depending on the text box's settings.
                if (boxTitle != null)
                    boxTitle.text = pages[currPageIndex].title;

                // Checks if the text should be shown automatically, or if it should be shown letter by letter.
                if (instantText) // Instant
                {
                    // Sets the box text.
                    boxText.text = pages[currPageIndex].text;
                }
                else // Letter by Letter
                {
                    // Set to load characters, and loads up the char queue.
                    loadingChars = true;
                    charQueue.Clear();
                    charQueue = new Queue<char>(pages[currPageIndex].text);
                    charTimer = 0.0F;


                    // If the textbox controls should be disabled when the animation skip is turned off.
                    if (!enableAnimationSkip && DisableControlsIfAnimSkipDisabled)
                    {
                        // Disables the next button.
                        DisableNextPageButton();

                        // Disables the previous button.
                        if (!enableAnimationBackSkip)
                            DisablePreviousPageButton();
                    }
                }
            }
            // Invalid page index.
            else
            {
                // If the box title is set, clear it.
                if (boxTitle != null)
                    boxTitle.text = string.Empty;

                // Clears the box title.
                boxText.text = string.Empty;

                // TODO: close textbox?
            }

            // Sets the auto next timer to max. If autoNext is set to false, the timer won't decrease anyway.
            SetAutoNextTimerToMax();

            // The page has changed.
            OnPageChanged(pages[currPageIndex], currPageIndex);

            // If characters were loaded instantly...
            // Call the OnCharactersFinishedLoading() and OnTextBoxTextChanged() function.
            // If the characters aren't being laoded instantly, don't call it here, since the first set...
            // Of characters will be loaded by the LoadCharacterByCharacter() function.
            if (!loadingChars)
            {
                // In the load character by character function the characters finished loading function...
                // Is called after the text box text changed function, so that's done here as well.
                OnTextBoxTextChanged(boxText.text);
                OnCharactersFinishedLoading();
            }
        }

        // Called wheen the text box text has been changed.
        // If characters are being loaded gradually, this is called everytime a character is loaded.
        // If you want a function that's only called when the page has changed and not the text, use OnPageChanged().
        // newText: the text that has been loaded into the text box.
        public virtual void OnTextBoxTextChanged(string newText)
        {
            // ...
        }

        // Returns 'true' if the text box is loading characters.
        public bool IsLoadingCharacters()
        {
            return loadingChars;
        }

        // Returns the remaining characters that must be loaded.
        public int GetRemainingCharactersToLoad()
        {
            return charQueue.Count;
        }

        // Loads character by character.
        private void LoadCharacterByCharacter()
        {
            // Checks if there are characters to load from the queue.
            if (charQueue.Count != 0)
            {
                // If the timer has reached 0 or less.
                if (charTimer <= 0.0F)
                {
                    // Adds to the string.
                    string temp = boxText.text;

                    // Loads (X) amount of characters based on 'charsPerLoad'.
                    // Stops if the char queue runs out of characters.
                    for(int i = 0; i < charsPerLoad && charQueue.Count > 0; i++)
                    {
                        temp += charQueue.Dequeue();
                    }
                    
                    boxText.text = temp;


                    // NOTE: why did I divide by text speed instead of applying it to the timer itself?

                    // // If the text speed is set to 0 the new char will load on the next frame.
                    // // NOTE: past a certain point, the char gets put every frame, which means there's a limit to the text speed.
                    // if (textSpeed > 0)
                    //     charTimer = 1.0F / textSpeed;
                    // else
                    //     charTimer = 0.0F;

                    // Reset the value to 1 second. How much gets reduced is based on the text speed.
                    // If textSpeed is set to '0', then a character is loaded every frame.
                    if (textSpeed > 0)
                        charTimer = 1.0F;
                    else
                        charTimer = 0.0F;

                    // The text box text has changed.
                    OnTextBoxTextChanged(boxText.text);
                }
                else // Reduce timer.
                {
                    // If the text speed is greater than 0, reduce the timer.
                    if(textSpeed > 0)
                    {
                        // If scaled delta time should be used.
                        if (useScaledDeltaTime)
                        {
                            charTimer -= Time.deltaTime * textSpeed;
                        }
                        // If unscaled delta time should be used.
                        else
                        {
                            charTimer -= Time.unscaledDeltaTime * textSpeed;
                        }

                        // If the char timer is negative, set it to 0.
                        if (charTimer < 0.0F)
                            charTimer = 0.0F;
                    }
                    // The text speed is 0 or less, so set char timer to 0 (load characters every frame).
                    else
                    {
                        charTimer = 0.0F;
                    }
                }
            }
            else
            {
                // No characters to load.
                loadingChars = false;
                charTimer = 0.0F;

                // If the text box should automatically go onto the next page when it's done after a certain period of time...
                // Set the timer.
                if (autoNextEnabled)
                    SetAutoNextTimerToMax();


                // Enables the text box controls when the animation ends.
                // The textbox controls should be disabled when the animation skip is turned off.
                if (enableTextControlsOnAnimEnd && !enableAnimationSkip && DisableControlsIfAnimSkipDisabled)
                {
                    // Enable the controls.
                    EnableTextBoxControls();
                }

                // If the previous button should be disabled on the first page, attempt to disable it.
                // This is put after the auto skip settings.
                if (autoDisablePrevButtonOnFirstPage)
                {
                    DisablePreviousPageButtonOnFirstPage();
                }

                // The characters have finished loading, so call the related function.
                OnCharactersFinishedLoading();
            }
        }

        // Called when all characters have finished being loaded for the current page.
        public virtual void OnCharactersFinishedLoading()
        {
            // ...
        }

        // Updates the text box page number text.
        public virtual void UpdatePageNumberText()
        {
            // The box page number text is set.
            if(boxPageNumberText != null)
            {
                // There are pages.
                if(pages.Count > 0)
                {
                    // If true, the page number is shown as a fraction ((page index + 1) / page number).
                    if(showPageNumberAsFraction)
                    {
                        // Set as current page index + 1 / page count.
                        boxPageNumberText.text =
                            (currPageIndex + 1).ToString() + "/" + pages.Count.ToString();
                    }
                    // If false, the page number is shown as a whole number (page index + 1).
                    else
                    {
                        // Set as current page index + 1.
                        boxPageNumberText.text = (currPageIndex + 1).ToString();
                    }

                }
                // No pages.
                else
                {
                    boxPageNumberText.text = string.Empty;
                }
            }
        }

        // Sets the timer to its max.
        public void SetAutoNextTimerToMax()
        {
            // Set the time to the max.
            autoNextTimer = autoNextTimerMax;

            // This has been commented out so that programs that don't use utils GameSettings can still use this.
            // // Adds the extra time for TTS reading.
            // if (addTtsExtraTime && GameSettings.Instance.UseTextToSpeech)
            //     autoNextTimer += ttsExtraTime;
        }

        
        // A callback function for when all the text is finished.
        // This is only called if the user attempts to go onto the next page when there is none.
        public void OnTextBoxFinishedAddCallback(TextBoxCallback callback)
        {
            finishedCallback += callback;
        }

        // Removes the callback.
        public void OnTextBoxFinishedRemoveCallback(TextBoxCallback callback)
        {
            finishedCallback -= callback;
        }

        // Called when all the text has been displayed.
        private void OnTextBoxFinished()
        {
            // Checks if there are functions to call.
            if (finishedCallback != null)
                finishedCallback();

            // If the text box should be closed when it's done.
            if (closeOnEnd)
                Close();
        }

        // Clears out all pages.
        // It's recommended that this function is called instead of clearing the page list inself.
        // There are extra elements that are cleared this function are called, so use this instead of pages.Clear().
        public void ClearPages()
        {
            // Clears out the pages.
            pages.Clear();
            pages = new List<Page>();

            // Now at index 0.
            currPageIndex = 0;

            // If the box title is set, clear the text.
            if (boxTitle != null)
                boxTitle.text = string.Empty;

            // Clear out the text and characters in the queue.
            // Also reset the character load timer.
            loadingChars = false;
            boxText.text = string.Empty;
            charQueue.Clear();
            charTimer = 0.0F;

            // Clears the page number text if the object is set.
            if(boxPageNumberText != null)
                boxPageNumberText.text = string.Empty;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // Changed this from the courtine version.
            if (loadingChars)
            {
                LoadCharacterByCharacter();
            }

            // If the page should automatically change.
            if(autoNextEnabled)
            {
                // If the timer is not finished yet, reduce the time.
                // Don't do it if the timer is paused.
                if(autoNextTimer > 0.0F && !autoNextTimerPaused)
                {
                    // If scaled delta time is used, use Time.deltaTime.
                    // If unscaled delta time is used, use Time.unscaledDeltaTime.
                    autoNextTimer -= (useScaledDeltaTime) ? Time.deltaTime : Time.unscaledDeltaTime;

                    // If the timer is now finished, turn the page.
                    if(autoNextTimer <= 0.0F)
                    {
                        // Set the timer to 0.
                        autoNextTimer = 0.0F;

                        // Moves onto the next page.
                        NextPage();
                    }
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed
        protected virtual void OnDestroy()
        {
           // ... 
        }
    }
}