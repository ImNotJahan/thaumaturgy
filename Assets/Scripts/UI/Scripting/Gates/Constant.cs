using UnityEngine;

public class Constant : Gate
{
    [SerializeField]
    NodeValue value;

    void Start()
    {
        outputNodes[0].SetNodeValue(value);
    }
}
