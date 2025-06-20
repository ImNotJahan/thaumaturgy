using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    RawImage image;
    RectTransform rectTransform;
    GismosHandler gismosHandler;

    InputAction pointAction;
    InputAction clickAction; // will be used for drawing chained wires in the future
    InputAction rightClickAction;

    Vector2 origin;
    Vector2 end;

    public Node valueProvidingNode;
    public Node valueRecievingNode;

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
        rectTransform.localScale = new Vector3(direction.magnitude, 1f, 1f);
    }

    public void BeginDrawing(RectTransform origin, Node originNode, GismosHandler gismosHandler)
    {
        if (originNode.nodeType == Node.NodeType.Output)
            valueProvidingNode = originNode;
        else
            valueRecievingNode = originNode;

        this.origin = origin.position;
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
}
