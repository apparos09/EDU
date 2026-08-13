using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // Holds all the data for the action stage.
    public class ActionStageList : MonoBehaviour
    {
        // The stage generation data.
        // Since new data is generated everytime a stage data function is called...
        // This is a class, not a struct.
        public class StageGenerationData
        {
            // The map the player will use.
            public string[,] map = null;

            // The overlays on the map.
            // For visual simplicity, this is an int instead of the enum it represents.
            public int[,] overlays = null;

            // The wind ratings.
            public ActionUnit.statRating[] windRatings = null;
        }

        // TODO: create a stage struct that contains the map and wind settings.

        // The singleton instance.
        private static ActionStageList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private ActionStageList()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Gets the instance.
        public static ActionStageList Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionStageList>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Stage List (singleton)");
                        instance = go.AddComponent<ActionStageList>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Stage maps are 16x7.
        // The lane blasters are all in the first column, so the usable space for the player is 15x7.
        // For the tile set, the two digit number determines the tile type, and the letter on the end determines the version used (e.g., "00A").
        /*
         * Tileset:
         *  0: nothing
         *  1: land
         *  2: river
         *  3: sea
         */

        // Generates a map based on the provided value. If there's no map to match that value, null is returned.
        public static StageGenerationData GenerateStageMap(int idNumber)
        {
            // The data to be returned.
            StageGenerationData data;

            // Checks the value to see what map to return.
            switch(idNumber)
            {
                default:
                    data = null;
                    break;
                    
                case 0:
                    data = GenerateStageDataDebug();
                    break;

                case 1:
                    data = GenerateStageData01();
                    break;

                case 2:
                    data = GenerateStageData02();
                    break;

                case 3:
                    data = GenerateStageData03();
                    break;
                
                case 4: // This is a knowledge stage, so this is unused.
                    data = GenerateStageData04();
                    break;

                case 5:
                    data = GenerateStageData05();
                    break;

                case 6:
                    data = GenerateStageData06();
                    break;
                
                case 7: // This is a knowledge stage, so this is unused.
                    data = GenerateStageData07();
                    break;

                case 8: // This is a knowledge stage, so this is unused.
                    data = GenerateStageData08();
                    break;

                case 9:
                    data = GenerateStageData09();
                    break;
            }

            return data;
        }

        // NOTE: the rows are inverted from how they're formatted (the top row in the code is actually the bottom row, and vice versa).
        // Keep this in mind.

        // Generates and returns empty stage data.
        private static StageGenerationData GenerateStageDataEmpty()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT];

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns a full stage (metal row with grass tiles only).
        private static StageGenerationData GenerateStageDataFull()
        {
            // The map only has metal tiles and land tiles.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01K" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04D", "01A", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            // The stage uses no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
            {ActionUnit.statRating.none, ActionUnit.statRating.none, ActionUnit.statRating.none };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns the debug stage map.
        public static StageGenerationData GenerateStageDataDebug()
        {
            // A debug stage map.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01P", "02B", "01P", "01P", "03E", "03F", "03G", "01P", "01P", "02D", "04P" },
                { "04H", "01P", "02B", "01P", "01P", "03E", "03F", "03G", "02A", "02A", "02D", "04P" },
                { "04H", "01P", "02B", "01P", "01P", "03E", "03F", "03G", "01P", "01P", "02D", "04P" },
                { "04H", "01P", "02B", "01P", "01P", "03E", "03F", "03G", "02C", "02C", "02D", "04P" },
                { "04D", "01P", "02B", "01P", "01P", "03E", "03F", "03G", "01P", "01P", "02D", "04P" }
            };

            // An debug map overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 2, 0, 0, 0, 0, 6, 0, 2, 0, 0, 0 },
                { 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0, 0, 4, 0, 3, 0, 0, 0 },
                { 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 6, 0, 0, 0, 0, 6, 0, 5, 0, 0, 0 }
            };

            // The wind ratings.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                {ActionUnit.statRating.none, ActionUnit.statRating.medium, ActionUnit.statRating.veryHigh };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 01 (solar, coal).
        public static StageGenerationData GenerateStageData01()
        {
            // The map only has metal tiles and land tiles.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01L", "03I", "03K", "01I", "01J", "01J", "01K", "03I", "03K", "01I", "01K" },
                { "04H", "01H", "03E", "03G", "01E", "01F", "01F", "01G", "03E", "03G", "01E", "01G" },
                { "04H", "01H", "03E", "03G", "01E", "01F", "01F", "01G", "03E", "03G", "01E", "01G" },
                { "04H", "01H", "03E", "03G", "01E", "01F", "01F", "01G", "03E", "03G", "01E", "01G" },
                { "04D", "01D", "03A", "03C", "01A", "01B", "01B", "01C", "03A", "03C", "01A", "01C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 3, 0, 0, 0, 3, 0, 3, 0, 0, 0, 3 },
                { 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 3, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 3, 0 },
                { 0, 3, 0, 0, 0, 3, 0, 3, 0, 0, 0, 3 }
            };

            // The stage uses no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
            {ActionUnit.statRating.none, ActionUnit.statRating.none, ActionUnit.statRating.none };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 02 (wind, oil).
        public static StageGenerationData GenerateStageData02()
        {
            // The map has metal tiles, land tiles, and sea tiles
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01J", "01N", "01N", "01N", "01O", "03I", "03J", "03K", "01I", "01K" },
                { "04H", "01E", "01G", "03I", "03J", "03J", "03J", "03F", "03F", "03G", "01E", "01G" },
                { "04H", "01E", "01G", "03E", "03F", "03F", "03F", "03F", "03F", "03G", "01E", "01G" },
                { "04H", "01E", "01G", "03E", "03F", "03F", "03B", "03B", "03B", "03C", "01E", "01G" },
                { "04D", "01A", "01C", "03A", "03B", "03C", "01M", "01N", "01N", "01N", "01B", "01C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 6, 0, 0, 0, 6, 0, 6, 0, 6, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 6, 0, 6, 0, 6, 0, 6, 0, 6, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 6, 0, 6, 0, 6, 0, 0, 0, 6, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                {ActionUnit.statRating.medium, ActionUnit.statRating.veryLow, ActionUnit.statRating.veryHigh };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 03 (geothermal, hydro).
        public static StageGenerationData GenerateStageData03()
        {
            // The map only has metal tiles and land tiles.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01L", "02D", "01I", "01J", "01K", "02B", "01I", "01J", "01K", "02D", "01L" },
                { "04H", "01H", "02D", "01E", "01F", "01G", "02B", "01E", "01F", "01G", "02D", "01H" },
                { "04H", "01H", "02D", "01E", "01F", "01G", "02B", "01E", "01F", "01G", "02D", "01H" },
                { "04H", "01H", "02D", "01E", "01F", "01G", "02B", "01E", "01F", "01G", "02D", "01H" },
                { "04D", "01D", "02D", "01A", "01B", "01C", "02B", "01A", "01B", "01C", "02D", "01D" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0 },
                { 0, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                {ActionUnit.statRating.none, ActionUnit.statRating.none, ActionUnit.statRating.none };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 04 (unused).
        // Originally data for final action stage for area 1 (geothermal, hydro, solar, wind, coal, oil).
        // Stage 04 is now a knowledge stage.
        public static StageGenerationData GenerateStageData04()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01K", "02D", "01I", "01N", "01N", "01N", "01N", "01N", "01J", "01K" },
                { "04H", "01E", "01G", "02D", "01H", "03M", "03N", "03N", "03N", "03N", "03O", "01H" },
                { "04H", "01E", "01G", "02D", "01E", "01N", "01N", "01N", "01N", "01N", "01N", "01F" },
                { "04H", "01E", "01G", "02D", "01H", "03M", "03N", "03N", "03N", "03N", "03O", "01H" },
                { "04D", "01A", "01C", "02D", "01A", "01N", "01N", "01N", "01N", "01N", "01B", "01C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 6, 0, 0, 3, 0, 6, 0, 3, 0, 6, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 2, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 3, 0, 0, 6, 0, 3, 0, 6, 0, 3, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                {ActionUnit.statRating.low, ActionUnit.statRating.medium, ActionUnit.statRating.high};

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 05 (wave, natural gas).
        public static StageGenerationData GenerateStageData05()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "03I", "03J", "03J", "03J", "03J", "03J", "03J", "03N", "03N", "03K" },
                { "04H", "01E", "03E", "03F", "03B", "03B", "03B", "03F", "03G", "01I", "01K", "03H" },
                { "04H", "01E", "03E", "03G", "01I", "01J", "01K", "03E", "03G", "01A", "01C", "03H" },
                { "04H", "01E", "03E", "03G", "01E", "01F", "01G", "03E", "03F", "03J", "03J", "03G" },
                { "04D", "01E", "03A", "03C", "01E", "01F", "01G", "03A", "03B", "03B", "03B", "03C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 3, 0, 0, 6, 0, 6, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 6, 0, 0, 0, 4, 6, 0 },
                { 0, 3, 0, 0, 4, 0, 4, 0, 0, 3, 4, 0 },
                { 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0 },
                { 0, 3, 0, 0, 4, 0, 4, 0, 0, 0, 0, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
            { ActionUnit.statRating.medium, ActionUnit.statRating.high, ActionUnit.statRating.veryHigh};

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 06 (biomass, nuclear).
        public static StageGenerationData GenerateStageData06()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01K", "03M", "03N", "03O", "01I", "01J", "01J", "01J", "01K", "03L" },
                { "04H", "01E", "01F", "01J", "01J", "01J", "01F", "01F", "01F", "01F", "01G", "03H" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G", "03H" },
                { "04H", "01E", "01F", "01B", "01B", "01B", "01F", "01F", "01F", "01F", "01G", "03H" },
                { "04D", "01A", "01C", "03M", "03N", "03O", "01A", "01B", "01B", "01B", "01C", "03D" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 5, 0, 0, 0, 0, 5, 0, 5, 0, 5, 0 },
                { 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 5, 0, 0, 0, 0, 5, 0, 5, 0, 5, 0 },
                { 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 5, 0, 0, 0, 0, 5, 0, 5, 0, 5, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                { ActionUnit.statRating.none, ActionUnit.statRating.none, ActionUnit.statRating.none};

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 07 (unused)
        // Originally data for final action stage of area 2 (wave, natural gas, biomass, nuclear).
        // This is now a knowledge stage.
        public static StageGenerationData GenerateStageData07()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01J", "01K", "03I", "03J", "03J", "03J", "03J", "03J", "03K", "01L" },
                { "04H", "01E", "01F", "01G", "03E", "03B", "03B", "03F", "03B", "03B", "03G", "01H" },
                { "04H", "01E", "01F", "01G", "03H", "01M", "01O", "03H", "01M", "01O", "03H", "01H" },
                { "04H", "01E", "01F", "01G", "03E", "03J", "03J", "03F", "03J", "03J", "03G", "01H" },
                { "04D", "01A", "01B", "01C", "03A", "03B", "03B", "03B", "03B", "03B", "03C", "01D" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 5, 0, 4, 0, 0, 0, 0, 0, 0, 0, 5 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 4, 0, 5, 0, 4, 5, 0, 6, 5, 0, 4 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 5, 0, 4, 0, 0, 0, 0, 0, 0, 0, 5 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
            { ActionUnit.statRating.low, ActionUnit.statRating.medium, ActionUnit.statRating.high};

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 08 (all).
        // Originally this was stage 09, but since the game only has 8 stages now, this is stage 08.
        public static StageGenerationData GenerateStageData08()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01L", "03I", "03K", "01M", "01N", "01N", "01N", "01O", "03I", "03K", "01L" },
                { "04H", "01H", "03E", "03F", "02C", "02C", "02C", "02C", "02C", "03F", "03G", "01H" },
                { "04H", "01H", "03E", "03G", "01M", "01N", "01N", "01N", "01O", "03E", "03G", "01H" },
                { "04H", "01H", "03E", "03F", "02A", "02A", "02A", "02A", "02A", "03F", "03G", "01H" },
                { "04D", "01D", "03A", "03C", "01M", "01N", "01N", "01N", "01O", "03A", "03C", "01D" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 2, 0, 0, 2, 0, 3, 0, 5, 0, 0, 5 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 6, 0, 4, 0, 6, 0, 0, 4 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 3, 0, 0, 5, 0, 3, 0, 2, 0, 0, 6 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                { ActionUnit.statRating.veryLow, ActionUnit.statRating.medium, ActionUnit.statRating.veryHigh};

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns stage data for stage 09 (unused).
        // Originally there was a stage 9, but now the game only has 8 stages.
        public static StageGenerationData GenerateStageData09()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04L", "01I", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01J", "01K" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04H", "01E", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01F", "01G" },
                { "04D", "01A", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01B", "01C" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT];

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }


        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}