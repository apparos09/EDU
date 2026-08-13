using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The EDU version of the asynchronous scene laoder.
    public class EDU_AsyncSceneLoader : AsyncSceneLoader
    {
        // The loading screen graphic.
        public LoadingSceneGraphic loadingSceneGraphic;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, grab the loading scene graphic.
            if (loadingSceneGraphic == null)
                loadingSceneGraphic = GetComponent<LoadingSceneGraphic>();
        }

        // Called when the load scene async function is complete.
        public override void OnLoadSceneAsyncComplete()
        {
            base.OnLoadSceneAsyncComplete();
            
            // Play the closing animation to end the loading screen.
            loadingSceneGraphic.PlayLoadingGraphicClosingAnimation();
        }

    }
}