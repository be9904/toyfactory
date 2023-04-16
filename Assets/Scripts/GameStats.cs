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
        public int boxCount;
        // more data can be added here
    }
}