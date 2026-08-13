using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The action stage.
    public class ActionStage : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // If 'true', the ID number is automatically set from the manager.
        // This is off by default since the manager should be initialized first.
        public bool autoSetIdNumber = false;

        // If 'true', the map is generated in the Start() function.
        public bool generateMapOnStart = false;

        // The action tile prefabs.
        public ActionTilePrefabs actionTilePrefabs;

        // The action stage list.
        public ActionStageList actionStageList;

        [Header("Stage")]

        // The tile parent.
        public GameObject tileParent;

        // The default row count for the map.
        public const int MAP_ROW_COUNT_DEFAULT = 5; // Original: 7

        // The default colum count for a map.
        public const int MAP_COLUMN_COUNT_DEFAULT = 12; // Original: 15

        // Arrays - (r, w) = (y, x)
        // Action Tiles
        // The action tile array, which also determines the map size.
        public ActionTile[,] tiles = new ActionTile[MAP_ROW_COUNT_DEFAULT, MAP_COLUMN_COUNT_DEFAULT];

        // The origin of the tile. By default, it's the middle of the tile.
        private Vector2 tileOrigin = new Vector2(0.5F, 0.5F);

        // The default tile size x-length.
        public const float TILE_SIZE_X_DEFAULT = 1.28F;

        // The default tile size y-length.
        public const float TILE_SIZE_Y_DEFAULT = 1.28F;

        // The tile sprite size in pixels (length, width)
        private Vector2 tileSize = new Vector2(TILE_SIZE_X_DEFAULT, TILE_SIZE_Y_DEFAULT);

        // Returns 'true' if the tile highlights are enabled.
        private static bool tileHighlightingEnabled = true;

        // The original of the map.
        // (0.5, 0.5) is the centre, (1, 1) is the top right corner and (0, 0) is the bottom left corner.
        protected Vector2 mapOrigin = new Vector2(0.5F, 0.5F);

        // If 'true', the tiles are coloured to make a checkerboard that makes the boundaries between tiles clear.
        private static bool mapCheckerboardEnabled = true;

        // The list of metal tiles in the stage.
        private List<ActionTile> metalTiles = new List<ActionTile>();

        // If true, lane blasters are used.
        private bool useLaneBlasters = true;

        // The row enemy units.
        public List<List<ActionUnitEnemy>> rowEnemyUnits = new List<List<ActionUnitEnemy>>();

        // An offset that can be applied whenn checking if something's in the stage bounds or not.
        private Vector3 stageBoundsOffset = new Vector3(TILE_SIZE_X_DEFAULT * 8, TILE_SIZE_Y_DEFAULT * 8, 0.0F);

        // Gets set to 'true' when a map has been generated.
        private bool stageGenerated = false;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // If the action stage isn't set, set it to this.
            if (actionManager.actionStage == null)
                actionManager.actionStage = this;

            // Gets the instance.
            // NOTE: this uses instance for ease of access, but it should NOT be set this way.
            // This is because the prefabs aren't set if you aren't using a pre-built object.
            if (actionTilePrefabs == null)
                actionTilePrefabs = ActionTilePrefabs.Instance;

            // Gets the instance.
            if (actionStageList == null)
                actionStageList = ActionStageList.Instance;

            // If the map should be generated in the Start() function.
            if (generateMapOnStart)
            {
                GenerateStage();
            }
        }

        // Gets the tile size.
        public Vector2 TileSize
        {
            get { return tileSize; }
        }

        // Gets the map size.
        public Vector2 MapSize
        {
            get
            {
                // return mapSize; 
                // (row, col) = (y, x)
                return new Vector2(tiles.GetLength(1), tiles.GetLength(0));
            }
        }

        // Gets the maximum amount of tiles in the map.
        public int MapTileCountMax
        {
            get { return tiles.Length; }
        }

        // Gets the map's row count.
        public int MapRowCount
        {
            get { return tiles.GetLength(0); }
        }

        // Get's the map's column count.
        public int MapColumnCount
        {
            get { return tiles.GetLength(1); }
        }

        // Gets the id number, which is taken from the action manager.
        public int GetIdNumber()
        {
            return actionManager.idNumber;
        }

        // STAGE DATA / GENERATION //
        // Gets the stage map data. Returns null if there's no data.
        public static ActionStageList.StageGenerationData GetStageData(int idNumber)
        {
            // Gets the data.
            ActionStageList.StageGenerationData data = ActionStageList.GenerateStageMap(idNumber);

            // Return map.
            return data;
        }

        // Gets the stage map data. Returns null if there's no data.
        public ActionStageList.StageGenerationData GetStageData()
        {
            return GetStageData(GetIdNumber());
        }

        // Generates a map using the id number from the action manager.
        public void GenerateStage()
        {
            // Generates the map using the id number from the action manager.
            GenerateStage(actionManager.idNumber);
        }

        // Generates a map using the provided ID number. Saves the provided as the ID number.
        public void GenerateStage(int idNumber)
        {
            // Gets the map based on the id number.
            ActionStageList.StageGenerationData data = ActionStageList.GenerateStageMap(idNumber);

            // The data doesn't exist.
            if (data == null)
            {
                Debug.LogError("Data could not be found.");
                return;
            }

            // Sets the map.
            string[,] map = data.map;

            // The map doesn't exist.
            if (map == null)
            {
                Debug.LogError("No map data was found.");
                return;
            }

            // Clears the list of metal tiles.
            metalTiles.Clear();


            // The sfx world source and a temporary variable for getting the enabled value.
            AudioSource sfxWorldSource;
            bool sfxWorldEnabledTemp;

            // If the action audio is instantiated, get the sfx source.
            if (ActionAudio.Instantiated)
            {
                // Get the audio source, mute it, and save the mute value.
                // This is done to make sure no sounds are played when the stage is generated.
                sfxWorldSource = ActionAudio.Instance.sfxWorldSource;
                sfxWorldEnabledTemp = sfxWorldSource.enabled;
                sfxWorldSource.enabled = false;
            }
            else
            {
                sfxWorldSource = null;
                sfxWorldEnabledTemp = true;
            }


            // Goes through all the rows and columns, generating the tiles.
            // Row
            for (int r = 0; r < map.GetLength(0); r++)
            {
                // Column
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    // The tile id from the map.
                    // Format: ##A (Number-Number-Letter).
                    string tileId = map[r, c];

                    // The tile number, which is the first 2 digits.
                    int tileNumber = int.Parse(tileId.Substring(0, 2));

                    // The tile verison.
                    // If it's the right length (3), the last character is used as the version.
                    // If the length is wrong, it defaults to the first tile version (A).
                    // This accounts for cases where there's only the two number digits (00), though this should never happen.
                    char tileVersion = tileId.Length == 3 ? char.ToUpper(tileId[2]) : 'A';

                    // The tile prefab and the new tile.
                    // The indexes should match up with the id numbers, but using indexes is faster.
                    ActionTile tilePrefab = actionTilePrefabs.GetActionTilePrefab(tileNumber);
                    // ActionTile tilePrefab = actionTilePrefabs.GetActionTilePrefabByIdNumber(tileNumber);
                    ActionTile newTile = null;

                    // If the tile prefab exists, create the tile.
                    if (tilePrefab != null)
                        newTile = Instantiate(tilePrefab, transform);

                    // If the new tile doesn't exist, go to the next tile.
                    // Tile 0 is an empty space.
                    if (newTile == null)
                    {
                        // Destroys the tile at the index if it eixsts.
                        if (tiles[r, c] != null)
                        {
                            Destroy(tiles[r, c].gameObject);
                            tiles[r, c] = null;
                        }

                        // Next.
                        continue;
                    }

                    // Applies the parent.
                    if (tileParent != null)
                        newTile.transform.parent = tileParent.transform;

                    // Sets the tile version.
                    newTile.tileVersion = tileVersion;

                    // These will be set automatically in the tile Start() functions.
                    // newTile.setTileVersionSpriteInStart = false;
                    // newTile.SetSpriteByTileVersion();

                    // Sets the map position of the new tile.
                    // Row = Y, Col = X
                    newTile.mapPos.x = c;
                    newTile.mapPos.y = r;

                    // Sets the new local position.
                    Vector3 newLocalPos = ConvertMapPositionToWorldUnits(c, r);
                    newTile.transform.localPosition = newLocalPos;


                    // CHECKERBOARD
                    // If the map checkerboard is enabled, alter the tile colours to make a checkerboard.
                    if (mapCheckerboardEnabled)
                    {
                        // The tile colour.
                        Color tileColor = Color.white;

                        // Sets the tile's colour, which is used to make a checkerboardp attern.
                        // The bottom left corner (0, 0) of the map is black (darkenend tile).
                        if (r % 2 == 0) // Black
                        {
                            // 0 = black, 1 = white
                            tileColor = c % 2 == 0 ? ActionTile.DarkenedColor : ActionTile.NormalColor;
                        }
                        else // White
                        {
                            // 0 = white, 1 = black
                            tileColor = c % 2 == 0 ? ActionTile.NormalColor : ActionTile.DarkenedColor;
                        }

                        // Sets the tile colour.
                        newTile.baseSpriteRenderer.color = tileColor;
                    }


                    // OVERLAY //
                    // If there are overlays, set the tile's default overlay.
                    if (data.overlays != null)
                    {
                        // Gets the new overlay, clamping it within valid values.
                        ActionTile.actionTileOverlay newOverlay =
                            (ActionTile.actionTileOverlay)(Mathf.Clamp(data.overlays[r, c], 0, ActionTile.ACTION_TILE_OVERLAY_TYPE_COUNT - 1));

                        // Sets the new default type.
                        newTile.SetDefaultTileOverlayType(newOverlay, true);
                    }

                    // If there's already a tile in this array index, destroy it so it can be replaced.
                    if (tiles[r, c] != null)
                    {
                        Destroy(tiles[r, c].gameObject);
                        tiles[r, c] = null;
                    }


                    // If this is a metal tile...
                    if (newTile.GetTileType() == ActionTile.actionTile.metal)
                    {
                        // Add the tile to the list.
                        metalTiles.Add(newTile);
                    }

                    // Add the tile to the array.
                    tiles[r, c] = newTile; // New
                }
            }

            // WIND //
            // If there's winds, use them for the stage.
            if (data.windRatings != null)
            {
                // Save the wind ratings.
                ActionManager.Instance.windRatings = data.windRatings;
            }

            // Put a lane blaster on the far left edge of the map.
            if (useLaneBlasters)
            {
                // Create the lane blasters in row 0.
                CreateLaneBlastersInRow0(true);
            }

            // If there are stage rows, clear all the lists
            if (rowEnemyUnits.Count > 0)
            {
                // Clears all the lists.
                foreach (List<ActionUnitEnemy> list in rowEnemyUnits)
                {
                    // Destroys all enemies in the list, before clearing it.
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        list[i].OnUnitDeath();
                    }

                    // Clears the list.
                    list.Clear();
                }
            }

            // Clear the list of enemy row unit lists.
            rowEnemyUnits.Clear();

            // Make a list for every row.
            for (int n = 0; n < map.GetLength(0); n++)
            {
                rowEnemyUnits.Add(new List<ActionUnitEnemy>());
            }

            // If the sfx world source was successfully gotten, switch the enabled parameter back.
            if(sfxWorldSource != null)
            {
                sfxWorldSource.enabled = sfxWorldEnabledTemp;
            }

            // The map has now been generated.
            stageGenerated = true;
        }

        // Returns true if the map has been generated.
        public bool StageGenerated
        {
            get { return stageGenerated; }
        }

        // Returns 'true' if the stage allows for tile highlighting.
        public static bool TileHighlightingEnabled
        {
            get { return tileHighlightingEnabled; }
        }

        // Returns 'true' if the map checkerboard is enabled.
        public static bool IsMapCheckerboardEnabled
        {
            get { return mapCheckerboardEnabled; }
        }

        // Returns 'true' if the stage has metal tiles.
        public bool HasMetalTiles
        {
            get { return metalTiles.Count > 0; }
        }

        // Returns the tile at the provided row and column.
        // Returns null if values are invalid.
        public ActionTile GetTile(int row, int column)
        {
            // If position valid, return tile. If false, return null.
            if (ValidMapPosition(row, column))
                return tiles[row, column];
            else
                return null;
        }

        // Gets a tile in the map using the parameter mapPos.
        public ActionTile GetTile(Vector2Int mapPos)
        {
            return GetTile(mapPos.y, mapPos.x);
        }


        // Gets a copy of the list of metal tiles.
        public List<ActionTile> GetListOfMetalTilesCopy()
        {
            List<ActionTile> list = new List<ActionTile>(metalTiles);
            return list;
        }


        // Returns 'true' if the provided map position is valid (valid index).
        public bool ValidMapPosition(int row, int column)
        {
            // If the tiles don't exist, the map position will always be invalid.
            if (tiles == null)
                return false;

            // Checks x and y for valid positions.
            bool validRow = row >= 0 && row < MapRowCount;
            bool validColumn = column >= 0 && column < MapColumnCount;

            return validRow && validColumn;
        }

        // Checks if the map position is valid.
        // Keep in mind that (X) is the column and (y) is the row.
        public bool ValidMapPosition(Vector2Int mapPos)
        {
            return ValidMapPosition(mapPos.y, mapPos.x);
        }

        // Returns 'true' if the row is valid.
        public bool ValidMapRow(int row)
        {
            return row >= 0 && row < MapRowCount;
        }

        // Returns 'true' if the column is alid
        public bool ValidMapColumn(int column)
        {
            return column >= 0 && column < MapColumnCount;
        }


        // Converts the provided map tile position to local position in world units.
        // Argument "mapPos" is the tile position in the map.
        public Vector2 ConvertMapPositionToWorldUnits(Vector2 mapPos)
        {
            // NOTE: the term "local" refers to the local position of the tile in world units.
            // So say, if the tile is a child of another object, the result is the local position of that tile...
            // In reference to its parent.

            // The map size.
            Vector2 mapSize = MapSize;

            // The map size in pixels.
            Vector2 mapPixelSize = mapSize * tileSize;

            // The map origin position offset.
            // The origin(0.5, 0.5) is considered the centre, so a calculation is done to offset it.
            // e.g., if the origin was (0.4, 0.4), the calculation would be (0.4, 0.4) - (0.5, 0.5) = (-0.1, -0.1).
            // This would result in tiles shifting over by 10% to the bottom left.
            Vector2 mapOriginPosOffset = mapOrigin - new Vector2(0.5F, 0.5F);

            // Calculates the map origin position given the map size.
            // It applies the origin position offset.
            Vector2 mapOriginPosLocal = mapPixelSize * (mapOrigin + mapOriginPosOffset);

            // The map position is a percentage.
            Vector2 mapPosPercent = mapPos / mapSize;

            // The map position in the local position.
            Vector2 mapPosLocal = mapPixelSize * mapPosPercent;

            // Adjusts the map position in the world based on the map's origin in the world.
            mapPosLocal -= mapOriginPosLocal;

            // The origin of the tile in pixels. This is the tile origin based on the size of the tile in pixels.
            Vector2 tilePixelOrigin = tileSize * tileOrigin;

            // Adjust the tile's world position by its tile origin.
            // The base calculation assumes the tile origin is its bottom-left corner.
            mapPosLocal += tilePixelOrigin;

            // Returns the map position in the world.
            return mapPosLocal;
        }

        // Creates lane blasters in row 0.
        // If 'killUserOnTile' is true, if there's a unit saved to the tile, that unit is killed...
        // So that the tile can be used by the lane blaster.
        public void CreateLaneBlastersInRow0(bool killTileUser)
        {
            // Gets the player user.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // Goes through each row.
            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                // If the tile exists.
                if (tiles[r, 0] != null)
                {
                    // If there's an action unit user on the tile and they should be killed...
                    if (killTileUser && tiles[r, 0].HasActionUnitUser())
                    {
                        // Kill the action unit user on the tile.
                        tiles[r, 0].KillActionUnitUser();

                    }

                    // Instantiate the lane laser prefab.
                    // Don't reduce the player's energy when making a lane blaster.
                    playerUser.InstantiateLaneBlaster(tiles[r, 0], true, false);
                }
            }
        }

        // Convert the map position to world units.
        public Vector2 ConvertMapPositionToWorldUnits(float mapPosX, float mapPosY)
        {
            // Convert to vector 2 to use other function.
            return ConvertMapPositionToWorldUnits(new Vector2(mapPosX, mapPosY));
        }

        // Gets the map lower bound in world units.
        // If 'useTileCorner' is true, the lower bounds is the edge of the tile.
        // If 'useTileCorner' is false, it goes by the tile position, which is the tile's centre (origin).
        public Vector2 GetMapLowerBoundsInWorldUnits(bool useTileCorner = true)
        {
            // Result.
            Vector2 result = ConvertMapPositionToWorldUnits(0, 0);

            // If using the tile corner, reduce it by the tile size / 2.
            // It's /2 because the tile origin is the tile's centre.
            if (useTileCorner)
                result -= TileSize / 2.0F;

            return result;
        }

        // Gets the map upper bound in world units.
        // If 'useTileCorner' is true, the upper bounds is the edge of the tile.
        // If 'useTileCorner' is false, it goes by the tile position, which is the tile's centre (origin).
        public Vector2 GetMapUpperBoundsInWorldUnits(bool useTileCorner = true)
        {
            // Result
            Vector2 result = ConvertMapPositionToWorldUnits(MapSize);

            // If using the tile corner, increase it by the tile size / 2.
            // It's /2 because the tile origin is the tile's centre.
            if (useTileCorner)
                result += TileSize / 2.0F;

            return result;
        }

        // Returns 'true' if the map contains the provided tile and the tile's saved mapPos.
        // If it doesn't, or if the tile isn't in the array, it returns false.
        public bool MapContainsTileAtIndex(ActionTile tile)
        {
            // The tile's row and column positions
            int tileRow = tile.GetMapRowPosition();
            int tileCol = tile.GetMapColumnPosition();

            // The result.
            bool result;

            // If the row and column values are valid.
            if (tileRow >= 0 && tileRow < tiles.GetLength(0) && tileCol >= 0 && tileCol < tiles.GetLength(1))
            {
                // If the tile is saved at the index, the check is valid.
                if (tiles[tileRow, tileCol] == tile)
                {
                    result = true;
                }
                else // Not in space, so check is false.
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        // Calculates the color of the tile at a given index.
        public Color CalculateTileColorInMap(int row, int col)
        {
            // The row and column counts.
            int rowCount = MapRowCount;
            int colCount = MapColumnCount;

            // The colour that will be returned.
            Color color;

            // If the index is valid, just send the color white.
            if (row >= 0 && row < rowCount && col >= 0 && col < colCount)
            {
                // Determines if the normal colour should be returned.
                // The "white" color is just the normal color.
                // The "black" color is just the darkened color.
                bool useNormal;

                // The bottom left corner (0, 0) is black.
                // 0 = black, 1 = white.
                if (row % 2 == 0) // Black
                {
                    // 0 = black, 1 = white
                    useNormal = col % 2 == 0 ? false : true;
                }
                else // White
                {
                    // 0 = white, 1 = black
                    useNormal = col % 2 == 0 ? true : false;
                }

                // Gets the color.
                color = useNormal ? ActionTile.NormalColor : ActionTile.DarkenedColor;
            }
            else
            {
                // Invalid index, so just return the colour white.
                Debug.LogAssertion("Index invalid. Returning Color.white.");
                color = Color.white;
            }

            // Returns the color.
            return color;
        }

        // Returns 'true' if tile highlighting is active.
        // Highlighting is on whenever the player has a unit selected.
        public bool IsTileHighlightingActive()
        {
            // If tile highlighting is off, the highlights are never active.
            if (TileHighlightingEnabled)
            {
                // If the player is selecting a unit prefab, then tile highlights must be on.
                return ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab();
            }
            else
            {
                return false;
            }
        }

        // Highlights all the tiles using the provided unit.
        public void HighlightAllTiles(ActionUnit compUnit)
        {
            // If highlighting is enabled.
            if (TileHighlightingEnabled)
            {
                // Goes through all the rows and columns.
                for (int r = 0; r < tiles.GetLength(0); r++)
                {
                    for (int c = 0; c < tiles.GetLength(1); c++)
                    {
                        // The tile exists.
                        if (tiles[r, c] != null)
                        {
                            tiles[r, c].HighlightTile(compUnit);
                        }
                    }
                }
            }
        }

        // Unhighlights all tiles if highlighting is enabled.
        public void UnhighlightAllTiles()
        {
            // If tile highlighting is enabled.
            if (TileHighlightingEnabled)
            {
                // Goes through all the rows and columns.
                for (int r = 0; r < tiles.GetLength(0); r++)
                {
                    for (int c = 0; c < tiles.GetLength(1); c++)
                    {
                        // The tile exists, so unhighlight it.
                        if (tiles[r, c] != null)
                        {
                            tiles[r, c].UnhighlightTile();
                        }
                    }
                }
            }
        }

        // Refreshes the highlighted tiles using the player's selected prefab.
        // If the player has no prefab selected, all tiles are unhighlighted.
        public void RefreshHighlightedTiles()
        {
            // Gets the player user.
            ActionPlayerUser playerUser = actionManager.playerUser;

            // If the player is selecting a prefab, highlight refresh.
            if (playerUser.IsSelectingActionUnitPrefab())
            {
                HighlightAllTiles(playerUser.selectedUnitPrefab);
            }
            // Player doesn't have a prefab, so unhighlight all tiles.
            else
            {
                UnhighlightAllTiles();
            }
        }

        // Called when the player user has selected a unit.
        public void OnPlayerUserSelectedUnit()
        {
            // The player user.
            ActionPlayerUser playerUser = actionManager.playerUser;

            // Called if the player user is selecting a unit.
            if (playerUser.IsSelectingActionUnitPrefab())
            {
                HighlightAllTiles(playerUser.selectedUnitPrefab);
            }
        }

        // Called when the player has unselected a unit.
        public void OnPlayerUserClearedSelectedUnit()
        {
            // The player user.
            ActionPlayerUser playerUser = actionManager.playerUser;

            // Called if the player user is selecting a unit.
            if (!playerUser.IsSelectingActionUnitPrefab())
            {
                // Unhighlights all the tiles.
                UnhighlightAllTiles();
            }
        }

        // Gets a vector representing the provided world position in reference to the map.
        // If a value for an axis is (0), it means the position is within the stage.
        // If it's a 1, it's outside the stage on the positive end of that axis.
        // If it's a -1, it's outisde the stage on the negative end of that axis.
        public Vector3 GetMapWorldPositionReferenceVector(Vector3 worldPos)
        {
            // The map bounds.
            Vector3 mapBoundsLower = GetMapLowerBoundsInWorldUnits();
            Vector3 mapBoundsUpper = GetMapUpperBoundsInWorldUnits();

            // The reference vector.
            Vector3 refVec = new Vector3(
                worldPos.x < mapBoundsLower.x ? -1 : worldPos.x > mapBoundsUpper.x ? 1 : 0,
                worldPos.y < mapBoundsLower.y ? -1 : worldPos.y > mapBoundsUpper.y ? 1 : 0,
                worldPos.z < mapBoundsLower.z ? -1 : worldPos.z > mapBoundsUpper.z ? 1 : 0
                );

            // Returns the vector.
            return refVec;
        }

        // Checks if there's at least 1 enemy in the provided row.
        public bool IsEnemyInRow(int row)
        {
            // Checks if the row is valid.
            if (row >= 0 && row < rowEnemyUnits.Count)
            {
                return rowEnemyUnits[row].Count > 0;
            }
            else
            {
                // Row not valid.
                return false;
            }
        }

        // Returns 'true' if there's an enemy in the row relative to the reference position.
        // If refDirec is negative, it checks to the left. If refDirec is positive, it checks to the right.
        //  - If refDirec is 0, it checks the exact position.
        // If 'ignoreTangible' is true, the tangible component is ignored. If false, intangible enemies will go undetected.
        public bool IsEnemyInRowReferencOfPosition(int row, Vector3 refPos, int refDirec, bool includeEqualTo, bool ignoreTangible)
        {
            // Checks if the row is valid.
            if (row >= 0 && row < rowEnemyUnits.Count)
            {
                // Gets set to true if the enemy is in range.
                bool inRange = false;

                // Goes through all enemies.
                foreach (ActionUnitEnemy enemy in rowEnemyUnits[row])
                {
                    // If the enemy isn't active, move onto the next one.
                    if (!enemy.isActiveAndEnabled)
                        continue;

                    // Checks if the enemy is hittable at all.
                    // If the tangible component should be ignored, the enemy can always be hit.
                    bool canBeHit = ignoreTangible ? true : enemy.tangible;

                    // If can be hit, check if in line of fire.
                    if (canBeHit)
                    {
                        if (refDirec < 0) // Negative (Left)
                        {
                            // Checks if equal to should be included.
                            if (includeEqualTo)
                            {
                                //  The enemy is to the left or equal to the reference position.
                                if (refPos.x >= enemy.transform.position.x)
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                            else
                            {
                                // The enemy is to the left of the reference position.
                                if (refPos.x > enemy.transform.position.x)
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                        }
                        else if (refDirec > 0) // Positive (Right)
                        {
                            // Checks if equal to should be included.
                            if (includeEqualTo)
                            {
                                //  The enemy is to the right or equal to the reference position.
                                if (refPos.x <= enemy.transform.position.x)
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                            else
                            {
                                // The enemy is to the right of the reference position.
                                if (refPos.x < enemy.transform.position.x)
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                        }
                        else // Exact position.
                        {
                            // Is in range if the reference position is the same as the enemy position.
                            if (refPos == enemy.transform.position)
                            {
                                inRange = true;
                                break;
                            }
                        }
                    }

                }

                return inRange;
            }
            else
            {
                // Row not valid.
                return false;
            }
        }


        // Returns 'true' if there's an enemy negative to the provided position (left of position).
        public bool IsEnemyInRowLeftOfPosition(int row, Vector3 refPos, bool includeEqualTo, bool ignoreTangible)
        {
            // Check negative direction (left).
            return IsEnemyInRowReferencOfPosition(row, refPos, -1, includeEqualTo, ignoreTangible);
        }

        // Returns 'true' if there's an enemy positive to the provided position (right of position).
        public bool IsEnemyInRowRightOfPosition(int row, Vector3 refPos, bool includeEqualTo, bool ignoreTangible)
        {
            // Check positive direction (right).
            return IsEnemyInRowReferencOfPosition(row, refPos, 1, includeEqualTo, ignoreTangible);
        }

        // Returns 'true' if the position is in the stage bounds.
        // If 'ignoreZ' is true, the z-position is ignored when checking if the position is within the stage bounds.
        //  * This is a 2D game, so the z-position should be ignored.
        // If 'applyOffset' is true, an offset is applied when checking if within the stage bounds.
        // If 'applyOffset' is false, no offset is applied, meaning an object is only in the stage bounds if...
        // It's position is in the map itself.
        public bool InStageBounds(Vector3 pos, bool ignoreZ = true, bool applyOffset = true)
        {
            // Gets the map lower bounds and upper bounds.
            Vector3 mapLowerBounds = GetMapLowerBoundsInWorldUnits();
            Vector3 mapUpperBounds = GetMapUpperBoundsInWorldUnits();

            // Calculates the lower bounds and upper bounds.
            // Gives different results if the stage bounds offset should be applied or not.
            Vector3 lowerBounds = applyOffset ? mapLowerBounds - stageBoundsOffset : mapLowerBounds;
            Vector3 upperBounds = applyOffset ? mapUpperBounds + stageBoundsOffset : mapUpperBounds;

            bool inX = pos.x >= lowerBounds.x && pos.x <= upperBounds.x;
            bool inY = pos.y >= lowerBounds.y && pos.y <= upperBounds.y;
            bool inZ = ignoreZ ? true : pos.z >= lowerBounds.x && pos.z <= upperBounds.x;

            // Returns true if the unit is in all 3 bounds.
            return inX && inY && inZ;
        }

        // Returns 'true' if the object is within the stage bounds.
        public bool InStageBounds(GameObject obj, bool ignoreZ = true, bool applyOffset = true)
        {
            return InStageBounds(obj.transform.position, applyOffset);
        }

        // Resets the stage.
        public void ResetStage()
        {
            // Resets all the tiles for the map.
            for (int r = 0; r < tiles.GetLength(0); r++) // Rows
            {
                for (int c = 0; c < tiles.GetLength(1); c++) // Columns
                {
                    // Tile exists, so reset it.
                    if (tiles[r, c] != null)
                    {
                        tiles[r, c].ResetTile();
                    }
                }
            }

            // If lane blasters should be used, create lane blasters in row 0. 
            if (useLaneBlasters)
            {
                CreateLaneBlastersInRow0(true);
            }
        }
    }
}