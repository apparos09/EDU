using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The enemies approaching notification.
    public class EnemiesApproachingNotification : ActionStageNotification
    {
        [Header("Enemies Approaching Notification")]

        // The action audio.
        public ActionAudio actionAudio;

        // If 'true', audio is played.
        private bool useAudio = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the action audio instance.
            if(actionAudio == null && ActionAudio.Instantiated)
            {
                actionAudio = ActionAudio.Instance;
            }
        }

        // SFX
        // Plays the warning sfx.
        public void PlayWarningSfx()
        {
            actionAudio.PlayWarningSfx();
        }

        // Stops the warning sfx.
        public void StopWarningSfx()
        {
            actionAudio.StopWarningSfx();
        }

        // ANIMATIONS
        // Called when the blink animation begins.
        public override void OnBlinkAnimationStart()
        {
            base.OnBlinkAnimationStart();

            // If audio should be used.
            if(useAudio)
            {
                // Play the warning SFX.
                PlayWarningSfx();
            }
        }

        // Called when the blink animation ends.
        public override void OnBlinkAnimationEnd()
        {
            base.OnBlinkAnimationEnd();

            // If audio should be used.
            if (useAudio)
            {
                // Stop the warning SFX.
                StopWarningSfx();
            }
        }
    }
}