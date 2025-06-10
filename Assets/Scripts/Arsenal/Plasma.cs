using UnityEngine;

public class Plasma : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rb;

    public void Init(Vector3 velocity) 
    {
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision col) 
    {
        Destroy(gameObject, 3);
    }
}