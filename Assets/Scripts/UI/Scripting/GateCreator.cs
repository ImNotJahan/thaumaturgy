using UnityEngine;
using UnityEngine.EventSystems;

public class GateCreator : MonoBehaviour, IPointerClickHandler
{
    public GameObject gate;
    public GameObject gismosHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject newGate = Instantiate(gate, gismosHandler.transform);
        newGate.GetComponent<Gate>().Place(gismosHandler.GetComponent<GismosHandler>());
    }
}
