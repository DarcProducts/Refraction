using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Variables/New Vector2 Variable")]
public class Vector2Variable : ScriptableObject 
{
    public UnityAction<Vector2> OnValueChanged;
    public Vector2 value;
    public bool resetOnDisable;

    void OnDisable()
    {
        if (resetOnDisable)
            value = Vector2.zero;
    }

    public Vector2 Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }
    }
}
