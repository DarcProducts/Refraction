using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenBounds : MonoBehaviour
{
    Camera mainCamera;

    void Awake() => mainCamera = GetComponent<Camera>();

    public float ScreenWidth() => ScreenHeight() * mainCamera.aspect;

    public float ScreenHeight() => 2.0f * transform.position.z * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
}
