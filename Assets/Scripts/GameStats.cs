using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    [CreateAssetMenu(fileName = "New Game Stats Data", menuName = "ScriptableObjects/Game Stats Data")]
    public class GameStats : ScriptableObject
    {
        // game data to be saved -> should be moved
        public int robotCount;
        // more data can be added here

        public void ResetStats()
        {
            robotCount = 0;
        }
    }
}