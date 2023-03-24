using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private static float _speed = 25f;
    private Vector3 direction = new(0, 0, 1);

    public static float Speed
    {
        get => _speed;
        private set {}
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody r;
        if ((r = other.gameObject.GetComponent<Rigidbody>()) != null)
        {
            r.velocity = Speed * direction * Time.deltaTime;
        }
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }
}
