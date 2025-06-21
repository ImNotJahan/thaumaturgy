using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GateFolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject contents;
    [SerializeField]
    public GameObject[] gates;
    [SerializeField]
    GameObject gateCreator;
    [SerializeField]
    public GameObject gismosHandler;

    public void OnPointerEnter(PointerEventData eventData)
    {
        contents.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        contents.SetActive(false);
    }

    void Start()
    {
        contents = transform.GetChild(0).gameObject;
        contents.SetActive(false);

        foreach (GameObject gate in gates)
        {
            GameObject newGateCreator = Instantiate(gateCreator, contents.transform);
            GateCreator gateCreatorComponent = newGateCreator.GetComponent<GateCreator>();

            gateCreatorComponent.gate = gate;
            gateCreatorComponent.gismosHandler = gismosHandler;

            newGateCreator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gate.name;
        }
    }

    
}
