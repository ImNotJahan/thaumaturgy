using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThaumaturgicInterpreter : MonoBehaviour
{
    public string blah = "";
    InputAction castAction;

    [SerializeField]
    TextMeshProUGUI displayText;

    Dictionary<string, (Delegate fn, int arguments)> codeToFn = new();

    public void Interpret(String spell)
    {
        string[] codes = spell.Split(' ');
        Stack stack = new Stack();

        for (int i = codes.Length - 1; i >= 0; i--)
        {
            if (codes[i] == "") continue;
            if (codes[i][0] == '>') // is constant value
            {
                stack.Push(parseConstant(codes[i]));
            }
            else
            {
                object[] arguments = new object[codeToFn[codes[i]].arguments];

                for (int j = 0; j < arguments.Length; j++)
                {
                    arguments[j] = stack.Pop();
                }

                stack.Push(codeToFn[codes[i]].fn.DynamicInvoke(arguments));
            }
        }
    }

    object parseConstant(string code)
    {
        code = code[1..];

        if (code == "True") return true;
        if (code == "False") return false;

        return null;
    }

    void Start()
    {
        codeToFn["and"] = (new Func<bool, bool, bool>(And), 2);
        codeToFn["or"] = (new Func<bool, bool, bool>(Or), 2);
        codeToFn["not"] = (new Func<bool, bool>(Not), 1);
        codeToFn["display"] = (new Func<object, object>(Display), 1);

        castAction = InputSystem.actions.FindAction("Cast");
    }

    void Update()
    {
        if (castAction.WasCompletedThisFrame()) Interpret(blah);
    }

    bool And(bool in1, bool in2)
    {
        return in1 && in2;
    }

    bool Or(bool in1, bool in2)
    {
        return in1 || in2;
    }

    bool Not(bool input)
    {
        return !input;
    }

    object Display(object input)
    {
        displayText.text = input.ToString();
        return null;
    }
}
