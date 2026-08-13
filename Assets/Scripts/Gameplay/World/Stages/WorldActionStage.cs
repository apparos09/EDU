using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world action stage.
    public class WorldActionStage : WorldStage
    {
        // [Header("Action")]



        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Gets the stage type.
        public override stageType GetStageType()
        {
            return stageType.action;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}