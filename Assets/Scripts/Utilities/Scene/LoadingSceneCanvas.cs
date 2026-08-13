using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The canvas for the loading graphic.
    // TODO: the async scene loader probably shouldn't be attached to the graphic.
    public class LoadingSceneCanvas : MonoBehaviour
    {
        // The canvas this script is attached to.
        public Canvas canvas;

        // The loading scene graphic.
        public LoadingSceneGraphic loadingGraphic;

        // If 'true', the loading graphic is used.
        private const bool USE_LOADING_GRAPHIC = true;

        // If 'true', the loading screen canvas isn't destroyed on load. This is applied in Start()
        [Tooltip("If 'true', this object is set to not be destroyed on load. This is set in the Start() function.")]
        public bool dontDestroyOnLoadOnStart = true;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Canvas is not set.
            if (canvas == null)
            {
                // Tries to set the canvas.
                if (!TryGetComponent(out canvas))
                {
                    Debug.LogWarning("The canvas component couldn't be found.");
                }
            }

            // Sets to not destroy this object if true.
            // This is done because this graphic is used for loading scenes.
            if (dontDestroyOnLoadOnStart)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        // Returns 'true' if the loading screen graphic should be used.
        // Returns 'false' if the loading screen graphic isn't being used...
        // The canvas is null or the loadingGraphic is null.
        // TODO: if this returns false, the loading scene won't work, since it relies on the loading scene graphic.
        // Either make this clearer, or move the loading functions to the loading scene canvas.
        public bool IsUsingLoadingGraphic()
        {
            // If loading should be used, and the related objects exist.
            return USE_LOADING_GRAPHIC && canvas != null && loadingGraphic != null;
        }

        // Gets the next scene.
        public virtual string GetNextScene()
        {
            return loadingGraphic.nextScene;
        }

        // Sets the next scene.
        public virtual void SetNextScene(string sceneName)
        {
            loadingGraphic.nextScene = sceneName;
        }

        // Loads the scene using the loading scene graphic.
        public virtual void LoadScene()
        {
            // Plays the loading screen opening animation.
            // If there is no opening animation, it calls start and end.
            loadingGraphic.PlayLoadingGraphicOpeningAnimation();
        }

        // Sets the scene name and runs the loading screen.
        public virtual void LoadScene(string sceneName)
        {
            SetNextScene(sceneName);
            LoadScene();
        }

        // Returns 'true' if the animation is playing.
        public virtual bool IsAnimationPlaying()
        {
            return loadingGraphic.IsAnimationPlaying();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // ...
        }

    }
}