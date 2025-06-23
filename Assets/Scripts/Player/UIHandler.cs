using Unity.VisualScripting;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject scriptingUI;
    private PlayerMovement playerMovement;
    private SpellcastingHandler spellcastingHandler;

    [SerializeField]
    GameObject crosshair;
    [SerializeField]
    GameObject healthBar;
    [SerializeField]
    GameObject manaBar;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spellcastingHandler = GetComponent<SpellcastingHandler>();

        scriptingUI.GetComponent<ScriptingUIHandler>().close += CloseScriptingUI;

        LockCursor();
        UnlockNonUI();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenScriptingUI()
    {
        UnlockCursor();
        scriptingUI.SetActive(true);
        LockNonUI();
    }

    void CloseScriptingUI()
    {
        LockCursor();
        scriptingUI.SetActive(false);
        UnlockNonUI();
    }

    void LockNonUI()
    {
        playerMovement.SetCanMove(false);
        crosshair.SetActive(false);
        manaBar.SetActive(false);
        healthBar.SetActive(false);
        spellcastingHandler.canCast = false;
    }

    void UnlockNonUI()
    {
        playerMovement.SetCanMove(true);
        crosshair.SetActive(true);
        manaBar.SetActive(true);
        healthBar.SetActive(true);
        spellcastingHandler.canCast = true;
    }
}
