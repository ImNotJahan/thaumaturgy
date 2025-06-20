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
}
