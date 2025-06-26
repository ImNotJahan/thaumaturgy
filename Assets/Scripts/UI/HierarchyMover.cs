using System;
using UnityEngine;
using UnityEngine.Events;

public class HierarchyMover : MonoBehaviour
{
    public UnityAction<int, int> positionChanged;

    public void MoveUp()
    {
        int index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(Math.Max(0, index - 1));

        positionChanged?.Invoke(index, -1);
    }

    public void MoveDown()
    {
        int index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(Math.Min(transform.hierarchyCount - 1, index + 1));

        positionChanged?.Invoke(index, +1);
    }
}
