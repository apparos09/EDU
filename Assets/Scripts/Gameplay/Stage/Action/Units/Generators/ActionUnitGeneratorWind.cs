using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace RM_EDU
{
    // Action Unit Generator - Wind
    public class ActionUnitGeneratorWind : ActionUnitGenerator
    {
        // The spin speed of the wind generator.
        public enum spinSpeed { none, verySlow, slow, medium, fast, veryFast }

        [Header("Generator/Wind")]

        // The sprite for the wind on land.
        [Tooltip("The sprite for the wind generator when on land.")]
        public Sprite landSprite;

        // The sprite for the wind on water.
        [Tooltip("The sprite for the wind generator when in the sea.")]
        public Sprite waterSprite;

        // The current spin speed.
        public spinSpeed currSpinSpeed = spinSpeed.none;

        // If 'true', the wind generator is using its restricted configuration.
        private bool restrictConfig = true;

        // If 'true', the spin animations are used.
        private bool useSpinAnims = true;

        [Header("Wind/Animations/Land")]

        // The animation for when there's no spinning on land. After it plays it switches to 'Empty State'.
        public string landSpinNoneAnim = "Action Unit Generator - Wind - Land - Spin - None Animation";

        // The animations for various land spin speeds.
        public string landSpinVerySlowAnim = "Action Unit Generator - Wind - Land - Spin - Very Slow Animation";
        public string landSpinSlowAnim = "Action Unit Generator - Wind - Land - Spin - Slow Animation";
        public string landSpinMediumAnim = "Action Unit Generator - Wind - Land - Spin - Medium Animation";
        public string landSpinFastAnim = "Action Unit Generator - Wind - Land - Spin - Fast Animation";
        public string landSpinVeryFastAnim = "Action Unit Generator - Wind - Land - Spin - Very Fast Animation";

        [Header("Wind/Animations/Water")]

        // The animation for when there's no spinning on land. After it plays it switches to 'Empty State'.
        public string waterSpinNoneAnim = "Action Unit Generator - Wind - Water - Spin - None Animation";

        // The animations for various water spin speeds.
        public string waterSpinVerySlowAnim = "Action Unit Generator - Wind - Water - Spin - Very Slow Animation";
        public string waterSpinSlowAnim = "Action Unit Generator - Wind - Water - Spin - Slow Animation";
        public string waterSpinMediumAnim = "Action Unit Generator - Wind - Water - Spin - Medium Animation";
        public string waterSpinFastAnim = "Action Unit Generator - Wind - Water - Spin - Fast Animation";
        public string waterSpinVeryFastAnim = "Action Unit Generator - Wind - Water - Spin - Very Fast Animation";


        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to wind.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.wind;
            }

            // Uses the wind to generate energy.
            if(!useWindToGenEnergy)
            {
                useWindToGenEnergy = true;
            }

            // The tile is set.
            if(tile != null)
            {
                // If it's a land tile, use the land sprite.
                // If it's wa ter tile, use the water sprite.
                spriteRenderer.sprite = tile.IsLandTile() ? landSprite : waterSprite;
                
                // Plays the spin - none animation to make sure the sprite is set properly.
                if(animator.enabled)
                    PlaySpinAnimation(spinSpeed.none);
            }

            // Set to none by default.
            SetCurrentSpinSpeed(spinSpeed.none);
        }

        // Checks if the tile configuration is valid.

        public override bool UsableTileConfiguration(ActionTile tile)
        {
            // If the tile configuration isn't restricted, all configurations are valid.
            // If the tile is null, this also returns true, since the game can't find any...
            // Reference tiles for applying the restricted configuration.
            if (!restrictConfig || tile == null)
            {
                return true;
            }

            // The result to return.
            bool result;

            // If this is a water tile, check if it's close to land.
            // A wind generator can only be used in the water if it's one tile out from a land tile.
            if(tile.IsWaterTile())
            {
                // Gets the stage.
                ActionStage stage = ActionManager.Instance.actionStage;

                // The tile's row and column positions.
                int tileRow = tile.GetMapRowPosition();
                int tileCol = tile.GetMapColumnPosition();

                // Gets set to 'true' if land is found.
                bool foundLand = false;

                // Gets set to true if the wind generator would be put in water next to a hydro generator.
                // Wind generators shouldn't be allowed near hydro generators if both are in water...
                // So if a hydro generator is next to the intended spot for a wind generator...
                // The wind generator cannot be placed there.
                bool inWaterNextToHydro = false;

                // Goes through every tile that's one space away from the current tile.
                for(int r = tileRow - 1; r < tileRow + 2; r++) // Row
                {
                    for(int c = tileCol - 1; c < tileCol + 2; c++) // Column
                    {                        
                        // If the provided row and column positions are valid.
                        if(stage.ValidMapPosition(r, c))
                        {
                            // The tile exists.
                            if(stage.tiles[r, c] != null)
                            {
                                // If this is a land tile, set 'foundLand' to true.
                                // If land has already been found, do nothing.
                                if(!foundLand)
                                    foundLand = stage.tiles[r, c].IsLandTile();


                                // The direction of the tile being checked, based on its position...
                                // In reference to the current tile.
                                Vector2Int direc = new Vector2Int();
                                direc.x = c - tileCol;
                                direc.y = r - tileRow;

                                /*
                                 * Steps to check for Hydro:
                                 *  - 1. Check if a potential hydro neighbor has yet to be found.
                                 *  - 2. Check if it's a space where a hydro generator would effect the placement of a wind generator.
                                 *  - 3. Check if there's an action unit user on that tile.
                                 *  - 4. Check if it's a generator.
                                 *  - 5. Downcast to generator and check if it's a hydro generator.
                                 */

                                // If not next to a hydro generator.
                                if (!inWaterNextToHydro)
                                {
                                    // Checks if this is a position to check for hydro generators (valid position).
                                    // If the hydro generator is in any of these positions...
                                    // The wind generator can't go there.
                                    // Left, Right, Up, Down (4 directions)
                                    if ((direc.x < 0 && direc.y == 0) || (direc.x > 0 && direc.y == 0)
                                        || (direc.x == 0 && direc.y > 0) || (direc.x == 0 && direc.y < 0))
                                    {
                                        // If this tile has an action user, check if it's hydro 
                                        if (stage.tiles[r, c].HasActionUnitUser())
                                        {
                                            // Checks if the unit user is a hydro generator.
                                            if (stage.tiles[r, c].actionUnitUser is ActionUnitGenerator)
                                            {
                                                // Downcasts to the generator.
                                                ActionUnitGenerator generator = (ActionUnitGenerator)stage.tiles[r, c].actionUnitUser;

                                                // If the generator is a hydro generator, check its position relative...
                                                // To the current tile.
                                                if (generator.resource == NaturalResources.naturalResource.hydro)
                                                {
                                                    inWaterNextToHydro = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Commented out since the generator needs to check for hydro generators.
                        // // Nearby land has been found, so break.
                        // if (foundLand)
                        //     break;

                        // If next to a hydro generator, the wind generator can't go here, so break.
                        if (inWaterNextToHydro)
                            break;
                    }

                    // Commented out since the generator needs to check for hydro generators.
                    // // Nearby land has been found, so break.
                    // if (foundLand)
                    //     break;

                    // If next to a hydro generator, the wind generator can't go here, so break.
                    if (inWaterNextToHydro)
                        break;
                }

                // If land was found and the wind generator wouldn't be next to a hydro generator, the spot is valid.
                result = foundLand && !inWaterNextToHydro;
            }
            // Not a water tile, so treat it as usable by default.
            else
            {
                result = true;
            }

            // Return the result.
            return result;
        }

        // Converts the provided stat rating to a spin speed object.
        public static spinSpeed ConvertStatRatingToSpinSpeed(statRating rating)
        {
            // The value to return.
            spinSpeed value;

            // Checks what value to use for the rating.
            switch(rating)
            {
                default:
                case statRating.unknown:
                case statRating.noneMinus:
                case statRating.none:
                    value = spinSpeed.none;
                    break;

                case statRating.veryLow:
                    value = spinSpeed.verySlow;
                    break;

                case statRating.low:
                    value = spinSpeed.slow;
                    break;

                case statRating.medium:
                    value = spinSpeed.medium;
                    break;

                case statRating.high:
                    value = spinSpeed.fast;
                    break;

                case statRating.veryHigh:
                case statRating.maximum:
                case statRating.maximumPlus:
                    value = spinSpeed.veryFast;
                    break;
            }

            return value;
        }

        // Sets the current wind speed.
        public void SetCurrentSpinSpeed(spinSpeed speed)
        {
            // Override current spin speed.
            currSpinSpeed = speed;

            // If the spin animations should be used.
            if (AnimationsEnabledAndUsingSpinAnimations)
            {
                PlaySpinAnimation(currSpinSpeed);
            }
        }


        // ANIMATIONS //
        // Returns 'true' if animations are enabled and spin animations are being used.
        public bool AnimationsEnabledAndUsingSpinAnimations
        {
            get { return AnimationsEnabled && useSpinAnims; }
        }

        // Plays the spin animation based on the current wind speed.
        public void PlaySpinAnimation(spinSpeed speed)
        {
            // Gets set to 'true' if this is a land tile. If false, it's a water tile.
            // True by default.
            bool onLand = (tile != null) ? tile.IsLandTile() : true;
            
            // The animation to play.
            string anim = string.Empty;

            // Checks the current spin speed to know what to play.
            switch(currSpinSpeed)
            {
                default:
                case spinSpeed.none:
                    anim = onLand ? landSpinNoneAnim : waterSpinNoneAnim;
                    break;

                case spinSpeed.verySlow:
                    anim = onLand ? landSpinVerySlowAnim : waterSpinVerySlowAnim;
                    break;

                case spinSpeed.slow:
                    anim = onLand ? landSpinSlowAnim : waterSpinSlowAnim;
                    break;

                case spinSpeed.medium:
                    anim = onLand ? landSpinMediumAnim: waterSpinMediumAnim;
                    break;

                case spinSpeed.fast:
                    anim = onLand ? landSpinFastAnim : waterSpinFastAnim;
                    break;

                case spinSpeed.veryFast:
                    anim = onLand ? landSpinVeryFastAnim : waterSpinVeryFastAnim;
                    break;
            }

            // If the animation was set, play it.
            if(anim != "")
            {
                animator.Play(anim);
            }
        }

        // Converts the wind rating to the spin speed.
        public void PlaySpinAnimation(statRating windRating)
        {
            PlaySpinAnimation(ConvertStatRatingToSpinSpeed(windRating));
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        protected override void Update()
        {
            base.Update();

            // Gets the current wind rating (don't recalculate) and converts it to the spin speed.
            statRating windRating = actionManager.GetCurrentWindRating(false);
            spinSpeed ratingSpeed = ConvertStatRatingToSpinSpeed(windRating);

            // If the spin speed doesn't match the current wind rating, change it.
            if (currSpinSpeed != ratingSpeed)
            {
                // Set the current spin speed.
                SetCurrentSpinSpeed(ratingSpeed);
            }
        }
    }
}