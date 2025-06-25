public class EqualsGate : Gate
{
    public override string GetCode()
    {
        return "equals";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue value = new NodeValue(NodeValue.ValueType.Boolean);
        value.booleanValue = inputNodes[0].GetNodeValue().Compare(inputNodes[1].GetNodeValue());

        outputNodes[0].SetNodeValue(value);
    }
}
