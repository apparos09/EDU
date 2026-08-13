using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The dialog for the game being complete.
    public class GameCompleteDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // If 'true', the BGM is changed when this dialog is opened/closed.
        [Tooltip("Changes the music when this dialog is opened or closed if true.")]
        public bool changeBgm = true;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // If the BGM should be changed, play the game results BGM.
            if(changeBgm)
            {
                PlayGameResultsBgm();
            }
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            // If the BGM should be changed, play the world BGM.
            if (changeBgm)
            {
                PlayWorldBgm();
            }
        }

        // Plays the world BGM.
        public void PlayWorldBgm()
        {
            // World audio exists.
            if(WorldAudio.Instantiated)
            {
                WorldAudio.Instance.PlayWorldBgm();
            }
        }

        // Plays the game results BGM.
        public void PlayGameResultsBgm()
        {
            // World audio exists.
            if (WorldAudio.Instantiated)
            {
                WorldAudio.Instance.PlayGameCompleteBgm();
            }
        }

        // Completes the game.
        public void CompleteGame()
        {
            worldUI.CompleteGame();
        }
    }
}