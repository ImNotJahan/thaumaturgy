using UnityEngine;

public class ScalarMultiplyGate : Gate
{
    public override string GetCode()
    {
        return "scalar_multiply";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute || inputNodes[0].GetNodeValue().valueType != NodeValue.ValueType.Vector3D
            || inputNodes[1].GetNodeValue().valueType != NodeValue.ValueType.Number)
        {
            outputNodes[0].SetNodeValue(new NodeValue());
            return;
        }

        NodeValue newValue = new(NodeValue.ValueType.Vector3D);
        newValue.vector3DValue = inputNodes[0].GetNodeValue().vector3DValue * inputNodes[1].GetNodeValue().numberValue;

        outputNodes[0].SetNodeValue(newValue);
    }
}
