using Unity.VisualScripting;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject scriptingUI;
    private PlayerMovement playerMovement;
    [SerializeField]
    GameObject crosshair;

    void Start()
    {
        lockCursor();
        playerMovement = GetComponent<PlayerMovement>();

        scriptingUI.GetComponent<ScriptingUIHandler>().close += CloseScriptingUI;
    }

    private void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);
    }

    private void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        crosshair.SetActive(false);
    }

    public void OpenScriptingUI()
    {
        unlockCursor();
        scriptingUI.SetActive(true);
        playerMovement.SetCanMove(false);
    }

    void CloseScriptingUI()
    {
        lockCursor();
        scriptingUI.SetActive(false);
        playerMovement.SetCanMove(true);
    }
}
