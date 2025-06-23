using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class Gate : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    protected Node[] inputNodes;
    [SerializeField]
    protected Node[] outputNodes;

    protected bool canExecute = false;

    RectTransform rectTransform;

    public UnityAction<Vector3> onDrag;
    public UnityAction onDestroy;

    public GismosHandler gismosHandler;
    bool placing = false;

    InputAction pointAction;
    InputAction clickAction;

    public int id; // used during serialization

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        int nodeCounter = 0;

        foreach (Node inputNode in inputNodes)
        {
            inputNode.onNodeValueChanged += UpdateOutput;
            inputNode.gate = this;

            inputNode.id = nodeCounter;
            nodeCounter++;
        }

        foreach (Node outputNode in outputNodes)
        {
            outputNode.gate = this;

            outputNode.id = nodeCounter;
            nodeCounter++;
        }

        pointAction = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
    }

    void Update()
    {
        if (placing)
        {
            transform.position = pointAction.ReadValue<Vector2>();

            if (clickAction.IsPressed())
            {
                placing = false;
            }
        }
    }

    protected virtual void UpdateOutput()
    {
        // verify all inputs are nonnull
        foreach (Node inputNode in inputNodes)
        {
            if (inputNode.GetNodeValue() == null)
            {
                canExecute = false;
                NullifyOutputs();
                return;
            }
        }

        canExecute = true;
    }

    protected void NullifyOutputs()
    {
        foreach (Node outputNode in outputNodes)
        {
            outputNode.SetNodeValue(new NodeValue());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // the scale of the canvas does not affect the drag delta, so we need to divide it by the canvas scale so that it looks like it matches
        float canvasScale = gismosHandler != null ? gismosHandler.canvasScale : 1;

        rectTransform.anchoredPosition += eventData.delta / canvasScale;
        onDrag?.Invoke(eventData.delta); // only invoke if onDrag isn't null
    }

    public void Place(GismosHandler gismosHandler)
    {
        this.gismosHandler = gismosHandler;
        gismosHandler.AddGate(this);
        placing = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onDestroy?.Invoke();
            gismosHandler.RemoveGate(id);
            Destroy(gameObject);
        }
    }

    public abstract string GetCode();

    public Node[] GetInputNodes()
    {
        return inputNodes;
    }

    public virtual GateData Serialize()
    {
        GateData data = new();

        data.position_x = rectTransform.position.x;
        data.position_y = rectTransform.position.y;
        data.gate_code = GetCode();
        data.id = id;

        return data;
    }

    public virtual void Deserialize(GateData data)
    {
        rectTransform.position = new Vector2(data.position_x, data.position_y);
        id = data.id;
    }

    public Node GetNode(int id)
    {
        if (id < inputNodes.Length) return inputNodes[id];
        return outputNodes[id - inputNodes.Length];
    }
}

public class GateData
{
    public float position_x;
    public float position_y;
    public string gate_code;
    public int id;
}
