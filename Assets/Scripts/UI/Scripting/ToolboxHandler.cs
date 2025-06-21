using System;
using UnityEngine;

public class ToolboxHandler : MonoBehaviour
{
    [SerializeField]
    GameObject gateFolder;
    [SerializeField]
    GameObject gismosHandler;
    [SerializeField]
    GameObjectArrayWrapper[] gateCollections;

    void Start()
    {
        foreach (GameObjectArrayWrapper gateCollection in gateCollections)
        {
            GameObject newGateFolder = Instantiate(gateFolder, transform);
            GateFolder gateFolderComponent = newGateFolder.GetComponent<GateFolder>();

            gateFolderComponent.gates = gateCollection.array;
            gateFolderComponent.gismosHandler = gismosHandler;
        }
    }
}

[Serializable]
public class GameObjectArrayWrapper
{
    public GameObject[] array;
}