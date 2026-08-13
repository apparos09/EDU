using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The action stage end dialog.
    public class ActionStageEndDialog : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The dialog box image.
        public Image dialogImage;

        // The default image color.
        public Color defaultImageColor = Color.grey;

        // The user won color.
        public Color userWonImageColor = Color.yellow;

        // The user lost color.
        public Color userLostImageColor = Color.red;

        // The stage end message text.
        public TMP_Text stageEndMessageText;

        // The component for speaking text on enable.
        public SpeakTextOnEnable speakTextOnEnable;

        // The stage over message.
        public const string STAGE_OVER_MESSAGE_KEY = "asg_msg_stageOver";

        // The player user has won message key.
        // Since the LOL verison only uses generation mode, only that language text has been included.
        public const string PLAYER_USER_WON_GEN_MESSAGE_KEY = "asg_msg_userWon";
        // public const string PLAYER_USER_WON_GEN_MESSAGE_KEY = "asg_msg_userWon_gen"; // Unused

        // The player user has won message key (defense mode) - unused.
        // public const string PLAYER_USER_WON_DEF_MESSAGE_KEY = "asg_msg_userWon_def";

        // The player user lost message key.
        public const string PLAYER_USER_LOST_MESSAGE_KEY = "asg_msg_userLost";

        [Header("Stats")]

        // The stage time.
        public TMP_LabeledValue stageTime;

        // The stage score.
        public TMP_LabeledValue stageScore;

        // The stage user energy total.
        public TMP_LabeledValue stageEnergyTotal;

        // The stage air pollution.
        public TMP_LabeledValue stageAirPollution;

        [Header("Buttons")]

        // The buttons used when the player user wins.
        public GameObject userWonButtons;

        // The buttons used when the player user loses.
        public GameObject userLostButtons;

        // The world button.
        public Button worldButton;

        // The reset/restart button.
        public Button resetButton;

        // The finish button.
        public Button finishButton;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the action UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // If not set, auto set.
            if (speakTextOnEnable == null)
                speakTextOnEnable = GetComponent<SpeakTextOnEnable>();
        }

        // // This function is called when the behaviour becomes disabled or inactive.
        // private void OnDisable()
        // {
        //     // Sets to the default outcome.
        //     SetStageEndOutcomeDefault();
        // }

        // Sets the stage end outcome.
        // 0 = unknown
        // 1 = player user won
        // 2 = player user lost.
        public void SetStageEndOutcome(int outcome, bool updateStats = true)
        {
            // The number of outcomes.
            int outcomeCount = 3;

            // The message.
            string message = "";

            // Valid outcome number.
            int validOutcome = Mathf.Clamp(outcome, 0, outcomeCount - 1);

            // Both on by default for safety.
            userWonButtons.gameObject.SetActive(true);
            userLostButtons.gameObject.SetActive(true);

            // The speak key that may be used.
            string messageKey;

            // The outcome is valid.
            switch (validOutcome)
            {
                default:
                case 0: // Default
                    message = GetStageEndDefaultMessage();
                    dialogImage.color = defaultImageColor;

                    userWonButtons.gameObject.SetActive(false);
                    userLostButtons.gameObject.SetActive(true);

                    // The stage over message key.
                    messageKey = STAGE_OVER_MESSAGE_KEY;

                    break;

                case 1: // Player (User) Won
                    message = GetStageEndUserWonMessage();
                    dialogImage.color = userWonImageColor;

                    userWonButtons.gameObject.SetActive(true);
                    userLostButtons.gameObject.SetActive(false);

                    // The user won message key.
                    // Since the LOL version only uses generation mode, that's the only message key used.
                    messageKey = PLAYER_USER_WON_GEN_MESSAGE_KEY;
                    // messageKey = (GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation) ?
                    //     PLAYER_USER_WON_GEN_MESSAGE_KEY : PLAYER_USER_WON_DEF_MESSAGE_KEY;

                    break;

                case 2: // Player (User) Lost
                    message = GetStageEndUserLostMessage();
                    dialogImage.color = userLostImageColor;

                    userWonButtons.gameObject.SetActive(false);
                    userLostButtons.gameObject.SetActive(true);

                    // The user lost message key.
                    messageKey = PLAYER_USER_LOST_MESSAGE_KEY;

                    break;
            }

            // Sets the message.
            stageEndMessageText.text = message;

            // Sets the message key to the speak text on enable.
            // As long as the speak text call in this script happens after this is set...
            // It should be updated in time for TTS.
            speakTextOnEnable.key = messageKey;

            // If this script is active and enabled, and the start function in SpeakTextOnEnable...
            // Has been cleared, speak the text.
            // This is to address a bug where speak text on enable was reading an older message.
            if(isActiveAndEnabled && speakTextOnEnable.StartCleared)
            {
                // This function does safety checks.
                SpeakText(messageKey);
            }

            // Update the stage end stats.
            if(updateStats)
            {
                UpdateStageEndStats();
            }
        }

        // Sets the stage end outcome to unknown.
        public void SetStageEndOutcomeDefault()
        {
            SetStageEndOutcome(0);
        }

        // Sets the stage end outcome to user won.
        public void SetStageEndOutcomeUserWon()
        {
            SetStageEndOutcome(1);
        }

        // Sets the stage end outome to user lost.
        public void SetStageEndOutcomeUserLost()
        {
            SetStageEndOutcome(2);
        }

        // Automatically sets the stage end outcome using the action manager.
        public void AutoSetStageEndOutcome()
        {
            // Grabs the action manager instance.
            ActionManager actionManager = ActionManager.Instance;

            // Checks the winner to know what to set the dialog to.
            if (actionManager.HasPlayerUserWon())
            {
                SetStageEndOutcomeUserWon();
            }
            else if (actionManager.HasPlayerEnemyWon())
            {
                SetStageEndOutcomeUserLost();
            }
            else
            {
                SetStageEndOutcomeDefault();
            }
        }

        // MESSAGES
        // Gets the stage end unknown message.
        public string GetStageEndDefaultMessage()
        {
            // If the LOLSDK is set, use the translated message.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return SystemManager.GetLanguageTextStatic(STAGE_OVER_MESSAGE_KEY);
            }
            // Use default message.
            else
            {
                return "The stage is over!";
            }
        }

        // Gets the user won message.
        public string GetStageEndUserWonMessage()
        {
            // Set to 'true' if the game is using generation mode.
            bool genMode = GameSettings.Instance.gameplayMode == GameSettings.gameMode.generation;

            // If the LOLSDK is set, use the translated message.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return SystemManager.GetLanguageTextStatic(PLAYER_USER_WON_GEN_MESSAGE_KEY);

                // // Returns the generation mode message.
                // if(genMode)
                // {
                //     return LOLManager.GetLanguageTextStatic(PLAYER_USER_WON_GEN_MESSAGE_KEY);
                // }
                // // Returns the defense mode message.
                // else
                // {
                //     return LOLManager.GetLanguageTextStatic(PLAYER_USER_WON_DEF_MESSAGE_KEY);
                // }
            }
            // Use default message.
            else
            {
                // No translation.
                // Returns the generation mode message.
                if(genMode)
                {
                    return "Your side has won! You've generated enough energy!";
                }
                // Returns the defense mode message.
                else
                {
                    return "Your side has won! The enemy side has run out of power!"; // Old (creator written)
                    // return "Your side has won! The enemy side have run out of power!"; // New (reviewer edited)

                }
            }
        }

        // Gets the user lost message.
        public string GetStageEndUserLostMessage()
        {
            // If the LOLSDK is set, use the translated message.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                return SystemManager.GetLanguageTextStatic(PLAYER_USER_LOST_MESSAGE_KEY);
            }
            // Use default message.
            else
            {
                return "The enemy side has won! Enemies have made it into the base!";
            }
        }

        // Updates the stage end stats.
        public void UpdateStageEndStats()
        {
            // Gets the action manager instance.
            ActionManager actionManager = ActionManager.Instance;

            // Updates the values.
            stageTime.valueText.text = StringFormatter.FormatTime(actionManager.stageTimer, false, true, false);
            stageScore.valueText.text = Mathf.CeilToInt(actionManager.GetStageScore()).ToString();
            stageEnergyTotal.valueText.text = Mathf.CeilToInt(actionManager.GetStageEnergyTotal()).ToString();
            stageAirPollution.valueText.text = Mathf.CeilToInt(actionManager.GetStageAirPollution()).ToString();
        }

        // Speak text using the provided key.
        // This function checks if TTS is available and the key is valid.
        public void SpeakText(string key)
        {
            // Checks if the instances exist: LOL SDK, Text-to-Speech, and GameSettings.
            // Also checks that the key is set.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized() && TextToSpeech.Instantiated &&
                GameSettings.Instantiated && key != "")
            {
                // Gets the instances.
                GameSettings gameSettings = GameSettings.Instance;
                SystemManager lolManager = SystemManager.Instance;

                // Checks if TTS should be used, speak the key.
                if (gameSettings.UseTextToSpeech)
                {
                    // Grabs the LOL Manager to trigger text-to-speech.
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }

        // Quits the stage, which goes to the results scene.
        public void QuitStage()
        {
            actionUI.QuitStage();
        }

        // Called to finish the stage.
        public void FinishStage()
        {
            actionUI.FinishStage();
        }

        // Resets the stage.
        public void ResetStage()
        {
            actionUI.ResetStage();
        }

    }
}