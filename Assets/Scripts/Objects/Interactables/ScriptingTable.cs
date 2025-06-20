using UnityEngine;

public class ScriptingTable : Interactable
{
    public override void Interact(GameObject player)
    {
        player.GetComponent<UIHandler>().OpenScriptingUI();
    }
}
