using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Variables/New Int Variable")]
public class IntVariable : ScriptableObject
{
    public Action<int> OnValueChanged;
    [SerializeField] int _value;
    public bool resetOnDisable;

    void OnDisable()
    {
        if (resetOnDisable)
            _value = 0;
    }

    public int Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
