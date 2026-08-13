using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world knowledge stage.
    public class WorldKnowledgeStage : WorldStage
    {
        // [Header("Knowledge")]


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Gets the stage type.
        public override stageType GetStageType()
        {
            return stageType.knowledge;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}