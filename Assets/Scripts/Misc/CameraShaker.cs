using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker S = null;
    [SerializeField] Camera mainCamera;
    [SerializeField] float defaultIntesity = .2f;
    float currentTime;
    float currentMagnitude;
    Vector3 cameraOriginalPos;
    Vector3 newFactor = Vector3.zero;
    bool shakeCamera;
    bool hasPosition = false;


    void Awake() => S = this;

    void OnEnable()
    {
        currentTime = currentMagnitude;
        cameraOriginalPos = mainCamera.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (mainCamera == null) return;
        currentTime = currentTime < 0 ? 0 : currentTime -= Time.fixedDeltaTime;
        if (currentTime > 0 && shakeCamera)
        {
            newFactor = cameraOriginalPos + Random.insideUnitSphere * currentMagnitude;
            mainCamera.transform.localPosition = newFactor;
        }
    }

    public void Trigger(float magnitude) 
    {
        if (mainCamera == null) return;
        float clampedMag = Mathf.Clamp01(magnitude);
        currentMagnitude = clampedMag;
        StartCoroutine(ShakeCamera(clampedMag));
    }

    public void Trigger() => Trigger(defaultIntesity);

    IEnumerator ShakeCamera(float magnitude)
    {
        if (!hasPosition)
        {
            cameraOriginalPos = mainCamera.transform.localPosition;
            hasPosition = true;
        }
        currentTime = magnitude * 2;
        shakeCamera = true;
        yield return new WaitForSeconds(magnitude * 2);
        mainCamera.transform.localPosition = cameraOriginalPos;
        hasPosition = false;
        shakeCamera = false;
    }
}