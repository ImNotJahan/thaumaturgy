public class VectorAddGate : VectorGate
{
    public override string GetCode()
    {
        return "vector_add";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue newValue = new(NodeValue.ValueType.Vector3D);
        newValue.vector3DValue = inputNodes[0].GetNodeValue().vector3DValue + inputNodes[1].GetNodeValue().vector3DValue;

        outputNodes[0].SetNodeValue(newValue);
    }
}
