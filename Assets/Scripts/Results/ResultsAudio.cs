using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The audio for the results screen.
    public class ResultsAudio : EDU_GameAudio
    {
        // The singleton instance.
        private static ResultsAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;


        [Header("Results")]

        // The results bgm.
        public AudioClip resultsBgm;

        // The start of the results BGM loop.
        public float resultsBgmLoopStart;

        // The ned of the results BGM loop.
        public float resultsBgmLoopEnd;

        // If 'true', the bgm is played in start.
        public bool playBgmInStart = true;

        // Constructor
        private ResultsAudio()
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
            resultsBgmLoopStart = Mathf.Clamp(resultsBgmLoopStart, 0.0F, resultsBgm.length);
            resultsBgmLoopEnd = Mathf.Clamp(resultsBgmLoopEnd, 0.0F, resultsBgm.length);

            // If the BGM should be played in start, play it.
            if (playBgmInStart)
            {
                PlayResultsBgm();
            }
        }

        // Gets the instance.
        public static ResultsAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ResultsAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ResultsAudio (singleton)");
                        instance = go.AddComponent<ResultsAudio>();
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

        // Plays the results BGM.
        public void PlayResultsBgm()
        {
            PlayBackgroundMusic(resultsBgm, resultsBgmLoopStart, resultsBgmLoopEnd);
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