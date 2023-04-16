using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    [CreateAssetMenu(fileName = "New Game Settings Data", menuName = "ScriptableObjects/Game Settings Data")]
    public class GameSettings : ScriptableObject
    {
        public float timeLimit;
        public float conveyorSpeed;
        public float paintSpeed;
        
        [Range(0f, 1f)]
        public float painterMoveSpeed;
    }
}
