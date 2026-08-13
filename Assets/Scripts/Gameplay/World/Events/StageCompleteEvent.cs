using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // An event that's completed when a stage is completed.
    public class StageCompleteEvent : GameEvent
    {
        [Header("Stage Complete")]

        // The world stage.
        public WorldStage worldStage;

        // Start is called before the first frame update
        protected override void Start()
        {
            // If the world stage isn't set, set it.
            if (worldStage == null)
                worldStage = GetComponent<WorldStage>();

            base.Start();
        }

        // Initializes the event.
        public override void InitalizeEvent()
        {
            // Refreshes the complete parameter.
            worldStage.RefreshComplete();

            // Call base.
            base.InitalizeEvent();
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // If the world stage is complete.
            if(worldStage.IsComplete())
            {
                cleared = true;   
            }
        }

        // Called when the event is completed.
        public override void OnEventComplete()
        {
            // Sets the stage to complete.
            worldStage.SetComplete(true);

            // Call base.
            base.OnEventComplete();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}