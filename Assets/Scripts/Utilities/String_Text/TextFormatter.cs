using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // The text formatter.
    public class TextFormatter : StringFormatter
    {
        [Header("Text")]
        // The text.
        public Text text;

        // The TMP text.
        public TMP_Text TMP_text;

        // The substrings of the text.
        public List<string> substrings;

        // Formats the text on start.
        public bool formatOnStart = true;

        // Start is called before the first frame update
        void Start()
        {
            // Auto-sets the text.
            if (text == null)
                text = GetComponent<Text>();

            // Auto-sets the TMP text.
            if (TMP_text == null)
                TMP_text = GetComponent<TMP_Text>();


            // Formats the string.
            if (formatOnStart)
            {
                // Formats the standard text and the TMP_text.
                FormatText();
                FormatTMP_Text();
            }

            // Debug.Log("TEXT LEN: " + GetTextLengthNoFromatting().ToString());
            // Debug.Log("TMP_TEXT LEN: " + GetTMP_TextLengthNoFromatting().ToString());
        }

        // Formats the text string.
        private string FormatTextString(string inputText)
        {
            // The result.
            string result = inputText;

            // Goes through each substring.
            foreach (string substr in substrings)
            {
                // Formats the string.
                result = FormatString(result, substr);
            }

            // Overwrites the text with the formatted version.
            return result;
        }


        // Formats the text.
        public void FormatText()
        {
            // Formats the standard text object.
            if (text != null)
            {
                text.text = FormatTextString(text.text);
            }
        }

        // Formats the TMP text.
        public void FormatTMP_Text()
        {
            // Formats the standard TMP text object.
            if (TMP_text != null)
            {
                TMP_text.text = FormatTextString(TMP_text.text);
            }
        }

        // Gets the length of the set text with no formatting.
        // public int GetLengthNoFromatting()
        // {
        //     // The resulting length.
        //     int result = 0;
        // 
        //     // Validity check.
        //     if (text != null)
        //         result = GetTextLengthNoFromatting();
        //     else if (TMP_text != null)
        //         result = GetTMP_TextLengthNoFromatting();
        // 
        //     return result;
        // }

        // // Gets the length of the regular text with no formatting.
        // public int GetTextLengthNoFromatting()
        // {
        //     int result = 0;
        // 
        //     if (text != null)
        //         result = GetStringLengthNoFormatting(text.text);
        // 
        //     return result;
        // 
        // }

        // // Gets the length of the tmp_text with no formatting.
        // public int GetTMP_TextLengthNoFromatting()
        // {
        //     int result = 0;
        // 
        //     if (TMP_text != null)
        //         result = GetStringLengthNoFormatting(TMP_text.text);
        // 
        //     return result;
        // }
    }
}