using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

// The stage results info.
namespace RM_EDU
{
    // The results stage info.
    public class ResultsStageInfo : MonoBehaviour
    {
        // The results UI.
        public ResultsUI resultsUI;

        [Header("Labels")]
        // The stage number.
        public TMP_LabeledValue stageNumber;

        // The stage type.
        public TMP_LabeledValue stageType;

        // The stage time.
        public TMP_LabeledValue stageTime;

        // The stage score.
        public TMP_LabeledValue stageScore;

        // The stage energy total (removed).
        // public TMP_LabeledValue stageEnergyTotal;


        // Start is called before the first frame update
        void Start()
        {
            // If the UI isn't set, get the instance.
            if(resultsUI == null)
            {
                resultsUI = ResultsUI.Instance;
            }
        }

        // Applies the world stage data.
        public void ApplyWorldStageData(WorldStage.WorldStageData data)
        {
            // If the data exists, get the info.
            if(data != null)
            {
                // Values.
                stageNumber.valueText.text = (data.idNumber).ToString();
                stageType.valueText.text = WorldStage.GetStageTypeName(data.stageType);
                stageTime.valueText.text = StringFormatter.FormatTime(data.time, false, true, false);

                stageScore.valueText.text = Mathf.CeilToInt(data.score).ToString();
                // stageEnergyTotal.valueText.text = Mathf.CeilToInt(data.energyTotal).ToString();
            }
            // Data is null, so clear the world stage data.
            else
            {
                ClearWorldStageData();
            }
        }

        // Clears the world stage data.
        public void ClearWorldStageData()
        {
            stageNumber.valueText.text = "-";
            stageType.valueText.text = "-";
            stageTime.valueText.text = "-";
            stageScore.valueText.text = "-";
            // stageEnergyTotal.valueText.text = "-";
        }
    }
}