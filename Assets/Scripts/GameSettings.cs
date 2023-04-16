using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    [CreateAssetMenu(fileName = "New Stage Settings Data", menuName = "ScriptableObjects/Stage Settings Data")]
    public class GameSettings : ScriptableObject
    {
        public float timeLimit;
        public float conveyorSpeed;
        public float paintSpeed;
        // game data to be saved -> should be moved
        public int robotCount;
        public int boxCount;
    }
}
