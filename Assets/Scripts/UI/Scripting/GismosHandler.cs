using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GismosHandler : MonoBehaviour
{
    public Wire wireBeingDrawn = null;
    public ScriptingUIHandler scriptingUIHandler;

    public List<Gate> thaumaturgicGates = new List<Gate>();

    public void TranspileGates()
    {
        StringBuilder transpiledCode = new StringBuilder();

        foreach (Gate thaumaturgicGate in thaumaturgicGates)
        {
            ExploreGates(thaumaturgicGate, transpiledCode);
        }

        Debug.Log(transpiledCode.ToString());
    }

    void ExploreGates(Gate gate, StringBuilder stringBuilder)
    {
        stringBuilder.Append(gate.GetCode());
        stringBuilder.Append(" ");

        foreach (Node input in gate.GetInputNodes())
        {
            ExploreGates(input.connectedNode.gate, stringBuilder);
        }
    }
}
