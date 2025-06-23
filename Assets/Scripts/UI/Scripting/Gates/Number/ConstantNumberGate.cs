using TMPro;
using UnityEngine;

public class ConstantNumberGate : Constant
{
    [SerializeField]
    TMP_InputField input;
    
    public void UpdateValue(string newValue)
    {
        UpdateValue(newValue, false);
    }

    public void UpdateValue(string newValue, bool changeInputText)
    {
        value.numberValue = float.Parse(newValue);
        outputNodes[0].SetNodeValue(value);

        if (changeInputText) input.text = newValue;
    }
}
