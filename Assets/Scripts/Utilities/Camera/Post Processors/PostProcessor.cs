using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sources:
 *  - https://www.youtube.com/watch?v=ahplcYCmfG0
 *  - https://www.youtube.com/watch?v=gY1Mx4kkZPU
 *  - https://docs.unity3d.com/Manual/Shaders.html
 *  - https://docs.unity3d.com/Manual/PostProcessingOverview.html
 *  - https://docs.unity3d.com/ScriptReference/Graphics.Blit.html
 *  - https://docs.unity3d.com/ScriptReference/RenderTexture.html
 *  - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/RenderTexture.GetTemporary.html
 *  - https://docs.unity3d.com/540/Documentation/Manual/WritingImageEffects.html
 *  - https://docs.unity3d.com/540/Documentation/Manual/SL-CameraDepthTexture.html
 *  - https://docs.unity3d.com/es/530/Manual/SL-DataTypesAndPrecision.html
 *  - https://docs.unity3d.com/Manual/shader-writing.html
 *  - https://docs.unity3d.com/Manual/built-in-shader-examples.html
 *  - https://stackoverflow.com/questions/45402927/unity-apply-post-processing-effects-to-all-ui-elements
 */

namespace util
{
    // If 'true', the effects are executed in edit mode and scene view.
    // [ExecuteInEditMode, ImageEffectAllowedInSceneView]
    public class PostProcessor : MonoBehaviour
    {
        // The camera the post processor is attached to.
        public new Camera camera;

        // The shader for the post processor.
        public Shader postShader;

        // The material for the post processor.
        protected Material postMaterial;

        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            TrySetCamera();
            TryCreateMaterial();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the shader has not been set.
            // If rendering is attempted without a shader, the final result is black.
            if(postShader == null)
            {
                Debug.LogWarning("No shader set.");
            }
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Tries to create the material if it doesn't exist yet.
            TryCreateMaterial();

            // You can make a new render texture, but it's better to make a temporary one...
            // Since it automatically gets deleted.
            // If this was a depth texture you could put in something for the depth buffer.
            RenderTexture renderTexture = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                source.format
                );

            // e.g. copies the source texture into the destination texture.
            // Graphics.Blit(source, destination);

            // Blits the source to the render texture...
            // Then blits it to the destination.
            // These functions default the pass to -1, which makes it draw all saved passes.
            // You can manually set it to only do a certain pass.
            Graphics.Blit(source, renderTexture, postMaterial);
            Graphics.Blit(renderTexture, destination);

            // Clears the render texture since it's been blitted.
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        // Tries to set the camera the post processor is attached to.
        // Returns 'true' if the camera is set. Returns false if the camera...
        // Couldn't be set. If 'overwrite' is false, the camera object won't be overwritten...
        // If one is saved. Such a case will return false.
        public bool TrySetCamera(bool overwrite = false)
        {
            // The result.
            bool result;

            // Checks if the saved camera should be overwritten.
            if(overwrite)
            {
                // Tries to get the camera. This overwrites the set camera if it exists.
                result = TryGetComponent(out camera);
            }
            else
            {
                // If the camera is null, try to get the component.
                // If it's set, return false, since it shouldn't be overwritten.
                if (camera == null)
                    result = TryGetComponent(out camera);
                else
                    result = false;
            }

            return result;
        }

        // Tries to create the material that will be used.
        protected bool TryCreateMaterial()
        {
            // If the material does not exist, make a material using the shader.
            if (postMaterial == null)
            {
                // // If there is no shader, don't allow the creation of the material.
                // if (postShader == null)
                //     return false;

                // Creates the material.
                postMaterial = new Material(postShader);
                return true;
            }
            else
            {
                // The material exists, so nothing to create.
                return false;
            }
        }

        // Destroys the saved material if it exists.
        // Call this if the shader has been replaced so that the material is remade.
        public void DestroyMaterial()
        {
            if(postMaterial != null)
                Destroy(postMaterial);
        }

        // Update is called once per frame
        protected void Update()
        {
            // ...
        }
    }
}