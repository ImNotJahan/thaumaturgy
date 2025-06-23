using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellcastingHandler : MonoBehaviour
{
    public string currentSpell;

    IEnumerator<string> cast;
    bool casting = false;
    public bool canCast = true;
    float timeSinceLastSyllable = 0f;

    [SerializeField]
    float syllableTime = 0.5f;

    [SerializeField]
    ThaumaturgicInterpreter thaumaturgicInterpreter;
    [SerializeField]
    GismosHandler gismosHandler;
    [SerializeField]
    TextMeshProUGUI spellText;
    Player player;

    InputAction castAction;

    void Start()
    {
        gismosHandler.spellTranspiled += SetSpell;
        castAction = InputSystem.actions.FindAction("Cast");
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (castAction.WasCompletedThisFrame() && canCast)
        {
            if (!casting)
            {
                cast = thaumaturgicInterpreter.InterpretFancily(currentSpell, player).GetEnumerator();
                casting = true;
            }
            else casting = false;
        }

        if (casting)
        {
            timeSinceLastSyllable += Time.deltaTime;

            if (timeSinceLastSyllable >= syllableTime)
            {
                timeSinceLastSyllable = 0f;

                bool hadNext = cast.MoveNext();
                spellText.text += cast.Current;

                if (cast.Current == "END" || !hadNext)
                {
                    spellText.text = "";
                    casting = false;
                }
            }
        }
    }

    void SetSpell(string spell)
    {
        currentSpell = spell;
    }
}
