using Unity.VisualScripting;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject scriptingUI;
    private PlayerMovement playerMovement;

    void Start()
    {
        lockCursor();
        playerMovement = GetComponent<PlayerMovement>();

        scriptingUI.GetComponent<ScriptingUIHandler>().close += CloseScriptingUI;
    }

    private void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
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
