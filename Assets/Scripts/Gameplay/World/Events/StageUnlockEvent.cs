using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // An event used to unlock a stage.
    public class StageUnlockEvent : GameEvent
    {
        [Header("Stage Unlock")]

        // The world stage.
        public WorldStage worldStage;

        // The list of required stages. All required stages must be completed to unlock this stage.
        [Tooltip("The stages that must be completed to unlock this stage.")]
        public List<WorldStage> requiredStages = new List<WorldStage>();

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
            // Sets by default if the stage should be locked or unlocked.
            worldStage.SetLocked(!AreRequirementsMet());

            // Call base.
            base.InitalizeEvent();
        }

        // Returns 'true' if the requirements have been met.
        public bool AreRequirementsMet()
        {
            // True by default.
            bool result = true;

            // Goes through the required stages.
            foreach(WorldStage reqStage in requiredStages)
            {
                // The stage exists.
                if(reqStage != null)
                {
                    // If a stage isn't complete, the stage should remain locked.
                    if(!reqStage.IsComplete())
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // Checks of the requirements have been met.
            bool result = AreRequirementsMet();

            // If the required stages have been completed.
            if (result)
            {
                cleared = true;
            }
        }

        // Called when the event is completed.
        public override void OnEventComplete()
        {
            // Unlocks the stage.
            worldStage.UnlockStage();

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