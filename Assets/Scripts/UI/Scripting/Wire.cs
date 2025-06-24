using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    RawImage image;
    RectTransform rectTransform;
    public GismosHandler gismosHandler;

    InputAction pointAction;
    InputAction clickAction; // will be used for drawing chained wires in the future
    InputAction rightClickAction;

    Vector2 origin;
    Vector2 end;

    public Node valueProvidingNode;
    public Node valueRecievingNode;

    bool originIsRecieving;

    Gate originGate;
    Gate endGate;

    bool drawing = true;

    void Awake()
    {
        image = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        pointAction = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
        rightClickAction = InputSystem.actions.FindAction("RightClick");
    }

    void Update()
    {
        if (drawing)
        {
            // stop drawing line on right click
            if (rightClickAction.IsPressed())
            {
                gismosHandler.wireBeingDrawn = null;
                Destroy(gameObject);
            }

            end = pointAction.ReadValue<Vector2>();
            DrawLine(origin, end);

            //if (clickAction.IsPressed()) drawing = false;
        }
    }

    private void DrawLine(Vector2 origin, Vector2 end)
    {
        Vector2 midpoint = (origin + end) / 2f;

        rectTransform.position = midpoint;

        Vector2 direction = end - origin;
        rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        rectTransform.localScale = new Vector3(direction.magnitude, 1f, 1f) / gismosHandler.canvasScale;
    }

    public void BeginDrawing(RectTransform origin, Node originNode, GismosHandler gismosHandler)
    {
        BeginDrawing(origin.position, originNode, gismosHandler);
    }

    public void BeginDrawing(Vector2 origin, Node originNode, GismosHandler gismosHandler)
    {
        if (originNode.nodeType == Node.NodeType.Output)
        {
            valueProvidingNode = originNode;
            originIsRecieving = false;
        }
        else
        {
            valueRecievingNode = originNode;
            originIsRecieving = true;
        }

        this.origin = origin;
        this.gismosHandler = gismosHandler;

        originGate = originNode.gate;
    }

    public void EndDrawing(Node endNode)
    {
        // stop drawing if the wire is connecting to a valid node type (input -> output or output -> input)
        if (endNode.nodeType == Node.NodeType.Input && valueProvidingNode != null && endNode.GetNodeValue().IsNull() ||
            endNode.nodeType == Node.NodeType.Output && valueProvidingNode == null)
        {
            if (endNode.nodeType == Node.NodeType.Input) valueRecievingNode = endNode;
            else valueProvidingNode = endNode;

            UpdateValueRecievingNode();
            valueRecievingNode.connectedNode = valueProvidingNode;
            valueProvidingNode.onNodeValueChanged += UpdateValueRecievingNode;

            drawing = false;
            gismosHandler.wireBeingDrawn = null;
            image.raycastTarget = true;

            endNode.gate.onDrag += OnEndDrag;
            gismosHandler.scriptingUIHandler.onDrag += OnBackgroundDrag;

            endNode.gate.onDestroy += DestroyWire;

            originGate.onDrag += OnOriginDrag;
            originGate.onDestroy += DestroyWire;

            endGate = endNode.gate;

            gismosHandler.wires.Add(this);

            valueProvidingNode.ConnectWire();
            valueRecievingNode.ConnectWire();
        }
    }

    void UpdateValueRecievingNode()
    {
        valueRecievingNode.SetNodeValue(valueProvidingNode.GetNodeValue());
    }

    public void OnClick(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        if (data.button == PointerEventData.InputButton.Right) DestroyWire();
    }

    void DestroyWire()
    {
        // make sure to unsubscribe from drag events to avoid null pointer errors
        gismosHandler.scriptingUIHandler.onDrag -= OnBackgroundDrag;
        originGate.onDrag -= OnOriginDrag;
        originGate.onDestroy -= DestroyWire;
        endGate.onDrag -= OnEndDrag;
        endGate.onDestroy -= DestroyWire;

        valueRecievingNode.SetNodeValue(new NodeValue());
        valueRecievingNode.connectedNode = null;

        valueRecievingNode.DisconnectWire();
        valueProvidingNode.DisconnectWire();

        gismosHandler.wires.Remove(this);

        Destroy(gameObject);
    }

    void OnOriginDrag(Vector3 delta)
    {
        Vector2 drag = new(delta.x, delta.y);
        origin += drag;

        DrawLine(origin, end);
    }

    void OnEndDrag(Vector3 delta)
    {
        Vector2 drag = new(delta.x, delta.y);
        end += drag;

        DrawLine(origin, end);
    }

    void OnBackgroundDrag(Vector3 delta)
    {
        Vector2 drag = new(delta.x, delta.y);

        origin += drag;
        end += drag;
    }

    // convert the wire into a json string for saving
    public WireData Serialize()
    {
        WireData data = new();

        data.origin_x = origin.x;
        data.origin_y = origin.y;
        data.end_x = end.x;
        data.end_y = end.y;
        data.origin_gate = originGate.id;
        data.end_gate = endGate.id;

        if (originIsRecieving)
        {
            data.origin_node = valueRecievingNode.GetId();
            data.end_node = valueProvidingNode.GetId();
        }
        else
        {
            data.origin_node = valueProvidingNode.GetId();
            data.end_node = valueRecievingNode.GetId();
        }

        return data;
    }

    public void Deserialize(WireData data)
    {
        origin = new Vector2(data.origin_x, data.origin_y);
        end = new Vector2(data.end_x, data.end_y);

        BeginDrawing(origin, gismosHandler.GetGate(data.origin_gate).GetNode(data.origin_node), gismosHandler);
        DrawLine(origin, end);
        EndDrawing(gismosHandler.GetGate(data.end_gate).GetNode(data.end_node));
    }
}

public class WireData
{
    public float origin_x;
    public float origin_y;
    public float end_x;
    public float end_y;
    public int origin_gate;
    public int end_gate;
    public int origin_node;
    public int end_node;
}