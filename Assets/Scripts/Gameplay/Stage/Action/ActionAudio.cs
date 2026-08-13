using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for action stages.
    public class ActionAudio : StageAudio
    {
        // The singleton instance.
        private static ActionAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Action")]

        // The action manager.
        public ActionManager actionManager;

        // The warning audio source.
        public AudioSource warningSource;

        // Set to true if the warning audio source is marked as being paused.
        private bool warningSourceMarkedPaused = false;

        // The enemy attack audio source (loops).
        public AudioSource enemyAttackSource;

        // The number of calls that have happened for the enemy attack source.
        // Calling play increments this, and calling stop decrements this.
        // When there are no more calls, the source is stopped.
        [Tooltip("Tracks how many play calls have been made for enemy attack. Decreases for every stop call.")]
        public int enemyAttackPlayCalls;

        // Set to true if the enemy attack source is marked as being paused.
        private bool enemyAttackSourceMarkedPaused = false;

        // Constructor
        private ActionAudio()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();

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

            // Sets the instance.
            if (actionManager == null)
            {
                actionManager = ActionManager.Instance;
            }
        }
        

        // Gets the instance.
        public static ActionAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ActionAudio (singleton)");
                        instance = go.AddComponent<ActionAudio>();
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

        // WARNING SFX
        // Plays the warning sound effect (loops).
        public void PlayWarningSfx()
        {
            // Plays the warning SFX source.
            if(!warningSource.isPlaying)
            {
                warningSource.Play();

                // Since it's now playing, it's not paused.
                warningSourceMarkedPaused = false;
            }
        }

        // Stops the warning sound effect.
        public void StopWarningSfx()
        {
            // Stops the source and marks that it's not paused.
            warningSource.Stop();
            warningSourceMarkedPaused = false;
        }

        // Sets the warning SFX paused.
        public void SetWarningSfxPaused(bool paused)
        {
            // Pause
            if (paused)
            {
                warningSource.Pause();
                warningSourceMarkedPaused = true;
            }
            // Unpause
            else
            {
                // Unpause the warning.
                warningSource.UnPause();
                warningSourceMarkedPaused  = false;
            }
        }

        // Pauses the warning SFX.
        public void PauseWarningSfx()
        {
            SetWarningSfxPaused(true);
        }

        // Unpauses the warning SFX.
        public void UnpauseWarningSfx()
        {
            SetWarningSfxPaused(false);
        }

        // Returns 'true' if the warning source is playing.
        public bool IsWarningSourcePlaying()
        {
            return warningSource.isPlaying;
        }

        // Returns 'true' if warning source is marked as being paused.
        public bool IsWarningSourceMarkedPaused()
        {
            return warningSourceMarkedPaused;
        }

        // Returns 'true' if the warning source isn't playing and is marked as paused.
        public bool IsWarningSourceNotPlayingAndMarkedPaused()
        {
            return !warningSource.isPlaying && warningSourceMarkedPaused;
        }

        // ENEMY ATTACK SFX
        // Plays the enemy attack.
        public void PlayEnemyAttackSfx()
        {
            // Play the enemy attack if it isn't playing and it isn't paused.
            if(!enemyAttackSource.isPlaying && !enemyAttackSourceMarkedPaused)
            {
                enemyAttackSource.Play();

                // Since it's now playing, it's not paused.
                enemyAttackSourceMarkedPaused = false;
            }

            // Increase the number of calls.
            enemyAttackPlayCalls++;
        }

        // Stops the enemy attack SFX if no one is using it anymore.
        // ignoreCalls: if true, the enemy attack SFX is stopped and the call count is set to 0.
        // - If false, the call count is decreased. If there are no more calls, the SFX is stopped.
        public void StopEnemyAttackSfx(bool ignoreCalls)
        {
            // Checks if calls should be ignored.
            if(ignoreCalls)
            {
                // No more calls.
                enemyAttackPlayCalls = 0;

                // Stops the audio source.
                enemyAttackSource.Stop();
                enemyAttackSourceMarkedPaused = false;
            }
            else
            {
                // Reduce calls.
                enemyAttackPlayCalls--;

                // If true, stop the sound.
                if (enemyAttackPlayCalls <= 0)
                {
                    enemyAttackPlayCalls = 0;
                    enemyAttackSource.Stop();
                    enemyAttackSourceMarkedPaused = false;
                }
            }
        }

        // Sets the enemy attack SFX paused.
        public void SetEnemyAttackSfxPaused(bool paused)
        {
            // Pause
            if(paused)
            {
                enemyAttackSource.Pause();
                enemyAttackSourceMarkedPaused = true;
            }
            // Unpause
            else
            {
                // Unpause the enemy attack.
                enemyAttackSource.UnPause();
                enemyAttackSourceMarkedPaused = false;

                // If there aren't any calls, stop the attack sound effect.
                if (enemyAttackPlayCalls <= 0)
                    StopEnemyAttackSfx(true);
            }
        }

        // Pauses the enemy attack SFX.
        public void PauseEnemyAttackSfx()
        {
            SetEnemyAttackSfxPaused(true);
        }

        // Unpauses the enemy attack SFX.
        public void UnpauseEnemyAttackSfx()
        {
            SetEnemyAttackSfxPaused(false);
        }

        // Returns 'true' if the enemy attack source is playing.
        public bool IsEnemyAttackSourcePlaying()
        {
            return enemyAttackSource.isPlaying;
        }

        // Returns 'true' if the enemy attack source is marked as being paused.
        public bool IsEnemyAttackSourceMarkedPaused()
        {
            return enemyAttackSourceMarkedPaused;
        }

        // Returns 'true' if the enemy attack source isn't playing and is marked as paused.
        public bool IsEnemyAttackSourceNotPlayingAndMarkedPaused()
        {
            return !enemyAttackSource.isPlaying && enemyAttackSourceMarkedPaused;
        }

        // Called when the stage is being reset.
        public override void ResetStage()
        {
            base.ResetStage();

            // Stops the enemy attack audio source.
            StopEnemyAttackSfx(true);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Checks if the stage is playing and runs related code.
            // This doesn't check for the game being paused since code within this conditional statement...
            // Is run based on if the game is paused or not.
            if(actionManager.IsStagePlaying())
            {
                // Saves if the game is paused or not.
                bool gamePaused = actionManager.IsGamePaused();

                // Warning
                // If the warning sound is playing.
                if (warningSource.isPlaying)
                {
                    // The game is paused, so make sure the warning sound is also paused.
                    if (gamePaused && !warningSourceMarkedPaused)
                    {
                        PauseWarningSfx();
                    }
                    // If the game isn't paused, but the warning source is marked as paused, unpause it.
                    else if(!gamePaused && warningSourceMarkedPaused)
                    {
                        UnpauseWarningSfx();
                    }
                }
                else
                {
                    // If the game isn't paused but the warning sound is paused, unpause it.
                    if (!gamePaused && warningSourceMarkedPaused)
                    {
                        UnpauseWarningSfx();
                    }
                }

                // Enemey Attack Safety Check
                // If the enemy attack source is playing.
                if (enemyAttackSource.isPlaying)
                {
                    // If the game is paused and the attack source isn't marked as paused.
                    if (gamePaused && !enemyAttackSourceMarkedPaused)
                    {
                        PauseEnemyAttackSfx();
                    }

                    // No spawned enemies, so stop the attack source.
                    if (actionManager.playerEnemy.spawnedEnemies.Count <= 0)
                    {
                        StopEnemyAttackSfx(true);
                    }
                }
                else
                {
                    // If the game isn't paused but the enemy attack source is paused, unpause it.
                    if (!gamePaused && enemyAttackSourceMarkedPaused)
                    {
                        UnpauseEnemyAttackSfx();
                    }
                }
            }
            
        }

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