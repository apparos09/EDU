using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // An area in the world.
    public class WorldArea : MonoBehaviour
    {
        // The world manager.
        public WorldManager worldManager;

        // A game object used for positioning the camera.
        // This world position is used to change the camera's position.
        public GameObject cameraPos;

        // The sprite for the area.
        public SpriteRenderer background;

        // The stages in the area.
        public List<WorldStage> stages = new List<WorldStage>();

        // The area complete event.
        public AreaCompleteEvent areaCompleteEvent;

        // Start is called before the first frame update
        void Start()
        {
            // If the world manager isn't set, grab the instance.
            if(worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }

            // If not set, try to get component.
            if (areaCompleteEvent == null)
                areaCompleteEvent = GetComponent<AreaCompleteEvent>();

            // Finds all the stage children and puts them into the list if the list is empty.
            if (stages.Count <= 0)
            {
                stages.Clear();
                stages.AddRange(GetComponentsInChildren<WorldStage>());
            }
        }

        // Returns 'true' if all stages in this area are complete.
        public bool AllStagesComplete()
        {
            // The result to return.
            bool result = true;

            // Checks if stages exist.
            if (stages.Count > 0)
            {
                // Goes through all stages.
                for (int i = 0; i < stages.Count; i++)
                {
                    // Stage exists.
                    if (stages[i] != null)
                    {
                        // If an incomplete area has been found, the area isn't complete.
                        if (!stages[i].IsComplete())
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            // No stages, so true by default.
            else
            {
                result = true;
            }

            return result;
        }

        // Returns 'true' if the world area has been cleared.
        public bool IsAreaCompleteEventCleared()
        {
            return areaCompleteEvent.cleared;
        }

        // Checks if all stages in the area are complete. If so, the area is complete.
        // If the area has no stages, this returns true.
        public bool IsComplete()
        {
            // The result to be returned.
            bool result;

            // If an area complete event exists, check that.
            if(areaCompleteEvent != null)
            {
                result = areaCompleteEvent.cleared;
            }
            // No area complete event, so manually check.
            else
            {
                result = AllStagesComplete();   
            }

            return result;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}