using System;
using UnityEngine;

namespace AttnKare
{
    public class Conveyor : MonoBehaviour
    {
        private static float _speed = 25f;
        private Vector3 direction = new(0, 0, 1);

        public bool hasStopPoint;
        public Transform stopPoint;
        private bool isDisabled;

        public static Action<bool, GameObject> RobotInPosition;

        public static float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void OnEnable()
        {
            GameManager.EndGame += DisableConveyor;
        }
        
        private void OnDisable()
        {
            GameManager.EndGame -= DisableConveyor;
        }

        private void Start()
        {
            if (stopPoint == null)
                stopPoint = transform;
        }

        private void OnTriggerStay(Collider other)
        {
            Rigidbody r;
            if ((r = other.gameObject.GetComponent<Rigidbody>()) != null)
            {
                if (isDisabled)
                {
                    r.velocity = Vector3.zero;
                    return;
                }
                
                r.velocity = Speed * direction * Time.deltaTime;
                if (Mathf.Abs(other.gameObject.transform.position.z - stopPoint.position.z) < 0.001f 
                    && hasStopPoint && other.gameObject.CompareTag("Robot"))
                {
                    Robot robotCmp = other.gameObject.GetComponent<Robot>();
                    if (!robotCmp.IsPainted)
                    {
                        r.velocity = Vector3.zero;
                        RobotInPosition?.Invoke(true, other.gameObject);
                    }
                    else // if IsPainted is true
                    {
                        RobotInPosition?.Invoke(false, null);
                    }
                }
            }
        }

        public static void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void DisableConveyor()
        {
            isDisabled = true;
            enabled = false;
        }
    }
}
