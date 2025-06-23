using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class GismosHandler : MonoBehaviour
{
    public Wire wireBeingDrawn = null;
    public ScriptingUIHandler scriptingUIHandler;

    [SerializeField]
    TMP_InputField spellNameInputField;

    public float canvasScale = 1;

    public List<Gate> thaumaturgicGates = new();
    Dictionary<int, Gate> gates = new();
    public List<Wire> wires = new();

    [SerializeField]
    CodeGatePair[] codeGatePairs;
    [SerializeField]
    GameObject wireGameObject;
    Dictionary<string, GameObject> codeToGateObject = new();

    void Start()
    {
        foreach (CodeGatePair pair in codeGatePairs)
        {
            codeToGateObject[pair.code] = pair.gate;
        }
    }

    // can be greater than actual number of gates, just used to make sure each gate has unique ID
    int gateCount = 0;

    public Action<string> spellTranspiled;

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

    public void SaveSpellDesign()
    {
        SpellDesignObject data = new();
        data.gateDatas = new GateData[gates.Count];
        data.wireDatas = new WireData[wires.Count];

        for (int i = 0; i < gates.Count; i++)
        {
            data.gateDatas[i] = gates[i].Serialize();
        }

        for (int i = 0; i < wires.Count; i++)
        {
            data.wireDatas[i] = wires[i].Serialize();
        }
        
        JsonDataService.SaveData(data, "spell_designs", spellNameInputField.text + ".json");
    }

    public void LoadSpellDesign(string fileName)
    {
        ClearWorkspace();

        SpellDesignObject data = JsonDataService.LoadData<SpellDesignObject>("spell_designs", fileName + ".json");

        foreach (GateData gateData in data.gateDatas)
        {
            GameObject newGate = Instantiate(GetGateObjectFromCode(gateData.gate_code), transform);
            Gate gateComponent = newGate.GetComponent<Gate>();

            gateComponent.gismosHandler = this;
            gateComponent.Deserialize(gateData);

            gates.Add(gateData.id, gateComponent);
        }

        foreach (WireData wireData in data.wireDatas)
        {
            GameObject newWire = Instantiate(wireGameObject, transform);
            Wire wireComponent = newWire.GetComponent<Wire>();

            wireComponent.gismosHandler = this;
            wireComponent.Deserialize(wireData);
        }
    }

    public void AddGate(Gate gate)
    {
        gate.id = gateCount;
        gates[gateCount] = gate;
        gateCount++;
    }

    public Gate GetGate(int id)
    {
        return gates[id];
    }

    public void RemoveGate(int id)
    {
        gates.Remove(id);
    }

    public void ClearWorkspace()
    {
        gates = new();
        wires = new();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    GameObject GetGateObjectFromCode(string code)
    {
        return codeToGateObject[code];
    }
}

[Serializable]
class CodeGatePair
{
    public string code;
    public GameObject gate;
}

class SpellDesignObject
{
    public WireData[] wireDatas;
    public GateData[] gateDatas;
}