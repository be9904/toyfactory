using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AttnKare
{
    public class Robot : Spawnable
    {
        public Color robotColor;

        private bool _isPainted;
        public bool IsPainted
        {
            get => _isPainted;
            set => _isPainted = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            InitRandom();
        }

        public override void InitRandom()
        {
            
        }
    }
}
