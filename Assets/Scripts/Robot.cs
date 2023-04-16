using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AttnKare
{
    public class Robot : Spawnable
    {
        private GameManager.RobotColor robotColor;

        public GameManager.RobotColor Color => robotColor;

        [SerializeField] private float paintProgress;

        public float PaintProgress => paintProgress;

        private bool _isPainted;
        public bool IsPainted
        {
            get => _isPainted;
            set => _isPainted = value;
        }

        public void PaintRobot(float paintSpeed)
        {
            if(paintProgress < 100)
                paintProgress += Time.deltaTime * paintSpeed;
        }

        public void SetColor(GameManager.RobotColor color)
        {
            robotColor = color;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
        }
    }
}
