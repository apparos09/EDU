using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world stage select dialog..
    public class WorldStageSelectDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The world stage the prompt is for.
        private WorldStage worldStage;

        [Header("UI")]

        // The stage type text.
        public TMP_Text stageTypeText;

        // The resources text.
        public TMP_Text resourcesText;

        // Stage Name Keys
        public const string WORLD_STAGE_KEY = "kwd_worldStage";
        public const string ACTION_STAGE_KEY = "kwd_actionStage";
        public const string KNOWLEDGE_STAGE_KEY = "kwd_knowledgeStage";
        public const string UNKNOWN_STAGE_KEY = "kwd_unknownStage";

        [Header("UI/Symbols")]

        // Renewable
        public Image biomassSymbolImage;
        public Image geothermalSymbolImage;
        public Image hydroSymbolImage;
        public Image solarSymbolImage;
        public Image waveSymbolImage;
        public Image windSymbolImage;

        // Non-renewable
        public Image coalSymbolImage;
        public Image naturalGasSymbolImage;
        public Image nuclearSymbolImage;
        public Image oilSymbolImage;

        // The colour for when a resource is being used.
        private Color includedResourceColor = Color.white;

        // The colour for a resoruce that's being unused.
        private Color excludedResourceColor = new Color(0.25F, 0.25F, 0.25F);


        // Start is called before the first frame update
        void Start()
        {
            // Sets the world UI.
            if(worldUI == null)
            {
                worldUI = WorldUI.Instance;
            }
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            // Tries to speak the stage type using TTS.
            SpeakStageType();   
        }

        // The color for a resource that's being included.
        public Color IncludedResourceColor
        {
            get { return includedResourceColor; }
        }

        // The color for a resource that's being excluded.
        public Color ExcludedResourceColor
        {
            get { return excludedResourceColor; }
        }

        // Gets the stage name translated. Returns empty string if LOLSDK isn't initialized or the key is blank.
        private string GetStageNameTranslated(string key)
        {
            // Checks if the LOL SDK is initialized.
            if (SystemManager.IsLanguageLoaded() && key != "")
            {
                return SystemManager.GetLanguageTextStatic(key);
            }
            else
            {
                return "";
            }
        }

        // Gets the world stage string.
        public string GetWorldStageString()
        {
            if (SystemManager.IsLanguageLoaded())
                return GetStageNameTranslated(WORLD_STAGE_KEY);
            else
                return "World Stage";
        }

        // Gets the action stage string.
        public string GetActionStageString()
        {
            if (SystemManager.IsLanguageLoaded())
                return GetStageNameTranslated(ACTION_STAGE_KEY);
            else
                return "Action Stage";
        }

        // Gets the knowledge stage string.
        public string GetKnowledgeStageString()
        {
            if (SystemManager.IsLanguageLoaded())
                return GetStageNameTranslated(KNOWLEDGE_STAGE_KEY);
            else
                return "Knowledge Stage";
        }

        // Gets the unknown stage string.
        public string GetUnknownStageString()
        {
            if (SystemManager.IsLanguageLoaded())
                return GetStageNameTranslated(UNKNOWN_STAGE_KEY);
            else
                return "Unknown Stage";
        }

        // Returns 'true' if the world stage.
        public bool HasWorldStage()
        {
            return worldStage != null;
        }


        // Sets the world stage.
        public void SetWorldStage(WorldStage worldStage)
        {
            this.worldStage = worldStage;

            // Checks if the world stage is an action stage.
            if(worldStage is WorldActionStage)
            {
                stageTypeText.text = GetActionStageString();
            }
            // Checks if a world stage is a knowledge stage.
            else if(worldStage is WorldKnowledgeStage)
            {
                stageTypeText.text = GetKnowledgeStageString();
            }
            // Don't know what stage it is., so set it as unknown.
            else
            {
                stageTypeText.text = GetUnknownStageString();
            }

            // If the world stage exists, get the resources as a string.
            if(worldStage != null)
            {
                resourcesText.text = worldStage.ResourcesToString();
            }
            else
            {
                resourcesText.text = "-";
            }


            // Symbol Colors
            // Changes colours to show which resources are included or excluded.
            // Renewable
            biomassSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.biomass) ? 
                includedResourceColor : excludedResourceColor;

            geothermalSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.geothermal) ?
                includedResourceColor : excludedResourceColor;

            hydroSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.hydro) ?
                includedResourceColor : excludedResourceColor;

            solarSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.solar) ?
                includedResourceColor : excludedResourceColor;

            waveSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.wave) ?
                includedResourceColor : excludedResourceColor;

            windSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.wind) ?
                includedResourceColor : excludedResourceColor;


            // Non-renewable
            coalSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.coal) ?
                includedResourceColor : excludedResourceColor;

            naturalGasSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.naturalGas) ?
                includedResourceColor : excludedResourceColor;

            nuclearSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.nuclear) ?
                includedResourceColor : excludedResourceColor;

            oilSymbolImage.color = worldStage.naturalResources.Contains(NaturalResources.naturalResource.oil) ?
                includedResourceColor : excludedResourceColor;
        }

        // Clears the world stage.
        public void ClearWorldStage()
        {
            // Sets the stage.
            worldStage = null;

            // Changes the stage and resources text.
            stageTypeText.text = GetWorldStageString();
            resourcesText.text = "-";

            // Symbol Colors
            // Sets all colors to white (default).
            // Renewable
            biomassSymbolImage.color = Color.white;
            geothermalSymbolImage.color = Color.white;
            hydroSymbolImage.color = Color.white;
            solarSymbolImage.color = Color.white;
            waveSymbolImage.color = Color.white;
            windSymbolImage.color = Color.white;

            // Non-renewable
            coalSymbolImage.color = Color.white;
            naturalGasSymbolImage.color = Color.white;
            nuclearSymbolImage.color = Color.white;
            oilSymbolImage.color = Color.white;
        }

        // Called to start the stage.
        public void StartStage()
        {
            // If the world stage isn't equal to null, start the stage.
            if(worldStage != null)
            {
                WorldManager.Instance.StartStage(worldStage);
            }
            
        }

        // Opens the dialog using the current world stage by calling the world UI.
        public void OpenDialog()
        {
            // Opens the world stage dialog.
            WorldUI.Instance.OpenWorldStageSelectDialog(worldStage);
        }

        // Opens the dialog given the provided world stage.
        public void OpenDialog(WorldStage worldStage)
        {
            WorldUI.Instance.OpenWorldStageSelectDialog(worldStage);
        }

        // Closes the dialog.
        public void CloseDialog()
        {
            ClearWorldStage();
            WorldUI.Instance.CloseWorldStageSelectDialog();
        }

        // SPEAK TEXT
        // Uses TTS to speak the stage type.
        public void SpeakStageType()
        {
            // If the LOL Manager has been initialized, TTS is instantiated, and GameSettings is instantiated.
            if (SystemManager.IsLanguageLoaded() && TextToSpeech.Instantiated && GameSettings.Instantiated)
            {
                // If text-to-speech is enabled, try to speak the text.
                if (GameSettings.Instance.UseTextToSpeech)
                {
                    // The key to be used for speaking.
                    string key = "";

                    // Checks if the world stage exists.
                    if(worldStage != null)
                    {
                        // Checks the stage type to know what key to use.
                        switch(worldStage.GetStageType())
                        {
                            default:
                            case WorldStage.stageType.unknown:
                                key = UNKNOWN_STAGE_KEY;
                                break;

                            case WorldStage.stageType.action:
                                key = ACTION_STAGE_KEY;
                                break;

                            case WorldStage.stageType.knowledge:
                                key = KNOWLEDGE_STAGE_KEY;
                                break;
                        }
                    }
                    else
                    {
                        key = WORLD_STAGE_KEY;
                    }

                    // If the key is set, speak the text.
                    if(key != "")
                    {
                        SystemManager.Instance.SpeakText(key);
                    }
                }
            }
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}