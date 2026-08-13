using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Animations for an action unit.
    public class ActionUnitAnimations : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The animator.
        public Animator animator;

        // The action unit this animations script belongs to.
        public ActionUnit actionUnit;

        [Header("Animations")]

        // The empty state animation.
        public string emptyStateAnim = "Empty State";

        // Animation for the unit becoming usable.
        public string usableAnim = "Action Unit - Usable Animation";

        // Animation for the unit becoming unusable.
        public string unusableAnim = "Action Unit - Unusable Animation";

        // Plays the red animation.
        public string flashRedAnim = "Action Unit - Flash - Red Animation";

        // Plays the blue animation.
        public string flashBlueAnim = "Action Unit - Flash - Blue Animation";

        // The death animation.
        public string deathAnim = "Action Unit - Death Animation";

        // The explosion animation.
        public string explosionAnim = "Action Unit - Explosion Animation";

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Set manager if null.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Sets the animator if it's not set.
            if(animator == null)
                animator = GetComponent<Animator>();

            // Set action unit if not set.
            if(actionUnit == null)
                actionUnit = GetComponent<ActionUnit>();

        }

        // Plays an animation.
        public void PlayAnimation(string animationName)
        {
            // TODO: check if animation is playing.

            // If animations are enabled and there's an animation, play the animation.
            if (actionUnit.AnimationsEnabled && animationName != "")
            {
                animator.Play(animationName);
            }
        }

        // Plays the provided animation for the given layer.
        public void PlayAnimation(string animationName, int layer)
        {
            // If animations are enabled, play the animation.
            if(actionUnit.AnimationsEnabled && animationName != "")
            {
                animator.Play(animationName, layer);
            } 
        }

        // EMPTY
        // Plays the empty state animation.
        public void PlayEmptyStateAnimation(int layer)
        {
            PlayAnimation(emptyStateAnim, layer);
        }

        // Plays the empty state animation on the base layer.
        public virtual void PlayEmptyStateAnimationBaseLayer()
        {
            PlayEmptyStateAnimation(0);
        }

        // Plays the empty state animation on the overlay layer.
        public virtual void PlayEmptyStateAnimationOverlayLayer()
        {
            PlayEmptyStateAnimation(1);
        }

        // USABLE / UNUSABLE
        // Plays the usable animaiton.
        public void PlayUsableAnimation()
        {
            PlayAnimation(usableAnim);
        }

        // Plays the unusuable animation.
        public void PlayUnusableAnimation()
        {
            PlayAnimation(unusableAnim);
        }

        // FLASH

        // Plays the flash red animation.
        public void PlayFlashRedAnimation()
        {
            PlayAnimation(flashRedAnim);
        }

        // Plays the flash blue animation.
        public void PlayFlashBlueAnimation()
        {
            PlayAnimation(flashBlueAnim);
        }

        // DEATH
        // Returns 'true' if the action unit has a death animation.
        public bool HasDeathAnimation()
        {
            return deathAnim != "";
        }

        // Plays the death animation.
        public void PlayDeathAnimation()
        {
            PlayAnimation(deathAnim);
        }

        // Called when the death animation starts.
        public void OnDeathAnimationStart()
        {
            // NOTE: make sure the death animation turns off the collider.
        }

        // Called when the death animation ends.
        public void OnDeathAnimationEnd()
        {
            PlayEmptyStateAnimation(1);
            actionUnit.OnUnitDeath();
        }

        // EXPLOSION
        // Plays the explosion animation.
        public void PlayExplosionAnimation()
        {
            animator.Play(explosionAnim);
        }

        // Called when the explosion animation starts.
        public void OnExplosionAnimationStart()
        {
            // ...
        }

        // Called when the explosion animation ends.
        public void OnExplosionAnimationEnd()
        {
            PlayEmptyStateAnimation(1);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}