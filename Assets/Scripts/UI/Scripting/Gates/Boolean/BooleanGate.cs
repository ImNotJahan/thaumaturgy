using UnityEngine;

public class BooleanGate : Gate
{
    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        // verify that all inputs are booleans
        foreach (Node inputNode in inputNodes)
        {
            if (inputNode.GetNodeValue().valueType != NodeValue.ValueType.Boolean)
            {
                canExecute = false;
                NullifyOutputs();
                return;
            }
        }
    }
}
