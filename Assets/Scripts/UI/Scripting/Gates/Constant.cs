using UnityEngine;

public class Constant : Gate
{
    [SerializeField]
    NodeValue value;

    void Start()
    {
        outputNodes[0].SetNodeValue(value);

        // no need to call base here, as value of a constant never updates
    }
}
