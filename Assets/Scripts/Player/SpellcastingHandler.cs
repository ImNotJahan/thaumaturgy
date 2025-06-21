using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellcastingHandler : MonoBehaviour
{
    public String currentSpell;

    IEnumerator<String> cast;
    bool casting = false;
    float timeSinceLastSyllable = 0f;

    [SerializeField]
    float syllableTime = 0.5f;

    [SerializeField]
    ThaumaturgicInterpreter thaumaturgicInterpreter;
    [SerializeField]
    GismosHandler gismosHandler;
    [SerializeField]
    TextMeshProUGUI spellText;

    InputAction castAction;

    void Start()
    {
        gismosHandler.spellTranspiled += SetSpell;
        castAction = InputSystem.actions.FindAction("Cast");
    }

    void Update()
    {
        if (castAction.WasCompletedThisFrame())
        {
            if (!casting)
            {
                cast = thaumaturgicInterpreter.InterpretOverTime(currentSpell).GetEnumerator();
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
