using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class GismosHandler : MonoBehaviour
{
    public Wire wireBeingDrawn = null;
    public ScriptingUIHandler scriptingUIHandler;

    public List<Gate> thaumaturgicGates = new List<Gate>();

    public Action<String> spellTranspiled;

    public void TranspileGates()
    {
        StringBuilder transpiledCode = new StringBuilder();

        foreach (Gate thaumaturgicGate in thaumaturgicGates)
        {
            ExploreGates(thaumaturgicGate, transpiledCode);
        }

        Debug.Log(transpiledCode.ToString());
        spellTranspiled?.Invoke(transpiledCode.ToString());
    }

    void ExploreGates(Gate gate, StringBuilder stringBuilder)
    {
        foreach (Node input in gate.GetInputNodes())
        {
            ExploreGates(input.connectedNode.gate, stringBuilder);
        }

        stringBuilder.Append(gate.GetCode());
        stringBuilder.Append(" ");
    }
}
