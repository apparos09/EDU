using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The EDU loading scene canvas.
    public class EDU_LoadingSceneCanvas : LoadingSceneCanvas
    {
        // The singleton instance.
        private static EDU_LoadingSceneCanvas instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private EDU_LoadingSceneCanvas()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }

            // Calls base Awake.
            base.Awake();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            // An open call isn't set as a callback since that happens when a scene is loaded...
            // And couldn't be tied to an animation anyway.

            // When the loading screen is closign animation finishes...
            // Deactivate the loading screen graphic.
            loadingGraphic.OnLoadingScreenClosingEndAddCallback(DeactivateLoadingSceneGraphic);

            // If no animation is playing, close the loading graphic.
            if(!loadingGraphic.IsAnimationPlaying())
            {
                loadingGraphic.gameObject.SetActive(false);
            }
        }

        // Gets the instance.
        public static EDU_LoadingSceneCanvas Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<EDU_LoadingSceneCanvas>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("EDU_LoadingSceneCanvas (singleton)");
                        instance = go.AddComponent<EDU_LoadingSceneCanvas>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // If the object has been instantiated, and the loading screen is being used.
        public static bool IsInstantiatedAndUsingLoadingGraphic()
        {
            // Checks if instantiated.
            if (Instantiated)
            {
                // Checks if the loading screen is being used.
                return Instance.IsUsingLoadingGraphic();
            }
            else
            {
                return false;
            }
        }

        // Sets the loading scene graphic active.
        public void SetLoadingSceneGraphicActive(bool active)
        {
            loadingGraphic.gameObject.SetActive(active);
        }

        // Activate the loading scene graphic.
        public void ActivateLoadingSceneGraphic()
        {
            SetLoadingSceneGraphicActive(true);
        }

        // Deactivate the loading scene graphic.
        public void DeactivateLoadingSceneGraphic()
        {
            SetLoadingSceneGraphicActive(false);
        }

        // Loads the scene using the loading scene graphic.
        public override void LoadScene()
        {
            // Activate the loading scene graphic.
            loadingGraphic.gameObject.SetActive(true);

            base.LoadScene();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }

            // Calls the base Destroy function.
            base.OnDestroy();
        }
    }
}