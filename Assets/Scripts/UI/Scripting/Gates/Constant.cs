using System;
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
}
