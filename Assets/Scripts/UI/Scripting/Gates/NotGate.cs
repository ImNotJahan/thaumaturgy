using UnityEngine;

public class NotGate : Gate
{
    [SerializeField]
    Node inputNode;
    [SerializeField]
    Node outputNode;

    void Start()
    {
        inputNode.onNodeValueChanged += UpdateOutput;
    }

    void UpdateOutput()
    {
        NodeValue val = inputNode.GetNodeValue();

        if (val.IsNull())
        {
            outputNode.SetNodeValue(new NodeValue());
            return;
        }

        if (val.valueType != NodeValue.ValueType.Boolean)
        {
            outputNode.SetNodeValue(new NodeValue());
            return;
        }

        NodeValue newValue = new NodeValue(NodeValue.ValueType.Boolean);
        newValue.booleanValue = !val.booleanValue;

        outputNode.SetNodeValue(newValue);
    }
}