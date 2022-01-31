using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewBoolVariable", menuName = "Variables/New Bool Variable")]
public class BoolVariable : ScriptableObject 
{ 
    [SerializeField] bool _value; 
    public Action<bool> OnValueChanged;
    public bool resetFalseOnDisable;
    public bool resetTrueOnDisable;
    public bool Value { 
        get { return _value; } 
        set {
            _value = value;
            OnValueChanged?.Invoke(_value);
        } 
    }

    void OnDisable()
    {
        if (resetFalseOnDisable)
            _value = false;
        if (resetTrueOnDisable)
            _value = true;
    }
}
