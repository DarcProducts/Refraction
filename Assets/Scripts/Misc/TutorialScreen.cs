using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] float timeRateDuringTutorial;
    public void CloseTutorialScreen() => gameObject.SetActive(false);

    void LateUpdate() => Time.timeScale = timeRateDuringTutorial;

    void OnDisable() => Time.timeScale = 1;
}