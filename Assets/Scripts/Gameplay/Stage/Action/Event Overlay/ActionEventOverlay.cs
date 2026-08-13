using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace RM_EDU
{
    // The action event overlay.
    public class ActionEventOverlay : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The game object visual.
        public GameObject visualObject;

        // The event background.
        public Image backgroundImage;

        // The event text.
        public TMP_Text text;

        [Header("Animations")]

        // The animator for the event.
        public Animator animator;

        // The empty state animation.
        public string emptyStateAnim = "Empty State";

        // The action event overlay - flash animation.
        public string eventFlashAnim = "Action Event Overlay - Flash";

        // Start is called before the first frame update
        void Start()
        {
            // If the action UI is null, set it.
            if(actionUI == null)
            {
                actionUI = ActionUI.Instance;
            }
        }

        // Plays an event.
        public void PlayEvent(string textStr)
        {
            // Clears the event.
            ClearEvent();

            // Turns on the visual object.
            visualObject.gameObject.SetActive(true);

            // Sets the text string.
            text.text = textStr;

            // Plays the event flash animation.
            PlayEventFlashAnimation();
        }

        // Clears the event.
        public void ClearEvent()
        {
            // Plays the empty animation before turning the object off.
            // This is to make sure the empty state is the current state.
            visualObject.gameObject.SetActive(true);
            PlayEmptyStateAnimation();
            visualObject.gameObject.SetActive(false);

            // Clear out the text.
            text.text = string.Empty;
        }

        // Animations
        // Plays an animation.
        public void PlayAnimation(string animationName)
        {
            // This doesn't check for an empty animation since this script...
            // Only works when tied to animations.
            animator.Play(animationName);
        }

        // Plays the empty state animation.
        public void PlayEmptyStateAnimation()
        {
            PlayAnimation(emptyStateAnim);
        }

        // Plays the event flash animation.
        public void PlayEventFlashAnimation()
        {
            PlayAnimation(eventFlashAnim);
        }

    }
}