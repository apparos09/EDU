using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for knowledge stages.
    public class KnowledgeAudio : StageAudio
    {
        // The singleton instance.
        private static KnowledgeAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // [Header("Knowledge")]
        // 
        // // Sound effect for the statement button.
        // public AudioClip statementButtonSfx;
        // 
        // // Sound effect for the resource butotn.
        // public AudioClip resourceButtonSfx;
        // 
        // // Sound effect for the verify button.
        // public AudioClip verifyButtonSfx;

        // Constructor
        private KnowledgeAudio()
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
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }

        // Gets the instance.
        public static KnowledgeAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("KnowledgeAudio (singleton)");
                        instance = go.AddComponent<KnowledgeAudio>();
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

        // // Plays the statement button sound effect.
        // public void PlayStatementButtonSfx()
        // {
        //     PlaySoundEffect(statementButtonSfx, false);
        // }
        // 
        // // Plays the resource button sound effect.
        // public void PlayResourceButtonSfx()
        // {
        //     PlaySoundEffect(resourceButtonSfx, false);
        // }
        // 
        // // Plays the verify button sound effect.
        // public void PlayVerifyButtonSfx()
        // {
        //     PlaySoundEffect(verifyButtonSfx, false);
        // }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}