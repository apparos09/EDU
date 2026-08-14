using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The list of knowledge statemnets.
    public class KnowledgeStatementList : MonoBehaviour
    {
        // A statement that is paired with a group.
        public class Statement
        {
            // Data for a statement.
            [System.Serializable]
            public class StatementData
            {
                // The ID number.
                public int idNumber;

                // The group resource.
                public NaturalResources.naturalResource groupResource;

                // The statement resource.
                public NaturalResources.naturalResource statementResource;

                // Returns true if this data matches the comparison data.
                public bool EqualsByValue(StatementData compData)
                {
                    return
                        idNumber == compData.idNumber &&
                        groupResource == compData.groupResource &&
                        statementResource == compData.statementResource;
                }

                // Checks if the comparison data is equal to this data by reference.
                public bool EqualsByReference(StatementData compData)
                {
                    return this == compData;
                }
            }

            // The id number of the statement.
            public int idNumber = 0;

            // The text for the statement.
            public string text = string.Empty;

            // The language key for the statement.
            public string key = string.Empty;

            // If 'true', the text has been translated and overwritten to the text object.
            // Make sure to reset this if the translation key and text are changed.
            public bool translatedText = false;

            // If 'true', once a text translation call is made, the 'text' object is overwritten.
            public bool saveTranslatedText = true;

            // The resource this statement belongs to.
            public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

            // Constructor
            public Statement()
            {

            }

            // Constructor
            public Statement(int idNumber, string text)
            {
                this.idNumber = idNumber;
                this.text = text;
            }

            // Constructor
            public Statement(int idNumber, string text, string key)
            {
                this.idNumber = idNumber;
                this.text = text;
                this.key = key;
            }

            // Constructor
            public Statement(int idNumber, string text, string key, NaturalResources.naturalResource resource)
            {
                this.idNumber = idNumber;
                this.text = text;
                this.key = key;
                this.resource = resource;
            }

            // Generates the identification text for the statement, which displays...
            // The statement's ID number and what resource it belongs to.
            // This is "[Resource] [ID Number]" (e.g., 'Oil 2').
            public string GenerateIdText()
            {
                // "[Resource Name] [ID Number]"
                string result = NaturalResources.GetNaturalResourceName(resource) + " " + idNumber.ToString();
                return result;
            }

            // Returns the translated version if the stagement text.
            public string GetStatementTextTranslated()
            {
                // Result to be returned.
                string result;

                // If translated text should be saved, and the text is marked as being translated already...
                // Translate it.
                if(saveTranslatedText && translatedText)
                {
                    result = text;
                }
                else
                {

                    // If the LOLManager is instantiated, the LOL SDK eis initialized...
                    // And the key is set, use the key to translate the statement.
                    if (SystemManager.IsInstantiatedAndIsLanguageLoaded() && key != "")
                    {
                        result = SystemManager.GetLanguageTextStatic(key);

                        // If translated text should be saved in the text variable, save it.
                        if(saveTranslatedText)
                        {
                            // Saves the text and makrs that the text has been translated.
                            text = result;
                            translatedText = true;
                        }
                    }
                    // The LOL SDK isn't being used, so just use the regular text.
                    else
                    {
                        result = text;
                    }
                }

                // Returns the result.
                return result;
            }

            // Generates the statement data. The group resource is the same as the statement's resource.
            public StatementData GenerateStatementData()
            {
                StatementData statementData = new StatementData();
                statementData.idNumber = idNumber;
                statementData.statementResource = resource;
                statementData.groupResource = statementData.statementResource;

                return statementData;
            }

            // Generates the statement data and gives it the provided group resource.
            public StatementData GenerateStatementData(NaturalResources.naturalResource groupResource)
            {
                StatementData statementData = new StatementData();
                statementData.idNumber = idNumber;
                statementData.statementResource = resource;
                statementData.groupResource = groupResource;

                return statementData;
            }

            // Returns true if the provided data matches this statement. Cannot check group resource.
            public bool MatchesData(StatementData data)
            {
                return data.idNumber == idNumber && data.statementResource == resource;
            }
        }

        // A group for a given resource. It holds all the statements that this resource uses.
        public class StatementGroup
        {
            // Data for a statement group.
            [System.Serializable]
            public class StatementGroupData
            {
                // The resource for this group.
                public NaturalResources.naturalResource resource;

                // The statement data.
                public List<Statement.StatementData> statementDatas = new List<Statement.StatementData>();

                // Constructor
                public StatementGroupData()
                {
                    // ...
                }

                // Constructor
                public StatementGroupData(NaturalResources.naturalResource resource)
                {
                    this.resource = resource;
                }

                // Constructor
                public StatementGroupData(NaturalResources.naturalResource resource, List<Statement.StatementData> statementDatas)
                {
                    this.resource = resource;

                    // Clears the statement data and adds the passed data.
                    this.statementDatas.Clear();
                    this.statementDatas.AddRange(statementDatas);
                }
            }

            // The resource this group is for.
            public NaturalResources.naturalResource resource;

            // The statements in this group.
            public List<Statement> statements = new List<Statement>();

            // Constructor
            public StatementGroup()
            {
                
            }

            // Constructor
            public StatementGroup(NaturalResources.naturalResource resource)
            {
                this.resource = resource;
            }

            // Gets the statements as a list.
            public List<Statement> GetStatementList()
            {
                List<Statement> statementList = new List<Statement>(statements);
                return statementList;
            }

            // Sets all statements to the group resource.
            public void SetAllStatementsToGroupResource()
            {
                // Sets the statement resources to the group resource.
                foreach(Statement statement in statements)
                {
                    statement.resource = resource;
                }
            }

            // Generates a copy of the group.
            public StatementGroup GenerateGroupCopy()
            {
                StatementGroup groupCopy = new StatementGroup(resource);
                groupCopy.statements = new List<Statement>(statements);
                groupCopy.SetAllStatementsToGroupResource();
                return groupCopy;
            }

            // Generates the statement group data.
            public StatementGroupData GenerateStatementGroupData()
            {
                // The statement group data and the provided resource.
                StatementGroupData statementGroupData = new StatementGroupData();
                statementGroupData.resource = resource;

                // Generates the data from each statement.
                foreach(Statement statement in statements)
                {
                    statementGroupData.statementDatas.Add(statement.GenerateStatementData(statementGroupData.resource));
                }

                // Returns the group data.
                return statementGroupData;
            }
        }

        // The singleton instance.
        private static KnowledgeStatementList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Natural Resource Groups
        // Renewable
        private StatementGroup biomassGroup;
        private StatementGroup hydroGroup;
        private StatementGroup geothermalGroup;
        private StatementGroup solarGroup;
        private StatementGroup waveGroup;
        private StatementGroup windGroup;

        // Non-renewable
        private StatementGroup coalGroup;
        private StatementGroup oilGroup;
        private StatementGroup naturalGasGroup;
        private StatementGroup nuclearGroup;

        // Gets set to 'true' if the groups have all been generated.
        private bool groupsGenerated = false;

        // Constructor
        private KnowledgeStatementList()
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

                // Generates the groups.
                GenerateGroups();
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
        void Start()
        {
            // ...
        }

        // Gets the instance.
        public static KnowledgeStatementList Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeStatementList>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("KnowledgeStatementList (singleton)");
                        instance = go.AddComponent<KnowledgeStatementList>();
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

        // Generates a test statement.
        public static Statement GenerateTestStatement()
        {
            Statement statement = new Statement();

            // Default values.
            statement.idNumber = 0;
            statement.text = "This is a test statement.";
            statement.key = "";
            statement.resource = NaturalResources.naturalResource.unknown;

            return statement;
        }

        // Generates the groups.
        public void GenerateGroups()
        {
            // Renewable
            // Biomass
            biomassGroup = new StatementGroup(NaturalResources.naturalResource.biomass);
            biomassGroup.statements.Add(new Statement(0,
                "A renewable resource that burns plant and/or waste materials to turn a turbine to generate energy.", 
                "kst_bms_00"));
            biomassGroup.statements.Add(new Statement(1,
                "A renewable resource that burns plant and/or waste materials for generating energy. It can also be processed to make fuel that can be used for vehicle engines.", 
                "kst_bms_01"));
            biomassGroup.statements.Add(new Statement(2,
                "A renewable resource that burns plant and/or waste materials for generating energy. It can also be used for fuel, which cuts down on fossil fuel usage.", 
                "kst_bms_02"));
            biomassGroup.statements.Add(new Statement(3,
                "A renewable resource that burns plant and/or waste materials, which produces carbon dioxide. Plants grown for this resource can consume the carbon dioxide, reducing the gases released into the air.", 
                "kst_bms_03"));
            biomassGroup.statements.Add(new Statement(4,
                "A renewable resource that burns plant and/or waste materials to generate energy. Plants used for this process cannot be used for other activities.", 
                "kst_bms_04"));
            biomassGroup.SetAllStatementsToGroupResource();

            // Geothermal
            geothermalGroup = new StatementGroup(NaturalResources.naturalResource.geothermal);
            geothermalGroup.statements.Add(new Statement(0,
                "A renewable resource that uses the Earth's internal heat to heat water and/or make steam to turn a turbine and generate energy.", 
                "kst_gtl_00"));
            geothermalGroup.statements.Add(new Statement(1,
                "A resource that involves drilling wells to get hot water or steam from the Earth's internal heat. Water can be pumped into these wells to use this resource perpetually.",
                "kst_gtl_01"));
            geothermalGroup.statements.Add(new Statement(2,
                "A renewable resource that uses the Earth's internal heat for generating energy. This is a clean, safe resource that can be used without processing.", 
                "kst_gtl_02"));
            geothermalGroup.statements.Add(new Statement(3,
                "A resource that utilizes the Earth's internal heat. Many parts of the world don't have underground heat sources close enough to the Earth's surface to make using this resource economically viable.", 
                "kst_gtl_03"));
            geothermalGroup.statements.Add(new Statement(4,
                "A resource that uses hot water and/or steam created from the Earth's internal heat. The steam can come with unwanted mineral byproducts, and resource-related activities can cause earthquakes.", 
                "kst_gtl_04"));
            geothermalGroup.SetAllStatementsToGroupResource();

            // Hydro
            hydroGroup = new StatementGroup(NaturalResources.naturalResource.hydro);
            hydroGroup.statements.Add(new Statement(0,
                "A renewable resource that harnesses water moving through a stream to spin turbines to generate energy.", 
                "kst_hdo_00"));
            hydroGroup.statements.Add(new Statement(1,
                "A resource that uses moving water to generate energy. The water is held in a reservoir and flows downstream through a dam.", 
                "kst_hdo_01"));
            hydroGroup.statements.Add(new Statement(2,
                "A resource that uses water flowing through a stream to generate energy. It doesn't burn fuel, doesn't use up water, and causes less pollution than other resources.", 
                "kst_hdo_02"));
            hydroGroup.statements.Add(new Statement(3,
                "A renewable resource that involves water dams, which disrupt the flow of streams. These dams can cause flooding upstream and reshape environments downstream.", 
                "kst_hdo_03"));
            hydroGroup.statements.Add(new Statement(4,
                "A resource that uses water moving through a dam. The dam slows the release of silt, which can cause downstream areas to become more exposed to storms and rising sea levels.", 
                "kst_hdo_04"));
            hydroGroup.SetAllStatementsToGroupResource();

            // Solar
            solarGroup = new StatementGroup(NaturalResources.naturalResource.solar);
            solarGroup.statements.Add(new Statement(0,
                "A renewable resource that uses panels to convert sunlight to energy. The panels are made up of multiple cells.", 
                "kst_slr_00"));
            solarGroup.statements.Add(new Statement(1,
                "A resource that uses panels to generate energy from sunlight. The panels can be placed in fields, attached to buildings, and mounted on certain structures.", 
                "kst_slr_01"));
            solarGroup.statements.Add(new Statement(2,
                "A resource that uses panels to generate energy from sunlight. This generation method produces virtually no pollutants when in operation.", 
                "kst_slr_02"));
            solarGroup.statements.Add(new Statement(3,
                "A resource that uses sunlight to generate energy. No energy can be generated at night, so extra energy from the daytime must be stored using a battery.", 
                "kst_slr_03"));
            solarGroup.statements.Add(new Statement(4,
                "A renewable resource that converts sunlight into energy. While sunlight is in abundance, the technology to harness it can be expensive.", 
                "kst_slr_04"));
            solarGroup.SetAllStatementsToGroupResource();

            // Wave
            waveGroup = new StatementGroup(NaturalResources.naturalResource.wave);
            waveGroup.statements.Add(new Statement(0,
                "A renewable resource that uses the waves and tides to generate energy. A narrow bay or estuary may need to be closed off to use these types of energy plants.", 
                "kst_wve_00"));
            waveGroup.statements.Add(new Statement(1,
                "A clean, renewable resource that generates energy by using waves. Areas with high wind have the most potential to produce waves.", 
                "kst_wve_01"));
            waveGroup.statements.Add(new Statement(2,
                "A resource that uses waves to generate energy. The structures used for this method are designed to withstand corrosive seawater and costal storms.", 
                "kst_wve_02"));
            waveGroup.statements.Add(new Statement(3,
                "A renewable resource that generates energy from waves. No energy can be produced if there are no waves.", 
                "kst_wve_03"));
            waveGroup.statements.Add(new Statement(4,
                "A resource that uses waves and tides to generate energy. The structures used for this resource are expensive to install and maintain.", 
                "kst_wve_04"));
            waveGroup.SetAllStatementsToGroupResource();

            // Wind
            windGroup = new StatementGroup(NaturalResources.naturalResource.wind);
            windGroup.statements.Add(new Statement(0,
                "A renewable resource that uses moving air to turn turbines to generate energy. The air moves by convection: warm air rises and cold air comes in to fill the left-over space.", 
                "kst_wnd_00"));
            windGroup.statements.Add(new Statement(1,
                "A resource that generates energy by turning turbines using moving air. The turbines can be placed on land or offshore in shallow waters.", 
                "kst_wnd_01"));
            windGroup.statements.Add(new Statement(2,
                "A clean renewable resource that uses moving air for generating energy. This resource produces no pollutants and can be found effectively anywhere.", 
                "kst_wnd_02"));
            windGroup.statements.Add(new Statement(3,
                "A resource that turns turbines with moving air to generate energy, which depends on the weather. When there's none of this resource, no energy is generated.", 
                "kst_wnd_03"));
            windGroup.statements.Add(new Statement(4,
                "A renewable resource that uses moving air to spin turbines to generate energy. Structures made for this resource can be expensive and wear out quickly.", 
                "kst_wnd_04"));
            windGroup.SetAllStatementsToGroupResource();

            // Non-renewable
            // Coal
            coalGroup = new StatementGroup(NaturalResources.naturalResource.coal);
            coalGroup.statements.Add(new Statement(0,
                "A non-renewable fossil fuel resource that's turned into powder and fed to a furnace to create heat. This heat boils water to produce steam and said steam turns a turbine to generate energy.", 
                "kst_col_00"));
            coalGroup.statements.Add(new Statement(1,
                "A non-renewable fossil fuel resource formed from plant remains at the bottom of swamps. The organic matter turns into this resource after millions of years and is obtained via mining.", 
                "kst_col_01"));
            coalGroup.statements.Add(new Statement(2,
                "A non-renewable resource that's burned for producing energy. It can be mined on the surface or underground. Surface mining is safer for miners but has a higher chance of releasing toxic materials into the air.", 
                "kst_col_02"));
            coalGroup.statements.Add(new Statement(3,
                "When burned for producing energy, this resource releases carbon dioxide and potentially other harmful materials into the air. This resource produces more carbon dioxide than oil and natural gas when burned.", 
                "kst_col_03"));
            coalGroup.statements.Add(new Statement(4,
                "When this resource is mined on the surface, it exposes sulfur to the atmosphere. Sulfur can mix with water and air to create sulfuric acid, a highly corrosive chemical that can damage environments.", 
                "kst_col_04"));
            coalGroup.SetAllStatementsToGroupResource();

            // Natural Gas
            naturalGasGroup = new StatementGroup(NaturalResources.naturalResource.naturalGas);
            naturalGasGroup.statements.Add(new Statement(0,
                "A non-renewable fossil fuel resource that's burned to produce hot gases or steam to turn a turbine to generate energy. It's mostly methane.", 
                "kst_ngs_00"));
            naturalGasGroup.statements.Add(new Statement(1,
                "A non-renewable resource that's burned for producing energy. It can often be found with oil and coal in underground deposits since it forms with them, though it forms at higher temperatures than oil.", 
                "kst_ngs_01"));
            naturalGasGroup.statements.Add(new Statement(2,
                "A non-renewable fossil fuel resource that's burned to produce hot gases or steam for generating energy. It can also be used for cooking and heating.", 
                "kst_ngs_02"));
            naturalGasGroup.statements.Add(new Statement(3,
                "A non-renewable resource that's burned to produce steam or hot gases for energy. It must be processed to remove poisonous chemicals and water before it can be burned.", 
                "kst_ngs_03"));
            naturalGasGroup.statements.Add(new Statement(4,
                "A fossil fuel resource that's burned for generating energy. It burns cleaner and produces less carbon dioxide than other fossil fuels, but it still emits pollutants. It's also highly flammable.", 
                "kst_ngs_04"));
            naturalGasGroup.SetAllStatementsToGroupResource();

            // Nuclear
            nuclearGroup = new StatementGroup(NaturalResources.naturalResource.nuclear);
            nuclearGroup.statements.Add(new Statement(0,
                "A non-renewable resource that generates energy by splitting the nucleus of an atom. This reaction produces heat, which boils water to produce steam. This steam is used to turn a turbine to generate energy.", 
                "kst_nlr_00"));
            nuclearGroup.statements.Add(new Statement(1,
                "A resource that uses uranium for generating energy. Uranium is concentrated into fuel rods and has its atoms split using tiny particles. These particles are controlled to avoid causing a dangerous explosion.", 
                "kst_nlr_01"));
            nuclearGroup.statements.Add(new Statement(2,
                "A non-renewable resource that generates energy by splitting uranium atoms. This resource produces no pollutants, and if there are no accidents, only steam is released into the air.", 
                "kst_nlr_02"));
            nuclearGroup.statements.Add(new Statement(3,
                "A non-renewable resource that generates energy by splitting uranium atoms, which also produces radioactive waste. This waste can remain dangerous for hundreds to thousands of years.", 
                "kst_nlr_03"));
            nuclearGroup.statements.Add(new Statement(4,
                "A resource that generates energy by splitting atoms. This reaction creates dangerous radioactive waste, which must be securely stored to prevent harm to people and the environment.", 
                "kst_nlr_04"));
            nuclearGroup.SetAllStatementsToGroupResource();

            // Oil
            oilGroup = new StatementGroup(NaturalResources.naturalResource.oil);
            oilGroup.statements.Add(new Statement(0,
                "A non-renewable fossil fuel resource that's burned to produce steam to spin a turbine to generate energy. It's a black or dark brown liquid found in rock layers in the Earth's crust.", 
                "kst_oil_00"));
            oilGroup.statements.Add(new Statement(1,
                "This liquid resource forms from dead organic material that's buried under sediments, kept away from oxygen, and exposed to both high heat and pressure. It forms at a lower temperature than natural gas.", 
                "kst_oil_01"));
            oilGroup.statements.Add(new Statement(2,
                "A resource that's obtained in a crude form, which has a mix of different hydrocarbons. It's then refined via heating to separate it into different compounds, which all boil at different temperatures.", 
                "kst_oil_02"));
            oilGroup.statements.Add(new Statement(3,
                "A non-renewable liquid resource that's burned for generating energy. It can be extracted on land and at sea, but drilling at sea can cause a spill if something goes wrong.", 
                "kst_oil_03"));
            oilGroup.statements.Add(new Statement(4,
                "A non-renewable liquid resource that can be burned for generating energy and can be refined for various uses. It takes millions of years to form and releases toxic chemicals into the air when burned.", 
                "kst_oil_04"));
            oilGroup.SetAllStatementsToGroupResource();

            // All the groups have been generated.
            groupsGenerated = true;
        }
        
        // Returns 'true' if the groups have been generated.
        public bool AreGroupsGenerated()
        {
            return groupsGenerated;
        }

        // Gets the requested group as a copy.
        protected StatementGroup GetGroup(NaturalResources.naturalResource res)
        {
            // The group to be returned.
            StatementGroup group;

            // Checks what group to return.
            switch(res)
            {
                default:
                    group = null;
                    break;

                case NaturalResources.naturalResource.biomass:
                    group = biomassGroup;
                    break;

                case NaturalResources.naturalResource.geothermal:
                    group = geothermalGroup;
                    break;

                case NaturalResources.naturalResource.hydro:
                    group = hydroGroup;
                    break;

                case NaturalResources.naturalResource.solar:
                    group = solarGroup;
                    break;

                case NaturalResources.naturalResource.wave:
                    group = waveGroup;
                    break;

                case NaturalResources.naturalResource.wind:
                    group = windGroup;
                    break;

                case NaturalResources.naturalResource.coal:
                    group = coalGroup;
                    break;

                case NaturalResources.naturalResource.oil:
                    group = oilGroup;
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    group = naturalGasGroup;
                    break;

                case NaturalResources.naturalResource.nuclear:
                    group = nuclearGroup;
                    break;
            }

            return group;
        }

        // Gets a group copy so that the original group won't be edited.
        public StatementGroup GetGroupCopy(NaturalResources.naturalResource res)
        {
            // The original group and the group copy.
            StatementGroup group = GetGroup(res);
            StatementGroup groupCopy = null;

            // If the group exists, create a group copy.
            if(group != null)
            {
                groupCopy = group.GenerateGroupCopy();
            }

            return groupCopy;
        }

        // Gets the groups as a list.
        protected List<StatementGroup> GetAllGroups()
        {
            // The statement groups.
            // Put in the same order as their enum in 'NaturalResources' class.
            List<StatementGroup> groups = new List<StatementGroup>()
            {
                biomassGroup,
                hydroGroup,
                geothermalGroup,
                solarGroup,
                waveGroup,
                windGroup,
                coalGroup,
                oilGroup,
                naturalGasGroup,
                nuclearGroup
            };

            return groups;
        }

        // Gets the groups copy as a list.
        protected List<StatementGroup> GetAllGroupsCopy()
        {
            // The statement group copy.
            // Put in the same order as their enum in 'NaturalResources' class.
            List<StatementGroup> groups = new List<StatementGroup>()
            {
                biomassGroup.GenerateGroupCopy(),
                hydroGroup.GenerateGroupCopy(),
                geothermalGroup.GenerateGroupCopy(),
                solarGroup.GenerateGroupCopy(),
                waveGroup.GenerateGroupCopy(),
                windGroup.GenerateGroupCopy(),
                coalGroup.GenerateGroupCopy(),
                oilGroup.GenerateGroupCopy(),
                naturalGasGroup.GenerateGroupCopy(),
                nuclearGroup.GenerateGroupCopy()
            };

            return groups;
        }

        // Gets the group data.
        public StatementGroup.StatementGroupData GetGroupData(NaturalResources.naturalResource res)
        {
            // The group data and the group.
            StatementGroup.StatementGroupData groupData;
            StatementGroup group = GetGroup(res);

            // If the group is set, get its data.
            if(group != null)
            {
                groupData = group.GenerateStatementGroupData();
            }
            // No group, so return null.
            else
            {
                groupData = null;
            }

            return groupData;
        }

        // Gets all the group datas.
        public List<StatementGroup.StatementGroupData> GetAllGroupDatas()
        {
            // The statement group datas.
            List<StatementGroup.StatementGroupData> groupDatas = new List<StatementGroup.StatementGroupData>()
            {
                biomassGroup.GenerateStatementGroupData(),
                hydroGroup.GenerateStatementGroupData(),
                geothermalGroup.GenerateStatementGroupData(),
                solarGroup.GenerateStatementGroupData(),
                waveGroup.GenerateStatementGroupData(),
                windGroup.GenerateStatementGroupData(),
                coalGroup.GenerateStatementGroupData(),
                oilGroup.GenerateStatementGroupData(),
                naturalGasGroup.GenerateStatementGroupData(),
                nuclearGroup.GenerateStatementGroupData()
            };

            return groupDatas;
        }
    }
}