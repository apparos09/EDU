using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_EDU
{
    // A labeled value.
    public class TMP_LabeledValue : MonoBehaviour
    {
        // The header text.
        public TMP_Text headerText;

        // The value text.
        public TMP_Text valueText;
        
        // Gets the header text string.
        public string GetHeaderTextString()
        {
            return headerText.text;
        }

        // Sets the header text string.
        public void SetHeaderTextString(string newStr)
        {
            headerText.text = newStr;
        }

        // Gets the value text string.
        public string GetValueTextString()
        {
            return valueText.text;
        }

        // Sets the value text string.
        public void SetValueTextString(string newStr)
        {
            valueText.text = newStr;
        }

        // Clears the text.
        public void ClearTextStrings()
        {
            headerText.text = string.Empty;
            valueText.text = string.Empty;
        }

    }
}