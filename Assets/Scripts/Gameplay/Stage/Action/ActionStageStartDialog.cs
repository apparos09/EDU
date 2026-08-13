using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action stage start dialog.
    public class ActionStageStartDialog : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The start button.
        public Button startButton;

        // These are all false since these functions are handled in Action UI.

        // If 'true', StopStage() is called in start.
        public bool stopStageInStart = false;

        // If 'true', the stage is stopped on disable.
        public bool stopStageOnEnable = false;

        // If 'true', the stage is started on disable.
        public bool startStageOnDisable = false;

        // Start is called before the first frame update
        void Start()
        {
            // If the action ui isn't set, set it.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Stops the stage in the start function if true.
            if(stopStageInStart)
            {
                StopStage();
            }
        }

        // This function is called when the object becomes enable and active.
        private void OnEnable()
        {
            // If the stage should be stopped on enable.
            if (stopStageOnEnable)
                StopStage();
        }

        // This function is called when the behaviour becomes disabled and inactive.
        private void OnDisable()
        {
            // If the start should be started on disable.
            if (startStageOnDisable)
                StartStage();
        }

        // Starts the stage, which also closes this dialog.
        public void StartStage()
        {
            // Starts the stage the stage BGM.
            ActionManager actionManager = ActionManager.Instance;
            actionManager.SetStagePlaying(true);
            actionManager.PlayStageBgm();

            // Closes the dialog.
            ActionUI.Instance.CloseStageStartDialog();
        }

        // Stops the stage, which also makes sure this dialog is open.
        public void StopStage()
        {
            ActionManager.Instance.SetStagePlaying(false);

            // Grabs the audio instance.
            ActionAudio actionAudio = ActionAudio.Instance;

            // If the stage preparation BGM isn't playing, play it.
            if(!actionAudio.IsStagePreparationBgmPlaying())
            {
                ActionAudio.Instance.PlayStagePreparationBgm();
            }
        }
    }
}