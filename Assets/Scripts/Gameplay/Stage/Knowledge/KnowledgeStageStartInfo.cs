using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The start info for the knowledge stage.
    public class KnowledgeStageStartInfo : StageStartInfo
    {
        [Header("Knowledge")]

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // Sets the start info from the provided stage.
        public override void SetStartInfo(WorldStage worldStage)
        {
            // If the world stage is not a knowledge stage, do nothing.
            if (worldStage is not WorldKnowledgeStage)
            {
                Debug.LogWarning("The provided WorldStage is not a WorldKnowledgeStage.");
                return;
            }
                

            // Calls the base function.
            base.SetStartInfo(worldStage);

            // Convert to a knowledge stage.
            WorldKnowledgeStage knowledgeStage = (WorldKnowledgeStage)worldStage;
        }

        // Applies the start info to the provided manager.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If this isn't the right manager type, return.
            if(manager is not KnowledgeManager)
            {
                Debug.Log("The provided GameplayManager is not a KnoweldgeManager.");
                return;
            }

            // Apply the start info.
            base.ApplyStartInfo(manager);

            // Gets the knowledge manager.
            KnowledgeManager knowledgeManager = (KnowledgeManager)manager;
        }

    }
}