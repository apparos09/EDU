using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The UI for the selected action unit.
    public class ActionUnitSelectedUI : MonoBehaviour
    {
        // The aciton UI.
        public ActionUI actionUI;

        // The selected unit.
        public ActionUnitCard selectedUnit;

        // The selected unit name text.
        public TMP_Text selectedUnitNameText;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action unit UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Gets the component in the children if it isn't set.
            if (selectedUnit == null)
                selectedUnit = GetComponentInChildren<ActionUnitCard>();
        }

        // Applies information from action unit.
        public void ApplyActionUnitInfo(ActionUnit actionUnit)
        {
            // Applys the unit info to the selected unit.
            selectedUnit.ApplyActionUnitInfo(actionUnit);

            // Sets the name.
            selectedUnitNameText.text = actionUnit.GetUnitNameTranslated();

            // Reads the unit name key.
            SpeakText(actionUnit.unitNameKey);
        }

        // Applies information from an action unit button.
        public void ApplyActionUnitInfo(ActionUnitButton unitButton)
        {
            // Sets the action unit info using the unit button.
            selectedUnit.ApplyActionUnitInfo(unitButton);

            // Checks if the button has a unit prefab.
            if(unitButton.HasUnitPrefab())
            {
                selectedUnitNameText.text = unitButton.unitPrefab.GetUnitNameTranslated();

                // Reads the unit name key.
                SpeakText(unitButton.unitPrefab.unitNameKey);
            }
            else
            {
                selectedUnitNameText.text = "-";
            }
            
        }

        // Applies info from the remove button.
        public void ApplyRemoveActionUnitInfo(ActionUnitRemoveButton removeButton)
        {
            selectedUnit.ApplyRemoveActionUnitInfo(removeButton);

            selectedUnitNameText.text = removeButton.GetRemoveCardName();
        }

        // Clears the information displayed for the icon.
        public void ClearActionUnitInfo()
        {
            selectedUnit.ClearActionUnitInfo();

            selectedUnitNameText.text = "-";
        }

        // Speaks the provided text key.
        public void SpeakText(string key)
        {
            // Key is set.
            if(key != "")
            {
                // Checks if the instances exist: LOL SDK, Text-to-Speech, and GameSettings.
                if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized() && TextToSpeech.Instantiated && GameSettings.Instantiated)
                {
                    // Gets the instances.
                    GameSettings gameSettings = GameSettings.Instance;
                    SystemManager lolManager = SystemManager.Instance;

                    // Checks if TTS should be used.
                    if (gameSettings.UseTextToSpeech)
                    {
                        // Grabs the LOL Manager to trigger text-to-speech.
                        lolManager.textToSpeech.SpeakText(key);
                    }
                }
            }
        }

    }
}