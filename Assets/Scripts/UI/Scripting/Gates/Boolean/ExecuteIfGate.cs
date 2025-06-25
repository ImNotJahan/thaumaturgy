public class ExecuteIfGate : Gate
{
    public override string GetCode()
    {
        return "if";
    }

    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        if (inputNodes[1].GetNodeValue().booleanValue != true)
        {
            NullifyOutputs();
            return;
        }

        outputNodes[0].SetNodeValue(inputNodes[0].GetNodeValue());
    }
}
