using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The defense unit for the player.
    public class ActionUnitDefense : ActionUnitUser
    {
        // The type of the defense unit.
        public enum defenseType { unknown, blaster, shield, trap }

        // The total number of defense IDs.
        public const int DEFENSE_ID_COUNT = 17;

        [Header("Defense")]

        // The type of the defense unit.
        public defenseType defType = defenseType.unknown;

        // The sprite renderer for a platform that can be displayed below the user unit.
        [Tooltip("A platform shown under the unit if it's on a water tile.")]
        public SpriteRenderer platformSpriteRenderer;

        // Enables visuals that are used for showing that the attack energy is being blocked.
        private bool attackEnergyBlockedVisualAvailable = true;

        // If 'true', the unit shows when it's attack energy is being blocked.
        [Tooltip("If true, the unit shows if it's attack energy is being blocked.")]
        public bool showAttackEnergyBlocked = true;

        // Gets set to 'true' if marked as energy blocked.
        private bool markedAsAttackEnergyBlocked = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Updates the platform visibility.
            UpdatePlatformVisible();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            // If this unit can attack.
            if (CanAttack())
            {
                // Checks if colliding with an enemy unit.
                ActionUnitEnemy enemyUnit;

                // Tries to get the component.
                if (collision.TryGetComponent(out enemyUnit))
                {
                    // Perform an attack.
                    PerformAttack();
                }
            }
        }


        // Use the one in the Action Unit Prefabs class.
        // // Generate ID List
        // // Returns the defense id list based on the DEFENSE_ID_COUNT.
        // // includeId0: determines if ID 0 should be included.
        // public static List<int> GenerateDefenseIdList(bool includeId0)
        // {
        //     // The list to return.
        //     List<int> list = new List<int>();
        // 
        //     // The id number.
        //     int id = 0;
        // 
        //     // While the id number is less than the defense id count.
        //     while(id < DEFENSE_ID_COUNT)
        //     {
        //         // Checks id.
        //         switch(id)
        //         {
        //             case 0: // ID = 0
        //                 // If id0 should be included, add it.
        //                 if(includeId0)
        //                 {
        //                     list.Add(id);
        //                 }
        // 
        //                 break;
        // 
        //             default: // OTHER
        //                 list.Add(id);
        //                 break;
        //         }
        // 
        //         // Increase the ID.
        //         id++;
        //     }
        // 
        //     // Returns the list.
        //     return list;
        // }


        // TYPE, NAMES //
        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.defense;
        }

        // Gets the display name for the unit's card.
        public override string GetUnitCardDisplayName()
        {
            return GetDefenseTypeAbbreviation(defType);
        }

        // Gets the defenes type.
        public static string GetDefenseTypeName(defenseType type)
        {
            // The value to return.
            string value;

            // The LOL SDK has been initialized, so use the key.
            if (SystemManager.IsInstantiatedAndIsLanguageLoaded())
            {
                // Gets the defense type abbreviation key.
                string key = GetDefenseTypeNameKey(type);

                // Gets the text.
                value = SystemManager.GetLanguageTextStatic(key);
            }
            else
            {
                // Checks the defense type to see what text to use
                switch (type)
                {
                    default:
                        value = "Unknown";
                        break;

                    case defenseType.blaster:
                        value = "Blaster";
                        break;

                    case defenseType.shield:
                        value = "Shield";
                        break;

                    case defenseType.trap:
                        value = "Trap";
                        break;
                }
            }

            return value;
        }

        // Gets the defenes type name key.
        public static string GetDefenseTypeNameKey(defenseType type)
        {
            // The value to return.
            string value;

            // Checks the unit ID.
            switch (type)
            {
                default:
                    value = "kwd_unknown";
                    break;

                case defenseType.blaster:
                    value = "def_bsr_nme";
                    break;

                case defenseType.shield:
                    value = "def_shd_nme";
                    break;

                case defenseType.trap:
                    value = "def_trp_nme";
                    break;
            }

            return value;
        }

        // Gets the unit name abbreviation.
        public static string GetDefenseTypeAbbreviation(defenseType type)
        {
            // The value to return.
            string value;

            // If the LOLSDK exists, get the abbreviation from that.
            if(SystemManager.IsInstantiatedAndIsLanguageLoaded())
            {
                // Gets the defense type abbreviation key.
                string key = GetDefenseTypeAbbreviationKey(type);

                // Gets the text.
                value = SystemManager.GetLanguageTextStatic(key);
            }
            else
            {
                // Checks the defense type to see what text to use
                switch (type)
                {
                    default:
                        value = "UKN";
                        break;

                    case defenseType.blaster:
                        value = "BSR";
                        break;

                    case defenseType.shield:
                        value = "SHD";
                        break;

                    case defenseType.trap:
                        value = "TRP";
                        break;
                }
            } 

            return value;
        }

        // Gets the unit name abbreviation key.
        public static string GetDefenseTypeAbbreviationKey(defenseType type)
        {
            // The value to return.
            string value;

            // Checks the unit ID.
            switch(type)
            {
                default:
                    value = "kwd_unknown_abv";
                    break;

                case defenseType.blaster:
                    value = "def_bsr_nme_abv";
                    break;

                case defenseType.shield:
                    value = "def_shd_nme_abv";
                    break;

                case defenseType.trap:
                    value = "def_trp_nme_abv";
                    break;
            }

            return value;
        }

        // PLATFORM
        // Returns 'true' if the platform is active.
        public bool IsPlatformVisible()
        {
            return platformSpriteRenderer.gameObject.activeSelf;
        }

        
        // Sets if the platform should be shown or not.
        public void SetPlatformVisible(bool value)
        {
            platformSpriteRenderer.gameObject.SetActive(value);
        }

        // Shows the platform sprite.
        public void ShowPlatform()
        {
            SetPlatformVisible(true);
        }

        // Hides the platform sprite.
        public void HidePlatform()
        {
            SetPlatformVisible(false);
        }

        // Enables the platform if the unit is on water. Disables it if the unit isn't on water.
        public void UpdatePlatformVisible()
        {
            // Checks if a tile exists.
            if(tile != null)
            {
                // If it's a land tile and the tile doesn't have a water hazard, don't use the platform.
                if(tile.IsLandTile() && !tile.HasTileOverlayWaterHazard())
                {
                    SetPlatformVisible(false);
                }
                // If it's not a land tile, use the platform.
                else
                {
                    SetPlatformVisible(true);
                }
            }
            else
            {
                // Tile doesn't exist, so the platform should be invisible by default.
                SetPlatformVisible(false);
            }
        }

        // Returns 'true' if the defense unit has a target to attack.
        // By default, this checks if there are any enemies in the same row as this defense unit.
        public virtual bool HasTarget()
        {
            // Result to be returned.
            bool hasTarget = false;

            // On a tile.
            if (tile != null)
            {
                // TODO: enable limit on range.

                // Gets the tile's row.
                int row = tile.GetMapRowPosition();

                // If the row is valid.
                // if(actionManager.actionStage.ValidMapPosition)
                if(actionManager.actionStage.ValidMapRow(row))
                {
                    // The defense has a target.
                    hasTarget = actionManager.actionStage.IsEnemyInRowRightOfPosition(row, transform.position, true, false);
                }

            }

            return hasTarget;
        }

        // Returns 'true' if the attack energy blocked visual is available for use.
        public bool IsAttackEnergyBlockedVisualAvailable()
        {
            return attackEnergyBlockedVisualAvailable;
        }

        // Returns 'true', if the unit should show when it's attack energy is being blocked.
        public bool ShowAttackEnergyBlocked()
        {
            return showAttackEnergyBlocked;
        }

        // Returns 'true' if the attack energy blocked visual is available and it should be shown.
        public bool IsAttackEnergyBlockedVisualAvailableAndShowable()
        {
            return attackEnergyBlockedVisualAvailable && showAttackEnergyBlocked;
        }

        // Returns 'true' if the unit is marked as having its attack energy blocked.
        public bool IsMarkedAsAttackEnergyBlocked()
        {
            return markedAsAttackEnergyBlocked;
        }

        // Performs an attack.
        public virtual void PerformAttack()
        {
            OnUnitAttackPerformed();
        }

        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            base.OnUnitDeath();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the action unit can attack.
            if(CanAttack())
            {
                // If the defense has a target.
                if(HasTarget())
                {
                    // Performs an attack.
                    PerformAttack();
                }
            }

            // The owner is set.
            if (owner != null)
            {
                // If attack energy being blocked should be shown...
                // Attacking is enabled, and there's an attack energy cost...
                // Alter the sprite to show if the unit is usable or unusable.
                if (IsAttackEnergyBlockedVisualAvailableAndShowable() && attackingEnabled && HasAttackEnergyCost())
                {
                    // Saves that the attack energy is blocked or not blocked.
                    bool attackEnergyBlocked = IsAttackEnergyBlocked();

                    // If the owner is blocking attack energy, make as unusable.
                    if (attackEnergyBlocked && !markedAsAttackEnergyBlocked)
                    {
                        unitAnimations.PlayUnusableAnimation();
                        markedAsAttackEnergyBlocked = true;
                    }
                    // Energy not being blocked, so mark as usable if not already.
                    else if (!attackEnergyBlocked && markedAsAttackEnergyBlocked)
                    {
                        unitAnimations.PlayUsableAnimation();
                        markedAsAttackEnergyBlocked = false;
                    }
                }
                else
                {
                    // If currenty marked as having its attack energy blocked when it shouldn't be...
                    // Mark the unit as usable.
                    if (markedAsAttackEnergyBlocked)
                    {
                        unitAnimations.PlayUsableAnimation();
                        markedAsAttackEnergyBlocked = false;
                    }
                }
            }

        }
    }
}