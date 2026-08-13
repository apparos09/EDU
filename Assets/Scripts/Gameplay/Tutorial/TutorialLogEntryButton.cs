using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The tutorial log entry button.
    public class TutorialLogEntryButton : MonoBehaviour
    {
        // The tutorial log.
        public TutorialLog tutorialLog;

        // The button this entry is attached to.
        public Button button;

        // The text for the log entry button.
        public TMP_Text text;

        // The entry info
        public Tutorials.TutorialInfo entryInfo;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, try to get the tutorial log in the parent.
            if (tutorialLog == null)
                tutorialLog = GetComponentInParent<TutorialLog>();

            // Gets the button component.
            if (button == null)
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
            // The tutorial log exists.
            if (tutorialLog != null)
            {
                // Sets the tutorial info.
                // If the entry is null, the info is clear.
                tutorialLog.SetCurrentTutorialInfo(entryInfo);
            }
        }

        // Returns 'true' if the entry button has an entry.
        public bool HasEntry()
        {
            return entryInfo != null;
        }

        // Applies the entry info.
        public void ApplyEntryInfo()
        {
            // If the tutorial log entry exists, apply the info.
            if (entryInfo != null)
            {
                button.interactable = true;
                text.text = entryInfo.title;
            }
            else
            {
                ClearEntryInfo();
            }
        }

        // Sets the tutorials entry info for the tutorial log.
        public void ApplyEntryInfo(Tutorials.TutorialInfo newInfo)
        {
            entryInfo = newInfo;

            ApplyEntryInfo();
        }

        // Clears the entry info.
        public void ClearEntryInfo()
        {
            entryInfo = null;
            button.interactable = false;
            text.text = "-";
        }
    }
}