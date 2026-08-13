using UnityEngine;

namespace util
{
    // The loading scene canvas as a singleton.
    public class LoadingSceneCanvasSingleton : LoadingSceneCanvas
    {
        // This is no longer a singleton in case it needs to be derived by another class.

        // The singleton instance.
        private static LoadingSceneCanvasSingleton instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private LoadingSceneCanvasSingleton()
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
        }

        // Gets the instance.
        public static LoadingSceneCanvasSingleton Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<LoadingSceneCanvasSingleton>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Loading Graphic Canvas Singleton (singleton)");
                        instance = go.AddComponent<LoadingSceneCanvasSingleton>();
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