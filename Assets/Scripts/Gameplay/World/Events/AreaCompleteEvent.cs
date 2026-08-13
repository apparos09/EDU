using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The area complete event.
    public class AreaCompleteEvent : GameEvent
    {
        // The world manager.
        public WorldManager worldManager;

        // The area.
        public WorldArea area;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the instance.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // If the area isn't set, get the area.
            if(area == null)
                area = GetComponent<WorldArea>();
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // Gets set to 'false' if an uncompleted stage is found.
            bool allComplete = true;

            // Goes through the stages list and sees that they're all complete.
            // If there are no stages in the list, it's automatically complete.
            foreach (WorldStage stage in area.stages)
            {
                // If the stage exists.
                if (stage != null)
                {
                    // If a stage isn't complete, then the game is complete.
                    if (!stage.IsComplete())
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

            // If area buttons should be effected by events.
            if (worldManager.EffectAreaButtons)
            {
                // The next area button is interactable.
                // NOTE: maybe don't do this if they're going to be refreshed again anyway?
                worldManager.worldUI.RefreshWorldAreaButtons();

                // In 2 frames, refresh the area buttons. This is to address an issue...
                // Where the area buttons weren't being set properly.
                worldManager.refreshAreaButtonsWaitFrames = 2;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}