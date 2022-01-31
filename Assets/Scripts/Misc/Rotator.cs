using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationVector;

    void FixedUpdate() => transform.rotation *= Quaternion.Euler(rotationVector);
}
