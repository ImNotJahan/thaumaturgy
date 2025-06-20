using TMPro;
using UnityEngine;

public class Vector3BuilderGate : Gate
{
    [SerializeField]
    TextMeshProUGUI text;

    public override string GetCode()
    {
        return "vector3";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        text.text = "";

        if (!canExecute) return;

        foreach (Node input in inputNodes)
        {
            if (input.GetNodeValue().valueType != NodeValue.ValueType.Number)
                return;
        }

        NodeValue newValue = new(NodeValue.ValueType.Vector3D);
        newValue.vector3DValue = new Vector3(inputNodes[0].GetNodeValue().numberValue, inputNodes[1].GetNodeValue().numberValue, inputNodes[2].GetNodeValue().numberValue);

        outputNodes[0].SetNodeValue(newValue);
        text.text = newValue.vector3DValue.ToString();
    }
}
