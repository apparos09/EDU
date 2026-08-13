using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for the title area.
    public class TitleAudio : EDU_GameAudio
    {
        // The singleton instance.
        private static TitleAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Title")]

        // The title bgm.
        public AudioClip titleBgm;

        // The start of the title BGM loop.
        public float titleBgmLoopStart;
        
        // The ned of the title BGM loop.
        public float titleBgmLoopEnd;

        // If 'true', the bgm is played in start.
        public bool playBgmInStart = true;

        // Constructor
        private TitleAudio()
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

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Clamp the start and end values.
            titleBgmLoopStart = Mathf.Clamp(titleBgmLoopStart, 0.0F, titleBgm.length);
            titleBgmLoopEnd = Mathf.Clamp(titleBgmLoopEnd, 0.0F, titleBgm.length);
        
            // If the BGM should be played in start, play it.
            if(playBgmInStart)
            {
                PlayTitleBgm();
            }
        }

        // Gets the instance.
        public static TitleAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<TitleAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("TitleAudio (singleton)");
                        instance = go.AddComponent<TitleAudio>();
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

        // Plays the title BGM.
        public void PlayTitleBgm()
        {
            PlayBackgroundMusic(titleBgm, titleBgmLoopStart, titleBgmLoopEnd);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
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