using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
// A collider used to do text-to-speech.
// This is used for UI.
    public class TextToSpeechCollider : MonoBehaviour
    {
        // The game settings.
        public GameSettings gameSettings;

        // The speak key for the text-to-speech.
        public string speakKey = "";

        // Start is called before the first frame update
        void Start()
        {
            // Settings
            if (gameSettings == null)
                gameSettings = GameSettings.Instance;
        }

        // // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
        // private void OnMouseDown()
        // {
        //     SpeakText();
        // }

        // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.
        private void OnMouseUpAsButton()
        {
            SpeakText();
        }

        // Speaks the text.
        public void SpeakText()
        {
            // If the speak key is set, and the game is set to use text-to-speech.
            if(speakKey != "" && gameSettings.UseTextToSpeech)
            {
                TextToSpeech.Instance.SpeakText(speakKey);
            }
        }
    }
}
