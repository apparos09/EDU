using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace RM_EDU
{
    // A button used for selecting an action unit by the player.
    public class ActionUnitButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The button script.
        public Button button;

        // The default image of the button.
        [Tooltip("The button's default image. Gets set to button's image on start if null.")]
        public Sprite buttonDefaultImageSprite = null;

        // The icon that's on the button.
        public Image unitIconImage;

        // The sprite for having an alpha value of 0.
        public Sprite alpha0Sprite;

        // The name of the unit.
        public TMP_Text unitNameText;

        // The energy cost text. This displays the amount of energy needed to create the unit.
        public TMP_Text energyCostText;

        // The highlight on the top of the card, which indicates the card above is usable.
        [Tooltip("An image above the card that indicates the card above the current one is usable.")]
        public Image highlightTopImage;

        // The highlight on the bottom of the card, which indicates the card below is usable.
        [Tooltip("An image below the card that indicates the card below the current one is usable.")]
        public Image highlightBottomImage;

        // The highlight on color.
        private Color highlightOnColor = Color.white;

        // The highlight off color.
        private Color highlightOffColor = Color.grey;

        // The unit prefab.
        public ActionUnit unitPrefab;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // If the button isn't set, try to get the component in the children.
            if (button == null)
                button = GetComponentInChildren<Button>();


            // Adds a Select() call on the button.
            if (button != null)
            {
                // Sets the default image.
                if (buttonDefaultImageSprite == null)
                    buttonDefaultImageSprite = button.image.sprite;

                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }

            // Applies the prefab.
            // If there is no prefab, the button is cleared.
            ApplyUnitPrefabInfo();
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // Gets the player user.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // If the player exists, give it this unit prefab.
            if(playerUser != null)
            {
                // Give the player the selected prefab.
                playerUser.SetSelectedActionUnitPrefab(unitPrefab);
            }
        }

        // Gets the unit type.
        public virtual ActionUnit.unitType GetUnitType()
        {
            // The unit type.
            ActionUnit.unitType unitType;

            // Prefab exists.
            if(unitPrefab != null)
            {
                unitType = unitPrefab.GetUnitType();
            }
            else
            {
                // There's no prefab, so return unknown type.
                unitType = ActionUnit.unitType.unknown;
            }

            // Returns the unit type.
            return unitType;
        }

        // Returns 'true' if the unit button has a unit prefab.
        public bool HasUnitPrefab()
        {
            return unitPrefab != null;
        }

        // Applies infromation from the unit prefab.
        public void ApplyUnitPrefabInfo()
        {
            // Checks if the prefab exists.
            if(unitPrefab != null)
            {
                // Icon image, display name, and energy cost.
                button.image.sprite = unitPrefab.cardBackgroundSprite;
                unitIconImage.sprite = unitPrefab.iconSprite;
                unitNameText.text = unitPrefab.GetUnitCardDisplayName();
                energyCostText.text = unitPrefab.energyCreationCost.ToString();

                // Refresh the unit button to set the interactable.
                RefreshUnitButtonInteractable();
            }
            // The prefab is null, so clear the unit button.
            else
            {
                ClearUnitButton();
            }
        }

        // Applies information from the unit prefab.
        public void ApplyUnitPrefabInfo(ActionUnit newUnitPrefab)
        {
            unitPrefab = newUnitPrefab;
            ApplyUnitPrefabInfo();
        }

        // Refresh the interactability of the unit button.
        public void RefreshUnitButtonInteractable()
        {
            // Checks if there's a prefab.
            if(unitPrefab != null)
            {
                // Sets interactable if the player can place this unit.
                button.interactable = ActionManager.Instance.playerUser.CanCreateActionUnit(unitPrefab);
            }
            // No prefab, so set as non-interactable.
            else
            {
                button.interactable = false;
            }
        }

        // The highlight on color.
        public Color HighlightOnColor
        {
            get { return highlightOnColor; }
        }

        // The highlight off color.
        public Color HighlightOffColor
        {
            get { return highlightOffColor; }
        }

        // Sets both the highlights with the same value.
        public void SetHighlightsOn(bool value)
        {
            SetHighlightTopOn(value);
            SetHighlightBottomOn(value);
        }

        // Returns 'true' if the highlight on the top is on.
        public bool IsHighlightTopOn()
        {
            return highlightTopImage.color == highlightOnColor;
        }

        // Sets the highlight top to its on or off color.
        public void SetHighlightTopOn(bool value)
        {
            highlightTopImage.color = (value) ? highlightOnColor : highlightOffColor;
        }

        // Returns 'true' if the highlight on the bottom is on.
        public bool IsHighlightBottomOn()
        {
            return highlightBottomImage.color == highlightOnColor;
        }

        // Sets the highlight bottom to its on or off color.
        public void SetHighlightBottomOn(bool value)
        {
            highlightBottomImage.color = (value) ? highlightOnColor : highlightOffColor;
        }

        // Clears the unit button.
        public void ClearUnitButton()
        {
            // NOTE: an empty image has a white square, so an empty image file is set for it instead.

            // Visual information.
            button.image.sprite = buttonDefaultImageSprite;
            unitIconImage.sprite = alpha0Sprite;
            unitNameText.text = "-";
            energyCostText.text = "-";
            

            // The button is no longer interactable.
            button.interactable = false;

            // Clears prefab.
            unitPrefab = null;
        }

    }
}