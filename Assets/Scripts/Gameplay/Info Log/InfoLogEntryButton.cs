using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The info log entry button.
    public class InfoLogEntryButton : MonoBehaviour
    {
        // The info log this entry button belongs to.
        public InfoLog infoLog;

        // The button this entry is attached to.
        public Button button;

        // The text for the log entry button.
        public TMP_Text text;

        // The info log entry.
        public InfoLog.InfoLogEntry entry;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, try to get the info log in the parent.
            if (infoLog == null)
                infoLog = GetComponentInParent<InfoLog>();

            // Gets the button component.
            if(button == null)
                button = GetComponent<Button>();

            // Adds a Select() call on the button.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // The info log exists.
            if(infoLog != null)
            {
                // Sets the info.
                // If the entry is null, the info is clear.
                infoLog.SetInfo(entry);
            }
        }

        // Returns 'true' if the entry button has an entry.
        public bool HasEntry()
        {
            return entry != null;
        }

        // Applies the entry info.
        public void ApplyEntryInfo()
        {
            // If the entry exists, apply the info.
            if(entry != null)
            {
                button.interactable = true;
                text.text = entry.name;
            }
            else
            {
                ClearEntryInfo();
            }
        }

        // Sets the entry for the info log.
        public void ApplyEntryInfo(InfoLog.InfoLogEntry newEntry)
        {
            entry = newEntry;

            ApplyEntryInfo();
        }

        // Clears the entry info.
        public void ClearEntryInfo()
        {
            entry = null;
            button.interactable = false;
            text.text = "-";
        }

    }
}