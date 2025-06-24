using UnityEngine;

public class DotProductGate : VectorGate
{
    public override string GetCode()
    {
        return "dot_product";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue newValue = new(NodeValue.ValueType.Number);
        newValue.numberValue = Vector3.Dot(inputNodes[0].GetNodeValue().vector3DValue, inputNodes[1].GetNodeValue().vector3DValue);

        outputNodes[0].SetNodeValue(newValue);
    }
}
