using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Nuclear
    public class ActionUnitGeneratorNuclear : ActionUnitGenerator
    {
        [Header("Generator/Nuclear")]

        // Kills the generator if it's no longer usable (no energy cycles left).
        [Tooltip("If the generator can't generate anymore energy, kill it.")]
        public bool killOnUnusable = true;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to nuclear.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.nuclear;
            }

            // Uses energy cycles.
            if (!useEnergyCycles)
                useEnergyCycles = true;
        }

        // Called to kill the unit.
        public override void Kill()
        {
            // If the unit can still generate energy...
            // Leave a hazard behind.
            if (IsDead() && CanGenerateEnergy())
            {
                // Give the tile a nuclear hazard.
                if (tile != null)
                    tile.SetTileOverlayType(ActionTile.actionTileOverlay.nuclearHazard);
            }

            base.Kill();
        }

        // Update is called every from, if the MonoBehaviour is enabled
        protected override void Update()
        {
            base.Update();

            // If the nuclear generator isn't usable.
            if(killOnUnusable)
            {
                // If the generator can't generate energy anymore, destroy it.
                if (!CanGenerateEnergy())
                {
                    Kill();
                }
            }
        }
    }
}