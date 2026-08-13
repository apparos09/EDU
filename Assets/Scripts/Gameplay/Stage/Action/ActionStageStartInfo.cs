using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // The action stage start info.
    public class ActionStageStartInfo : StageStartInfo
    {
        [Header("Action")]

        // The defense units to use for the stage.
        public List<int> actionDefenseIds = new List<int>();

        // Sets the start info using the provided world stage.
        public override void SetStartInfo(WorldStage worldStage)
        {
            // If the world stage is not an action stage, return.
            if (worldStage is not WorldActionStage)
            {
                Debug.LogWarning("The provided WorldStage is not a WorldActionStage.");
                return;
            }

            // Call the base function.
            base.SetStartInfo(worldStage);

            // Convert to a action stage.
            WorldActionStage actionStage = (WorldActionStage)worldStage;

            // If the data logger exists, clear the defense id list and add the defense units...
            // Saved in the data logger.
            if(DataLogger.Instantiated)
            {
                actionDefenseIds.Clear();
                actionDefenseIds.AddRange(DataLogger.Instance.defenseIds);
            }
        }

        // Applies the start info.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If this isn't the right manager type, return.
            if (manager is not ActionManager)
            {
                Debug.LogWarning("The provided GameplayManager is not an ActionManager.");
                return;
            }            

            // Apply the start info.
            base.ApplyStartInfo(manager);

            // Gets the action manager.
            ActionManager actionManager = (ActionManager)manager;

            // If there are action defense ids, use them to set the defense prefabs for the player user.
            if(actionDefenseIds.Count > 0)
            {
                // The action manager's stage initialize function will be used to set them to the object.
                actionManager.userDefenseIds.Clear();
                actionManager.userDefenseIds.AddRange(actionDefenseIds);
            }
                
        }
    }
}