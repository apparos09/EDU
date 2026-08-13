using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The enemy attack.
    public class EnemyAttack : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The action unit enemy this enemy attack belongs to.
        public ActionUnitEnemy unitEnemy;

        // The animator for the enemy attack.
        public Animator animator;

        // The target the enemy attack is being applied to.
        public ActionUnit target;

        // If 'true', the attack object's position is locked to the target's position.
        [Tooltip("Locks the attack to the target's position if true.")]
        public bool lockPosToTarget = true;

        // If 'true', the enemy attack turns its object inactive if it has no target.
        [Tooltip("Turns this object off if it has no target.")]
        public bool autoInactiveIfNoTarget = true;

        // Gets set to true when start has been called.
        private bool calledStart = false;

        [Header("Animations")]

        // The empty state animations.
        public string emptyStateAnim = "Empty State";

        // The pulsing animation.
        public string pulsingAnim = "Enemy Attack - Pulsing Animation";

        // If the pulsing animaiton should be used. If so, it's played in start.
        [Tooltip("If true, the pulsing animation is used, and is played in start.")]
        public bool usePulseAnim = true;

        [Header("Audio")]

        // The pulsing sound effect.
        public AudioClip pulsingSfx;

        // If 'true', the pulsing sound effect is looped. If false, it plays in one shot.
        [Tooltip("If true, loop the pulsing sound effect. If false, play it in one shot.")]
        public bool loopPulsingSfx = true;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Autosets the unit enemy. The attack should be a child of the enemy it belongs to.
            if (unitEnemy == null)
                unitEnemy = GetComponentInParent<ActionUnitEnemy>();

            // Sets animator if not set already.
            if(animator == null)
                animator = GetComponent<Animator>();

            // If the pulsing animation should be played.
            if(usePulseAnim)
            {
                PlayPulsingAnimation();
            }

            // Start has been called.
            calledStart = true;
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // Start has been called.
            if (calledStart)
            {
                // If the pulse sound effect should be played on loop.
                if (ActionAudio.Instantiated && usePulseAnim && loopPulsingSfx)
                {
                    ActionAudio.Instance.PlayEnemyAttackSfx();
                }
            }
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            // Start has been called.
            if(calledStart)
            {
                // If the pulse sound effect was playing on loop.
                if (ActionAudio.Instantiated && usePulseAnim && loopPulsingSfx)
                {
                    ActionAudio.Instance.StopEnemyAttackSfx(false);
                }
            }
        }

        // TARGET

        // Returns 'true' if the enemy attack has a target.
        public bool HasTarget()
        {
            return target != null;
        }
        
        // Sets the new target.
        // updatePos: updates the position based on the new target.
        public void SetTarget(ActionUnit newTarget, bool updatePos = true)
        {
            target = newTarget;

            // Updates the position if requested.
            if (updatePos)
                transform.position = (target != null) ? target.transform.position : Vector3.zero;
        }

        // Clears the target.
        // updatePos: if true, the local position is set to zero.
        public void ClearTarget(bool updatePos = true)
        {
            target = null;

            // If the position should be updated.
            if(updatePos)
                transform.localPosition = Vector3.zero;
        }

        // POSITIONING
        // Sets the object's local position to zero.
        public void SetLocalPositionToZero()
        {
            transform.localPosition = Vector3.zero;
        }


        // Sets to the provided position.
        public void SetToPosition(Vector3 newPos)
        {
            transform.position = newPos;
        }

        // Sets to the provided position with the provided offset.
        public void SetToPosition(Vector3 newPos, Vector3 offset)
        {
            transform.position = newPos + offset;
        }

        // Sets to the position of the provided object.
        public void SetToObjectPosition(GameObject posObject)
        {
            transform.position = posObject.transform.position;
        }

        // Sets to the position of the provided object with the offset.
        public void SetToObjectPosition(GameObject posObject, Vector3 offset)
        {
            transform.position = posObject.transform.position + offset;
        }

        // Sets to the target position.
        public void SetToTargetPosition()
        {
            SetToTargetPosition(Vector3.zero);
        }

        // Sets to the target's position. If there is no target, it's set to zero.
        public void SetToTargetPosition(Vector3 offset)
        {
            // If the target exists, use it as the position.
            if(target != null)
            {
                transform.position = target.transform.position + offset;
            }
            // Target doesn't exist, so set it to 0.
            else
            {
                transform.position = Vector3.zero;
            }
        }

        // ANIMATION
        // Plays an animation.
        public void PlayAnimation(string animationName)
        {
            // TODO: check if animation is playing.
            // Animation name exists.
            if (animationName != "")
            {
                animator.Play(animationName);
            }
        }

        // Plays the provided animation for the given layer.
        public void PlayAnimation(string animationName, int layer)
        {
            // TODO: check if animation is playing.
            // Animation name is set.
            if (animationName != "")
            {
                animator.Play(animationName, layer);
            }
        }

        // Plays the empty animation.
        public void PlayEmptyStateAnimation()
        {
            PlayAnimation(emptyStateAnim);
        }

        // Plays the pulsing animation.
        public void PlayPulsingAnimation()
        {
            PlayAnimation(pulsingAnim);

            // If the pulsing sound effect should be looped, play it.
            if(loopPulsingSfx)
            {
                PlayPulsingSfxAuto();
            }
        }

        // Plays the pulsing sound effect.
        public void PlayPulsingSfx(bool loop)
        {
            // Checks if sound can be played.
            bool playSound;

            // If the unit enemy is set, see if it can use sound.
            if(unitEnemy != null)
            {
                playSound = unitEnemy.CanPlayAudio();
            }
            else
            {
                // Check if the action audio exists.
                playSound = ActionAudio.Instantiated;
            }

            // If the sound should be played, play it.
            if(playSound)
            {
                // Determiens if the sound effect should be looped or not.
                // Loop
                if(loop)
                {
                    ActionAudio.Instance.PlayEnemyAttackSfx();
                }
                // One Shot
                else
                {
                    ActionAudio.Instance.PlaySoundEffectWorld(pulsingSfx);
                }
            }
        }

        // Plays the pulsing sound effect with auto loop settings.
        public void PlayPulsingSfxAuto()
        {
            PlayPulsingSfx(loopPulsingSfx);
        }

        // Play the pulsing sound effect as a loop.
        public void PlayPulsingSfxLoop()
        {
            PlayPulsingSfx(true);
        }

        public void PlayPulsingSfxOneShot()
        {
            PlayPulsingSfx(false);
        }

        // Update is called once per frame
        void Update()
        {
            // If the stage is playing and the game isn't paused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If the target is set.
                if (target != null)
                {
                    // If the target is NOT active and enabled, clear the target.
                    if (!target.isActiveAndEnabled)
                    {
                        ClearTarget();
                    }
                }
            }
        }

        // LateUpdate is called every frame, if the Behaviour is enabled
        private void LateUpdate()
        {
            // The stage is playing and the game isn't paused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // The target is set.
                if (target != null)
                {
                    // If the attack should lock to the target's position.
                    if (lockPosToTarget)
                    {
                        // If the target's position is not the attack's position, move the attack.
                        if (target.transform.position != transform.position)
                        {
                            // SetToTargetPosition();
                            transform.position = target.transform.position;
                        }
                    }

                }
                // Target not set.
                else
                {
                    // If the attack should lock to a target.
                    if (lockPosToTarget)
                    {
                        // If the local position isn't zero, make it zero.
                        if (transform.localPosition != Vector3.zero)
                        {
                            // SetLocalPositionToZero();
                            transform.localPosition = Vector3.zero;
                        }
                    }


                    // If there is no target, and the enemy attack should turn off...
                    // Under such a circumstance, disable the game object.
                    if (autoInactiveIfNoTarget)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }

        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If start has been called.
            if(calledStart)
            {
                // If action audio is instantied, the pulsing animation is being used, and the sound effect is being looped.
                if (ActionAudio.Instantiated && usePulseAnim && loopPulsingSfx)
                {
                    // Send a stop call.
                    // If this object is disabled, that means OnDisable hasn't called it.
                    if (!isActiveAndEnabled)
                    {
                        ActionAudio.Instance.StopEnemyAttackSfx(false);
                    }
                }
            }

        }

    }
}