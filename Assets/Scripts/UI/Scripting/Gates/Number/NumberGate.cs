using UnityEngine;

public abstract class NumberGate : Gate
{
    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        foreach (Node input in inputNodes)
        {
            if (input.GetNodeValue().valueType != NodeValue.ValueType.Number)
            {
                canExecute = false;
                NullifyOutputs();
                return;
            }
        }
    }
}
