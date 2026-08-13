using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action event overlay trigger.
    public class ActionEventOverlayTrigger : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The event text.
        public string eventText;

        // The event text key.
        public string eventTextKey;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Gets the event text translated.
        public string GetEventTextTranslated()
        {
            // NOTE: this doesn't translate the text beforehand in case the event text...
            // And event text key have been changed.

            // If the LOL Manager and SDK are instantiated, translate the text.
            if(LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return LanguageManager.GetLanguageTextStatic(eventTextKey);
            }
            // Can't translate, so return the event text.
            else
            {
                return eventText;
            }
        }

        // Sets the event text to its translation if translation is available.
        public void SetEventTextToTranslation()
        {
            eventText = GetEventTextTranslated();
        }

        // Plays the event with the event text and key.
        public void PlayEvent()
        {
            // Gets the translated event text and plays the event.
            string text = GetEventTextTranslated();
            // ActionUI.Instance.actionEventOverlay.PlayEvent(text);
        }

        // Plays the event, setting the event text and event text key.
        public void PlayEvent(string eventText, string eventTextKey)
        {
            // Sets the text.
            this.eventText = eventText;
            this.eventTextKey = eventTextKey;

            // Plays the event.
            PlayEvent();
        }
    }
}