using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    protected Node[] inputNodes;
    [SerializeField]
    protected Node[] outputNodes;

    protected bool canExecute = false;

    void Start()
    {
        foreach (Node inputNode in inputNodes)
        {
            inputNode.onNodeValueChanged += UpdateOutput;
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
}
