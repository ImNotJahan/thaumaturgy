public class NotGate : BooleanGate
{
    public override string GetCode()
    {
        return "not";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue newValue = new NodeValue(NodeValue.ValueType.Boolean);
        newValue.booleanValue = !inputNodes[0].GetNodeValue().booleanValue;

        outputNodes[0].SetNodeValue(newValue);
    }
}