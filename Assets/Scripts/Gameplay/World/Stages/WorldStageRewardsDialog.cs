using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world stage rewards dialog.
    public class WorldStageRewardsDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The world stage the rewards are being displayed from.
        public WorldStage worldStage;

        [Header("Rewards")]

        // The selected reward unit.
        public TMP_LabeledValue selectedUnitDisplay;      
        
        // The reward buttons.
        public List<ActionUnitRewardButton> rewardButtons = new List<ActionUnitRewardButton>();

        // The reward unit prefabs list.
        public List<ActionUnit> rewardUnitPrefabs = new List<ActionUnit>();

        // The index in the reward units prefab list.
        [Tooltip("The index in the reward unit prefabs list.")]
        public int rewardUnitPrefabsIndex = 0;

        // If 'true', the rewards the player already has are ignored.
        private bool ignoreEarnedRewards = true;

        // If 'true', the dialog is closed if there are no rewards.
        private bool closeIfNoRewards = true;

        // Sets to 'true' if the rewards have been loaded.
        private bool rewardsInfoLoaded = false;

        [Header("Unit Selector")]

        // The previous index button.
        public Button prevUnitIndexButton;

        // The next index button.
        public Button nextUnitIndexButton;

        [Header("Selected Unit")]

        // The reward unit prefab that's currently selected.
        [Tooltip("The rewarded unit the player has selected.")]
        public ActionUnit selectedUnitPrefab;

        // The selected unit description.
        [Tooltip("The description for the unit the player has selected.")]
        public List<string> selectedUnitDesc = new List<string>();

        // he selected unit description keys.
        [Tooltip("The description key for the unit the player has selected.")]
        public List<string> selectedUnitDescKeys = new List<string>();

        // The selected unit index.
        public int selectedUnitDescPageIndex = 0;

        // The unit description page text.
        public TMP_Text selectedUnitDescPageText;

        // The previous unit description page button.
        public Button prevUnitDescPageButton;

        // The next unit description page button.
        public Button nextUnitDescPageButton;

        [Header("Other")]

        // The continue button.
        public Button continueButton;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Clears the selected unit info if no rewards have been loaded.
            if(!rewardsInfoLoaded)
            {
                ClearSelectedUnitInfo();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // Clear the selected unit info on start.
            ClearSelectedUnitInfo();
        }

        // LOAD REWARDS //
        // Returns 'true' if the rewards info has been loaded.
        public bool IsRewardsInfoLoaded()
        {
            return rewardsInfoLoaded;
        }

        // Loads the rewards info.
        public void LoadRewardsInfo(WorldStage worldStage)
        {
            // Saves the world stage.
            this.worldStage = worldStage;

            // If the world stage exists, load the rewards info.
            if(worldStage != null)
            {
                // The action unit prefabs.
                ActionUnitPrefabs actionUnitPrefabs = null;

                // If the action unit prefab list is instantiated, get the instance.
                if (ActionUnitPrefabs.Instantiated)
                {
                    actionUnitPrefabs = ActionUnitPrefabs.Instance;
                }
                else
                {
                    Debug.LogWarning("The Action Unit Prefabs list hasn't been instantiated.");
                }

                // Clears the prefabs.
                rewardUnitPrefabs.Clear();

                // The reward unit ids.
                List<int> rewardUnitIds = new List<int>();

                // If there are reward units.
                if(worldStage.defenseIdRewards.Count > 0)
                    rewardUnitIds.AddRange(worldStage.defenseIdRewards);


                // If earned rewards should be ignored, remove them from the list.
                if(ignoreEarnedRewards)
                {
                    // Removes defense units the player already has.
                    // NOTE: make sure this function is called BEFORE the player...
                    // Has been given the rewards, so that this script knows what...
                    // Rewards have been earned.
                    DataLogger dataLogger = DataLogger.Instance;

                    // Goes through the defense ids in the data logger and removes them...
                    // From the rewards list.
                    for(int i = 0; i < dataLogger.defenseIds.Count; i++)
                    {
                        // If the defense id is in the reward units list, remove it.
                        if(rewardUnitIds.Contains(dataLogger.defenseIds[i]))
                        {
                            // Removes the defense unit id from the reward units list.
                            rewardUnitIds.Remove(dataLogger.defenseIds[i]);
                        }
                    }
                }


                // Goes through the reward units to load the prefabs.
                for(int i = 0; i < rewardUnitIds.Count; i++)
                {
                    // Gets the defense prefab.
                    // NOTE: this uses the index and not the id, but these should provide the same result.
                    ActionUnitDefense defensePrefab = actionUnitPrefabs.GetDefensePrefab(rewardUnitIds[i]);

                    // Adds the defense prefab to the list.
                    if (defensePrefab != null)
                        rewardUnitPrefabs.Add(defensePrefab);
                }

                // Set the index to 0, since the reward buttons are loaded from the start...
                // Of the list.
                rewardUnitPrefabsIndex = 0;

                // Load in the unit prefabs.
                for(int i = 0; i < rewardButtons.Count; i++)
                {
                    // If there's a reward unit prefab, apply the info.
                    if(i < rewardUnitPrefabs.Count)
                    {
                        rewardButtons[i].ApplyUnitPrefabInfo(rewardUnitPrefabs[i]);
                    }
                    // No prefab, so clear the button.
                    else
                    {
                        rewardButtons[i].ClearUnitPrefabInfo();
                    }
                }

                // Refresh the page buttons interactable.
                RefreshUnitPrefabIndexButtonsInteractable();

                // If the dialog should be closed if there are no rewards...
                // And there are no reward unit prefabs, close the dialog.
                if(closeIfNoRewards && rewardUnitPrefabs.Count <= 0)
                {
                    CloseDialog();
                }
            }
            // No world stage, so clear the info.
            else
            {
                // Clears the rewards dialog info.
                ClearInfo();
            }

            // The rewards have been loaded.
            rewardsInfoLoaded = true;
        }

        // UNIT PREFABS INDEX //
        // Sets the reward unit prefabs index, which acts as the current page.
        public void SetRewardUnitPrefabsIndex(int index)
        {
            // Sets the new index.
            rewardUnitPrefabsIndex = Mathf.Clamp(index, 0, rewardUnitPrefabs.Count - 1);
        
            // The current prefab index for loading information.
            int currPrefabIndex = rewardUnitPrefabsIndex;

            // Loads the info for the buttons.
            for(int i = 0; i < rewardButtons.Count; i++)
            {
                // If the prefab exists, apply the info.
                if (rewardUnitPrefabs[currPrefabIndex] != null)
                {
                    rewardButtons[i].ApplyUnitPrefabInfo(rewardUnitPrefabs[currPrefabIndex]);
                }
                // Prefab is null, so clear info.
                else
                {
                    rewardButtons[i].ClearUnitPrefabInfo();
                }

                // Increase the current prefab index.
                currPrefabIndex++;

                // If the prefab index has passed the end of the list...
                // Loop around to the beginning.
                if(currPrefabIndex >= rewardUnitPrefabs.Count)
                {
                    currPrefabIndex = 0;
                }
            }
        }

        // Goes to the previous reward unit prefabs index.
        public void PreviousRewardUnitPrefabsIndex()
        {
            // The current index minus 1.
            int index = rewardUnitPrefabsIndex - 1;

            // Loop around to the end.
            if (index < 0)
                index = rewardUnitPrefabs.Count - 1;

            // Sets the index.
            SetRewardUnitPrefabsIndex(index);
        }

        // Goes to the next reward unit prefabs index.
        public void NextRewardUnitPrefabsIndex()
        {
            // The current index plus 1.
            int index = rewardUnitPrefabsIndex + 1;

            // Loop around to the start.
            if (index >= rewardUnitPrefabs.Count)
                index = 0;

            // Sets the index.
            SetRewardUnitPrefabsIndex(index);
        }

        // Refreshes the page button interactables.
        public void RefreshUnitPrefabIndexButtonsInteractable()
        {
            // If there are more prefabs than there are buttons, there are multiple pages.
            if(rewardUnitPrefabs.Count > rewardButtons.Count)
            {
                prevUnitIndexButton.interactable = true;
                nextUnitIndexButton.interactable = true;
            }
            // Only one page, so disable buttons.
            else
            {
                prevUnitIndexButton.interactable = false;
                nextUnitIndexButton.interactable = false;
            }
        }

        // SELECTION //
        // Applies the selected unit information from the reward button.
        public void ApplySelectedUnitInfo(ActionUnitRewardButton rewardButton)
        {
            // If the reward button has a prefab.
            if(rewardButton.HasUnitPrefab())
            {
                // Saves the unit prefab.
                selectedUnitPrefab = rewardButton.unitPrefab;

                // Name.
                selectedUnitDisplay.headerText.text = selectedUnitPrefab.GetUnitNameTranslated();
                
                // Description.
                // Clears the selected unit desc and adds the range - will set text at end.
                selectedUnitDesc.Clear();
                selectedUnitDesc.AddRange(selectedUnitPrefab.GetUnitDescriptionTranslated());

                // Gets the description keys.
                selectedUnitDescKeys.Clear();
                selectedUnitDescKeys = selectedUnitPrefab.GenerateUnitDescriptionKeys();

                // Checks if the selected unit description has multiple pages.
                bool multPages = selectedUnitDesc.Count > 1;

                // Changes interactability of buttons.
                prevUnitDescPageButton.interactable = multPages;
                nextUnitDescPageButton.interactable = multPages;

                // Original - set description.
                // selectedRewardUnitDisplay.valueText.text = selectedRewardUnitPrefab.GetUnitDescriptionTranslated()[0];

                // Tries speaking the unit prefab's description.
                // SpeakText(selectedRewardUnitPrefab.unitDescKey);

                // New - selects page by index.
                SetSelectedUnitInfoDescriptionPageIndex(0);
            }
            // No prefab, so clear.
            else
            {
                ClearSelectedUnitInfo();
            }
        }

        // Returns the selected unit description count.
        public int GetSelectedUnitInfoDescriptionPageCount()
        {
            return selectedUnitDesc.Count;
        }

        // Sets the page index of the selected unit description.
        public void SetSelectedUnitInfoDescriptionPageIndex(int pageIndex)
        {
            // There are entries to use.
            if(selectedUnitDesc.Count > 0)
            {
                // Clamp the value.
                selectedUnitDescPageIndex = Mathf.Clamp(pageIndex, 0, selectedUnitDesc.Count - 1);

                // Set the description.
                selectedUnitDisplay.valueText.text = selectedUnitDesc[selectedUnitDescPageIndex];

                // Update the page text.
                selectedUnitDescPageText.text = (selectedUnitDescPageIndex + 1).ToString() + "/" + selectedUnitDesc.Count.ToString();

                // Tries speaking the unit prefab's description.
                SpeakText(selectedUnitDescKeys[selectedUnitDescPageIndex]);
            }
            else
            {
                // Clear.
                selectedUnitDescPageIndex = 0;
                selectedUnitDisplay.valueText.text = "-";
            }

            // Nothing.
            // selectedRewardUnitDisplay.valueText.text = rewardButton.unitPrefab.GetUnitDescriptionTranslated()[0];
        }

        // Goes to the previous page index of the selected unit description.
        public void PreviousSelectedUnitInfoDescriptionPageIndex()
        {
            // The new index.
            int newIndex = selectedUnitDescPageIndex - 1;

            // Loop around to the end.
            if (newIndex < 0)
                newIndex = selectedUnitDesc.Count - 1;

            // Set the new index.
            SetSelectedUnitInfoDescriptionPageIndex(newIndex);
        }

        // Goes to the next page index of the selected unit description.
        public void NextSelectedUnitInfoDescriptionPageIndex()
        {
            // The new index.
            int newIndex = selectedUnitDescPageIndex + 1;

            // Loop around to the beginning.
            if (newIndex >= selectedUnitDesc.Count)
                newIndex = 0;

            // Set the new index.
            SetSelectedUnitInfoDescriptionPageIndex(newIndex);
        }

        // Clears the selected unit information.
        public void ClearSelectedUnitInfo()
        {
            // Clear the selected prefab.
            selectedUnitPrefab = null;

            // Clears the selected unit description.
            selectedUnitDesc.Clear();

            // Set the description page index.
            selectedUnitDescPageIndex = 0;

            // Clear the selected prefab display.
            selectedUnitDisplay.headerText.text = "-";
            selectedUnitDisplay.valueText.text = "-";

            // Clear the selected unit description page text.
            selectedUnitDescPageText.text = "-";

            // Make the selected unit buttons non-interactable.
            prevUnitDescPageButton.interactable = false;
            nextUnitDescPageButton.interactable = false;
        }

        // INFO, CLEAR DIALOG
        // Clears the info.
        public void ClearInfo()
        {
            // Clears all the reward buttons.
            foreach(ActionUnitRewardButton rewardButton in rewardButtons)
            {
                rewardButton.ClearUnitPrefabInfo();
            }

            // Clears the selected unit info.
            ClearSelectedUnitInfo();

            // Clears the prefabs list and sets the index to 0.
            rewardUnitPrefabs.Clear();
            rewardUnitPrefabsIndex = 0;

            // The rewards info haven't been loaded.
            rewardsInfoLoaded = false;
        }

        // Closes the dialog.
        public void CloseDialog()
        {
            WorldUI.Instance.CloseWorldStageRewardsDialog();
        }

        // SPEAK TEXT
        // Speaks the reward text.
        public void SpeakText(string key)
        {
            // If the key exists.
            if(key != "")
            {
                // The LOL Manager has been initialized, TTS is instantiated, and GameSettings is instantiated.
                if (LanguageManager.IsLanguageLoaderInitialized() && TextToSpeech.Instantiated && GameSettings.Instantiated)
                {
                    // If text-to-speech is enabled.
                    if(GameSettings.Instance.UseTextToSpeech)
                    {
                        LanguageManager.Instance.SpeakText(key);
                    }
                }
            }
            
        }
    }
}