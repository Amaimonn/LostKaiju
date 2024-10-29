using UnityEngine;
public class DummyBullet : MonoBehaviour
{
    public Vector3 FlyDirection;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody _rigidbody;

    public void SetVelocity()
    {
        Vector3 velocity = FlyDirection * _speed;

        _rigidbody.linearVelocity = velocity;
        Debug.Log(velocity);
    }
}