using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for the world scene.
    public class WorldAudio : EDU_GameAudio
    {
        // The singleton instance.
        private static WorldAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;


        [Header("World")]

        // The world bgm.
        public AudioClip worldBgm;

        // The start of the world BGM loop.
        public float worldBgmLoopStart;

        // The ned of the world BGM loop.
        public float worldBgmLoopEnd;

        // If 'true', the bgm is played in start.
        public bool playWorldBgmInStart = false;

        // The game complete BGM, which is the stage results theme.
        public AudioClip gameCompleteBgm;

        // Game complete loop start.
        public float gameCompleteBgmLoopStart;

        // Stage results loop end.
        public float gameCompleteBgmLoopEnd;

        // Constructor
        private WorldAudio()
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
            worldBgmLoopStart = Mathf.Clamp(worldBgmLoopStart, 0.0F, worldBgm.length);
            worldBgmLoopEnd = Mathf.Clamp(worldBgmLoopEnd, 0.0F, worldBgm.length);

            // If the BGM should be played in start, play it.
            if (playWorldBgmInStart)
            {
                PlayWorldBgm();
            }
        }

        // Gets the instance.
        public static WorldAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<WorldAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldAudio (singleton)");
                        instance = go.AddComponent<WorldAudio>();
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

        // Plays the world BGM.
        public void PlayWorldBgm()
        {
            PlayBackgroundMusic(worldBgm, worldBgmLoopStart, worldBgmLoopEnd);
        }

        // Returns 'true' if the world BGM is playing.
        public bool IsPlayingWorldBgm()
        {
            return bgmSource.isPlaying && bgmSource.clip == worldBgm;
        }

        // Plays the game complete BGM, which is the stage results BGM.
        public void PlayGameCompleteBgm()
        {
            PlayBackgroundMusic(gameCompleteBgm, gameCompleteBgmLoopStart, gameCompleteBgmLoopEnd);
        }

        // Returns 'true' if the game complete BGM is playing.
        public bool IsPlayingGameCompleteBgm()
        {
            return bgmSource.isPlaying && bgmSource.clip == gameCompleteBgm;
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