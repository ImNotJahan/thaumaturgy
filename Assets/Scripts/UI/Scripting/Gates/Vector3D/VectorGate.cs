// gate where all inputs are vectors
public abstract class VectorGate : Gate
{
    protected override void UpdateOutput()
    {
        base.UpdateOutput();

        if (!canExecute) return;

        foreach (Node input in inputNodes)
        {
            if (input.GetNodeValue().valueType != NodeValue.ValueType.Vector3D)
            {
                canExecute = false;
                NullifyOutputs();
                return;
            }
        }
    }
}