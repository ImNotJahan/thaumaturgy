using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ScriptingUIHandler : MonoBehaviour
{
    [SerializeField]
    GismosHandler gismosHandler;
    RectTransform gismosRectTransform;

    public UnityAction<Vector3> onDrag;

    InputAction exitAction;

    public UnityAction close;

    [SerializeField]
    Canvas canvas;

    void Start()
    {
        exitAction = InputSystem.actions.FindAction("Exit");

        gismosHandler.canvasScale = canvas.scaleFactor;
        gismosRectTransform = gismosHandler.GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        gismosRectTransform.anchoredPosition += data.delta / canvas.scaleFactor;
        onDrag?.Invoke(data.delta);
    }

    void Update()
    {
        if (exitAction.IsPressed())
        {
            close?.Invoke();
        }
    }
}
