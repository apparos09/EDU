using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resources:
// * https://docs.unity3d.com/ScriptReference/AudioSource-time.html

namespace util
{
    // Loops a section of the audio.
    // NOTE: if the audio is compressed or changed in some similar way this script may not work properly.
    public class AudioSourceLooper : MonoBehaviour
    {
        // audio source
        public AudioSource audioSource = null;

        // Start and end of the audio clip being played. This is in seconds.
        // NOTE: be aware that the time of the audio may not be accurate if the audio is compressed.
        // As such, it may be best not to use this.

        // The start of the clip loop. If this value is negative, the clip continues like normal.
        // If loop start is less than 0, then it doesn't function.
        [Tooltip("The start of the clip loop in seconds.")]
        public float loopStart = 0.0F;

        // The end of the clip loop.
        // TODO: find out what happen if time is set greater than the length of an audio file.
        /// <summary>
        /// * I don't know what happens if the time is set beyond the clip length but I assume it just errors out.
        /// * on another note that if loopEnd is set to the end of the clip, the song will loop back to the start...
        /// * instead of looping back to the pre-defined clip start. 
        /// * so it's best to either have some silence, or continue the file a little longer so that the loop...
        /// * has time to work properly.
        /// </summary>
        [Tooltip("The end of the clip loop in seconds.")]
        public float loopEnd = 0.0F;

        // If 'true', a song will be limited to the clip range.
        // If 'false', the start of the song will play normally,...
        // But once within the clip range it will stay within the clip.
        [Tooltip("If true, the audio starts at loopStart instead of at the start of the audio clip itself when PlayAudio() is called.")]
        public bool playAtLoopStart = false;


        // Adjusts the loop point dynamically based on where the audio is.
        // e.g., if the audio is 1 second past the endpoint, it loops back to 1 second past the clip start point.
        [Tooltip("If true, loopStart is offset by where the audio is in reference to loopEnd when a loop is being performed.")]
        public bool loopRelative = true;

        // If 'true', the clip loop is applied. If false, this script does not apply the clip loop.
        [Tooltip("Enables the clip loop if true. It's recommended that this is set to false if the audio clip file loops perfectly.")]
        public bool loopEnabled = true;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the audio source is not set, try to grab it from the game object.
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            // gets the start and end of the clip if not set.
            if (audioSource != null)
            {
                // If the clip start and end are both set to zero (i.e. they weren't set)
                // Clip Start - avoid negative value.
                if (loopStart < 0.0F)
                    loopStart = 0.0F;

                // Clip End - avoid negative value.
                if (loopEnd < 0.0F)
                    loopEnd = 0.0F;

                // If the audio source is set.
                if(audioSource.clip != null)
                {
                    // If the clip end is less than 0, equal to 0, or is greater than the clip length...
                    // Set it to the clip length.
                    if (loopEnd <= 0.0F || loopEnd > audioSource.clip.length)
                        loopEnd = audioSource.clip.length;

                    // NOTE: this throws an error if loopStart is greater than the length of the audio.
                    // Starts at loopStart instead of at the start of the audio itself.
                    if (playAtLoopStart)
                    {
                        // If the loopStart is less than the length of the clip, play at loopStart.
                        if (loopStart < audioSource.clip.length)
                            audioSource.time = loopStart;
                    }
                }                
                    
            }
        }

        // Plays the audio - if limited to the clip start, it starts from the loop point.
        public void PlayAudio(bool resetAudio, float delay)
        {
            // Audio source or audio clip doesn't exist.
            if (audioSource == null || audioSource.clip == null)
                return;

            // Stops audio if it's currently playing.
            audioSource.Stop();

            // If the audio should be reset.
            if (resetAudio)
            {
                // If the audio should start at the clip start when first played.
                if (playAtLoopStart && loopStart >= 0.0F && loopStart < audioSource.clip.length)
                {
                    audioSource.time = loopStart;
                }
                else // Start source at the start of the audio.
                {
                    audioSource.time = 0.0F;
                }
            }

            // Plays the audio. If there is a delay, use play delayed.
            if(delay > 0)
            {
                audioSource.PlayDelayed(delay);
            }
            else
            {
                audioSource.Play();
            }
            
        }

        // Plays the audio with no delay.
        public void PlayAudio(bool resetAudio)
        {
            PlayAudio(resetAudio, 0);
        }

        // Stops the audio
        // If 'resetAudio' is true, the audio is set back to its start.
        public void StopAudio(bool resetAudio)
        {
            // Audio source or audio clip doesn't exist.
            if (audioSource == null || audioSource.clip == null)
                return;

            // Stops the audio source.
            audioSource.Stop();

            // If the audio should be reset.
            if(resetAudio)
            {
                // Bring audio to clip start.
                if (playAtLoopStart && loopStart >= 0.0F && loopStart < audioSource.clip.length)
                {
                    audioSource.time = loopStart;
                }
                else // Bring audio to start of the song.
                {
                    audioSource.time = 0.0F;
                }
            }
            
        }

        // If the audio is set to loop
        public bool GetLooping()
        {
            if (audioSource != null)
                return audioSource.loop;
            else
                return false;
        }

        // Sets the audio to loop
        public void SetLooping(bool looping)
        {
            if (audioSource != null)
                audioSource.loop = looping;
        }

        // Returns the length of the audio clip loop.
        public float GetLoopLength()
        {
            return loopEnd - loopStart;
        }

        // Returns the value of loop start.
        public float GetLoopStart()
        {
            return loopStart;
        }

