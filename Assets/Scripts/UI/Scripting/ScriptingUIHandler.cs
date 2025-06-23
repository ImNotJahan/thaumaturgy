using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScriptingUIHandler : MonoBehaviour
{
    [SerializeField]
    GismosHandler gismosHandler;
    RectTransform gismosRectTransform;
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    float offsetScaleFactor = 1;

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

        Vector2 offset = new Vector2(-gismosRectTransform.position.x / Screen.width, -gismosRectTransform.position.y / Screen.height);

        backgroundImage.material.SetVector("_Offset", offset * offsetScaleFactor);
    }

    void Update()
    {
        if (exitAction.IsPressed())
        {
            close?.Invoke();
        }
    }
}
