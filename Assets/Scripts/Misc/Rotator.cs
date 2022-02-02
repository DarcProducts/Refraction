using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationVector;

    void FixedUpdate() => transform.Rotate(Time.fixedDeltaTime * rotationVector, Space.Self);
}
