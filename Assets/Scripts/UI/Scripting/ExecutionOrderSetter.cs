using System.Collections.Generic;
using UnityEngine;

public class ExecutionOrderSetter : MonoBehaviour
{
    [SerializeField]
    GismosHandler gismosHandler;
    List<Gate> thaumaturgicGates;

    [SerializeField]
    GameObject movableChild;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gismosHandler.thaumaturgicGatesChanged += OnThaumaturgicGatesChanged;
    }

    void OnThaumaturgicGatesChanged(List<Gate> gates)
    {
        thaumaturgicGates = gates;
        DrawOrder();
    }

    void DrawOrder()
    {
        RemoveChildren();

        foreach (Gate gate in thaumaturgicGates)
        {
            GameObject element = Instantiate(movableChild, transform);
            element.GetComponent<TextExposer>().SetText(gate.name);
            element.GetComponent<HierarchyMover>().positionChanged += OnElementPositionChange;
        }
    }

    void RemoveChildren() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnElementPositionChange(int originalIndex, int movement) {
        // move gate to match position of elements
        Gate gate = thaumaturgicGates[originalIndex];
        thaumaturgicGates.RemoveAt(originalIndex);
        thaumaturgicGates.Insert(originalIndex + movement, gate);
    }
}
