using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AttnKare
{
    public class Robot : Spawnable
    {
        public Color robotColor;
        
        [SerializeField] private float paintProgress;

        public float PaintProgress => paintProgress;

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

        public void PaintRobot(float paintSpeed)
        {
            if(paintProgress < 100)
                paintProgress += Time.deltaTime * paintSpeed;
            /*else
            {
                _isPainted = true;
            }*/
        }

        public override void InitRandom() { }
    }
}
