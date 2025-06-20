using System;
using TMPro;
using UnityEngine;

public class Display : Gate
{
    [SerializeField]
    Node inputNode;

    [SerializeField]
    TextMeshProUGUI text;

    void Start()
    {
        inputNode.onNodeValueChanged += UpdateDisplay;
    }

    public void UpdateDisplay()
    {
        NodeValue value = inputNode.GetNodeValue();

        if (value.IsNull())
        {
            text.text = "";
            return;
        }

        switch (value.valueType)
        {
            case NodeValue.ValueType.Number:
                text.text = Convert.ToString(value.numberValue);
                break;

            case NodeValue.ValueType.Boolean:
                text.text = Convert.ToString(value.booleanValue);
                break;

            case NodeValue.ValueType.Vector2D:
                text.text = Convert.ToString(value.vector2DValue);
                break;

            case NodeValue.ValueType.Vector3D:
                text.text = Convert.ToString(value.vector3DValue);
                break;

            default:
                break;
        }
    }
}
