using System;
using UnityEngine;

namespace AttnKare
{
    public class Conveyor : MonoBehaviour
    {
        private static float _speed = 25f;
        private Vector3 direction = new(0, 0, 1);

        public bool useStopPoint;
        public Transform stopPoint;

        public static Action<bool, GameObject> RobotInPosition;

        public static float Speed
        {
            get => _speed;
            set => _speed = value;
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
                r.velocity = Speed * direction * Time.deltaTime;
                if (Mathf.Abs(other.gameObject.transform.position.z - stopPoint.position.z) < 0.001f 
                    && useStopPoint)
                {
                    other.GetComponent<BoxCollider>().enabled = false;
                    r.velocity = Vector3.zero;
                    if (other.gameObject.CompareTag("Robot"))
                    {
                        Debug.Log(other.gameObject.name);
                        RobotInPosition?.Invoke(true, other.gameObject);
                    }
                    else
                        Debug.Log("Object is not a Robot");
                }
            }
        }

        public static void SetSpeed(float speed)
        {
            Speed = speed;
        }
    }
}
