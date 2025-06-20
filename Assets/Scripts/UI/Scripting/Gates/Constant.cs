using UnityEngine;

public class Constant : Gate
{
    [SerializeField]
    Node outputNode;
    [SerializeField]
    NodeValue value;

    void Start()
    {
        outputNode.SetNodeValue(value);
    }
}
