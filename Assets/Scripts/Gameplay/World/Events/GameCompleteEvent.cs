using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The game complete event.
    public class GameCompleteEvent : GameEvent
    {
        [Header("Game Complete Event")]

        // The world manager.
        public WorldManager worldManager;

        // Opens the game complete dialog on the event being completed.
        // If false, the dialog is ignored.
        [Tooltip("If true, a dialog is opened upon the event being completed. If false, the dialog isn't used.")]
        public bool openDialogOnComplete = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the instance.
            if(worldManager == null)
                worldManager = WorldManager.Instance;
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // Gets set to 'false' if an uncompleted stage is found.
            bool allComplete = true;

            // You could check the areas instead of the stages, and it'd be a shorter check...
            // But this also works.
            // All stages should belong to an area, but in case one of them doesn't...
            // Doing a check this way works while doing a check by area would not.

            // Goes through all stages.
            foreach(WorldStage stage in worldManager.stages)
            {
                // If the stage exists.
                if(stage != null)
                {
                    // If a stage isn't complete, then the game is complete.
                    if(!stage.IsComplete())
                    {
                        allComplete = false;
                        break;
                    }
                }
            }

            // Set cleared to allComplete.
            cleared = allComplete;

            // TODO: maybe don't check every frame.
        }

        // Called when the event is completed.
        public override void OnEventComplete()
        {
            base.OnEventComplete();

            // For some reason, when the loading screen was active, calling...
            // CompleteGame() here didn't work the first time it's called.
            // callCompleteGameInLateUpdate was made to fix this.
            // The loading screen animation wasn't using unscaled delta time before, so maybe...
            // That was the problem, but this system is still in place regardless.

            // Gets the world UI instance.
            WorldUI worldUI = WorldUI.Instance;

            // If the dialog should be used and...
            // The stage complete dialog exists, use that.
            if(openDialogOnComplete && worldUI.gameCompleteDialog != null)
            {
                worldUI.OpenGameCompleteDialog();
            }
            // Dialog isn't set, so call complete function.
            else
            {
                // worldManager.CompleteGame(); // Old
                worldManager.callCompleteGameInLateUpdate = true; // New
            }
                
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}