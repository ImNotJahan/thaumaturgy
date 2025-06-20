using System;
using TMPro;
using UnityEngine;

public class Display : ThaumaturgicGate
{
    [SerializeField]
    TextMeshProUGUI text;

    public override string GetCode()
    {
        return "display";
    }

    protected override void UpdateOutput()
    {
        NodeValue value = inputNodes[0].GetNodeValue();

        text.text = value.IsNull() ? "" : value.ToString();
    }
}
