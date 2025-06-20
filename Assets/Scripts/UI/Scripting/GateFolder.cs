using UnityEngine;
using UnityEngine.EventSystems;

public class GateFolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject contents;

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
    }

    
}
