using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Natural Gas
    public class ActionUnitGeneratorNaturalGas : ActionUnitGenerator
    {
        [Header("Generator/Natural Gas")]

        // The sprite renderer for a platform that can be displayed below the user unit.
        public SpriteRenderer platformSpriteRenderer;

        // If the platform is usable, set to true. If true, it will be used when deemed necessary.
        private bool platformUsable = true;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to natural gas.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.naturalGas;
            }

            // Uses energy cycles.
            if (!useEnergyCycles)
                useEnergyCycles = true;

            // Updates the platform visibility.
            UpdatePlatformVisible();
        }

        // PLATFORM
        // Returns 'true' if the platform is active.
        public bool IsPlatformVisible()
        {
            return platformSpriteRenderer.gameObject.activeSelf;
        }


        // Sets if the platform should be shown or not.
        public void SetPlatformVisible(bool value)
        {
            platformSpriteRenderer.gameObject.SetActive(value);
        }

        // Shows the platform sprite.
        public void ShowPlatform()
        {
            SetPlatformVisible(true);
        }

        // Hides the platform sprite.
        public void HidePlatform()
        {
            SetPlatformVisible(false);
        }

        // Enables the platform if the unit is on water. Disables it if the unit isn't on water.
        public void UpdatePlatformVisible()
        {
            // If the platform is usable and the tile exists.
            if (platformUsable && tile != null)
            {
                // If it's a water tile, use the platform sprite.
                SetPlatformVisible(tile.IsWaterTile());
            }
            else
            {
                // Tile doesn't exist, so the platform should be invisible by default.
                SetPlatformVisible(false);
            }
        }
    }
}