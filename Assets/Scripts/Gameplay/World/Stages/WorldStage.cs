using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RM_EDU
{
    // The world stage.
    public abstract class WorldStage : MonoBehaviour
    {
        // The world stage data.
        [SerializeField]
        public class WorldStageData
        {
            // The world index of the stage.
            public int worldStageIndex = -1;

            // The id number of the stage.
            public int idNumber = 0;

            // The stage type.
            public stageType stageType;

            // The amount of time the stage took (in seconds).
            public float time = 0;

            // The stage score.
            public float score = 0;

            // The energy total for the stage.
            public float energyTotal = 0;

            // The air pollution for the stage.
            public float airPollution = 0;

            // Marks if the stage is complete.
            public bool complete = false;
        }

        // Enum for the world stage type.
        public enum stageType { unknown, action, knowledge };

        // The world manager.
        public WorldManager worldManager;

        // The sprite renderer for the stage light.
        public SpriteRenderer lightSpriteRenderer;

        // The sprite for the light when it's on.
        // This sprite is used for when the stage is unlocked and not cleared.
        [Tooltip("The sprite for when the light is on (bright light).")]
        public Sprite lightOnSprite;

        // The sprite for the light when it's off.
        // This sprite is used for when the stage is unlocked and cleared.
        [Tooltip("The sprite for when the light is off (dimmed light).")]
        public Sprite lightOffSprite;

        // The sprite for when there's no light at all (black).
        // This sprite is used for when the stage is locked.
        [Tooltip("The sprite for when there's no light at all (black).")]
        public Sprite lightNoneSprite;

        // The collider for the world stage.
        public new Collider2D collider;


        [Header("World Stage/Info")]
        // The stage's ID number.
        public int idNumber = 0;

        // The stage's difficulty.
        public int difficulty = 0;

        // The bgm number.
        public int bgmNumber = 0;

        // The time the stage took (in seconds).
        public float time = 0;

        // The score for this stage.
        public float score = 0;

        // The total amount of energy generated for this stage.
        [Tooltip("The total amount of energy generated in this stage.")]
        public float energyTotal = 0;

        // The amount of air pollution generated in this stage (action stage only).
        [Tooltip("The air pollution generated in this stage (action stage only).")]
        public float airPollution = 0;

        // Checks if the stage is locked. This isn't included in the stage data.
        [Tooltip("If 'true', the stage is locked and cannot be selected.")]
        public bool locked = false;

        // Marker for if the stage is complete.
        [Tooltip("If true, the stage is complete, and cannot be selected.")]
        public bool complete = false;

        [Header("World Stage/Resources")]
        // The natural resources the stage uses.
        [Tooltip("The natural resources for the stage.")]
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The defense units that are unlocked if the player beats the stage.
        [Tooltip("The ids for defense units that're unlocked if the player completes the stage.")]
        public List<int> defenseIdRewards = new List<int>();

        // If 'true', the rewards dialog is used.
        protected bool useRewardsDialog = true;

        [Header("World Stage/Events")]

        // The stage's unlock event.
        public StageUnlockEvent stageUnlockEvent;

        // The stage's complete event.
        public StageCompleteEvent stageCompleteEvent;

        [Header("World Stage/Audio")]

        // If 'true', the audio for the world stage is played.
        public bool playAudio = true;

        // The audio clip that plays when the stage is selected and the dialog opens successfully.
        [Tooltip("Plays when the stage is selected and the dialog was opened (stage available).")]
        public AudioClip selectSuccessSfx;

        // The audio clip that plays when the stage is selected, but the dialog doesn't open.
        [Tooltip("Plays when the stage is selected, but the dialog didn't open (stage unavailable).")]
        public AudioClip selectFailSfx;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the collider has not been set, try grabbing the collider.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the world manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // If the stage unlock event isn't set, try to get it.
            if(stageUnlockEvent == null)
                stageUnlockEvent = GetComponent<StageUnlockEvent>();

            // If the stage complete event isn't set, try to get it.
            if(stageCompleteEvent == null)
                stageCompleteEvent = GetComponent<StageCompleteEvent>();
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // If the pointer is over a game object in the event system, don't take in inputs.
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // Gets set to 'true' if this world stage was selected successfully.
            bool selectSuccess;

            // If the stage is not complete and isn't locked, open the stage dialog.
            if(!complete && !locked)
            {
                WorldManager.Instance.worldUI.OpenWorldStageSelectDialog(this);
                
                selectSuccess = true;
            }
            // The stage is locked or unavailable, so it can't be selected.
            else
            {
                selectSuccess = false;
            }

            // If audio should be played.
            if (playAudio && WorldAudio.Instantiated)
            {
                // Determines what clip to use, and plays it.
                WorldAudio.Instance.PlaySoundEffectWorld(selectSuccess ? selectSuccessSfx : selectFailSfx);
            }
        }


        // LIGHT SPRITE //
        // Sets the light sprite to on sprite (true) or off sprite (false).
        // <= -1 = off, 0 = none, >= 1 = on.
        public void SetLightSprite(int value)
        {
            // The value is negative, so switch it off.
            if(value < 0)
            {
                lightSpriteRenderer.sprite = lightOffSprite;
            }
            // The value is positive, so switch it on.
            else if(value > 0)
            {
                lightSpriteRenderer.sprite = lightOnSprite;
            }
            // No value, so set to none.
            else
            {
                lightSpriteRenderer.sprite = lightNoneSprite;
            }

        }

        // Returns 'true' if the light sprite is the on sprite.
        public bool IsLightSpriteOn()
        {
            return lightSpriteRenderer.sprite == lightOnSprite;
        }

        // Sets the light sprite to the on sprite.
        public void SetLightSpriteToOnSprite()
        {
            SetLightSprite(1);
        }

        // Returns 'true' if the light sprite is the off sprite.
        public bool IsLightSpriteOff()
        {
            return lightSpriteRenderer.sprite == lightOffSprite;
        }

        // Sets the light sprite to the off sprite.
        public void SetLightSpriteToOffSprite()
        {
            SetLightSprite(-1);
        }

        // Returns 'true' if the light sprite is none.
        public bool IsLightSpriteNone()
        {
            return lightSpriteRenderer.sprite == lightNoneSprite;
        }

        // Sets the light sprite to none.
        public void SetLightSpriteToNoneSprite()
        {
            SetLightSprite(0);
        }

        // RESOURCES
        // Gets the resources for this stage as a string.
        public string ResourcesToString()
        {
            // Value to be returned.
            string value = "";

            // Goes through all the resources.
            for(int i = 0; i < naturalResources.Count; i++)
            {
                // Gets the resource.
                NaturalResources.naturalResource res = naturalResources[i];

                // Add to the value.
                value += NaturalResources.GetNaturalResourceName(res);

                // If there's another entry, space out entries.
                if (i + 1 < naturalResources.Count)
                    value += ", ";
            }

            return value;
        }

        // REWARDS //

        // Gives the player their rewards.
        // showRewards: determines if the rewards should be shown to the player.
        public void GivePlayerRewards(bool showRewards)
        {
            // If the rewards dialog should be used...
            // And there was a request to show rewards.
            if(useRewardsDialog && showRewards)
            {
                // Gets the world UI.
                WorldUI worldUI = WorldUI.Instance;

                // Checks if the rewards dialog is open already.
                if(worldUI.IsWorldStageRewardsDialogOpen())
                {
                    // The loaded world stage isn't this stage, so replace it with this stage.
                    if(worldUI.stageRewardsDialog.worldStage != this)
                    {
                        worldUI.OpenWorldStageRewardsDialog(this);
                    }
                }
                else
                {
                    // Open the rewards dialog and give it this stage.
                    worldUI.OpenWorldStageRewardsDialog(this);
                }
            }

            // Give the player their defense units.
            GiveDefenseUnitsToPlayer();
        }

        // Gives the player their rewards, but doesn't show them the rewards.
        public void GivePlayerRewards()
        {
            GivePlayerRewards(false);
        }

        // Gives the player the defense units for this world stage.
        public void GiveDefenseUnitsToPlayer()
        {
            // If there are defense id rewards to give the player.
            if(defenseIdRewards.Count > 0)
            {
                // The data logger.
                DataLogger dataLogger = DataLogger.Instance;

                // Adds action defense units to the data logger.
                dataLogger.AddActionDefenseUnits(defenseIdRewards);
            }
        }


        // STAGE INFO //
        // TODO: add function to check when the object is enabled to refersh text.
        // Returns the index of this stage in the world manager. If -1 is returned, it's not in the world manager stage list.
        public int GetWorldStageIndex()
        {
            return WorldManager.Instance.GetWorldStageIndex(this);
        }

        // Gets the world area this stage is in.
        public WorldArea GetWorldStageArea()
        {
            return WorldManager.Instance.GetWorldStageArea(this);
        }

        // Gets the world stage area index.
        public int GetWorldStageAreaIndex()
        {
            // If the world manager isn't set, set it.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // Gets the area and the index of that area.
            WorldArea area = worldManager.GetWorldStageArea(this);

            // The area index.
            // If the area doesn't exist, set it to -1.
            int areaIndex = (area != null) ? worldManager.areas.IndexOf(area) : -1;

            // Returns the area index.
            return areaIndex;
        }

        // STAGE TYPE
        // Gets the stage type.
        public virtual stageType GetStageType()
        {
            return stageType.unknown;
        }

        // Gets the world stage type name.
        public string GetStageTypeName()
        {
            return GetStageTypeName(GetStageType());
        }

        // Gets the stage type name.
        public static string GetStageTypeName(stageType type)
        {
            // The result to be returned.
            string result;

            // If the LOL SDK is intialized, translate the name.
            if (LanguageManager.IsLanguageLoaderInitialized())
            {
                // Gets the key.
                string key = GetStageTypeNameKey(type);

                // Gets the result.
                result = LanguageManager.GetLanguageTextStatic(key);

                // Set the result to empty string if it's null.
                if (result == null)
                    result = "";
            }
            // Raw value.
            else
            {
                // Checks the type.
                switch (type)
                {
                    default:
                        result = "World";
                        break;

                    case stageType.unknown:
                        result = "Unknown";
                        break;

                    case stageType.action:
                        result = "Action";
                        break;

                    case stageType.knowledge:
                        result = "Knowledge";
                        break;
                }
            }

            // Returns the result.
            return result;
        }

        // Gets the stage type name key.
        public static string GetStageTypeNameKey(stageType type)
        {
            // The key to be return.
            string key;

            // The stage type.
            switch (type)
            {
                default:
                    key = "kwd_world";
                    break;

                case stageType.unknown:
                    key = "kwd_unknown";
                    break;

                case stageType.action:
                    key = "kwd_action";
                    break;

                case stageType.knowledge:
                    key = "kwd_knowledge";
                    break;
            }

            return key;
        }

        // LOCK
        // Returns 'true' if the stage is locked.
        public bool IsLocked()
        {
            return locked;
        }

        // Sets if the stage is locked.
        public void SetLocked(bool stageLocked)
        {
            // Set value.
            locked = stageLocked;

            // Checks if the stage is locked.
            if(locked)
            {
                // Now locked, so set the sprite to none.
                // SetLightSpriteToOffSprite();
                SetLightSpriteToNoneSprite();
            }
            // Stage is unlocked.
            else
            {
                // Chooses sprite based on if the stage is complete or not.
                if(complete)
                {
                    // Light off if complete.
                    SetLightSpriteToOffSprite();
                }
                else
                {
                    // Light on if incomplete.
                    SetLightSpriteToOnSprite();
                }
            }
        }

        // Locks the stage.
        public void LockStage()
        {
            SetLocked(true);
        }

        // Unlocks the stage.
        public void UnlockStage()
        {
            SetLocked(false);
        }


        // COMPLETE
        // Returns 'true' if the stage is complete.
        public bool IsComplete()
        {
            return complete;
        }

        // Sets if the stage is complete.
        // stageComplete: determines if the stage is complete.
        // showRewards: determines if the rewards should be shown to the player if applicable.
        public void SetComplete(bool stageComplete, bool showRewards)
        {
            complete = stageComplete;

            // If the stage is now complete.
            if(complete)
            {
                // If the stage is locked, use the off sprite.
                if (locked)
                {
                    SetLightSpriteToNoneSprite();
                }
                // Set the light sprite to off.
                else
                {
                    SetLightSpriteToOffSprite();
                }


                // Give rewards since the stage is complete.
                GivePlayerRewards(showRewards);
            }
            // If the stage is not complete.
            else
            {
                // If the stage is locked, turn the light off.
                if(locked)
                {
                    SetLightSpriteToNoneSprite();
                }
                // Stage is unlocked, but not complete, so set light to on sprite.
                else
                {
                    SetLightSpriteToOnSprite();
                }
            }
        }

        // Sets the stage complete parameter, and doesn't show rewards.
        public void SetComplete(bool stageComplete)
        {
            SetComplete(stageComplete, false);
        }

        // Refreshes the complete parameter by calling set complete.
        public void RefreshComplete()
        {
            SetComplete(complete);
        }

        // Generates the data.
        public WorldStageData GenerateWorldStageData()
        {
            // The data being generated.
            WorldStageData data = new WorldStageData();

            // The values returned.
            data.worldStageIndex = GetWorldStageIndex();
            data.idNumber = idNumber;
            data.stageType = GetStageType();

            data.time = time;
            data.score = score;
            data.energyTotal = energyTotal;
            data.airPollution = airPollution;

            data.complete = complete;

            return data;
        }

        // Applies world stage data.
        public void ApplyWorldStageData(WorldStageData data)
        {
            // If the world index, id number, or stage type don't match, display a message.
            if(data.worldStageIndex != GetWorldStageIndex() || idNumber != data.idNumber || data.stageType != GetStageType())
            {
                Debug.LogWarning("The id number or stage type doesn't match. Id number changed to match data.");
            }
            
            // Set the values.
            idNumber = data.idNumber;

            time = data.time;
            score = data.score;
            energyTotal = data.energyTotal;
            airPollution = data.airPollution;

            complete = data.complete;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}