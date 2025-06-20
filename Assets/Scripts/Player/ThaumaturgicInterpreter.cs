using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThaumaturgicInterpreter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayText;
    [SerializeField]
    Transform cameraTransform;

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
                // because we are reading the spell backwards, we need to reverse arguments for them to be in the right order
                Array.Reverse(arguments);

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

        if (int.TryParse(code, out int result)) return result;

        return null;
    }

    void Start()
    {
        codeToFn["and"] = (new Func<bool, bool, bool>(And), 2);
        codeToFn["or"] = (new Func<bool, bool, bool>(Or), 2);
        codeToFn["not"] = (new Func<bool, bool>(Not), 1);
        codeToFn["display"] = (new Func<object, object>(Display), 1);
        codeToFn["add_force"] = (new Func<Transform, Vector3, object>(AddForce), 2);
        codeToFn["player_facing"] = (new Func<Vector3>(PlayerFacing), 0);
        codeToFn["player_position"] = (new Func<Vector3>(PlayerPosition), 0);
        codeToFn["vector3"] = (new Func<int, int, int, Vector3>(MakeVector3), 3);
        codeToFn["ray"] = (new Func<Vector3, Vector3, Ray>(MakeRay), 2);
        codeToFn["object_raycast"] = (new Func<Ray, int, object>(ObjectRaycast), 2);
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

    Vector3 PlayerFacing()
    {
        return cameraTransform.forward;
    }

    Vector3 PlayerPosition()
    {
        return cameraTransform.position;
    }

    Transform ObjectRaycast(Ray ray, int maxDistance)
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red, maxDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            return hit.transform;
        }

        Debug.LogError("Raycast did not hit anything");
        return null;
    }

    object AddForce(Transform obj, Vector3 force)
    {
        obj.GetComponent<Rigidbody>().AddForce(force);
        return null;
    }

    Vector3 MakeVector3(int x, int y, int z)
    {
        return new Vector3(x, y, z);
    }

    Ray MakeRay(Vector3 position, Vector3 rotation)
    {
        return new Ray(position, rotation);
    }
}
