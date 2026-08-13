using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The stage start info.
    public abstract class StageStartInfo : GameplayStartInfo
    {
        [Header("Stage")]
        // The world index of the stage.
        public int worldStageIndex = -1;

        // The id number of the starting stage.
        public int idNumber = 0;

        // The resources that will be used.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The difficulty, which goes from 1-9. A difficulty of (0) is the default value.
        public int difficulty = 0;

        // The sound number. 0 = none, 1 = BGM 1, 2 = BGM 2
        public int bgmNumber = 0;

        // Sets the start info.
        public virtual void SetStartInfo(WorldStage worldStage)
        {
            // Sets the stage index and ID number.
            worldStageIndex = worldStage.GetWorldStageIndex();
            idNumber = worldStage.idNumber;

            // Set the difficulty and song.
            difficulty = worldStage.difficulty;
            bgmNumber = worldStage.bgmNumber;

            // Set the resources.
            naturalResources.Clear();
            naturalResources.AddRange(worldStage.naturalResources);
        }

        // Applies the start info.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If this is not a stage manager, don't do anything.
            if (manager is not StageManager)
            {
                Debug.LogWarning("Provided GameplayManager is not a StageManager.");
                return;
            }

            // Convert the manager.
            StageManager stageManager = (StageManager)manager;

            // Sets the index and id number.
            stageManager.worldStageIndex = worldStageIndex;
            stageManager.idNumber = idNumber;

            // Sets the difficulty and song number.
            stageManager.difficulty = difficulty;
            stageManager.bgmNumber = bgmNumber;

            // Gives it the natural resources.
            stageManager.naturalResources = naturalResources;
        }
    }
}