using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A tile that is facing a specific direction. By default, all player user units face right.
    public class ActionTileDirectional : ActionTile
    {
        [Header("Directional")]

        // The direction the tile is facing.
        public Vector2 direction = Vector2.right;

        // If 'true', set the direction by the tile version in start.
        public bool setDirecByTileVersionInStart = true;

        // TODO: replace with animations
        // The sprite for the sprite facing left.
        public Sprite direcLeftSprite;

        // The sprite for the tile facing right.
        public Sprite direcRightSprite;

        // The sprite for the tile facing up.
        public Sprite direcUpSprite;

        // The sprite for the tile facing down.
        public Sprite direcDownSprite;

        // If 'true', the directional sprite is set in the Start() function.
        [Tooltip("Sets the directional sprite in the start function.")]
        public bool setDirecSpriteInStart = true;

        [Header("Animations")]

        // The idle animations for the directional tile.
        public string idleLeftAnimName = "";
        public string idleRightAnimName = "";
        public string idleUpAnimName = "";
        public string idleDownAnimName = "";


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the direction shall be set by the tile version.
            if(setDirecByTileVersionInStart)
            {
                // Since this function already updates the sprite...
                // It checks 'setDirecSpriteInStart' to see if it should be changed.
                SetDirectionByTileVersion(setDirecSpriteInStart);
            }
            else
            {
                // If the directional sprite/animation should be set.
                if (setDirecSpriteInStart)
                    SetSpriteAndIdleAnimationByTileDirection();
            }

        }

        // Returns 'true' if direction is left.
        public bool IsDirectionLeft()
        {
            return direction.x < 0.0F;
        }

        // Returns 'true' if direction is right.
        public bool IsDirectionRight()
        {
            return direction.x > 0.0F;
        }

        // Returns 'true' if direction is up.
        public bool IsDirectionUp()
        {
            return direction.y > 0.0F;
        }

        // Returns 'true' if direction is down.
        public bool IsDirectionDown()
        {
            return direction.y < 0.0F;
        }

        // Gets the direction in cardinal form (NSEW). It can only return left, right, up, or down.
        // Returns (0, 0) if the direction is unknown.
        public Vector2 CalculateDirectionCardinal()
        {
            // The direction to return.
            Vector2 returnDirec;

            // Checks the direction to see what cardinal direction to return.
            if (direction.x < 0.0F) // Left
            {
                returnDirec = Vector2.left;
            }
            else if (direction.x > 0.0F) // Right
            {
                returnDirec = Vector2.right;
            }
            else if (direction.y > 0.0F) // Up
            {
                returnDirec = Vector2.up;
            }
            else if (direction.y < 0.0F) // Down
            {
                returnDirec = Vector2.down;
            }
            else // Unknown
            {
                returnDirec = Vector2.zero;
            }

            return returnDirec;
        }

        // Sets the direction by the tile version.
        // Right is the default.
        public void SetDirectionByTileVersion(bool updateSprite = true)
        {
            // The new direction.
            Vector2 newDirec;

            // Checks the tile version to know the direction.
            switch(tileVersion)
            {
                case 'A':
                case 'a': // Right
                default:
                    newDirec = Vector2.right;
                    break;

                case 'B':
                case 'b': // Up
                    newDirec = Vector2.up;
                    break;

                case 'C':
                case 'c': // Left
                    newDirec = Vector2.left;
                    break;

                case 'D':
                case 'd': // Down
                    newDirec = Vector2.down;
                    break;
            }

            // Sets the new direction.
            direction = newDirec;

            // Updates the sprite and animation.
            if (updateSprite)
            {
                SetSpriteAndIdleAnimationByTileDirection();
            }
                
        }

        // Sets the sprite by the tile direction.
        // If the direction is neutral (0, 0), no change occurs.
        public void SetSpriteByTileDirection()
        {
            // Checks the direction to see what sprite to set.
            if (direction.x < 0.0F) // Left
            {
                baseSpriteRenderer.sprite = direcLeftSprite;
            }
            else if (direction.x > 0.0F) // Right
            {
                baseSpriteRenderer.sprite = direcRightSprite;
            }
            else if (direction.y > 0.0F) // Up
            {
                baseSpriteRenderer.sprite = direcUpSprite;
            }
            else if(direction.y < 0.0F) // Down
            {
                baseSpriteRenderer.sprite = direcDownSprite;
            }
        }

        // Sets the sprite by the provided tile direction. This also overrides the saved direction.
        public void SetSpriteByTileDirection(Vector2 newDirection)
        {
            direction = newDirection;
            SetSpriteByTileDirection();
        }

        // Plays the idle animation by the tile direction.
        public void PlayIdleAnimationByTileDirection()
        {
            // Checks the direction to see what sprite to set.
            if (direction.x < 0.0F) // Left
            {
                PlayAnimation(idleLeftAnimName);
            }
            else if (direction.x > 0.0F) // Right
            {
                PlayAnimation(idleRightAnimName);
            }
            else if (direction.y > 0.0F) // Up
            {
                PlayAnimation(idleUpAnimName);
            }
            else if (direction.y < 0.0F) // Down
            {
                PlayAnimation(idleDownAnimName);
            }
        }

        // Sets the idle animation by the provided tile direction. This also overrides the saved direction.
        public void PlayIdleAnimationByTileDirection(Vector2 newDirection)
        {
            direction = newDirection;
            PlayIdleAnimationByTileDirection();
        }

        // Sets the sprite and idle animation by the tile direction.
        public void SetSpriteAndIdleAnimationByTileDirection()
        {
            SetSpriteByTileDirection();
            PlayIdleAnimationByTileDirection();
        }

        // Sets the sprite and idle animation by the tile direction.
        public void SetSpriteAndIdleAnimationByTileDirection(Vector2 newDirection)
        {
            direction = newDirection;
            SetSpriteAndIdleAnimationByTileDirection();
        }
    }

}