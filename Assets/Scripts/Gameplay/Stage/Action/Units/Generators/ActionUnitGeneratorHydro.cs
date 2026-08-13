using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Hydro
    public class ActionUnitGeneratorHydro : ActionUnitGenerator
    {
        [Header("Generator/Hydro")]

        // The facing direction of the hydro generator.
        public Vector2 facingDirec = Vector2.right;

        // If 'true', the hydro generator is using its restricted configuration.
        private bool restrictConfig = true;

        // The timer used for flooding. When the timer runs out, related tiles are flooded.
        [Tooltip("The countdown timer for nearby tiles being flooded.")]
        public float floodTimer = 0.0F;

        // The maximum of the flood timer.
        public float floodTimerMax = 25.0F;

        // Gets set to 'true' if the generator has flooded related tiles.
        public bool floodedTiles = false;

        // If 'true', the hydro generator can flood tiles.
        public bool floodingEnabled = true;

        [Header("Generator/Hydro/Visuals")]

        // The directional sprites.
        public Sprite direcLeftSprite;
        public Sprite direcRightSprite;
        public Sprite direcUpSprite;
        public Sprite direcDownSprite;

        [Header("Generator/Hydro/Audio")]

        // The flood sound effect.
        public AudioClip floodSfx;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to hydro.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.hydro;
            }

            // Updates the facing direction and sets the sprite.
            UpdateFacingDirectionByTile(true);

            // Sets the flood timer to max.
            SetFloodTimerToMax();
        }


        // TILE CONFIGURATION //
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

            // The result to be returned. Result 1 and 2 will be checked to make this final result.
            bool finalResult;

            // Determines what checks should be done.
            bool runCheck1 = true;
            bool runCheck2 = true;

            // Will be set to 'false' if the hydro generator does not have land on both sides.
            bool result1 = true;

            // Will be set to false if a nearby hydro or wind generator is found.
            bool result2 = true;

            // Gets the stage.
            ActionStage stage = ActionManager.Instance.actionStage;

            // The tile's row and column positions.
            int tileRow = tile.GetMapRowPosition();
            int tileCol = tile.GetMapColumnPosition();

            // RESULT 1 - See if there's land on both sides of the requested position.
            if(runCheck1)
            {
                // The tile direction.
                Vector2 tileDirec;

                int t1Row, t1Col;
                int t2Row, t2Col;

                // Checks if the tile is directional or not.
                if(tile is ActionTileDirectional)
                {
                    // Get direction.
                    tileDirec = (tile as ActionTileDirectional).CalculateDirectionCardinal();

                    // If none, face right by default.
                    if (tileDirec == Vector2.zero)
                        tileDirec = Vector2.right;
                }
                else
                {
                    // Face right.
                    tileDirec = Vector2.right;
                }

                // Checks the direction to see what tiles to select.
                if(tileDirec.y > 0 || tileDirec.y < 0) // Facing Up or Down
                {
                    // Tile to the left.
                    t1Row = tileRow;
                    t1Col = tileCol - 1;

                    // Tile to the right.
                    t2Row = tileRow;
                    t2Col = tileCol + 1;
                }
                else // Facing Left or Right
                {
                    // Tile above.
                    t1Row = tileRow + 1;
                    t1Col = tileCol;

                    // Tile below.
                    t2Row = tileRow - 1;
                    t2Col = tileCol;
                }

                // Gets the nearby tiles.
                ActionTile tile1 = stage.ValidMapPosition(t1Row, t1Col) ? stage.GetTile(t1Row, t1Col) : null;
                ActionTile tile2 = stage.ValidMapPosition(t2Row, t2Col) ? stage.GetTile(t2Row, t2Col) : null;

                // Checks the two tiles.
                // If the tile is null, it returns true by default.
                bool tile1Check = tile1 != null ? tile1.IsLandTile() : true;
                bool til2Check = tile2 != null ? tile2.IsLandTile() : true;

                // If both tile checks are valid, this result is true.
                result1 = tile1Check && til2Check;
            }


            // RESULT 2 - See that there's no nearby generators that would stop the hydro generator from being placed.
            if(runCheck2)
            {
                // Gets set to true if this tile is next to a hydro generator.
                // If it is, don't allow the hydro generator to be placed there.
                bool nextToHydroInWater = false;

                // Gets set to 'true' if next to a wind generator in the water.
                bool nextToWindInWater = false;

                // The program only checks up, down, left, and right, so these values can be hard coded.
                ActionTile tileLeft = null;
                ActionTile tileRight = null;
                ActionTile tileUp = null;
                ActionTile tileDown = null;

                // Setting left.
                if (stage.ValidMapPosition(tileRow - 1, tileCol))
                    tileLeft = stage.tiles[tileRow - 1, tileCol];

                // Setting right.
                if (stage.ValidMapPosition(tileRow + 1, tileCol))
                    tileRight = stage.tiles[tileRow + 1, tileCol];

                // Setting up.
                if (stage.ValidMapPosition(tileRow, tileCol + 1))
                    tileUp = stage.tiles[tileRow, tileCol + 1];

                // Setting down.
                if (stage.ValidMapPosition(tileRow, tileCol - 1))
                    tileDown = stage.tiles[tileRow, tileCol - 1];

                // Creates a list of tiles to be checked.
                List<ActionTile> checkTiles = new List<ActionTile>() { tileLeft, tileRight, tileUp, tileDown };

                // Goes through the check tiles.
                foreach (ActionTile checkTile in checkTiles)
                {
                    // Checks if the tile exists.
                    if (checkTile != null)
                    {
                        // Checks if there's an action user, then checks if it's a water tile.
                        if (checkTile.HasActionUnitUser() && checkTile.IsWaterTile())
                        {
                            // Checks if it's a generator.
                            if (checkTile.actionUnitUser is ActionUnitGenerator)
                            {
                                // Convert to generator.
                                ActionUnitGenerator generator = (ActionUnitGenerator)checkTile.actionUnitUser;

                                // Checks the resource.
                                // If it's hydro or wind, change the appropriate variables.
                                switch (generator.resource)
                                {
                                    case NaturalResources.naturalResource.hydro:
                                        nextToHydroInWater = true;
                                        break;

                                    case NaturalResources.naturalResource.wind:
                                        nextToWindInWater = true;
                                        break;
                                }

                            }
                        }
                    }

                    // If next to a hydro generator or a wind generator, you can't place the unit here.
                    if (nextToHydroInWater || nextToWindInWater)
                    {
                        // Result is false, and no more need for checks.
                        result2 = false;
                        break;
                    }
                }
            }
            

            // Calculates the final result.
            finalResult = result1 && result2;

            // Returns the result.
            return finalResult;
        }


        // FACING DIRECTION //
        // Returns 'true' if facing left.
        public bool IsFacingLeft()
        {
            return facingDirec.x < 0.0F;
        }

        // Returns 'true' if facing right.
        public bool IsFacingRight()
        {
            return facingDirec.x > 0.0F;
        }

        // Returns 'true' if facing up.
        public bool IsFacingUp()
        {
            return facingDirec.y > 0.0F;
        }

        // Returns 'true' if facing down.
        public bool IsFacingDown()
        {
            return facingDirec.y < 0.0F;
        }

        // Calculates the cardinal direction.
        public Vector2 CalculateDirectionCardinal()
        {
            // The direction to return.
            Vector2 returnDirec;

            // Checks the direction to see what cardinal direction to return.
            if (facingDirec.x < 0.0F) // Left
            {
                returnDirec = Vector2.left;
            }
            else if (facingDirec.x > 0.0F) // Right
            {
                returnDirec = Vector2.right;
            }
            else if (facingDirec.y > 0.0F) // Up
            {
                returnDirec = Vector2.up;
            }
            else if (facingDirec.y < 0.0F) // Down
            {
                returnDirec = Vector2.down;
            }
            else // Unknown
            {
                returnDirec = Vector2.zero;
            }

            return returnDirec;
        }

        // Updates the facing direction by the tile the generator is on.
        public void UpdateFacingDirectionByTile(bool updateSprite = true)
        {
            // Checks if tile is set.
            if(tile != null)
            {
                // If the tile is a directional, match the tile direction.
                if(tile is ActionTileDirectional)
                {
                    // Gets the directional tile.
                    ActionTileDirectional tileDirectional = (ActionTileDirectional)tile;

                    // Gets the tile direc in cardinal form.
                    Vector2 tileDirec = tileDirectional.CalculateDirectionCardinal();

                    // If the tile direction is zero, set it to right by default.
                    // If the tile direction is properly set, use it.
                    facingDirec = (tileDirec == Vector2.zero) ? Vector2.right : tileDirec;
                }
                else
                {
                    // Right by default.
                    facingDirec = Vector2.right;
                }
            }
            else
            {
                // Set to right by default.
                facingDirec = Vector2.right;
            }

            // Updates the sprite.
            if (updateSprite)
                UpdateSpriteByFacingDirection();
        }

        // Sets the sprite by the facing direction.
        public void UpdateSpriteByFacingDirection()
        {
            // Checks the facing direction to know what sprite to use.
            if(facingDirec.x < 0.0F) // Left
            {
                spriteRenderer.sprite = direcLeftSprite;
            }
            else if(facingDirec.x > 0.0F) // Right
            {
                spriteRenderer.sprite = direcRightSprite;
            }
            else if(facingDirec.y > 0.0F) // Up
            {
                spriteRenderer.sprite = direcUpSprite;
            }
            else if(facingDirec.y < 0.0F) // Down
            {
                spriteRenderer.sprite = direcDownSprite;
            }
        }


        // FLOODING //
        // Sets the flood timer to its max.
        public void SetFloodTimerToMax()
        {
            floodTimer = floodTimerMax;
        }

        // Sets the flood timer to its max and sets if the tiles have already been flooded or not.
        public void SetFloodTimerToMax(bool alreadyFloodedTiles)
        {
            SetFloodTimerToMax();
            floodedTiles = alreadyFloodedTiles;
        }

        // Flood the tiles.
        public void FloodTiles()
        {
            // The tile exists.
            if(tile != null)
            {
                // Gets the direction in cardinal.
                Vector2 cardinalDirec = CalculateDirectionCardinal();

                // If the cardinal direction is zero, set it to right by default.
                if (cardinalDirec == Vector2.zero)
                    cardinalDirec = Vector2.right;

                // The map position of the tile this generator is on.
                Vector2Int mapPos = tile.mapPos;

                // The map positions to be flooded.
                Vector2Int mapPos1 = Vector2Int.zero;
                Vector2Int mapPos2 = Vector2Int.zero;
                Vector2Int mapPos3 = Vector2Int.zero;

                // Gets set to 'true' when the tile positions have been set.
                bool mapPosSet = false;

                // Checks if facing the four directions (diagonals are ignored).
                if(cardinalDirec.x > 0) // Facing Right
                {
                    // Left-Up, Left, Left-Down
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x - 1, mapPos.y);
                    mapPos3 = new Vector2Int(mapPos.x - 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(cardinalDirec.x < 0) // Facing Left
                {
                    // Right-Up, Right, Right-Down
                    mapPos1 = new Vector2Int(mapPos.x + 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x + 1, mapPos.y);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(cardinalDirec.y > 0) // Facing Up
                {
                    // Left-Down, Down, Right-Down
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y - 1);
                    mapPos2 = new Vector2Int(mapPos.x, mapPos.y - 1);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(cardinalDirec.y < 0) // Facing Down
                {
                    // Left-Up, Up, Right-Up
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x, mapPos.y + 1);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y + 1);

                    mapPosSet = true;
                }

                // The three tiles.
                ActionTile tile1 = null;
                ActionTile tile2 = null;
                ActionTile tile3 = null;

                // Positions have been set.
                if(mapPosSet)
                {
                    // The action stage.
                    ActionStage actionStage = ActionManager.Instance.actionStage;

                    // Map position 1 is valid.
                    if(actionStage.ValidMapPosition(mapPos1.y, mapPos1.x))
                        tile1 = actionStage.tiles[mapPos1.y, mapPos1.x];

                    // Map position 2 is valid.
                    if (actionStage.ValidMapPosition(mapPos2.y, mapPos2.x))
                        tile2 = actionStage.tiles[mapPos2.y, mapPos2.x];

                    // Map position 3 is valid.
                    if (actionStage.ValidMapPosition(mapPos3.y, mapPos3.x))
                        tile3 = actionStage.tiles[mapPos3.y, mapPos3.x];

                }
                // No positions set, so somethign went wrong.
                else
                {
                    Debug.LogWarning("Facing direction wasn't set properly. No nearby tiles could be found.");
                }

                // Tile 1 exists.
                if(tile1 != null)
                {
                    tile1.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile1.KillActionUnitUser();
                }

                // Tile 2 exists.
                if(tile2 != null)
                {
                    tile2.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile2.KillActionUnitUser();
                }

                // Tile 3 exists.
                if(tile3 != null)
                {
                    tile3.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile3.KillActionUnitUser();
                }
            }
            else
            {
                Debug.LogWarning("No tile is set to this generator. Nearby tiles for flooding can't be found.");
            }

            // Tiles have been flooded.
            floodedTiles = true;

            // Tiles have been flooded, so play SFX.
            if (floodedTiles)
            {
                PlayFloodSfx();
            }

            // If the player is selecting an action prefab, that means the tiles are highlighted.
            // As such, refresh the highlighted tiles.
            if (ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab())
            {
                ActionManager.Instance.actionStage.RefreshHighlightedTiles();
            }
        }

        // Plays the flood sound effect.
        public void PlayFloodSfx()
        {
            if(CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(floodSfx);
            }
        }

        // Update is called every from, if the MonoBehaviour is enabled
        protected override void Update()
        {
            base.Update();

            // If flooding is enabled.
            if(floodingEnabled)
            {
                // If the flood timer isn't finished.
                if(floodTimer > 0.0F)
                {
                    floodTimer -= Time.deltaTime;

                    // Flood timer has reached end.
                    if(floodTimer <= 0.0F)
                    {
                        floodTimer = 0;
                        FloodTiles();
                    }
                }
            }
        }
    }
}