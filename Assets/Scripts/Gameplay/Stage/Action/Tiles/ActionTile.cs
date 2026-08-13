using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RM_EDU
{
    // A tile in the action stage.
    public class ActionTile : MonoBehaviour
    {
        // The action tile.
        public enum actionTile { unknown, land, river, sea, metal };

        // The tile type count.
        public const int ACTION_TILE_TYPE_COUNT = 5;

        // The action tile overlay.
        public enum actionTileOverlay { none, unusable, geothermalSource, coalSource, naturalGasSource, nuclearSource, oilSource, waterHazard, nuclearHazard, oilHazard }

        // The total number of values in the tile overlay enum.
        public const int ACTION_TILE_OVERLAY_TYPE_COUNT = 10;

        // The action manager.
        public ActionManager actionManager;

        // The tile ID number.
        [Tooltip("The id number of the tile.")]
        public int idNumber = 0;

        // The tile type.
        public actionTile tileType = actionTile.unknown;

        // The tile overlay.
        // This is public because the prefabs need to be able to set this.
        [Tooltip("The tile overlay type. Use the dedication function for changing this member so that the visuals update as well.")]
        public actionTileOverlay tileOverlayType = actionTileOverlay.none;

        // The default tile overlay type.
        [Tooltip("The default tile overlay type. This is what the tile is set to when reset.")]
        public actionTileOverlay defaultTileOverlayType = actionTileOverlay.none;

        // The tile verison.
        public char tileVersion = 'A';

        // The collider.
        public new Collider2D collider;

        // If 'true', the tile can be interacted with for game functions.
        [Tooltip("If true, the tile is interactable by the user.")]
        public bool interactable = true;

        [Header("Visuals")]

        // The animator.
        public Animator animator;

        // The empty animation.
        public string emptyAnimName = "Empty State";

        // If 'true', animations are used for this tile.
        protected bool useAnimations = true;

        // The highlight sprite renderer.
        [Tooltip("The sprite used to highlight the tile to show if it's usable or not.")]
        public SpriteRenderer highlightSpriteRenderer;

        // The overlay sprite renderer.
        [Tooltip("The sprite used to put things on top of the base sprite.")]
        public SpriteRenderer overlaySpriteRenderer;

        // The sprite renderer.
        [Tooltip("The base sprite iamge.")]
        public SpriteRenderer baseSpriteRenderer;

        // The tile variants.
        public List<Sprite> tileVersionSprites = new List<Sprite>();

        // Sets the tile verison sprite in start if true.
        [Tooltip("Automatically sets the tile version sprite in start if true.")]
        public bool setTileVersionSpriteInStart = true;

        // The normal and darkened colours are used to create a checkerboard for the game.
        // The normal tile color.
        private static Color normalColor = Color.white;

        // The darkened tile color.
        private static Color darkenedColor = new Color(0.50F, 0.50F, 0.50F);

        // The colour used to show that the tile is usable.
        private static Color usableColor = Color.yellow;

        // The colour used to show that the tile is unusable.
        private static Color unusableColor = Color.black;

        // The alpha of highlights on the tile.
        private static float highlightAlpha = 0.4F;

        [Header("World")]

        // The map position on the tile.
        // This is in map space, so its extents are (0, 0) to the size of the map.
        [Tooltip("The tile's position in the map's space.")]
        public Vector2Int mapPos = new Vector2Int(-1, -1);

        // The action unit user on the tile.
        [Tooltip("If action unit user on the tile. If null, then there's no user unit on the tile.")]
        public ActionUnitUser actionUnitUser = null;

        [Header("Energy Cycles")]

        // If a generator uses energy spots, they depelte from energy cycles for whatever...
        // Energy source they get from the tile.

        // The default number of energy cycles for this tile.
        public int energyCyclesDefault = 10;

        // The number of coal cycles allowed.
        public int coalCycles = 0;

        // The number of natural gas cycles allowed.
        public int naturalGasCycles = 0;

        // The number of nuclear cycles allowed.
        public int nuclearCycles = 0;

        // The number of oil cycles allowed.
        public int oilCycles = 0;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager isn't set, make the instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the collider.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // Tries to get the animator component.
            if(animator == null)
                animator = GetComponent<Animator>();

            // If the animator exists, enable/disable it based on if animations should be used.
            if (animator != null)
                animator.enabled = useAnimations;

            // Turn off highlight and overlay.
            SetHighlighted(false, true);
            SetTileOverlayType(defaultTileOverlayType);

            // Sets the sprite by the tile version.
            if(setTileVersionSpriteInStart)
                SetSpriteByTileVersion();

            // Sets the energy cycles to their default on start.
            SetEnergyCyclesToDefault();
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // If the pointer is over a game object in the event system, don't take in inputs.
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // Tries to perform a player user action.
            TryPerformPlayerUserAction();
        }

        // TILE TYPE //

        // Gets the tile type.
        public actionTile GetTileType()
        {
            return tileType;
        }

        // Returns 'true' if this is a land tile.
        // If the tile is unknown, it still registers as true.
        public static bool IsLandTile(actionTile tileType)
        {
            // Unknown, land, and metal tiles are land tiles.
            switch(tileType)
            {
                case actionTile.unknown:
                    Debug.Log("Tile type is unknown.");
                    return true;

                case actionTile.land:
                case actionTile.metal:
                    return true;

                default:
                    return false;
            }
        }

        // Returns 'true' if this is a land tile.
        public bool IsLandTile()
        {
            return IsLandTile(tileType);
        }

        // Returns 'true' if this is a water tile.
        // Unknown tiles still register as true.
        public static bool IsWaterTile(actionTile tileType)
        {
            // Unknown, rive, and sea tiles are water tiles.
            switch (tileType)
            {
                case actionTile.unknown:
                    Debug.Log("Tile type is unknown.");
                    return true;

                case actionTile.river:
                case actionTile.sea:
                    return true;

                default:
                    return false;
            }
        }

        // Returns 'true' if this is a water tile.
        public bool IsWaterTile()
        {
            return IsWaterTile(tileType);
        }

        // TILE VERSION //

        // Gets the tile version index.
        public int GetTileVersionIndex()
        {
            // The index.
            int index;

            // If there is a sprite, see if it's in the tile version list.
            if (baseSpriteRenderer.sprite != null)
            {
                // If the tile is in the list, get the index.
                if (tileVersionSprites.Contains(baseSpriteRenderer.sprite))
                {
                    index = tileVersionSprites.IndexOf(baseSpriteRenderer.sprite);
                }
                else
                {
                    index = -1;
                }
            }
            else
            {
                index = -1;
            }

            return index;
        }

        // Gets the tile version.
        public char GetTileVersion()
        {
            return tileVersion;
        }

        // Gets the tile verison as number in the English alphabet.
        // If it's not in the alphabet, it returns -1.
        public int GetTileVersionEnglishAlphabetNumber()
        {
            // Gets the result.
            // This function returns -1 if the provided character isn't an English alphabet number.
            int result = util.StringHelper.GetEnglishAlphabetLetterNumber(tileVersion);

            // Returns the result.
            return result;
        }

        // Sets the sprite by the tile version.
        public void SetSpriteByTileVersion()
        {
            // Checks if there are tile version sprites to use.
            if(tileVersionSprites.Count <= 0)
            {
                Debug.LogWarning("There are no tile version sprites to choose from.");
                return;
            }

            // Gets the version as a number.
            int verNum = GetTileVersionEnglishAlphabetNumber();

            // If the verison number is greater than 0, it can potentially be used as an index.
            if(verNum > 0)
            {
                // Calculates the index.
                // Since its the alphabet number, the index is 1 less than that.
                // e.g., (A) is letter (1), so tile (A) would be at index 0 (1 - 1 = 0).
                int index = verNum - 1;

                // Set the new sprite.
                if(index >= 0 && index < tileVersionSprites.Count)
                {
                    baseSpriteRenderer.sprite = tileVersionSprites[index];
                }
            }
        }

        // Cehcks if animations are being used.
        public bool UseAnimations
        {
            get { return useAnimations; }
        }


        // TILE OVERLAY //
        // Returns the tile overlay type.
        public actionTileOverlay GetTileOverlayType()
        {
            return tileOverlayType;
        }

        // Sets the tile overlay type.
        public void SetTileOverlayType(actionTileOverlay overlayType, bool setAsDefault = false)
        {
            // Sets the new type.
            tileOverlayType = overlayType;

            // If the overlay type should be set as the default, set it.
            if(setAsDefault)
                defaultTileOverlayType = overlayType;

            // Turns on the overlay sprite.
            overlaySpriteRenderer.gameObject.SetActive(true);

            // Gets the tile prefabs.
            ActionTilePrefabs tilePrefabs = ActionTilePrefabs.Instance;

            // Gets the new sprite.
            Sprite newSprite = tilePrefabs.GetActionTileOverlaySprite((int)tileOverlayType);

            // Sets the overlay to use the new sprite.
            overlaySpriteRenderer.sprite = newSprite;

            // If no overlay exists, turn off the overlay sprite renderer.
            if(tileOverlayType == actionTileOverlay.none)
            {
                overlaySpriteRenderer.gameObject.SetActive(false);
            }


            // Gets the tile overlay alpha and applies it to the tile color.
            float overlayAlpha = GetTileOverlayAlpha(tileOverlayType);
            Color overlayColor = overlaySpriteRenderer.color;
            overlayColor.a = overlayAlpha;
            overlaySpriteRenderer.color = overlayColor;


            // Checks if the tile is highlighted.
            if(IsHighlighted())
            {
                // If the player is selecting a unit prefab.
                if(ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab())
                {
                    // Checks if this tile is usable.
                    bool usable = ActionManager.Instance.playerUser.selectedUnitPrefab.UsableTile(this);

                    // Changes the highlight based on if this tile is usable or not.
                    SetHighlighted(true, usable);
                }
            }

            // OLD - didn't always work. Should be removed if the code above works fine.
            // // If the tile is currently highlighted as usable...
            // // And was changed to something that makes the tile unusable...
            // // Change the highlight.
            // if (IsHighlightedUsable())
            // {
            //     // Checks the type.
            //     switch(tileOverlayType)
            //     {
            //         case actionTileOverlay.waterHazard: // Water
            // 
            //             // If the player user is selecting a prefab.
            //             if(ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab())
            //             {
            //                 // Gets the type of the prefab the player user is selecting.
            //                 switch (ActionManager.Instance.playerUser.GetSelectedActionUnitPrefabType())
            //                 {
            //                     // If a generator is being selected, highlight the tile as not usuable...
            //                     // Since generators cannot go on this tile.
            //                     case ActionUnit.unitType.generator:
            //                         HighlightTile(false);
            //                         break;
            //                 }
            //             }
            // 
            //             break;
            // 
            //         // If now set to a hazard, unhighlight the tile, since it shouldn't be usable anymore.
            //         case actionTileOverlay.nuclearHazard:
            //         case actionTileOverlay.oilHazard:
            //             HighlightTile(false);
            //             break;
            //     }
            // }
        }

        // Returns the default tile overlay type.
        public actionTileOverlay GetDefaultTileOverlayType()
        {
            return defaultTileOverlayType;
        }

        // Sets the default tile overlay type.
        // If 'setCurrent' is true, the current default type is set as well.
        public void SetDefaultTileOverlayType(actionTileOverlay newDefaultType, bool setAsCurrent = false)
        {
            // Checks if the current overlay type should also be set.
            if (setAsCurrent)
                SetTileOverlayType(newDefaultType, true);
            else
                defaultTileOverlayType = newDefaultType;
        }

        // Resets the tile ovelray type.
        public void ResetTileOverlayType()
        {
            SetTileOverlayType(defaultTileOverlayType, true);
        }

        // Gets the tile overlay alpha value based on the type.
        public static float GetTileOverlayAlpha(actionTileOverlay type)
        {
            // The alpha to return.
            float alpha;

            // Checks the type.
            switch(type)
            {
                case actionTileOverlay.waterHazard:
                    alpha = 0.5F;
                    break;

                default:
                    alpha = 1.0F;
                    break;
            }

            return alpha;
        }

        // Returns 'true' if the tile has an overlay hazard.
        public bool HasTileOverlayHazard()
        {
            // Result to be returned.
            bool result;

            // Checks if it's one of the hazard overlays.
            switch(tileOverlayType)
            {
                default:
                    result = false;
                    break;

                case actionTileOverlay.waterHazard:
                case actionTileOverlay.nuclearHazard:
                case actionTileOverlay.oilHazard:
                    result = true;
                    break;
            }

            // Return result.
            return result;
        }

        // Returns true if the tile overlay is a water hazard.
        public bool HasTileOverlayWaterHazard()
        {
            return tileOverlayType == actionTileOverlay.waterHazard;
        }

        // Returns 'true' if the tile has a nuclear hazard.
        public bool HasTileOverlayNuclearHazard()
        {
            return tileOverlayType == actionTileOverlay.nuclearHazard;
        }

        // Returns 'true' if the tile has an oil hazard.
        public bool HasTileOverlayOilHazard()
        {
            return tileOverlayType == actionTileOverlay.oilHazard;
        }

        // COLOR CHANGES //
        // Gets the tile's base color.
        public Color GetTileBaseColor()
        {
            return baseSpriteRenderer.color;
        }

        // Returns 'true' if the base tile is its normal color.
        // This checks the base sprite renderer.
        public bool IsNormalBaseColor()
        {
            return baseSpriteRenderer.color == normalColor;
        }

        // Returns 'true' if the base tile is its darkened color.
        // This checks the base sprite renderer.
        public bool IsDarkenedBaseColor()
        {
            return baseSpriteRenderer.color == darkenedColor;
        }

        // Returns the normal color.
        public static Color NormalColor
        {
            get { return normalColor; }
        }

        // Returns the darkened color.
        public static Color DarkenedColor
        {
            get { return darkenedColor; }
        }

        // ANIMATION

        // Plays the provided animation.
        public void PlayAnimation(string animName)
        {
            // If the animator exists and a proper animation name was provided, play the provided animation.
            if(animator != null && animName != "")
            {
                // If animations are being used.
                if(useAnimations)
                {
                    animator.Play(animName);
                }
                else
                {
                    Debug.LogWarning("Animations are disabled.");
                }
            }
            else
            {
                Debug.LogError("Animator or animation name not set.");
            }
                
        }

        // Plays the empty animation.
        public void PlayEmptyAnimation()
        {
            PlayAnimation(emptyAnimName);
        }

        // HIGHLIGHT

        // The color used to show that a tile is usable.
        public static Color UsableColor
        {
            get { return usableColor; }
        }

        // The color used to show that a tile is unusable.
        public static Color UnusableColor
        {
            get { return unusableColor; }
        }

        // Returns the highlight alpha.
        public static float HighlightAlpha
        {
            get { return highlightAlpha; }
        }

        // Returns 'true' if the tile is currently highlighted.
        public bool IsHighlighted()
        {
            return highlightSpriteRenderer.enabled && highlightSpriteRenderer.gameObject.activeSelf;
        }

        // Returns true if the tile is highlighted as usable.
        public bool IsHighlightedUsable()
        {
            // Check if highlighted...
            if (IsHighlighted())
            {
                // Then check for usable color.
                Color tempColor = highlightSpriteRenderer.color;
                tempColor.a = highlightAlpha;

                // If the colors match, this is the usable color.
                return tempColor == UsableColor;
            }
            else
            {
                return false;
            }
        }

        // Returns ture if the tile is highlighted as unusuable.
        public bool IsHighlightedUnusable()
        {
            // Check if highlighted...
            if (IsHighlighted())
            {
                // Then check for usable color.
                Color tempColor = highlightSpriteRenderer.color;
                tempColor.a = highlightAlpha;

                // If the colors match, this is the usable color.
                return tempColor == UnusableColor;
            }
            else
            {
                return false;
            }
        }

        // Sets if the tile should be highlighted.
        // The 'usuable' argument determines what the highlight color will be.
        // If there will be no highlight, the highlight color is white.
        public void SetHighlighted(bool highlighted, bool usable)
        {
            // The new color.
            Color newColor;

            // Checks if the highlight will be turned on or off.
            if (highlighted)
            {
                newColor = usable ? UsableColor : UnusableColor;
            }
            else
            {
                // The tile won't be highlighted, so just make it white.
                newColor = Color.white;
            }

            // Change the alpha value.
            newColor.a = highlightAlpha;

            // Sets the new color.
            highlightSpriteRenderer.color = newColor;

            // Changes the activity of the sprite renderer based on if it's highlighted or not.
            highlightSpriteRenderer.gameObject.SetActive(highlighted);
        }

        // Highlights the tile.
        public void HighlightTile(bool usable)
        {
            SetHighlighted(true, usable);
        }

        // Highlights the tile.
        // If the unit can use this tile, it's highlighted as a usable tile.
        // Highlighted as not usable if the unit cannot use it.
        public void HighlightTile(ActionUnit compUnit)
        {
            SetHighlighted(true, compUnit.UsableTile(this));
        }

        // Unhighlights the tile.
        // Since the highlight is being turned off, usable can be set to anything.
        public void UnhighlightTile(bool usable)
        {
            SetHighlighted(false, usable);
        }

        // Unhighlights the tile. Automatically sets usable based on if there's an action unit on this tile.
        public void UnhighlightTile()
        {
            SetHighlighted(false, !HasActionUnitUser());
        }

        // Refreshes the highlighted tile.
        public void RefreshHighlightedTile()
        {
            // Checks if the tile is suable.
            bool usable;

            // If the tile has an action unit user, check if this tile is usable.
            if(HasActionUnitUser())
            {
                usable = actionUnitUser.UsableTile(this);
            }
            // If the tile has no action unit user, check if the player user as a unit selected.
            // If so, check if the unit is usable on this tile.
            else
            {
                // Checks if the action manager is instantiated. If it is, check the player user.
                // This is done to avoid instantiating the action manager after it has been destroyed...
                // When the scene is unloaded.
                if(ActionManager.Instantiated)
                {
                    // Gets the player user.
                    ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

                    // Checks if the player is selecting a unit prefab.
                    if (playerUser.IsSelectingActionUnitPrefab())
                    {
                        // Selecting something, so see if it can use the tile.
                        usable = ActionManager.Instance.playerUser.CanSelectedActionUnitUseTile(this);
                    }
                    // Selecting nothing, so set to not usable.
                    else
                    {
                        usable = false;
                    }
                }
                else
                {
                    usable = false;
                }
            }

            // SetHighlighted(IsHighlighted(), !HasActionUnitUser()); // Old
            SetHighlighted(IsHighlighted(), usable);
        }

        // WORLD
        // Gets the map position.
        public Vector2Int GetMapPosition()
        {
            return mapPos;
        }

        // Gets the tile's map row position, which corresponds to the tile's map y-position (mapPos.y).
        public int GetMapRowPosition()
        {
            return mapPos.y;
        }

        // Gets the tile's map colum position, which corresponds to the tile's map x-position (mapPos.x).
        public int GetMapColumnPosition()
        {
            return mapPos.x;
        }

        
        // ENERGY CYCLES //
        // Sets the energy cycles on a tile to default cycle count.
        public void SetEnergyCyclesToDefault()
        {
            coalCycles = energyCyclesDefault;
            naturalGasCycles = energyCyclesDefault;
            nuclearCycles = energyCyclesDefault;
            oilCycles = energyCyclesDefault;
        }

        // Returns true if the tile has cycles for the provided overlay.
        // If this returns false, then there are no cycles for the overlay...
        // Or that there are no cycles left for the overlay.
        public bool HasEnergyCycles(actionTileOverlay overlay)
        {
            // The result to be returned.
            bool result;

            // Checks the overlay to see which cycles to check.
            switch(overlay)
            {
                default:
                case actionTileOverlay.none: // No cycles
                case actionTileOverlay.unusable:
                case actionTileOverlay.geothermalSource:
                    result = false;
                    break;

                case actionTileOverlay.coalSource:
                    result = coalCycles > 0;
                    break;

                case actionTileOverlay.naturalGasSource:
                    result = naturalGasCycles > 0;
                    break;

                case actionTileOverlay.nuclearSource:
                    result = nuclearCycles > 0;
                    break;

                case actionTileOverlay.oilSource:
                    result = oilCycles > 0;
                    break;

                case actionTileOverlay.waterHazard:
                case actionTileOverlay.nuclearHazard:
                case actionTileOverlay.oilHazard:
                    // If it's a hazard tile, there are no cycles, since generators...
                    // Cannot use the tile.
                    result = false;
                    break;
            }

            // Returns the result.
            return result;
        }

        // Checks if the tile has energy cycles for the natural resource provided.
        // If the resource doesn't use cycles, it returns false.
        public bool HasEnergyCycles(NaturalResources.naturalResource resource)
        {
            // First checks if the resource uses cycles.
            if(NaturalResources.UsesEnergyCycles(resource))
            {
                // Set to true if the resource has cycles.
                bool hasCycles;

                // Checks the resource to see what type of cycles to check.
                // If the resource doesn't have cycles attached to it...
                // The result is false.
                switch(resource)
                {
                    default:
                        hasCycles = false;
                        break;

                    case NaturalResources.naturalResource.coal:
                        hasCycles = HasCoalEnergyCycles();
                        break;

                    case NaturalResources.naturalResource.naturalGas:
                        hasCycles = HasNaturalGasEnergyCycles();
                        break;

                    case NaturalResources.naturalResource.nuclear:
                        hasCycles = HasNuclearEnergyCycles();
                        break;

                    case NaturalResources.naturalResource.oil:
                        hasCycles = HasOilEnergyCycles();
                        break;
                }

                return hasCycles;
            }
            // Doesn't use cycles, so return false.
            else
            {
                return false;
            }
        }

        // Has coal energy cycles.
        public bool HasCoalEnergyCycles()
        {
            return HasEnergyCycles(actionTileOverlay.coalSource);
        }

        // Has natural gas energy cycles.
        public bool HasNaturalGasEnergyCycles()
        {
            return HasEnergyCycles(actionTileOverlay.naturalGasSource);
        }

        // Has nuclear energy cycles.
        public bool HasNuclearEnergyCycles()
        {
            return HasEnergyCycles(actionTileOverlay.nuclearSource);
        }

        // Has oil energy cycles.
        public bool HasOilEnergyCycles()
        {
            return HasEnergyCycles(actionTileOverlay.oilSource);
        }

        // Adjusts the energy cycles by the overlay type.
        // Postive amount to add, negative amount to subtract.
        public void AdjustEnergyCyclesByOverlayType(actionTileOverlay overlay, int amount)
        {
            // Gets set to 'true' if there are no more cycles for the type chosen.
            bool outOfCycles = false;

            // Checks the overlay to see which cycles to change.
            switch (overlay)
            {
                case actionTileOverlay.coalSource:
                    // Apply amount and save if out of cycles.
                    coalCycles += amount;

                    if (coalCycles <= 0)
                    {
                        coalCycles = 0;
                        outOfCycles = true;
                    }
                        

                    break;

                case actionTileOverlay.naturalGasSource:
                    // Apply amount and save if out of cycles.
                    naturalGasCycles += amount;

                    if(naturalGasCycles <= 0)
                    {
                        naturalGasCycles = 0;
                        outOfCycles = true;
                    }
                        

                    break;

                case actionTileOverlay.nuclearSource:
                    // Apply amount and save if out of cycles.
                    nuclearCycles += amount;

                    if (nuclearCycles <= 0)
                    {
                        nuclearCycles = 0;
                        outOfCycles = true;
                    }
                        

                    break;

                case actionTileOverlay.oilSource:
                    // Apply amount and save if out of cycles.
                    oilCycles += amount;

                    if (oilCycles <= 0)
                    {
                        oilCycles = 0;
                        outOfCycles = true;
                    }
                        

                    break;
            }

            // If the cycle chosen no longer has any cycles left.
            if(outOfCycles)
            {
                // If the current tile overlay matches the provided overlay...
                // Switch the overlay to none.
                if(tileOverlayType == overlay)
                {
                    // If this is an coal source and there's still natural gas, switch to a natural gas overlay.
                    if(tileOverlayType == actionTileOverlay.coalSource && naturalGasCycles > 0)
                    {
                        SetTileOverlayType(actionTileOverlay.naturalGasSource);
                    }
                    // If this is a oil source and there's still natural gas, switch to a natural gas overlay.
                    else if (tileOverlayType == actionTileOverlay.oilSource && naturalGasCycles > 0)
                    {
                        SetTileOverlayType(actionTileOverlay.naturalGasSource);
                    }
                    else
                    {
                        // Sets to none to hide the overlay since...
                        // No more of this cycle can be used here.
                        SetTileOverlayType(actionTileOverlay.none);
                    }
                }
            }
        }

        // Adds energy cycels by the overlay type.
        public void IncreaseEnergyCyclesByOverlayType(actionTileOverlay overlay, int amount = 1)
        {
            AdjustEnergyCyclesByOverlayType(overlay, amount);
        }

        // Adds energy cycles based on the current overlay type.
        public void IncreaseEnergyCyclesByOverlayType(int amount = 1)
        {
            AdjustEnergyCyclesByOverlayType(tileOverlayType, amount);
        }

        // Subtracts energy cycles by overlay type.
        public void DecreaseEnergyCyclesByOverlayType(actionTileOverlay overlay, int amount = 1)
        {
            AdjustEnergyCyclesByOverlayType(overlay, -amount);
        }

        // Subtract energy cycles based on the current overlay type.
        public void DecreaseEnergyCyclesByOverlayType(int amount = 1)
        {
            AdjustEnergyCyclesByOverlayType(tileOverlayType, -amount);
        }

        // Adjusts energy cycles by reosurces.
        public void AdjustEnergyCyclesByResource(NaturalResources.naturalResource resource, int amount)
        {
            // Checks the overlay to see which cycles to check.
            switch (resource)
            {
                case NaturalResources.naturalResource.coal:
                    AdjustEnergyCyclesByOverlayType(actionTileOverlay.coalSource, amount);

                    break;

                case NaturalResources.naturalResource.naturalGas:
                    AdjustEnergyCyclesByOverlayType(actionTileOverlay.naturalGasSource, amount);

                    break;

                case NaturalResources.naturalResource.nuclear:
                    AdjustEnergyCyclesByOverlayType(actionTileOverlay.nuclearSource, amount);

                    break;

                case NaturalResources.naturalResource.oil:
                    AdjustEnergyCyclesByOverlayType(actionTileOverlay.oilSource, amount);

                    break;
            }
        }

        // Adds energy cycles by resource.
        public void IncreaseEnergyCyclesByResource(NaturalResources.naturalResource resource, int amount = 1)
        {
            AdjustEnergyCyclesByResource(resource, amount);
        }

        // Removes energy cycles by resource.
        public void DecreaseEnergyCyclesByResource(NaturalResources.naturalResource resource, int amount = 1)
        {
            AdjustEnergyCyclesByResource(resource, -amount);
        }


        // ACTION UNUT USER
        // Called when the player unit user is selecting a unit.
        public void OnPlayerUserSelectedUnit(ActionUnit selectedUnit)
        {
            HighlightTile(selectedUnit.UsableTile(this));
        }

        // Called when the player user has deselected a unit.
        public void OnPlayerUserDeselectedUnit()
        {
            UnhighlightTile(HasActionUnitUser());
        }

        // Returns 'true' if an action unit can be placed on this tile.
        // If 'ignoreHasActionUser' is true, this ignores if there's a user on the tile.
        public bool IsUsableByActionUnitUser(ActionUnitUser compUnit, bool ignoreHasActionUser)
        {
            bool result = false;

            // If there are no valid tiles, or the valid tiles list contains this tile's type...
            // This aciton user can use this tile.
            if(compUnit.UsableTileType(this) && compUnit.UsableTileOverlayType(this))
            {
                // Checks if the saved user should be ignored.
                // If the current user should be ignored, this returns true.
                // If the current user shouldn't be ignored, return false if this tile is being used.
                result = ignoreHasActionUser ? true : !HasActionUnitUser();
            }

            // Return result.
            return result;
        }

        // Returns 'true' if an action unit is on the tile.
        public bool HasActionUnitUser()
        {
            return actionUnitUser;
        }

        // Returns 'true' if this action unit is on this tile.
        // Returns 'false' if this unit isn't on this tile or if the tile has no unit.
        public bool HasActionUnitUser(ActionUnitUser compUnit)
        {
            return actionUnitUser == compUnit;
        }

        // Returns the action unit user.
        public ActionUnitUser GetActionUnitUser()
        {
            return actionUnitUser;
        }

        // Sets the action unit.
        public void SetActionUnitUser(ActionUnitUser newUnitUser)
        {
            actionUnitUser = newUnitUser;

            // If the unit has been set.
            if(actionUnitUser != null)
            {
                actionUnitUser.tile = this;
            }

            // If the tile is currently highlighted, change the highlight.
            if(IsHighlighted())
                RefreshHighlightedTile();
        }

        // Clears the action unit on the tile.
        public void ClearActionUnitUser()
        {
            // If there is an action unit user.
            if(actionUnitUser != null)
            {
                // If the saved tile is this tile, set it to null.
                if (actionUnitUser.tile == this)
                {
                    actionUnitUser.tile = null;
                }
            }

            // Set unit to null.
            actionUnitUser = null;

            // If the tile is currently highlighted, change the highlight.
            if (IsHighlighted())
                RefreshHighlightedTile();
        }

        // Kills the action unit user.
        public void KillActionUnitUser()
        {
            // If there's an action user unit on this tile, kill it.
            if(actionUnitUser != null)
            {
                actionUnitUser.Kill();
            }

            actionUnitUser = null;
        }

        // Tries to perform a player user action when clicked on.
        // Returns true if an action was performed.
        public bool TryPerformPlayerUserAction()
        {
            // Gets set to 'true' if an action was performed.
            bool performed = false;

            // Gets the player user from the aciton manager.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // NOTE: since an action unit being on a tile would be blocking the tile's collider...
            // The remove call will likely never be triggered unless the player hits an edge of the tile...
            // That is not covered by the action unit's hit box.

            // Checks what mode the player user is in.
            if (playerUser.InSelectMode()) // Select
            {
                // If there is no unit on the tile, see if the player wants to place a unit.
                if (!HasActionUnitUser())
                {
                    // If the player user has a unit prefab and it can use this tile...
                    // Instantiate the unit and put it on this tile.
                    // This function also checks if the player user is selecting a tile.
                    if (playerUser.CanSelectedActionUnitUseTile(this))
                    {   
                        // Instantiate the prefab.
                        playerUser.InstantiateSelectedActionUnit(this);

                        // Since the player should still be selecting something, refresh the highlighs.
                        ActionManager.Instance.actionStage.RefreshHighlightedTiles();

                        // Action performed.
                        performed = true;
                    }
                }
            }
            else if (playerUser.InRemoveMode()) // Remove
            {
                // Checks if there's a unit on this tile to remove.
                if(HasActionUnitUser())
                {
                    // If the action unit user can be removed by the user.
                    if(actionUnitUser.IsRemovableByUser())
                    {
                        KillActionUnitUser(); // Kill the user on this platform.
                        performed = true;
                    }
                }
            }

            // Returns 'true' if an action was performed.
            return performed;
        }

        // Resets the tile.
        public void ResetTile()
        {
            // Clears the action user unit. 
            ClearActionUnitUser();

            // Resets the tile overlay type.
            ResetTileOverlayType();

            // Return the energy cycles to their default values.
            SetEnergyCyclesToDefault();
        }
    }
}