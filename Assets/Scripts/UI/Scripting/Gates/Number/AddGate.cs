public class AddGate : NumberGate
{
    public override string GetCode()
    {
        return "add";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        NodeValue value = new(NodeValue.ValueType.Number);
        value.numberValue = inputNodes[0].GetNodeValue().numberValue + inputNodes[1].GetNodeValue().numberValue;

        outputNodes[0].SetNodeValue(value);
    }
}
