using System;
using System.Collections.Generic;
using UnityEngine;

public class Constant : Gate
{
    [SerializeField]
    protected NodeValue value;

    public override string GetCode()
    {
        return ">" + value.ToString();
    }

    void Start()
    {
        outputNodes[0].SetNodeValue(value);
    }

    public override GateData Serialize()
    {
        GateData data = base.Serialize();

        switch (value.valueType)
        {
            case NodeValue.ValueType.Boolean:
                if (value.booleanValue) data.gate_code = "true";
                else data.gate_code = "false";
                break;

            case NodeValue.ValueType.Number:
                data.gate_code = "constant_number" + value.numberValue;
                break;
        }

        return data;
    }

    public override void Deserialize(GateData data)
    {
        base.Deserialize(data);

        switch (data.gate_code)
        {
            case "constant_number":
                value.numberValue = 0;
                break;
        }
    }
}