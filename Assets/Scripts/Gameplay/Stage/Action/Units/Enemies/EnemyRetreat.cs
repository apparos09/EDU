using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // An enemy that retreats back to the enemy's base.
    // It's spawned when an enemy dies.
    public class EnemyRetreat : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The sprite renderer of the retreating enemy.
        public SpriteRenderer spriteRenderer;

        // The rigid body, which is used for movement.
        public new Rigidbody2D rigidbody;

        // The movement direction.
        public Vector2 moveDirec = Vector2.right;

        // The movement speed.
        public float moveSpeed = 18.0F;

        // The energy death cost to be applied when the enemy reaches the stage bounds.
        public float energyDeathCost = 0;

        // The energy death cost factor to be applied.
        public float energyDeathCostFactor = 1.0F;

        // The enemy retreats.
        private static List<EnemyRetreat> enemyRetreats = new List<EnemyRetreat>();

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            // If this enemy retreat isn't in the enemy retreats list, add it.
            if (!enemyRetreats.Contains(this))
                enemyRetreats.Add(this);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Sets the manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the rigidbody if it's not set.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
        }

        // Cancles the velocity of the enemy retreat.
        // If 'checkVelocity' is true, then the velocity is checked for being zero first. If it is zero, no change is done.
        public void CancelVelocity(bool checkVelocity = true)
        {
            // If the velocity should be checked for it not being zero.
            if (checkVelocity)
            {
                if (rigidbody.velocity != Vector2.zero)
                    rigidbody.velocity = Vector2.zero;
            }
            // Do it regardless.
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

        // Gets the energy death cost modified by the cost factor.
        public float GetEnergyDeathCostModified()
        {
            return ActionUnit.CalculateEnergyDeathCost(energyDeathCost, energyDeathCostFactor);
        }

        // Kils the retreating enemy.
        protected virtual void Kill()
        {
            // Applies the energy death cost.
            // This doesn't check for a 0 value since all enemies have a death cost anyway.
            ActionManager.Instance.playerEnemy.DecreaseEnergy(GetEnergyDeathCostModified());

            // Destroys this enemy.
            Destroy(gameObject);
        }

        // Kills all the retreating enemies.
        public static void KillAllEnemyRetreats()
        {
            // Goes through all the enemy retreats, killing each one.
            for (int i = enemyRetreats.Count - 1; i >= 0; i--)
            {
                // If the enemy retreat exists, kill it.
                if (enemyRetreats[i] != null)
                {
                    // The enemy retreat should remove itself from the list upon being destroyed.
                    enemyRetreats[i].Kill();
                }
                else
                {
                    // If the enemy retreat is null, it shouldn't be in the list, so remove it.
                    enemyRetreats.RemoveAt(i);
                }
            }

            // All enemy retreats should be gone now, so clear the list.
            enemyRetreats.Clear();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // The stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Sets the velocity and then clamps it.
                rigidbody.velocity = moveDirec * moveSpeed;
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, moveSpeed);

                // If out of the stage bounds, the retreating enemy is now dead.
                if (!actionManager.actionStage.InStageBounds(transform.position))
                {
                    Kill();
                }
            }
            else
            {
                // Cancels out the velocity of the enemy if the game is paused or the stage isn't playing.
                CancelVelocity();
            }
        }

        // This function is called when the MonoBehaviour will be destroyed
        protected virtual void OnDestroy()
        {
            // If this enemy reetreat is in the enemy retreats list, remove it.
            if (enemyRetreats.Contains(this))
                enemyRetreats.Remove(this);
        }
    }
}