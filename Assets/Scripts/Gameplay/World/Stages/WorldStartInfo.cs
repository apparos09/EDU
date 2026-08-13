using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world start info.
    public class WorldStartInfo : GameplayStartInfo
    {
        [Header("World")]
        // Gets set to 'true' if coming from a stage.
        public bool fromStage = false;

        // The world index of the stage.
        public int worldStageIndex = -1;

        // The id number of the starting stage.
        public int idNumber = 0;

        // The stage type.
        public WorldStage.stageType stageType;

        // The stage time (in seconds0.
        public float stageTime = 0;

        // The score.
        public float stageScore = 0;

        // The energy total.
        public float stageEnergyTotal = 0;

        // The air pollution generated in this stage.
        public float stageAirPollution = 0;

        // Gets set to see if the stage the player is coming back from was completed.
        public bool stageCompleted;

        // Sets the start info.
        public virtual void SetStartInfo(GameplayManager manager)
        {
            // If this is a stage manager, mark that this is coming from a stage.
            if(manager is StageManager)
            {
                // Gets the stage manager.
                StageManager stageManager = manager as StageManager;

                // Gets the world stage index.
                worldStageIndex = stageManager.worldStageIndex;

                // Gets the id number.
                idNumber = stageManager.idNumber;

                // Gets the stage type.
                stageType = stageManager.GetStageType();

                // Gets the stage time.
                stageTime = stageManager.GetStageTimer();

                // Gets the stage score.
                stageScore = stageManager.GetStageScore();

                // Gets the stage energy total.
                stageEnergyTotal = stageManager.GetStageEnergyTotal();

                // Gets the stage air pollution.
                stageAirPollution = stageManager.GetStageAirPollution();

                // Set if the stage is complete.
                stageCompleted = stageManager.IsComplete();

                // Coming from stage.
                fromStage = true;
            }
        }

        // Applies the start info.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If not a world manager, return.
            if (manager is not WorldManager)
            {
                Debug.LogWarning("The provided GameplayManager is not a WorldManager.");
                return;
            }                

            // Convert the manager.
            WorldManager worldManager = manager as WorldManager;

            // If the info is coming from a stage.
            if(fromStage)
            {
                // Gets the world stage.
                WorldStage worldStage = worldManager.GetWorldStage(worldStageIndex);

                // The world stage is set.
                if(worldStage != null)
                {
                    // Sets the stage time.
                    worldStage.time = stageTime;

                    // Give the stage score.
                    worldStage.score = stageScore;

                    // Gets the stage's energy total.
                    worldStage.energyTotal = stageEnergyTotal;

                    // Sets the stage's air pollution.
                    worldStage.airPollution = stageAirPollution;

                    // Set if the stage was completed or not.
                    // If the stage was completed, show the player their rewards if applicable.
                    // The same bool can be used for the completion and show rewards parameters.
                    worldStage.SetComplete(stageCompleted, stageCompleted);

                    // Sets the current area to be this world area.
                    worldManager.SetCurrentWorldArea(worldStage.GetWorldStageArea());

                    // If the data logger exists and has been instantiated, give it the world data.
                    if(DataLogger.Instantiated)
                    {
                        // Saves the world stage data.
                        DataLogger.Instance.SaveWorldStageData(worldStage);
                    }
                    else
                    {
                        Debug.LogAssertion("The DataLogger does not exist, so the data couldn't be saved.");
                    }

                }

            }
        }
    }
}