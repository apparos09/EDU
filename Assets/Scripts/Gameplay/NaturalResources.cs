using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The natural resources list.
    public class NaturalResources : MonoBehaviour
    {
        // Natural Resource Enum.
        // Ordered by renewable then nonrenewable, then alphabetically.
        public enum naturalResource { unknown, biomass, geothermal, hydro, solar, wave, wind, coal, naturalGas, nuclear, oil }

        // The number of natural resource types.
        public const int NATURAL_RESOURCE_COUNT = 11;

        // The singleton instance.
        private static NaturalResources instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The natural resource symbols.
        public List<Sprite> naturalResourceSymbols =new List<Sprite>();

        // Constructor
        private NaturalResources()
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
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Gets the instance.
        public static NaturalResources Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<NaturalResources>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Natural Resources (singleton)");
                        instance = go.AddComponent<NaturalResources>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Returns a list of all the natural resource types.
        // If 'includeUnknown' is true, the unknown type is included. If it's false, unknown isn't included.
        public static List<naturalResource> GenerateNaturalResourceTypeList(bool includeUnknown)
        {
            // Makes a list of types.
            List<naturalResource> typeList = new List<naturalResource>()
            {
                naturalResource.unknown,
                naturalResource.biomass,
                naturalResource.geothermal, 
                naturalResource.hydro, 
                naturalResource.solar, 
                naturalResource.wave, 
                naturalResource.wind, 
                naturalResource.coal,
                naturalResource.naturalGas,
                naturalResource.nuclear, 
                naturalResource.oil
            };

            // If unknown shouldn't be included, remove unknown.
            if(!includeUnknown)
            {
                typeList.Remove(naturalResource.unknown);
            }

            return typeList;
        }

        // Gets the natural resource name.
        public static string GetNaturalResourceName(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Gets the key for translation.
            string key = GetNaturalResourceNameKey(res);

            // If the LOL SDK has been initialized and the key exists.
            if(LanguageManager.IsLanguageLoaderInitialized() && key != "")
            {
                result = LanguageManager.GetLanguageTextStatic(key);
            }
            // Either the LOL SDK isn't initialized or the key is empty, so manually fill the values.
            else
            {
                // Checks the resource type to know what name to return.
                switch (res)
                {
                    case naturalResource.unknown:
                        result = "Unknown";
                        break;

                    case naturalResource.biomass:
                        result = "Biomass";
                        break;

                    case naturalResource.geothermal:
                        result = "Geothermal";
                        break;

                    case naturalResource.hydro:
                        result = "Hydro";
                        break;

                    case naturalResource.solar:
                        result = "Solar";
                        break;

                    case naturalResource.wave:
                        result = "Wave";
                        break;

                    case naturalResource.wind:
                        result = "Wind";
                        break;

                    case naturalResource.coal:
                        result = "Coal";
                        break;

                    case naturalResource.naturalGas:
                        result = "Natural Gas";
                        break;

                    case naturalResource.nuclear:
                        result = "Nuclear";
                        break;

                    case naturalResource.oil:
                        result = "Oil";
                        break;

                    default:
                        result = string.Empty;
                        break;
                }
            }
         

            return result;
        }
        
        // Gets the key for the natural resource, which is used for the language file.
        public static string GetNaturalResourceNameKey(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch(res)
            {
                case naturalResource.unknown:
                    result = "kwd_unknown"; // Uses keyword "Unknown".
                    break;

                case naturalResource.biomass:
                    result = "nrs_bms_nme";
                    break;

                case naturalResource.geothermal:
                    result = "nrs_gtl_nme";
                    break;

                case naturalResource.hydro:
                    result = "nrs_hdo_nme";
                    break;

                case naturalResource.solar:
                    result = "nrs_slr_nme";
                    break;

                case naturalResource.wave:
                    result = "nrs_wve_nme";
                    break;

                case naturalResource.wind:
                    result = "nrs_wnd_nme";
                    break;

                case naturalResource.coal:
                    result = "nrs_col_nme";
                    break;

                case naturalResource.naturalGas:
                    result = "nrs_ngs_nme";
                    break;

                case naturalResource.nuclear:
                    result = "nrs_nlr_nme";
                    break;

                case naturalResource.oil:
                    result = "nrs_oil_nme";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        // Gets the natural resource name.
        public static string GetNaturalResourceNameAbbreviation(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Gets the key for translation.
            string key = GetNaturalResourceNameAbbreviationKey(res);

            // If the LOL SDK has been initialized and the key exists.
            if (LanguageManager.IsLanguageLoaderInitialized() && key != "")
            {
                result = LanguageManager.GetLanguageTextStatic(key);
            }
            // Either the LOL SDK isn't initialized or the key is empty, so manually fill the values.
            else
            {
                // Checks the resource type to know what name abbreviation to return.
                switch (res)
                {
                    case naturalResource.unknown:
                        result = "UKN";
                        break;

                    case naturalResource.biomass:
                        result = "BMS";
                        break;

                    case naturalResource.geothermal:
                        result = "GTL";
                        break;

                    case naturalResource.hydro:
                        result = "HDO";
                        break;

                    case naturalResource.solar:
                        result = "SLR";
                        break;

                    case naturalResource.wave:
                        result = "WVE";
                        break;

                    case naturalResource.wind:
                        result = "WND";
                        break;

                    case naturalResource.coal:
                        result = "COL";
                        break;

                    case naturalResource.naturalGas:
                        result = "NGS";
                        break;

                    case naturalResource.nuclear:
                        result = "NLR";
                        break;

                    case naturalResource.oil:
                        result = "OIL";
                        break;

                    default:
                        result = string.Empty;
                        break;
                }
            }


            return result;
        }

        // Gets the key for the natural resource abbreviation, which is used for the language file.
        public static string GetNaturalResourceNameAbbreviationKey(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch (res)
            {
                case naturalResource.unknown:
                    result = "kwd_unknown_abv"; // Uses keyword "Unknown".
                    break;

                case naturalResource.biomass:
                    result = "nrs_bms_nme_abv";
                    break;

                case naturalResource.geothermal:
                    result = "nrs_gtl_nme_abv";
                    break;

                case naturalResource.hydro:
                    result = "nrs_hdo_nme_abv";
                    break;

                case naturalResource.solar:
                    result = "nrs_slr_nme_abv";
                    break;

                case naturalResource.wave:
                    result = "nrs_wve_nme_abv";
                    break;

                case naturalResource.wind:
                    result = "nrs_wnd_nme_abv";
                    break;

                case naturalResource.coal:
                    result = "nrs_col_nme_abv";
                    break;

                case naturalResource.naturalGas:
                    result = "nrs_ngs_nme_abv";
                    break;

                case naturalResource.nuclear:
                    result = "nrs_nlr_nme_abv";
                    break;

                case naturalResource.oil:
                    result = "nrs_oil_nme_abv";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        // Gets the natural resource description.
        public static List<string> GetNaturalResourceDescription(naturalResource res)
        {
            // The result to be returned. Each element is a seperate page.
            List<string> result = new List<string>();
            
            // Gets the key for translation.
            string key = GetNaturalResourceDescriptionKey(res);

            // If true, the text should be translated.
            bool translate = LanguageManager.IsLanguageLoaderInitialized() && key != "";

            // Checks the resource type to know what description to return.
            switch (res)
            {
                case naturalResource.unknown:
                    if (translate)
                    {
                        // Uses the language key for unknown, which is just a single word.
                        result.Add(LanguageManager.GetLanguageTextStatic(key));
                    }
                    else
                    {
                        result.Add("Unknown");
                    }

                    break;

                case naturalResource.biomass:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                    }
                    else
                    {
                        result.Add("A renewable resource that burns plant materials such as wood and grains to turn a turbine to generate energy. Waste materials can also be used.");
                        result.Add("Along with generating energy, biomass materials can be used to make biofuel, which can be used for cars and trucks.");
                        result.Add("Plants can be grown for biomass, which can consume the carbon dioxide created by biomass activities, reducing gases being released into the air. However, once a plant is used for biomass energy, it cannot be used for other activities.");
                    }
                    break;

                case naturalResource.geothermal:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_03"));
                    }
                    else
                    {
                        result.Add("A renewable resource that uses the Earth's internal heat to produce hot water and/or steam to turn a turbine to generate energy.");
                        result.Add("Geothermal resources are obtained via drilling wells, and water can be pumped into these wells to use a geothermal spot perpetually.");
                        result.Add("Geothermal energy can only be obtained from underground heat sources close enough to the Earth's surface to make power plants viable.");
                        result.Add("Steam from geothermal wells can have unwanted mineral byproducts. Geothermal activities can also cause earthquakes.");
                    }
                    break;

                case naturalResource.hydro:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                    }
                    else
                    {
                        result.Add("A renewable resource that generates energy by having reservoir water flow through a dam to turn a turbine to generate energy.");
                        result.Add("Hydropower doesn't use fuel, doesn't use up water, and causes little pollution. However, the dam can cause flooding upstream and slow the release of silt downstream.");
                        result.Add("Flooding can potentially cause harmful chemicals to be released from vegetation being destroyed. Relatedly, the reduction of silt can cause areas to become more vulnerable to rising sea levels and coastal storms.");
                    }

                    break;

                case naturalResource.solar:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                    }
                    else
                    {
                        result.Add("A renewable resource that generates energy from sunlight by using solar panels, which are composed of solar cells. Solar panels can only generate energy during the day.");
                        result.Add("Solar panels produce no pollution when in operation. Solar panels can be placed in fields, attached to buildings, or be mounted on structures.");
                    }

                    break;

                case naturalResource.wave:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                    }
                    else
                    {
                        result.Add("A renewable resource that uses waves and tides to generate energy. A narrow bay or estuary may need to be closed off to use wave power plants.");
                        result.Add("Waves are most prominent in areas with high wind, and no energy can be generated if there are no waves.");
                        result.Add("Wave plants are constructed to withstand corrosive seawater and costal storms. This is a clean resource, but wave structures are costly to install and maintain.");
                    }

                    break;

                case naturalResource.wind:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                    }
                    else
                    {
                        result.Add("A renewable resource that uses wind to turn turbines to generate energy. Wind can be harnessed practically anywhere, but no energy can be generated if there's no wind.");
                        result.Add("Wind turbines can be installed on land or offshore in shallow waters. Wind turbines produce no pollutants but can be expensive and wear out quickly.");
                    }

                    break;

                case naturalResource.coal:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                    }
                    else
                    {
                        result.Add("A non-renewable fossil fuel resource that's crushed into powder and fed to a furnace to produce heat. This heat is used to boil water to make steam, which then turns a turbine to generate energy.");
                        result.Add("Coal is formed from plant waste in swamps over the course of millions of years. When burned, coal releases carbon dioxide and potentially other harmful minerals into the air. Coal produces more carbon dioxide than oil and natural gas when burned.");
                        result.Add("Coal can be mined underground or on the surface. Surface mining is safer for miners but releases harmful materials into the atmosphere, such as sulfur. Sulfur can combine with water and air to make sulfuric acid, which can damage environments.");
                    }

                    break;

                case naturalResource.naturalGas:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                    }
                    else
                    {
                        result.Add("A non-renewable fossil fuel resource that's burned to produce steam or hot gases to spin a turbine to generate energy. It can also be used for heating or cooking.");
                        result.Add("Natural gas can often be found along with coal or oil in underground deposits, but it forms at a higher temperature than oil. It's mostly methane and must have its water and poisonous chemicals removed before it can be burned.");
                        result.Add("While natural gas burns cleaner and releases less carbon dioxide than other fossil fuels, it still emits pollutants. It's also highly flammable, so natural gas leaks can cause explosions.");
                    }

                    break;

                case naturalResource.nuclear:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_03"));
                    }
                    else
                    {
                        result.Add("A non-renewable resource that involves splitting the nucleus of an atom to produce heat, which is used to boil water. This boiled water produces steam, which then spins a turbine to generate energy.");
                        result.Add("Atoms split for nuclear energy come from uranium, which is concentrated into fuel rods and has its atoms split using tiny particles. These particles must be controlled, otherwise they'll trigger a dangerous explosion.");
                        result.Add("If there are no accidents when generating energy, only steam is produced, which causes no pollution, but creates radioactive waste.");
                        result.Add("Radioactive waste can remain dangerous for hundreds to thousands of years. Radioactive waste must be securely stored to prevent damage to living things and the environment.");
                    }

                    break;

                case naturalResource.oil:
                    if (translate)
                    {
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_00"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_01"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_02"));
                        result.Add(LanguageManager.GetLanguageTextStatic(key + "_03"));
                    }
                    else
                    {
                        result.Add("A non-renewable fossil fuel resource that's burned to produce steam to spin a turbine to generate energy. Oil is a black or dark brown liquid that's found in rock layers in the Earth's crust.");
                        result.Add("Oil is formed from dead organic material that's buried under sediments that's kept away from oxygen and subjected to both high heat and pressure. After millions of years pass, said organic material becomes oil.");
                        result.Add("Oil can be extracted on land or at sea, though drilling for oil in the ocean can cause a spill if not done properly. Raw oil starts out as crude oil, which contains a mixture of various hydrocarbons.");
                        result.Add("Crude oil is refined via heating to separate it into different components, which boil at different temperatures. These components can be used for various purposes such as gasoline, and when burned release toxic chemicals into the air.");
                    }

                    break;

                default:
                    // Load empty string. Nothing to translate.
                    result.Add(string.Empty);
                    break;
            }


            return result;
        }

        // Gets the key for the natural resource description, which is used for the language file.
        public static string GetNaturalResourceDescriptionKey(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch (res)
            {
                case naturalResource.unknown:
                    result = "kwd_unknown"; // Uses keyword "Unknown".
                    break;

                case naturalResource.biomass:
                    result = "nrs_bms_dsc";
                    break;

                case naturalResource.geothermal:
                    result = "nrs_gtl_dsc";
                    break;

                case naturalResource.hydro:
                    result = "nrs_hdo_dsc";
                    break;

                case naturalResource.solar:
                    result = "nrs_slr_dsc";
                    break;

                case naturalResource.wave:
                    result = "nrs_wve_dsc";
                    break;

                case naturalResource.wind:
                    result = "nrs_wnd_dsc";
                    break;

                case naturalResource.coal:
                    result = "nrs_col_dsc";
                    break;

                case naturalResource.naturalGas:
                    result = "nrs_ngs_dsc";
                    break;

                case naturalResource.nuclear:
                    result = "nrs_nlr_dsc";
                    break;

                case naturalResource.oil:
                    result = "nrs_oil_dsc";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        // Generates the natural resource description keys.
        public static List<string> GenerateNaturalResourceDescriptionKeys(naturalResource res)
        {
            // The resulting keys that will be returned.
            List<string> resultKeys = new List<string>();

            // Gets the base key.
            string baseKey = GetNaturalResourceDescriptionKey(res);

            // NOTE: make sure this matches the number of pages in GetNaturalResourceDescription().
            // You could grab the list of descriptions and base the number on that, but this is...
            // More sufficient. You could also do this in a loop instead of manaully, but that...
            // Would also be more intensive programming wise, so you decided to just manually do it.

            // Checks the resource type to know what keys to generate.
            switch (res)
            {
                case naturalResource.unknown:
                    // Unknown doesn't have multiple pages, so just add the base key.
                    resultKeys.Add(baseKey);
                    break;

                case naturalResource.biomass:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    break;

                case naturalResource.geothermal:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    resultKeys.Add(baseKey + "_03");
                    break;

                case naturalResource.hydro:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    break;

                case naturalResource.solar:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    break;

                case naturalResource.wave:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    break;

                case naturalResource.wind:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    break;

                case naturalResource.coal:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    break;

                case naturalResource.naturalGas:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    break;

                case naturalResource.nuclear:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    resultKeys.Add(baseKey + "_03");
                    break;

                case naturalResource.oil:
                    resultKeys.Add(baseKey + "_00");
                    resultKeys.Add(baseKey + "_01");
                    resultKeys.Add(baseKey + "_02");
                    resultKeys.Add(baseKey + "_03");
                    break;

                default:
                    // Nothing
                    break;
            }

            // Return the result keys.
            return resultKeys;
        }

        // Gets the natural resource sprite. The resource number shoud match up with the sprite index value.
        public Sprite GetNaturalResourceSymbol(naturalResource resource)
        {
            // The symbol sprite.
            Sprite symbolSprite;

            // The index, which is taken by converting the resource number.
            int index = (int)resource;

            // Uses the sprite list to get the sprite.
            if(index >= 0 && index < naturalResourceSymbols.Count)
            {
                symbolSprite = naturalResourceSymbols[index];
            }
            // Index invalid, so set to null.
            else
            {
                symbolSprite = null;
            }

            return symbolSprite;
        }

        // Generates an info log entry for the provided resource.
        public static InfoLog.InfoLogEntry GenerateInfoLogEntry(naturalResource resource)
        {
            // The entry.
            InfoLog.InfoLogEntry entry = new InfoLog.InfoLogEntry();

            // Setting values.
            entry.name = GetNaturalResourceName(resource);
            entry.nameKey = GetNaturalResourceNameKey(resource);
            entry.description = GetNaturalResourceDescription(resource);
            entry.descriptionKeys = GenerateNaturalResourceDescriptionKeys(resource);

            // If the natural resources have been instantiated, provide the symbol.
            // If false, set to null.
            entry.iconSprite = Instantiated ? Instance.GetNaturalResourceSymbol(resource) : null;

            return entry;
        }

        // Gets the color assigned to this natural resource.
        public static Color GetNaturalResourceColor(naturalResource res)
        {
            // The color to be returned.
            Color color;

            // Checks the resource type to know what key to return.
            switch (res)
            {
                default:
                case naturalResource.unknown:
                    color = Color.white;
                    break;

                case naturalResource.biomass:
                    color = new Color(0.055F, 0.631F, 0.216F);
                    break;

                case naturalResource.geothermal:
                    color = new Color(0.945F, 0.118F, 0.024F);
                    break;

                case naturalResource.hydro:
                    color = new Color(0.09F, 0.176F, 0.839F);
                    break;

                case naturalResource.solar:
                    color = new Color(1.0F, 0.588F, 0.0F);
                    break;

                case naturalResource.wave:
                    color = new Color(0.051F, 0.757F, 0.98F);
                    break;

                case naturalResource.wind:
                    color = new Color(0.749F, 1.0F, 1.0F);
                    break;

                case naturalResource.coal:
                    color = new Color(0.259F, 0.275F, 0.333F);
                    break;

                case naturalResource.naturalGas:
                    color = new Color(1.0F, 1.0F, 0.0F);
                    break;

                case naturalResource.nuclear:
                    color = new Color(0.553F, 1.0F, 0.0F);
                    break;

                case naturalResource.oil:
                    color = new Color(0.259F, 0.102F, 0.094F);
                    break;
            }

            return color;
        }

        // Checks if the natural resource is renewable.
        public static bool IsRenewable(naturalResource res)
        {
            // Bool to be returned to see if the natural resource is renewable.
            bool renewable;

            // Checks all types to see which ones are rneewable and which ones aren't.
            switch(res)
            {
                case naturalResource.biomass:
                case naturalResource.geothermal:
                case naturalResource.hydro:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    renewable = true;
                    break;

                case naturalResource.coal:
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                case naturalResource.oil:
                    renewable = false;
                    break;

                default:
                    renewable = false;
                    break;
            }

            return renewable;
        }

        // Checks if the natural resource is nonrenewable.
        public static bool IsNonrenewable(naturalResource res)
        {
            // Bool to be returned to see if the natural resource is nonrenewable.
            bool nonrenewable;

            // Checks all types to see which ones are rneewable and which ones aren't.
            switch (res)
            {
                case naturalResource.biomass:
                case naturalResource.geothermal:
                case naturalResource.hydro:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    nonrenewable = false;
                    break;

                case naturalResource.coal:
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                case naturalResource.oil:
                    nonrenewable = true;
                    break;

                default:
                    nonrenewable = false;
                    break;
            }

            return nonrenewable;
        }

        // Returns 'true' if the natural resource causes air pollution when used to generate power.
        public static bool CausesAirPollution(naturalResource res)
        {
            // The result to be returned.
            bool result;

            switch (res)
            {
                case naturalResource.biomass:
                case naturalResource.geothermal:
                    result = false;
                    break;

                case naturalResource.hydro:
                    // Can cause the breakdown of plants which releases methane into the atmosphere.
                    result = true;
                    break;
                
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    result = false;
                    break;

                case naturalResource.coal:
                case naturalResource.naturalGas:
                case naturalResource.oil:
                    // Releases pollutants into the air when used to generate enegry.
                    result = true;
                    break;

                case naturalResource.nuclear:
                    result = false;
                    break;

                default:
                    result = false;
                    break;

            }
            
            // Returns the result.
            return result;
        }

        // Returns 'true' if the provided resource uses energy cycles (energy spots)
        // Energy cycles determines how much energy a resource can get out of a tile...
        // Before the generator no longer works. If a generator doesn't use cycles...
        // Then it can get energy infinitely.
        public static bool UsesEnergyCycles(naturalResource resource)
        {
            bool result;

            // Checks to resource to see if it uses energy cycles or not.
            switch (resource)
            {
                default:
                case naturalResource.unknown:
                case naturalResource.biomass:
                case naturalResource.hydro:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    result = false;
                    break;

                case naturalResource.geothermal:
                case naturalResource.coal:
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                case naturalResource.oil:
                    result = true;
                    break;
            }

            return result;
        }
    }
}