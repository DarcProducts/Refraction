using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent<GameObject> Response;

    void OnEnable() => Event.RegisterListener(this);
    void OnDisable() => Event.UnregisterListener(this);
    public void OnEventRaised(GameObject obj)
    {
        if (!obj.activeSelf) return;
        Response?.Invoke(obj);
    }
}
