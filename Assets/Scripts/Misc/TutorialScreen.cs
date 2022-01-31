using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] bool HasSeen;

    void Start()
    {
        if (HasSeen)
            gameObject.SetActive(false);
    }

    public void CloseTutorialScreen()
    {
        HasSeen = true;
        gameObject.SetActive(false);
    }
}