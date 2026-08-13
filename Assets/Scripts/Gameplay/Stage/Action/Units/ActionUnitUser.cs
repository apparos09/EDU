using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The player user unit.
    public abstract class ActionUnitUser : ActionUnit
    {
        [Header("User")]

        // The tiles that are valid to place this unit on.
        // The metal tile is only for the lane blaster, so it's not included by default.
        public List<ActionTile.actionTile> validTiles = new List<ActionTile.actionTile>();

        // The tile the user unit is placed on.
        public ActionTile tile = null;

        // The position offset of the unit when placed on a tile.
        public Vector3 tilePosOffset = Vector3.zero;

        // Returns 'true' if the unit can be removed by the user player once it's placed.
        public bool removableByUser = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // Tries to perform a player user action.
            TryPerformPlayerUserAction();
        }

        // Returns 'true' if the provided tile can be used by this action unit.
        public override bool UsableTile(ActionTile tile)
        {
            // The result to be returned. 
            bool result;

            // If the tile exists, check if it's usable.
            if(tile != null)
            {
                // Checks if the tile is interactable, if the tile is usable by an action unit,...
                // And if the tile already has an action unit.
                // If the tile cannot be used for any reason, return false.
                if(!tile.interactable || !tile.IsUsableByActionUnitUser(this, false))
                {
                    // Tile already being used.
                    result = false;
                }
                // No unit on tile, so check if usable.
                else
                {
                    // Sets result based on if the tile is usable by its type.
                    result = UsableTileType(tile) && UsableTileOverlayType(tile) && UsableTileConfiguration(tile);
                }
            }
            // The tile doesn't exist, so nothing can be placed there.
            else
            {
                result = false;
            }
            
            return result;
        }

        // Returns 'true' if the tile type is usable.
        public bool UsableTileType(ActionTile.actionTile tileType)
        {
            // The tile type.
            bool result;

            // Checks if there are specific tiles that are valid.
            if (validTiles.Count > 0)
            {
                // If the valid list contains the tile type, the unit can be placed there.
                result = validTiles.Contains(tileType);
            }
            // Since there are no valid tiles, the game acts as if there are no invalid tiles.
            else
            {
                result = true;
            }

            return result;
        }

        // Returns 'true' if the tile is usable based purely on its type.
        // If there is some other factor that makes the tile usable or unusable, it isn't considered.
        public bool UsableTileType(ActionTile tile)
        {
            // If the tile exists, check it's tile type.
            if(tile != null)
            {
                return UsableTileType(tile.GetTileType());
            }
            // Tile doesn't exist, so no type to chck, so null.
            else
            {
                return false;   
            }
        }

        // Returns true if the tile overlay type is usable.
        public bool UsableTileOverlayType(ActionTile.actionTileOverlay overlay)
        {
            // The result to be returned.
            bool result;

            // Checks what overlay types are usable and unusable.
            switch (overlay)
            {
                default:
                case ActionTile.actionTileOverlay.none:
                    // If the unit is a generator.
                    if(this is ActionUnitGenerator)
                    {
                        // Gets the generator.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks if the generator uses energy cycles.
                        // If it doesn't use energy cycles, it can use this tile.
                        // If it does use energy cycles, it can't use this tile since there's no...
                        // Overlay to specify the use of energy cycles.
                        result = !NaturalResources.UsesEnergyCycles(tempGen.resource);
                    }
                    else
                    {
                        // Not a generator, so true by default.
                        result = true;
                    }
                        
                    break;

                case ActionTile.actionTileOverlay.unusable:
                    result = false;
                    break;

                case ActionTile.actionTileOverlay.geothermalSource:
                    // If this is a generator, check for valid types.
                    if(this is ActionUnitGenerator)
                    {
                        // Used to get this object coverted to a generator if applicable.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks what resources are valid.
                        switch(tempGen.resource)
                        {
                            case NaturalResources.naturalResource.biomass:
                            case NaturalResources.naturalResource.geothermal:
                            case NaturalResources.naturalResource.hydro:
                            case NaturalResources.naturalResource.solar:
                            case NaturalResources.naturalResource.wave:
                            case NaturalResources.naturalResource.wind:
                                result = true;
                                break;

                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    
                    break;

                case ActionTile.actionTileOverlay.coalSource:
                    // If this is a generator, check for valid types.
                    if (this is ActionUnitGenerator)
                    {
                        // Used to get this object coverted to a generator if applicable.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks what resources are valid.
                        switch (tempGen.resource)
                        {
                            case NaturalResources.naturalResource.biomass:
                            case NaturalResources.naturalResource.hydro:
                            case NaturalResources.naturalResource.solar:
                            case NaturalResources.naturalResource.wave:
                            case NaturalResources.naturalResource.wind:
                            case NaturalResources.naturalResource.coal:
                            case NaturalResources.naturalResource.naturalGas:
                                result = true;
                                break;

                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        result = true;
                    }

                    break;

                case ActionTile.actionTileOverlay.naturalGasSource:
                    // If this is a generator, check for valid types.
                    if (this is ActionUnitGenerator)
                    {
                        // Used to get this object coverted to a generator if applicable.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks what resources are valid.
                        switch (tempGen.resource)
                        {
                            case NaturalResources.naturalResource.biomass:
                            case NaturalResources.naturalResource.hydro:
                            case NaturalResources.naturalResource.solar:
                            case NaturalResources.naturalResource.wave:
                            case NaturalResources.naturalResource.wind:
                            case NaturalResources.naturalResource.naturalGas:
                                result = true;
                                break;

                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        result = true;
                    }

                    break;

                case ActionTile.actionTileOverlay.nuclearSource:
                    // If this is a generator, check for valid types.
                    if (this is ActionUnitGenerator)
                    {
                        // Used to get this object coverted to a generator if applicable.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks what resources are valid.
                        switch (tempGen.resource)
                        {
                            case NaturalResources.naturalResource.biomass:
                            case NaturalResources.naturalResource.hydro:
                            case NaturalResources.naturalResource.solar:
                            case NaturalResources.naturalResource.wave:
                            case NaturalResources.naturalResource.wind:
                            case NaturalResources.naturalResource.nuclear:
                                result = true;
                                break;

                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        result = true;
                    }

                    break;

                case ActionTile.actionTileOverlay.oilSource:
                    // If this is a generator, check for valid types.
                    if (this is ActionUnitGenerator)
                    {
                        // Used to get this object coverted to a generator if applicable.
                        ActionUnitGenerator tempGen = (ActionUnitGenerator)this;

                        // Checks what resources are valid.
                        switch (tempGen.resource)
                        {
                            case NaturalResources.naturalResource.biomass:
                            case NaturalResources.naturalResource.hydro:
                            case NaturalResources.naturalResource.solar:
                            case NaturalResources.naturalResource.wave:
                            case NaturalResources.naturalResource.wind:
                            case NaturalResources.naturalResource.naturalGas:
                            case NaturalResources.naturalResource.oil:
                                result = true;
                                break;

                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        result = true;
                    }

                    break;

                case ActionTile.actionTileOverlay.waterHazard:
                    // Flooded tile.
                    // If this is a generator, don't allow it to use this tile.
                    switch(GetUnitType())
                    {
                        case unitType.generator:
                            result = false;
                            break;

                        default:
                            result = true;
                            break;
                    }
                    break;

                case ActionTile.actionTileOverlay.nuclearHazard:
                case ActionTile.actionTileOverlay.oilHazard:
                    result = false;
                    break;
            }

            return result;
        }

        // Returns true if the tile overlay type is usable.
        public bool UsableTileOverlayType(ActionTile tile)
        {
            return UsableTileOverlayType(tile.tileOverlayType);
        }

        // Checks if using this tile will be a valid configuration for this unit.
        public virtual bool UsableTileConfiguration(ActionTile tile)
        {
            return true;
        }


        // Sets the position to the provided tile's position.
        public void SetPositionToTilePosition(ActionTile refTile)
        {
            transform.position = refTile.transform.position + tilePosOffset;
        }

        // Sets the position to the provided tile's position.
        public void SetPositionToTilePosition()
        {
            SetPositionToTilePosition(tile);
        }

        // Clears the tile this unit is on.
        public void ClearTile()
        {
            // The tile exists.
            if (tile != null)
            {
                // The tile has this action unit, so clear the tile.
                if (tile.actionUnitUser == this)
                {
                    // Save the tile temporarily.
                    ActionTile tempTile = tile;

                    // Clear the user.
                    tile.ClearActionUnitUser();

                    // If highlighting is enabled...
                    if(ActionStage.TileHighlightingEnabled)
                    {
                        // If the tile is highlighted, refresh the highlight.
                        if (tempTile.IsHighlighted())
                            tempTile.RefreshHighlightedTile();
                    }
                }
            }

            // Clear tile.
            tile = null;
        }

        // Returns 'true' if the unit can be removed by the user.
        public bool IsRemovableByUser()
        {
            return removableByUser;
        }

        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            // Tries to remove the unit from its user list.
            actionManager.playerUser.TryRemoveCreatedUnitFromList(this);

            // Clears the tile this unit is on.
            ClearTile();

            base.OnUnitDeath();
        }

        // Tries to perform a player user action when clicked on.
        // Returns true if an action was performed.
        public bool TryPerformPlayerUserAction()
        {
            // Gets set to 'true' if an action was performed.
            bool performed = false;

            // Gets the player user from the aciton manager.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // If the player is in remove mode, kill the unit.
            if (playerUser.InRemoveMode())
            {
                // If this unit can be removed by the player user.
                if(IsRemovableByUser())
                {
                    // Play the removed sound.
                    playerUser.PlayUnitRemovedSfx();

                    // Disables the death animation when killing, then puts it back to original setting.
                    bool deathAnimTemp = deathAnimationEnabled;
                    deathAnimationEnabled = false;
                    Kill();
                    deathAnimationEnabled = deathAnimTemp;

                    // Action performed.
                    performed = true;
                }
            }

            // Returns 'true' if an action was performed.
            return performed;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Tries to remove the unit from its user list.
            actionManager.playerUser.TryRemoveCreatedUnitFromList(this);

            // Clears the tile.
            ClearTile();
        }
    }
}