using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Gate : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    protected Node[] inputNodes;
    [SerializeField]
    protected Node[] outputNodes;

    protected bool canExecute = false;

    RectTransform rectTransform;

    public UnityAction<Vector3> onDrag;
    public UnityAction onDestroy;

    GismosHandler gismosHandler;
    bool placing = false;

    InputAction pointAction;
    InputAction clickAction; // will be used for drawing chained wires in the future

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        foreach (Node inputNode in inputNodes)
        {
            inputNode.onNodeValueChanged += UpdateOutput;
            inputNode.gate = this;
        }

        foreach (Node outputNode in outputNodes)
        {
            outputNode.gate = this;
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
        rectTransform.anchoredPosition += eventData.delta;
        onDrag?.Invoke(eventData.delta); // only invoke if onDrag isn't null
    }

    public void Place(GismosHandler gismosHandler)
    {
        this.gismosHandler = gismosHandler;
        placing = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}
