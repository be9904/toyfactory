using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare
{
    [CreateAssetMenu(fileName = "New Stage Settings Data", menuName = "ScriptableObjects/Stage Settings Data")]
    public class StageSettings : ScriptableObject
    {
        public float conveyorSpeed;
        public int totalCount;
    }
}
