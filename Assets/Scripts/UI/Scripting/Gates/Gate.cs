using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Gate : MonoBehaviour, IDragHandler
{
    [SerializeField]
    protected Node[] inputNodes;
    [SerializeField]
    protected Node[] outputNodes;

    protected bool canExecute = false;

    RectTransform rectTransform;

    public UnityAction<Vector3> onDrag;

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
}
