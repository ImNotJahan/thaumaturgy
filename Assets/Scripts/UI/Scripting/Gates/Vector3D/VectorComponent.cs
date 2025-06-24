using Unity.VisualScripting;
using UnityEngine;

public class VectorComponent : VectorGate
{
    public enum Component { X, Y, Z, Unknown };
    [SerializeField]
    Component component;

    public override string GetCode()
    {
        return component switch
        {
            Component.X => "x_component",
            Component.Y => "y_component",
            Component.Z => "z_component",
            _ => "unknown_component",
        };
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue newValue = new(NodeValue.ValueType.Number);
        newValue.numberValue = GetComponent();

        outputNodes[0].SetNodeValue(newValue);
    }

    float GetComponent()
    {
        Vector3 vector = inputNodes[0].GetNodeValue().vector3DValue;

        return component switch
        {
            Component.X => vector.x,
            Component.Y => vector.y,
            Component.Z => vector.z,
            _ => 0,
        };
    }

    public override void Deserialize(GateData data)
    {
        base.Deserialize(data);

        component = data.gate_code switch
        {
            "x_component" => Component.X,
            "y_component" => Component.Y,
            "z_component" => Component.Z,
            _ => Component.Unknown,
        };
    }
}
