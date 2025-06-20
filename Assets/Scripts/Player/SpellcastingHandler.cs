using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellcastingHandler : MonoBehaviour
{
    public String currentSpell;

    [SerializeField]
    ThaumaturgicInterpreter thaumaturgicInterpreter;
    [SerializeField]
    GismosHandler gismosHandler;

    InputAction castAction;

    void Start()
    {
        gismosHandler.spellTranspiled += SetSpell;
        castAction = InputSystem.actions.FindAction("Cast");
    }

    void Update()
    {
        if (castAction.WasCompletedThisFrame())
            thaumaturgicInterpreter.Interpret(currentSpell);
    }

    void SetSpell(string spell)
    {
        currentSpell = spell;
    }
}
