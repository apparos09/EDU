using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action unit prefabs.
    public class ActionUnitPrefabs : MonoBehaviour
    {
        // The singleton instance.
        private static ActionUnitPrefabs instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // NOTE: the values at index 0 shouldn't be used, as those are just default versions or empty values.
        // The order in the list should match up with the id numbers.

        // The generator prefabs.
        public List<ActionUnitGenerator> generatorPrefabs = new List<ActionUnitGenerator>();

        // The defense prefabs.
        public List<ActionUnitDefense> defensePrefabs = new List<ActionUnitDefense>();

        // The enemy prefabs.
        public List<ActionUnitEnemy> enemyPrefabs = new List<ActionUnitEnemy>();

        // Constructor
        private ActionUnitPrefabs()
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

        // Gets the instance.
        public static ActionUnitPrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionUnitPrefabs>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Unit Prefabs (singleton)");
                        instance = go.AddComponent<ActionUnitPrefabs>();
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

        // TODO: the get prefab functions should use generic unit lists and cite them instead...
        // Of citing a unique function per list.
        // Gets the prefab.
        protected ActionUnit GetPrefab(ActionUnit.unitType unitType, int index)
        {
            // The prefab.
            ActionUnit prefab;

            // Gets the prefab based on the provided tpye.
            switch (unitType)
            {
                case ActionUnit.unitType.unknown:
                default:
                    prefab = null;
                    break;

                case ActionUnit.unitType.generator:
                    prefab = GetGeneratorPrefab(index);
                    break;

                case ActionUnit.unitType.defense:
                    prefab = GetDefensePrefab(index);
                    break;

                case ActionUnit.unitType.enemy:
                    prefab = GetEnemyPrefab(index);
                    break;
            }

            return prefab;
        }

        // Gets the prefab by its id.
        public ActionUnit GetPrefabById(ActionUnit.unitType unitType, int id)
        {
            // The prefab.
            ActionUnit prefab;

            // Gets the prefab based on the provided tpye.
            switch (unitType)
            {
                case ActionUnit.unitType.unknown:
                default:
                    prefab = null;
                    break;

                case ActionUnit.unitType.generator:
                    prefab = GetGeneratorPrefabById(id);
                    break;

                case ActionUnit.unitType.defense:
                    prefab = GetDefensePrefabById(id);
                    break;

                case ActionUnit.unitType.enemy:
                    prefab = GetEnemyPrefabById(id);
                    break;
            }

            return prefab;
        }

        // Instantiates and returns a prefab.
        // The unit id determines what kind of action unit to use.
        protected ActionUnit InstantiatePrefab(ActionUnit.unitType unitType, int index)
        {
            // The prefab and the new object.
            ActionUnit prefab = GetPrefab(unitType, index);
            ActionUnit actionUnit = null;

            // If the prefab isn't equal to null, instantiate it.
            if (prefab != null)
            {
                actionUnit = Instantiate(prefab);
            }

            // Returns the new UNIT.
            return actionUnit;
        }

        // Generates an id list using the provided action unit list.
        protected static List<int> GenerateActionUnitIdList(List<ActionUnit> unitList, bool includeId0, bool includeId1)
        {
            // The list to be returned.
            List<int> list = new List<int>();

            // Goes through all the units in the provided unit list.
            foreach (ActionUnit unit in unitList)
            {
                // If the unit exists.
                if (unit != null)
                {
                    // ID isn't in list, so add it.
                    if (!list.Contains(unit.idNumber))
                    {
                        // Checks the unit's id number.
                        switch (unit.idNumber)
                        {
                            default: // Add
                                list.Add(unit.idNumber);
                                break;

                            case 0:
                                // If ID 0 should be included, add it.
                                if (includeId0)
                                    list.Add(unit.idNumber);
                                break;

                            case 1:
                                // If ID 1 should be included, add it.
                                if (includeId1)
                                    list.Add(unit.idNumber);
                                break;
                        }
                    }
                }
            }

            // Returns the result.
            return list;
        }


        // GENERATOR //
        // Gets a generator prefab. This does NOT instantiate the prefab.
        public ActionUnitGenerator GetGeneratorPrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < generatorPrefabs.Count)
            {
                return generatorPrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Gets a generator prefab by its id. This does NOT instantiate the prefab.
        public ActionUnitGenerator GetGeneratorPrefabById(int id)
        {
            // Goes through all the prefabs.
            foreach (ActionUnitGenerator generator in generatorPrefabs)
            {
                // Generator exists.
                if (generator != null)
                {
                    // Generator id matches provided id.
                    if (generator.idNumber == id)
                    {
                        return generator;
                    }
                }
            }

            // None found.
            return null;
        }

        // Gets the generator prefab using the resource it represents.
        // The generators in the list should match up with the numbers for their resources, so using GetGeneratorPrefab...
        // Is preferable and presumably safe.
        public ActionUnitGenerator GetGeneratorPrefabByResource(NaturalResources.naturalResource resource)
        {
            // Goes through all the prefabs.
            foreach(ActionUnitGenerator generator in generatorPrefabs)
            {
                // Generator exists.
                if(generator != null)
                {
                    // Generator resource matches provided resource.
                    if(generator.resource == resource)
                    {
                        return generator;
                    }
                }
            }

            // None found.
            return null;
        }

        // Instantiates and returns a prefab.
        public ActionUnitGenerator InstantiateGeneratorPrefab(int index)
        {
            // Gets the prefab.
            ActionUnitGenerator prefab = GetGeneratorPrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Instantiates and returns a prefab based on the provided resource type.
        public ActionUnitGenerator InstantiateGeneratorPrefabByResource(NaturalResources.naturalResource resource)
        {
            // Gets the prefab.
            ActionUnitGenerator prefab = GetGeneratorPrefabByResource(resource);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Generates a list of generator prefab ids.
        public List<int> GenerateGeneratorPrefabIdList(bool includeId0)
        {
            // Creates a unit list and adds the generator prefabs.
            List<ActionUnit> unitList = new List<ActionUnit>();
            unitList.AddRange(generatorPrefabs);

            // Uses the generic action unit id list generator function.
            return GenerateActionUnitIdList(unitList, includeId0, true);

            // // The list to be returned.
            // List<int> list = new List<int>();
            // 
            // // Goes through all the prefabs.
            // foreach (ActionUnitGenerator generator in generatorPrefabs)
            // {
            //     // If the generator exists.
            //     if (generator != null)
            //     {
            //         // ID isn't in list, so add it.
            //         if (!list.Contains(generator.idNumber))
            //         {
            //             switch (generator.idNumber)
            //             {
            //                 default: // Add
            //                     list.Add(generator.idNumber);
            //                     break;
            // 
            //                 case 0:
            //                     // If ID 0 should be included, add it.
            //                     if (includeId0)
            //                         list.Add(generator.idNumber);
            //                     break;
            //             }
            //         }
            //     }
            // }
            // 
            // // Returns the result.
            // return list;
        }


        // DEFENSE //
        // Gets a defense prefab. This does NOT instantiate the prefab.
        public ActionUnitDefense GetDefensePrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < defensePrefabs.Count)
            {
                return defensePrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Gets a defense prefab by its id. This does NOT instantiate the prefab.
        public ActionUnitDefense GetDefensePrefabById(int id)
        {
            // Goes through all the prefabs.
            foreach (ActionUnitDefense defense in defensePrefabs)
            {
                // Defense exists.
                if (defense != null)
                {
                    // Defense id matches provided id.
                    if (defense.idNumber == id)
                    {
                        return defense;
                    }
                }
            }

            // None found.
            return null;
        }

        // Instantiates and returns a prefab.
        public ActionUnitDefense InstantiateDefensePrefab(int index)
        {
            // Gets the prefab.
            ActionUnitDefense prefab = GetDefensePrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Instantiates and returns a prefab.
        public ActionUnitDefense InstantiateDefensePrefabById(int id)
        {
            // Gets the prefab.
            ActionUnitDefense prefab = GetDefensePrefabById(id);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Generates the defense prefab ID list.
        public List<int> GenerateDefensePrefabIdList(bool includeId0, bool includeId1)
        {
            // Creates a unit list and adds the defense prefabs.
            List<ActionUnit> unitList = new List<ActionUnit>();
            unitList.AddRange(defensePrefabs);

            // Uses the generic action unit id list generator function.
            return GenerateActionUnitIdList(unitList, includeId0, includeId1);

            // // The list to be returned.
            // List<int> list = new List<int>();
            // 
            // // Goes through all the prefabs.
            // foreach(ActionUnitDefense defense in  defensePrefabs)
            // {
            //     // If the defense exists.
            //     if(defense != null)
            //     { 
            //         // ID isn't in list, so add it.
            //         if(!list.Contains(defense.idNumber))
            //         {
            //             switch(defense.idNumber)
            //             {
            //                 default: // Add
            //                     list.Add(defense.idNumber);
            //                     break;
            // 
            //                 case 0:
            //                     // If ID 0 should be included, add it.
            //                     if(includeId0)
            //                         list.Add(defense.idNumber);
            //                     break;
            // 
            //                 case 1:
            //                     // If ID 1 should be included, add it.
            //                     if(includeId1)
            //                         list.Add(defense.idNumber);
            //                     break;
            //             }
            //         }
            //     }
            // }
            // 
            // // Returns the result.
            // return list;
        }


        // ENEMY //
        // Gets a enemy prefab. This does NOT instantiate the prefab.
        public ActionUnitEnemy GetEnemyPrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < enemyPrefabs.Count)
            {
                return enemyPrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Gets a enemy prefab by its id. This does NOT instantiate the prefab.
        public ActionUnitEnemy GetEnemyPrefabById(int id)
        {
            // Goes through all the prefabs.
            foreach (ActionUnitEnemy enemy in enemyPrefabs)
            {
                // Defense exists.
                if (enemy != null)
                {
                    // Defense id matches provided id.
                    if (enemy.idNumber == id)
                    {
                        return enemy;
                    }
                }
            }

            // None found.
            return null;
        }

        // Instantiates and returns a prefab.
        public ActionUnitEnemy InstantiateEnemyPrefab(int index)
        {
            // Gets the prefab.
            ActionUnitEnemy prefab = GetEnemyPrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Instantiates and returns a prefab.
        public ActionUnitEnemy InstantiateEnemyPrefabById(int id)
        {
            // Gets the prefab.
            ActionUnitEnemy prefab = GetEnemyPrefabById(id);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Generates the enemy prefab ID list.
        public List<int> GenerateEnemyPrefabIdList(bool includeId0)
        {
            // Creates a unit list and adds the enemy prefabs.
            List<ActionUnit> unitList = new List<ActionUnit>();
            unitList.AddRange(enemyPrefabs);

            // Uses the generic action unit id list generator function.
            return GenerateActionUnitIdList(unitList, includeId0, true);

            // // The list to be returned.
            // List<int> list = new List<int>();
            // 
            // // Goes through all the prefabs.
            // foreach (ActionUnitEnemy enemy in enemyPrefabs)
            // {
            //     // If the enemy exists.
            //     if (enemy != null)
            //     {
            //         // ID isn't in list, so add it.
            //         if (!list.Contains(enemy.idNumber))
            //         {
            //             // Goes through all the id numbers.
            //             switch (enemy.idNumber)
            //             {
            //                 default: // Add
            //                     list.Add(enemy.idNumber);
            //                     break;
            // 
            //                 case 0:
            //                     // If ID 0 should be included, add it.
            //                     if (includeId0)
            //                         list.Add(enemy.idNumber);
            //                     break;
            //             }
            //         }
            //     }
            // }
            // 
            // // Returns the result.
            // return list;
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