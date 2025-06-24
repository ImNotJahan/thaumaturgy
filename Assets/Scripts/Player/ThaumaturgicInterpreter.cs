using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThaumaturgicInterpreter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayText;
    [SerializeField]
    Transform cameraTransform;
    Player player;
    [SerializeField]
    SpellParticlesHandler spellParticlesHandler;

    readonly Dictionary<string, (Delegate fn, int arguments)> codeToFn = new();
    readonly Dictionary<string, string> syllables = new()
    {
        {"and", "en"}, {"or", "in"}, {"not", "ta"}, {"display", "tt"},
        { "add_force", "ll"}, {"player_facing", "is"}, {"player_position", "ka"}, {"vector3", "li"},
        { "ray", "it"}, {"object_raycast", "an"}, {"vector_add", "ol"}, {"player_object", "ja"},
        {"x_component", "ai"}, {"y_component", "et"}, {"z_component", "st"}, {"dot_product", "va"},
        {"scalar_multiply", "te"}, {"add", "se"}, {"multiply", "ne"}, {"switch", "on"},
    };

    // Interprets over time + charges mana
    public IEnumerable<string> InterpretFancily(string spell, Player player)
    {
        this.player = player;

        string[] codes = spell.Split(' ');
        Stack stack = new Stack();

        for (int i = 0; i < codes.Length; i++)
        {
            if (codes[i] == "") continue;
            if (codes[i][0] == '>') // is constant value
            {
                stack.Push(ParseConstant(codes[i]));
                yield return " ";
            }
            else
            {
                object[] arguments = new object[codeToFn[codes[i]].arguments];

                for (int j = arguments.Length - 1; j >= 0; j--)
                {
                    arguments[j] = stack.Pop();
                }

                try
                {
                    stack.Push(codeToFn[codes[i]].fn.DynamicInvoke(arguments));
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                    break;
                }

                yield return syllables[codes[i]];
            }
        }

        yield return "END";
    }

    object ParseConstant(string code)
    {
        code = code[1..];

        if (code == "True") return true;
        if (code == "False") return false;

        if (float.TryParse(code, out float result)) return result;

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
        codeToFn["vector3"] = (new Func<float, float, float, Vector3>(MakeVector3), 3);
        codeToFn["ray"] = (new Func<Vector3, Vector3, Ray>(MakeRay), 2);
        codeToFn["object_raycast"] = (new Func<Ray, float, object>(ObjectRaycast), 2);
        codeToFn["x_component"] = (new Func<Vector3, float>(XComponent), 1);
        codeToFn["y_component"] = (new Func<Vector3, float>(YComponent), 1);
        codeToFn["z_component"] = (new Func<Vector3, float>(ZComponent), 1);
        codeToFn["player_object"] = (new Func<Transform>(PlayerObject), 0);
        codeToFn["vector_add"] = (new Func<Vector3, Vector3, Vector3>(VectorAdd), 2);
        codeToFn["dot_product"] = (new Func<Vector3, Vector3, float>(DotProduct), 2);
        codeToFn["scalar_multiply"] = (new Func<Vector3, float, Vector3>(ScalarMultiply), 2);
        codeToFn["add"] = (new Func<float, float, float>(Add), 2);
        codeToFn["multiply"] = (new Func<float, float, float>(Multiply), 2);
        codeToFn["switch"] = (new Func<object, object, bool, object>(Switch), 3);
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

    Transform ObjectRaycast(Ray ray, float maxDistance)
    {
        spellParticlesHandler.DrawRay(ray, maxDistance);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            return hit.transform;
        }

        Debug.LogWarning("Raycast did not hit anything");
        return null;
    }

    object AddForce(Transform obj, Vector3 force)
    {
        obj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        player.UseMana(Math.Max((int)Mathf.Pow(Math.Max(force.magnitude - 100, 2), 0), 100));
        spellParticlesHandler.CastSparkles();

        return null;
    }

    Vector3 MakeVector3(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    Ray MakeRay(Vector3 position, Vector3 rotation)
    {
        return new Ray(position, rotation);
    }

    Vector3 ScalarMultiply(Vector3 vector, float scalar)
    {
        return vector * scalar;
    }

    float DotProduct(Vector3 vector1, Vector3 vector2)
    {
        return Vector3.Dot(vector1, vector2);
    }

    float XComponent(Vector3 input)
    {
        return input.x;
    }

    float YComponent(Vector3 input)
    {
        return input.y;
    }

    float ZComponent(Vector3 input)
    {
        return input.z;
    }

    Vector3 VectorAdd(Vector3 vector1, Vector3 vector2)
    {
        return vector1 + vector2;
    }

    Transform PlayerObject()
    {
        return player.transform;
    }

    float Add(float input1, float input2)
    {
        return input1 + input2;
    }

    float Multiply(float input1, float input2)
    {
        return input1 * input2;
    }

    object Switch(object input1, object input2, bool input3)
    {
        if (input3) return input2;
        return input1;
    }
}
