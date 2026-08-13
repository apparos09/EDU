using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The tutorial text box.
    public class TutorialTextBox : TextBox
    {
        [Header("Tutorials")]
        // The UI for the tutorial.
        public TutorialUI tutorialUI;

        // The display object.
        public GameObject displayObject;

        // The display image.
        public Image displayImage;

        [Header("Tutorials/Next Page Button Delay")]
        // If 'true', the next page button being made active is delayed.
        public bool delayNextPageButton = false;

        // The timer for delaying the next page button.
        public float delayNextPageButtonTimer;

        // The maximum time for delaying the next page button.
        // Default: 4 seconds.
        public float delayNextPageButtonTimerMax = 4.0F;

        // The timer bar that's used to display how long the player must wait...
        // Until they can go onto the next page.
        public ProgressBar delayNextPageButtonTimerBar;

        [Header("Tutorials/Sprites")]
        // A transparent sprite (alpha = 0).
        public Sprite alpha0Sprite;

        // The diagram none sprite.
        public Sprite noneSprite;
        
        // The two stage types: action and knowledge
        public Sprite stageTypesSprite;

        // The log and options buttons
        public Sprite logOptionsButtonsSprite;
        public Sprite logButtonSprite;
        public Sprite optionsButtonSprite;


        [Header("Tutorials/Sprites/Natural Resources")]
        // Display sprites for the natural resources.
        public Sprite biomassSprite;
        public Sprite geothermalSprite;
        public Sprite hydroSprite;
        public Sprite solarSprite;
        public Sprite waveSprite;
        public Sprite windSprite;

        public Sprite coalSprite;
        public Sprite naturalGasSprite;
        public Sprite nuclearSprite;
        public Sprite oilSprite;

        [Header("Tutorials/Sprites/Action")]

        // Action user and enemy energy bar
        public Sprite actionEnergyBarUserSprite;
        public Sprite actionEnergyBarEnemySprite;

        // Action unit buttons
        [Header("Tutorials/Sprites/Action/Units")]
        public Sprite actionUnitButtonsSprite;
        public Sprite actionUnitSelectorArrowsSprite;
        public Sprite actionUnitSelectorArrowsAltSprite;
        public Sprite actionUnitSelectorArrowsGuideSprite;
        public Sprite actionUnitSelectedSprite;

        // Defense types
        public Sprite actionDefenseTypesSprite;
        public Sprite actionDefenseLaneBlasterSprite;

        // Displays/Indicators
        [Header("Tutorials/Sprites/Action/Displays, Indicators")]
        public Sprite energyAirPollutionDisplaySprite;
        public Sprite indicatorsSprite;
        public Sprite dayNightIndicatorSprite;
        public Sprite windIndicatorSprite;

        // The action buttons: speed, deselect, remove, and energy block.
        [Header("Tutorials/Sprites/Action/Stage Buttons")]
        public Sprite actionButtonsSprite;
        public Sprite stageSpeedSprite;
        public Sprite unitDeselectSprite;
        public Sprite unitRemoveSprite;
        public Sprite energyBlockSprite;

        [Header("Tutorials/Sprites/Knowledge")]
        // The knowledge resource and statement buttons.
        public Sprite knowledgeElementsSprite;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // If the next page button will get delayed, don't enable...
            // The text controls on animation end.
            // If the next page button won't be delayed...
            // Make sure the text controls are activated on animation end.
            enableTextControlsOnAnimEnd = !delayNextPageButton;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the tutorial UI isn't set, get the instance.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;

            // Adds a callback for when the text box has been closed.
            OnTextBoxClosedAddCallback(OnTextBoxClosed);
        }

        // DELAY
        // Sets the dely next page button timer to max.
        public void SetDelayNextPageButtonToMax()
        {
            delayNextPageButtonTimer = delayNextPageButtonTimerMax;
        }

        // Returns 'true' if the next page button delay is active.
        // Returns 'false' if the delay isn't being used, or if the delay timer isn't running.
        public bool IsDelayNextPageButtonActiveAndRunning()
        {
            return delayNextPageButton && delayNextPageButtonTimer > 0.0F;
        }

        // Updates the next page button timer bar.
        public void UpdateDelayNextPageButtonTimerBar()
        {
            // The bar exists, so set it to the current time.
            if (delayNextPageButtonTimerBar != null)
            {
                delayNextPageButtonTimerBar.SetValueAsPercentage(
                    delayNextPageButtonTimer / delayNextPageButtonTimerMax, false);
            }
        }

        // Sets the next page button to max and update's the timer bar.
        public void SetDelayNextPageButtonToMaxAndUpdateTimerBar()
        {
            SetDelayNextPageButtonToMax();
            UpdateDelayNextPageButtonTimerBar();
        }

        // DISPLAY
        // Returns 'true' if the display is active.
        public bool IsDisplayActive()
        {
            return displayObject.activeSelf;
        }

        // Sets the display object as active. 
        public void SetDisplayActive(bool active)
        {
            displayObject.SetActive(active);
        }

        // Refreshes display active.
        // If there is no sprite for the display image, the object is disabled.
        public void RefreshDisplayActive()
        {
            // If there's a sprite and the display is inactive, activate the display.
            if(displayImage.sprite != null && !displayObject.activeSelf)
            {
                SetDisplayActive(true);
            }
            // If there's no sprite and the display is active, hide the display.
            else if(displayImage.sprite == null && displayObject.activeSelf)
            {
                SetDisplayActive(false);
            }
        }
        
        // Sets the display image sprite.
        public void SetDisplayImageSprite(Sprite newSprite)
        {
            displayImage.sprite = newSprite;
        }

        // Clears the display image sprite.
        public void ClearDisplayImageSprite()
        {
            displayImage.sprite = null;
        }

        // Returns the natural resource diagram sprite by the provided resource.
        public Sprite GetNaturalResourceDiagramSprite(NaturalResources.naturalResource resource)
        {
            // The sprite to be returned.
            Sprite sprite = null;

            // Checks the resource.
            switch(resource)
            {
                default:
                case NaturalResources.naturalResource.unknown:
                    sprite = noneSprite;
                    break;

                case NaturalResources.naturalResource.biomass:
                    sprite = biomassSprite;
                    break;

                case NaturalResources.naturalResource.geothermal:
                    sprite = geothermalSprite;
                    break;

                case NaturalResources.naturalResource.hydro:
                    sprite = hydroSprite;
                    break;

                case NaturalResources.naturalResource.solar:
                    sprite = solarSprite;
                    break;

                case NaturalResources.naturalResource.wave:
                    sprite = waveSprite;
                    break;

                case NaturalResources.naturalResource.wind:
                    sprite = windSprite;
                    break;

                case NaturalResources.naturalResource.coal:
                    sprite = coalSprite;
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    sprite = naturalGasSprite;
                    break;

                case NaturalResources.naturalResource.nuclear:
                    sprite = nuclearSprite;
                    break;

                case NaturalResources.naturalResource.oil:
                    sprite = oilSprite;
                    break;
            }

            return sprite;
        }

        // PAGE CHANGED/TEXT CHANGED
        // Called when the page has changed.
        public override void OnPageChanged(Page newPage, int newPageIndex)
        {
            // The page has changed.
            base.OnPageChanged(newPage, newPageIndex);

            // If the new page is an EDU page.
            if(newPage is EDU_Page)
            {
                // The converted page.
                EDU_Page eduPage = (EDU_Page)newPage;

                // Saves the sprite to the display image if it should be changed.
                if(displayImage.sprite != eduPage.displaySprite)
                {
                    displayImage.sprite = eduPage.displaySprite;
                }
            }

            // Refreshes the display so that its shown if there's a sprite...
            // And hidden if there's not a sprite.
            RefreshDisplayActive();

            // If the next page button active is delayed...
            // Sets the next page button timer to max and updates the delay next page button bar.
            if(delayNextPageButton)
            {
                SetDelayNextPageButtonToMaxAndUpdateTimerBar();
            }
        }

        // You tried to prevent the buttons for being active for a brief frame using this function...
        // But it didn't work.
        // // Called when the text box text has changed.
        // public override void OnTextBoxTextChanged(string newText)
        // {
        //     base.OnTextBoxTextChanged(newText);
        // 
        //     // If the delay is being used, the delay is running...
        //     // And the text box is done loading characters...
        //     // Make sure the next page button is false. 
        //     if(IsDelayNextPageButtonActiveAndRunning() && !IsLoadingCharacters())
        //     {
        //         nextPageButton.interactable = false;
        //     }
        // }

        // Called when the characters have finished loading.
        public override void OnCharactersFinishedLoading()
        {
            base.OnCharactersFinishedLoading();

            // If the next page button should be delayed.
            if(delayNextPageButton)
            {
                // Disable the button and set the delay next page button to max.
                nextPageButton.interactable = false;
                SetDelayNextPageButtonToMax();
            }
        }

        // Called when the text box has been closed.
        protected void OnTextBoxClosed()
        {
            // Clear the diagram image and refresh the diagram object.
            ClearDisplayImageSprite();
            RefreshDisplayActive();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the next page button is being delayed.
            if(delayNextPageButton)
            {
                // If characters aren't being loaded and the timer is above 0.
                if(!IsLoadingCharacters() && delayNextPageButtonTimer > 0.0F)
                {
                    // Reduce the time.
                    delayNextPageButtonTimer -= Time.unscaledDeltaTime;
                    
                    // The delay next page button timer has reached zero or less.
                    if(delayNextPageButtonTimer <= 0.0F)
                    {
                        // Set the timer to 0 and make the next page button active.
                        delayNextPageButtonTimer = 0.0F;
                        nextPageButton.interactable = true;
                    }

                    // Updates the bar.
                    UpdateDelayNextPageButtonTimerBar();
                }
            }
        }
    }
}