        // Sets the value of loop start in seconds
        // This only works if the audioClip has been set.
        public void SetLoopStartInSeconds(float seconds)
        {
            // If the audio source is null.
            if (audioSource == null)
                return;

            // If the audio clip is null.
            if (audioSource.clip == null)
                return;


            // Setting value to clip start.
            loopStart = (seconds >= 0.0F && seconds <= audioSource.clip.length) ?
                seconds : loopStart;
        }

        // Sets the loop start time as a percentage, with 0 being 0% and 1 being 100%.
        public void SetLoopStartAsPercentage(float t)
        {
            // If the audio source is null.
            if (audioSource == null)
                return;

            // If the audio clip is null.
            if (audioSource.clip == null)
                return;

            t = Mathf.Clamp(t, 0.0F, 1.0F);
            loopStart = Mathf.Lerp(0.0F, audioSource.clip.length, t);
        }

        // Returns the value of loop end.
        public float GetLoopEnd()
        {
            return loopEnd;
        }

        // Sets the value of loop end in seconds
        // This only works if the audioClip has been set.
        public void SetLoopEndInSeconds(float seconds)
        {
            // If the audio source is null.
            if (audioSource == null)
                return;

            // If the audio clip is null.
            if (audioSource.clip == null)
                return;


            // Setting value to clip start.
            loopEnd = (seconds >= 0.0F && seconds <= audioSource.clip.length) ?
                seconds : loopEnd;
        }

        // Sets the clip end time as a percentage, with 0 being 0% and 1 being 100%.
        public void SetLoopEndAsPercentage(float t)
        {
            // If the audio source is null.
            if (audioSource == null)
                return;

            // If the audio clip is null.
            if (audioSource.clip == null)
                return;

            t = Mathf.Clamp(t, 0.0F, 1.0F);
            loopEnd = Mathf.Lerp(0.0F, audioSource.clip.length, t);
        }

        // Gets the variable that says whether or not to start the song at the start of the clip.
        public bool GetPlayAtLoopStart()
        {
            return playAtLoopStart;
        }

        // Sets the play at clip start
        public void SetPlayAtLoopStart(bool pacs)
        {
            playAtLoopStart = pacs;

            // If the audio source is null.
            if (audioSource == null)
                return;

            // If the audio clip is null.
            if (audioSource.clip == null)
                return;

            // If the audio should play at the start of the clip.
            if (playAtLoopStart)
            {
                // If the start of the clip is greater than the current time of the clip...
                // The audioSouce is set to the start of the clip.
                if (loopStart > audioSource.time)
                    audioSource.time = loopStart;
            }
        }

        // TODO: check how well this works with compressed audio.
        // Sets the current time in audio clip as a percentage of the audio clip's length.
        public void SetClipTime(float t)
        {
            // Sets the clip time
            audioSource.time = Mathf.Clamp(t, 0, audioSource.clip.length);
        }

        // Sets the clip time as a percentage. Argument 'percent' ranges from 0 to 1.
        public void SetClipTimeAsPercentage(float percent)
        {
            // Sets the clip time
            audioSource.time = audioSource.clip.length * Mathf.Clamp01(percent);
        }

        // Called to loop the clip back to its start.
        protected virtual void OnLoopClip()
        {
            // Checks to see if the audio is looping
            switch (audioSource.loop)
            {
                case true: // Audio is looping
                    // Checks if loopStart should be offset relative to how far past loopEnd the audio currently is.
                    if(loopRelative) // Offset clip start.
                    {
                        // Calculates how much loopStart should be offset by.
                        float offsetStart = audioSource.time - loopEnd;

                        // Set current loop start as loopStart adjusted by the offset amount.
                        float currLoopStart = loopStart + offsetStart;


                        // If the current clip start is negative (i.e., it's before the start of the audio itself)...
                        // Then use normal loopStart.
                        if(currLoopStart >= 0)
                            audioSource.time = currLoopStart;
                        else
                            audioSource.time = loopStart;
                    }
                    else // Set back to loopStart.
                    {
                        audioSource.time = loopStart;
                    }
                    break;

                case false: // Audio is not looping
                            
                    // Audio is stopped, and returns to clip start.
                    audioSource.Stop();
                    audioSource.time = loopStart;
                    break;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // Audio source, or the clip are not set.
            if (audioSource == null)
                return;
            if (audioSource.clip == null)
                return;

            // Clamp loopStart and loopEnd
            // TODO: see if using if statements is less computationally expensive (2 conditional statements per clamp)
            loopStart = Mathf.Clamp(loopStart, 0.0F, audioSource.clip.length);
            loopEnd = Mathf.Clamp(loopEnd, 0.0F, audioSource.clip.length);

            // if the clips are the same, no audio can play.
            if (loopStart == loopEnd)
            {
                return;
            }
            // If the clip end is greater than the clip start, then the values are swapped.
            else if (loopStart > loopEnd)
            {
                float temp = loopStart;
                loopStart = loopEnd;
                loopEnd = temp;
            }

            // If the clip loop is enabled, and the audio source is playing.
            if (loopEnabled && audioSource.isPlaying)
            {
                // This isn't needed since using the Play() function in this class handles this.
                // Puts the audio source at the clip start.
                // if (audioSource.time < loopStart && !playAtLoopStart)
                //     audioSource.time = loopStart;

                // The audioSource has reached the end of the clip.
                if (audioSource.time >= loopEnd)
                {
                    // Call to loop the clip.
                    OnLoopClip();
                }
            }
        }
    }
}