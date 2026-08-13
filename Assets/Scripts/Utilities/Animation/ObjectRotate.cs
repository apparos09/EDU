using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Rotates the object.
    public class ObjectRotate : MonoBehaviour
    {
        // If 'true', the object rotates.
        [Tooltip("If true, rotation is enabled.")]
        public bool rotateEnabled = true;

        [Header("Rotation Axes")]
        // The rotations on the axes.
        public bool rotateX = false;
        public bool rotateY = false;
        public bool rotateZ = true;

        // The rotation speeds.
        [Header("Speeds (Degrees)")]
        public float speedX = 1.0F;
        public float speedY = 1.0F;
        public float speedZ = 1.0F;

        // Resets the rotation of the object.
        public void ResetRotation()
        {
            // Resets the local rotation.
            transform.localEulerAngles = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            // If the object should be rotated.
            if (rotateEnabled)
            {
                // Calculates the rotations.
                Vector3 eulers = new Vector3();
                eulers.x = (rotateX) ? speedX * Time.deltaTime : 0;
                eulers.y = (rotateY) ? speedY * Time.deltaTime : 0;
                eulers.z = (rotateZ) ? speedZ * Time.deltaTime : 0;

                // Applies the rotations.
                transform.Rotate(eulers);
            }
        }
    }
}