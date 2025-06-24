using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public enum NodeType { Input, Output, Disabled };
    public NodeType nodeType = NodeType.Disabled;

    [SerializeField]
    GameObject wire;
    GismosHandler gismosHandler;

    [SerializeField]
    NodeValue nodeValue = new NodeValue();
    public UnityAction onNodeValueChanged;

    public Gate gate;
    public Node connectedNode; // used for input nodes, as each input only connects to one output
    public List<Node> connectedNodes = new List<Node>(); // used for output nodes, as an output can have multiple inputs
    int connectedWires = 0;

    [SerializeField]
    TextMeshProUGUI inputText;
    [SerializeField]
    TextMeshProUGUI outputText;

    public int id;

    void Awake()
    {
        gismosHandler = transform.parent.parent.GetComponent<GismosHandler>();

        CheckText();
    }

    void CheckText()
    {
        if (connectedWires == 0)
        {
            if (nodeType == NodeType.Input)
            {
                outputText.enabled = false;
                inputText.enabled = true;
                inputText.text = gameObject.name;

                return;
            }
            else if (nodeType == NodeType.Output)
            {
                outputText.enabled = true;
                inputText.enabled = false;
                outputText.text = gameObject.name;

                return;
            }
        }

        outputText.enabled = false;
        inputText.enabled = false;
    }

    public void OnClick()
    {
        if (gismosHandler.wireBeingDrawn == null)
        {
            GameObject newWire = Instantiate(wire, transform.parent.parent);
            Wire wireComponent = newWire.GetComponent<Wire>();
            wireComponent.BeginDrawing(GetComponent<RectTransform>(), this, gismosHandler);
            gismosHandler.wireBeingDrawn = wireComponent;
        }
        else
        {
            gismosHandler.wireBeingDrawn.EndDrawing(this);
        }
    }

    public void SetNodeValue(NodeValue value)
    {
        nodeValue = value;
        onNodeValueChanged?.Invoke();
    }

    public NodeValue GetNodeValue()
    {
        return nodeValue;
    }

    public int GetId()
    {
        return id;
    }

    public void ConnectWire()
    {
        connectedWires++;

        CheckText();
    }

    public void DisconnectWire()
    {
        connectedWires--;

        CheckText();
    }
}
