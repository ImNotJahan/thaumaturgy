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

    public Node valueProvidingNode;
    public Node valueRecievingNode;

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

            Vector2 end = pointAction.ReadValue<Vector2>();
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
            valueProvidingNode.onNodeValueChanged += UpdateValueRecievingNode;

            drawing = false;
            gismosHandler.wireBeingDrawn = null;
            image.raycastTarget = true;
        }
    }

    void UpdateValueRecievingNode()
    {
        valueRecievingNode.SetNodeValue(valueProvidingNode.GetNodeValue());
    }

    public void OnClick(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        if (data.button == PointerEventData.InputButton.Right)
        {
            valueRecievingNode.SetNodeValue(new NodeValue());
            Destroy(gameObject);
        }
    }
}
