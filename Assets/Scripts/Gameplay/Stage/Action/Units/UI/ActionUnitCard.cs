using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // The action unit card.
    public class ActionUnitCard : MonoBehaviour
    {
        // The card background.
        public Image unitCardBackground;

        // The default card background.
        public Sprite unitCardDefaultBackground;

        // The unit name text.
        public TMP_Text unitNameText;

        // The icon image.
        public Image unitIconImage;

        // The unit energy cost text.
        public TMP_Text energyCostText;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Clears the action unit info.
            ClearActionUnitInfo();
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        //     // ...
        // }

        // Applies information from action unit.
        public void ApplyActionUnitInfo(ActionUnit actionUnit)
        {
            // If the unit exists, get the info.
            if(actionUnit != null)
            {
                unitCardBackground.sprite = actionUnit.cardBackgroundSprite;
                unitNameText.text = actionUnit.GetUnitCardDisplayName();
                unitIconImage.sprite = actionUnit.iconSprite;
                energyCostText.text = actionUnit.energyCreationCost.ToString();
            }
            // Null unit, so clear the info instead.
            else
            {
                ClearActionUnitInfo();
            }
        }

        // Applies information from an action unit button.
        public void ApplyActionUnitInfo(ActionUnitButton unitButton)
        {
            // If the button exists, get the info.
            if (unitButton != null)
            {
                unitCardBackground.sprite = unitButton.button.image.sprite;
                unitNameText.text = unitButton.unitNameText.text;
                unitIconImage.sprite = unitButton.unitIconImage.sprite;
                energyCostText.text = unitButton.energyCostText.text;
            }
            // Null object, so clear the info instead.
            else
            {
                ClearActionUnitInfo();
            }
        }

        // Applies info from the remove button.
        public void ApplyRemoveActionUnitInfo(ActionUnitRemoveButton removeButton)
        {
            // Checks if remove button exists.
            if(removeButton != null)
            {
                unitCardBackground.sprite = removeButton.cardBackgroundSprite;
                unitNameText.text = removeButton.GetRemoveCardDisplayName();
                unitIconImage.sprite = removeButton.cardIconSprite;
                energyCostText.text = "-";
            }
            else
            {
                ClearActionUnitInfo();
            }
        }

        // Clears the information displayed for the icon.
        public void ClearActionUnitInfo()
        {
            unitCardBackground.sprite = unitCardDefaultBackground;
            unitNameText.text = "-";
            unitIconImage.sprite = null;
            energyCostText.text = "-";
        }
    }
}