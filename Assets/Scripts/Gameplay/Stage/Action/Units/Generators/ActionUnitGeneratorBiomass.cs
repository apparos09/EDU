using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Biomass
    public class ActionUnitGeneratorBiomass : ActionUnitGenerator
    {
        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to biomass.
            if(resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.biomass;
            }
        }
    }
}