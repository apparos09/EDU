using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The knowledge stage end dialog.
    public class KnowledgeStageEndDialog : MonoBehaviour
    {
        // The knowledge UI.
        public KnowledgeUI knowledgeUI;

        // The speak text on enable object attached to this dialog.
        public SpeakTextOnEnable speakTextOnEnable;

        [Header("Stats")]

        // The stage time.
        public TMP_LabeledValue stageTime;

        // The stage score.
        public TMP_LabeledValue stageScore;

        // The stage user energy total.
        public TMP_LabeledValue stageEnergyBonus;

        [Header("Buttons")]

        // The finish button.
        public Button finishButton;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the UI instance.
            if (knowledgeUI == null)
                knowledgeUI = KnowledgeUI.Instance;

            // If not set, auto set.
            if (speakTextOnEnable == null)
                speakTextOnEnable = GetComponent<SpeakTextOnEnable>();
        }

        // Updates the stage end stats.
        public void UpdateStageEndStats()
        {
            // Gets the knowledge manager instance.
            KnowledgeManager knowledgeManager = KnowledgeManager.Instance;

            // Updates the values.
            stageTime.valueText.text = StringFormatter.FormatTime(knowledgeManager.stageTimer, false, true, false);
            stageScore.valueText.text = Mathf.CeilToInt(knowledgeManager.GetStageScore()).ToString();
            stageEnergyBonus.valueText.text = Mathf.CeilToInt(knowledgeManager.CalculateEnergyBonus()).ToString();
        }

        // Called to finish the stage.
        public void FinishStage()
        {
            KnowledgeUI.Instance.FinishStage();
        }
    }
}