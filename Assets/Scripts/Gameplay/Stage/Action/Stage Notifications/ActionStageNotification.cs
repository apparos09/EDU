using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A notification that shows up in action stages.
    public class ActionStageNotification : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The visual object.
        public GameObject visualObject;

        // The background image of the notification.
        public Image backgroundImage;

        // The notification text.
        public TMP_Text text;

        // The TMP text translator.
        public TMP_TextTranslator textTranslator;

        [Header("Animation")]
        // The animator.
        public Animator animator;

        // The empty state animation.
        public string emptyStateAnim = "Empty State";

        // The blink animation.
        public string blinkAnim = "Action Stage Notification - Blinking Animation";

        // If 'true', the notification text is read when the blinking animation is played.
        [Tooltip("If true, speaks the text when the blinking animation is played if possible")]
        public bool speakTextOnBlinkStart = true;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Gets the action UI instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Makes sure the visual object is off.
            if (visualObject != null)
                visualObject.gameObject.SetActive(false);
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

        // Plays the blink animation.
        public virtual void PlayBlinkingAnimation()
        {
            PlayAnimation(blinkAnim);
        }

        // Called when the blink animation begins.
        public virtual void OnBlinkAnimationStart()
        {
            // Speaks the text on blink.
            if(speakTextOnBlinkStart)
            {
                SpeakText();
            }
        }

        // Called when the blink animation ends.
        public virtual void OnBlinkAnimationEnd()
        {
            PlayEmptyStateAnimation();
        }

        // Reads the notification message.
        public void SpeakText()
        {
            // If the key is set, LOL manager is istantiated, the LOL_SDK is instantiated...
            // And the game settings are instantiated.
            if(textTranslator.key != "" && LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized() && GameSettings.Instantiated)
            {
                // TTS is enabled.
                if(GameSettings.Instance.UseTextToSpeech)
                {
                    LanguageManager.Instance.SpeakText(textTranslator.key);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}