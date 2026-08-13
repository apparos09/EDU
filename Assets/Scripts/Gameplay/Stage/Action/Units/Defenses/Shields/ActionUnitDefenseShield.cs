using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RM_EDU
{
    // An action unit shield.
    public class ActionUnitDefenseShield : ActionUnitDefense
    {
        [Header("Defense/Shield")]

        // If 'ture', the shield's sprite is changed to show the shield's damage.
        [Tooltip("If true, the shield sprite is changed to show damage based on current health.")]
        public bool showShieldDamage = true;

        // If 'true', the shield's sprite is updated by checking every frame.
        // This is set to false to optmize updates.
        private bool autoUpdateShieldSprite = false;

        // The sprite for when the shield has no damage.
        public Sprite noDamageSprite;

        // The sprite for when the shield has light damage.
        public Sprite lightDamageSprite;

        // The sprite for when the shield has heavy damage.
        public Sprite heavyDamageSprite;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the defense type is unknown, set to default value based on script.
            if (defType == defenseType.unknown)
            {
                defType = defenseType.shield;
            }
        }

        // Called when the unit has been damaged.
        // Provides the amount of damage that was calculated.
        public override void OnUnitDamaged(float damage)
        {
            base.OnUnitDamaged(damage);

            // If the shield should show damage.
            if(showShieldDamage)
            {
                // This doesn't check if auto updating is enabled, but that's fine.
                // The auto update should be off for the final game for optimzation.

                // Update the damage sprite.
                UpdateDamageSprite();
            }
        }

        // Gets the damage sprite based on the current health and max health.
        public Sprite GetDamageSpriteByHealth()
        {
            return GetDamageSpriteByHealth(health, maxHealth);
        }

        // Gets the damage sprite by the provided health and max health.
        public Sprite GetDamageSpriteByHealth(float health, float maxHealth)
        {
            // Gets the health percentage within the bounds of (0, 1).
            float healthPercent = Mathf.Clamp01(health / maxHealth);

            // The sprite to be returned.
            Sprite resultSprite;

            // No Damage
            if (healthPercent > 0.666F)
            {
                resultSprite = noDamageSprite;
            }
            // Light Damage
            else if (healthPercent > 0.333F)
            {
                resultSprite = lightDamageSprite;
            }
            // Heavy Damage
            else
            {
                resultSprite = heavyDamageSprite;
            }

            // Returns the result sprite.
            return resultSprite;
        }

        // Returns 'true' if the shield is set to the correct damage sprite based on its current health.
        public bool HasCorrectDamageSprite()
        {
            // Gets the comparison sprite and sees if it's correctly set.
            Sprite compSprite = GetDamageSpriteByHealth();
            bool result = spriteRenderer.sprite == compSprite;

            // Returns the result.
            return result;
        }

        // Updates the damage sprite of the shield to match its current health.
        public void UpdateDamageSprite()
        {
            // Gets the sprite.
            Sprite newSprite = GetDamageSpriteByHealth();

            // If the current sprite isn't set properly, set it.
            if(spriteRenderer.sprite != newSprite)
            {
                spriteRenderer.sprite = newSprite;
            }
        }

        // Forces an update of the damage sprite, even if it doesn't need it.
        public void ForceUpdateDamageSprite()
        {
            // Updates the sprite even if not necessary.
            spriteRenderer.sprite = GetDamageSpriteByHealth();
        }



        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the shield should show how much damage it has.
            if(showShieldDamage)
            {
                // If the shield sprite should be automatically updated.
                if(autoUpdateShieldSprite)
                {
                    UpdateDamageSprite();
                }
            }
        }
    }
}