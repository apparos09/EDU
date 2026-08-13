using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The dialog for selecting the game mode.
    public class GameModeDialog : MonoBehaviour
    {
        // The title UI.
        public TitleUI titleUI;

        // The generation mode button.
        public Button generationModeButton;

        // The defense mode button.
        public Button defenseModeButton;

        // The game settings object, which is given the mode.
        public GameSettings gameSettings;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the title UI instance.
            if (titleUI == null)
                titleUI = TitleUI.Instance;

            // Gets the game settings instance.
            if (gameSettings == null)
                gameSettings = GameSettings.Instance;
        }

        // Sets the gameplay mode.
        public void SetGameMode(GameSettings.gameMode newGameMode)
        {
            gameSettings.gameplayMode = newGameMode;
            OnModeSelected();
        }

        // Sets the current mode to generation mode.
        public void SetToGenerationMode()
        {
            SetGameMode(GameSettings.gameMode.generation);
        }

        // Sets the current mode to defense mode.
        public void SetToDefenseMode()
        {
            SetGameMode(GameSettings.gameMode.defense);
        }

        // Called when a mode has been selected.
        protected void OnModeSelected()
        {
            // Starts the new game.
            titleUI.titleManager.StartNewGame();

            // Closes the dialog (unneeded).
            // titleUI.CloseDialog(gameObject);
        }

        // // Closes the dialog.
        // public void CloseDialog()
        // {
        //     // Close this dialog.
        //     titleUI.CloseDialog(gameObject);
        // }

    }
}