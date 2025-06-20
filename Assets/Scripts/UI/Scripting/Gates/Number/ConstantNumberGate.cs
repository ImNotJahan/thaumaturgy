public class ConstantNumberGate : Constant
{
    public void UpdateValue(string newValue)
    {
        value.numberValue = int.Parse(newValue);
        outputNodes[0].SetNodeValue(value);
    }
}
