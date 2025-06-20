public class AndGate : BooleanGate
{
    public override string GetCode()
    {
        return "and";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;
        
        NodeValue newValue = new NodeValue(NodeValue.ValueType.Boolean);
        newValue.booleanValue = inputNodes[0].GetNodeValue().booleanValue && inputNodes[1].GetNodeValue().booleanValue;

        outputNodes[0].SetNodeValue(newValue);
    }
}