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
    public Node connectedNode; // only is set if this is an input node

    void Awake()
    {
        gismosHandler = transform.parent.parent.GetComponent<GismosHandler>();
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
}
