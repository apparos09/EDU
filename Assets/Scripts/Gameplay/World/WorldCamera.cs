using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world camera. Attach this to a camera so that it can be moved around for world functions.
    // NOTE: this script will use transform.position of the object it's attached to.
    // This script assumes the object it's attached to is a camera, so make sure it's attached to the camera object directly.
    public class WorldCamera : MonoBehaviour
    {
        // The world manager.
        public WorldManager worldManager;

        // The camera script.
        public new Camera camera;

        [Header("Transition")]
        // The camera's destination position.
        public Vector3 destPos;

        // Keeps the camera's z-axis position the same if true.
        [Tooltip("If true, the camera's z-position is kept the same.")]
        public bool freezePosZ = true;

        // Sets set to 'true', when the camera is transitioning to another position.
        private bool inTransition = false;

        // The camera's movement speed.
        public float moveSpeed = 10.0F;

        // Start is called before the first frame update
        void Start()
        {
            // Automatically sets the manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // If not set, try to get the camera component.
            if (camera == null)
            {
                // If that doesn't work, set it as the main camera.
                if (!TryGetComponent<Camera>(out camera))
                {
                    camera = Camera.main;
                }
            }

            // Set to camera's position by default.
            destPos = transform.position;
        }

        // Returns 'true' if the camera is transitioning from one spot to another.
        public bool InTransition
        {
            get { return inTransition; }
        }

        // Sets a position using a vector 2. The camera's z-position remains uncahnged.
        public void SetPosition(Vector2 newPos)
        {
            // Gives the new position vector 3 the current z-position.
            Vector3 newPosV3 = newPos;
            newPosV3.z = transform.position.z;

            // Sets the position.
            SetPosition(newPosV3);
        }

        // Sets the position of the world camera. This doesn't cause a transition.
        public void SetPosition(Vector3 newPos)
        {
            // If the position on the z-axis should stay the same, override the z in newPos
            if(freezePosZ)
                newPos.z = transform.position.z;

            transform.position = newPos;
            destPos = newPos;
            inTransition = false;
        }


        // Sets the position of the world camera. This doesn't cause a transition.
        public void SetPosition(GameObject destObject)
        {
            SetPosition(destObject.transform.position);
        }

        // Sets the position using the provided area.
        // If there is no camera target, the position of the area is used instead.
        public void SetPosition(WorldArea destArea)
        {
            // If the destination area has a camera position, use that.
            if(destArea.cameraPos != null)
            {
                SetPosition(destArea.cameraPos.transform.position);
            }
            // No camera position, so just use the area's position.
            else
            {
                SetPosition(destArea.transform.position);
            }
        }

        // Moves the camera to the provided new position.
        // If 'instant' is true, the camera moves gradually. If false, the camera moves instantly.
        public void Move(Vector3 newPos, bool instant)
        {
            // If the camera should instantly move to its given location.
            if (instant)
            {
                SetPosition(newPos);

                // Movement finished.
                OnMovementFinished();
            }
            else
            {
                // If the z-position should remain the same, override newPos.z.
                if (freezePosZ)
                    newPos.z = transform.position.z;

                destPos = newPos;
                inTransition = true;
            }

        }

        // Sets the new position.
        // Checks 'instant' to see if the camera should move instantly to the spot or gradually move to it.
        public void Move(GameObject newPos, bool instant)
        {
            Move(newPos.transform.position, instant);
        }

        // Move to the destination area.
        public void Move(WorldArea destArea, bool instant)
        {
            // If the camera should move instantly, use the appropriate SetPosition() function.
            if (instant)
            {
                SetPosition(destArea);
            }
            else
            {
                // If the camera position exists, use that.
                if(destArea.cameraPos != null)
                {
                    Move(destArea.cameraPos.transform.position, instant);
                }
                // Camera position doesn't exist, so ise object transform.
                else
                {
                    Move(destArea.transform.position, instant);
                }
            }
        }

        // Called when the camera's movement has been finished.
        public void OnMovementFinished()
        {
            // The world camera is now in the current area and is done moving for now.
            WorldManager.Instance.OnWorldCameraInCurrentArea();
        }

        // Update is called once per frame
        void Update()
        {
            // Checks if the camera is transitioning.
            if (inTransition)
            {
                // If the camera's z-position should be kept the same, leave it as is. 
                if (freezePosZ && destPos.z != camera.transform.position.z)
                {
                    destPos.z = camera.transform.position.z;
                }

                // Generate the new position.
                Vector3 newPos = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);

                // Set the new position.
                transform.position = newPos;

                // Destination reached if true.
                if (newPos == destPos)
                {
                    // No longer in transition.
                    inTransition = false;

                    // Movement finished.
                    OnMovementFinished();
                }
            }
        }
    }

}