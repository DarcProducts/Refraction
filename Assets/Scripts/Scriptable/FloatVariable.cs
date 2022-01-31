using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Variables/New Float Variable")]
public class FloatVariable : ScriptableObject 
{
    public Action<float> OnValueChanged;
    [SerializeField] float _value;
    public bool resetOnDisable;

    void OnDisable()
    {
        if (resetOnDisable)
            _value = 0;
    }

    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
