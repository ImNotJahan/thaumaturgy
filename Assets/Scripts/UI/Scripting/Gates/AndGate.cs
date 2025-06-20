using UnityEngine;

public class AndGate : Gate
{
    [SerializeField]
    Node inputNode1;
    [SerializeField]
    Node inputNode2;
    [SerializeField]
    Node outputNode;

    void Start()
    {
        inputNode1.onNodeValueChanged += UpdateOutput;
        inputNode2.onNodeValueChanged += UpdateOutput;
    }

    void UpdateOutput()
    {
        NodeValue val1 = inputNode1.GetNodeValue();
        NodeValue val2 = inputNode2.GetNodeValue();

        if (val1.IsNull() || val2.IsNull())
        {
            outputNode.SetNodeValue(new NodeValue());
            return;
        }

        if (val1.valueType != NodeValue.ValueType.Boolean || val2.valueType != NodeValue.ValueType.Boolean)
        {
            outputNode.SetNodeValue(new NodeValue());
            return;
        }

        NodeValue newValue = new NodeValue(NodeValue.ValueType.Boolean);
        newValue.booleanValue = val1.booleanValue && val2.booleanValue;

        outputNode.SetNodeValue(newValue);
    }
}