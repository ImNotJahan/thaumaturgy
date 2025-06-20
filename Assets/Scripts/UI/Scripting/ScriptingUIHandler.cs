using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ScriptingUIHandler : MonoBehaviour
{
    [SerializeField]
    RectTransform gismosRectTransform;

    public UnityAction<Vector3> onDrag;

    InputAction exitAction;

    public UnityAction close;

    void Start()
    {
        exitAction = InputSystem.actions.FindAction("Exit");

        gameObject.SetActive(false);
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        gismosRectTransform.anchoredPosition += data.delta;
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
