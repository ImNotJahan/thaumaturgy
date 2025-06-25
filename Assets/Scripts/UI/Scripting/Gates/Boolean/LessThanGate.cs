public class LessThanGate : NumberGate
{
    public override string GetCode()
    {
        return "less_than";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue value = new NodeValue(NodeValue.ValueType.Boolean);
        value.booleanValue = inputNodes[0].GetNodeValue().numberValue < inputNodes[1].GetNodeValue().numberValue;

        outputNodes[0].SetNodeValue(value);
    }
}
