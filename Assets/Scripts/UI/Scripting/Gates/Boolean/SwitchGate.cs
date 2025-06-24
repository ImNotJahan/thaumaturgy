public class SwitchGate : Gate
{
    public override string GetCode()
    {
        return "switch";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        if (inputNodes[2].GetNodeValue().valueType != NodeValue.ValueType.Boolean)
        {
            NullifyOutputs();
            return;
        }

        outputNodes[0].SetNodeValue(inputNodes[2].GetNodeValue().booleanValue ? inputNodes[1].GetNodeValue() : inputNodes[0].GetNodeValue());
    }
}
