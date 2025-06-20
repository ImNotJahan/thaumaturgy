using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScriptingUIHandler : MonoBehaviour
{
    [SerializeField]
    RectTransform gismosRectTransform;

    public UnityAction<Vector3> onDrag;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        gismosRectTransform.anchoredPosition += data.delta;
        onDrag?.Invoke(data.delta);
    }
}
