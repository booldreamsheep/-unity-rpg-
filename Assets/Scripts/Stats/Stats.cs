using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats 
{
    [SerializeField]private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        int finaValue= baseValue;
        foreach(int modifiers in modifiers)
        {
            finaValue += modifiers;
        }


        return finaValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModiFier(int _modifier)
    {
        modifiers.Add( _modifier );
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove( _modifier );
    }
}
