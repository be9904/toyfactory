using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AttnKare
{
    public class Robot : Spawnable
    {
        public Color robotColor;

        private BoxCollider boxCollider;
        [SerializeField] private float paintProgress;
        private bool _isPainted;
        public bool IsPainted
        {
            get => _isPainted;
            set => _isPainted = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
            InitRandom();
        }

        public void PaintRobot(float paintSpeed)
        {
            if(paintProgress < 100)
                paintProgress += Time.deltaTime * paintSpeed;
            else
            {
                _isPainted = true;
                boxCollider.enabled = true;
                Conveyor.RobotInPosition?.Invoke(false, null);
            }
        }

        public override void InitRandom() { }
    }
}
