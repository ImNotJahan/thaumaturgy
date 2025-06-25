using System;
using UnityEngine;

[Serializable]
public class NodeValue
{
    public enum ValueType
    {
        Number,
        Boolean,
        Vector2D,
        Vector3D,
        Null
    };

    public ValueType valueType;
    public float numberValue;
    public bool booleanValue;
    public Vector2 vector2DValue;
    public Vector3 vector3DValue;

    public NodeValue(ValueType type)
    {
        valueType = type;
    }

    public NodeValue()
    {
        valueType = ValueType.Null;
    }

    public bool IsNull()
    {
        return valueType == ValueType.Null;
    }

    public override string ToString()
    {
        string value;

        switch (valueType)
        {
            case ValueType.Number:
                value = Convert.ToString(numberValue);
                break;

            case ValueType.Boolean:
                value = Convert.ToString(booleanValue);
                break;

            case ValueType.Vector2D:
                value = Convert.ToString(vector2DValue);
                break;

            case ValueType.Vector3D:
                value = Convert.ToString(vector3DValue);
                break;

            default:
                value = "NULL";
                break;
        }

        return value;
    }

    public bool Compare(NodeValue other)
    {
        if (other.valueType != valueType) return false;

        return valueType switch
        {
            ValueType.Boolean => other.booleanValue == booleanValue,
            ValueType.Number => other.numberValue == numberValue,
            _ => false
        };
    }
}
