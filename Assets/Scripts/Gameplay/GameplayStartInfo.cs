using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds the information needed to start a given area of the game (world, action, knowledge).
    public abstract class GameplayStartInfo : MonoBehaviour
    {
        // Applies the start info to the provided manager.
        public abstract void ApplyStartInfo(GameplayManager manager);
    }
}