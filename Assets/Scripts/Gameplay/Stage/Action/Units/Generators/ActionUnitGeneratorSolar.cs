using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RM_EDU
{
    // Action Unit Generator - Solar
    public class ActionUnitGeneratorSolar : ActionUnitGenerator
    {
        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to solar.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.solar;
            }

            // Solar units can only be used during the day.
            if(timeUsage != timeConstraint.dayOnly)
                timeUsage = timeConstraint.dayOnly;
        }
    }
}