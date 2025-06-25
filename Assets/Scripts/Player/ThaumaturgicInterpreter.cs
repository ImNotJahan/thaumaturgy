using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        {"save", "el"}, {"load", "oi"}, {"call", "le"}, {"define", "al"},
        {"equals", "ti"}, {"less_than", "ut"},
    };

    readonly Dictionary<float, object> variables = new();
    readonly Dictionary<float, List<string>> functions = new();
    
    enum InterpretState {Succeeded, Errored, Passed, FunctionStart, FunctionCall};

    // Interprets over time + charges mana
    public IEnumerable<string> InterpretFancily(string spell, Player player)
    {
        this.player = player;

        List<string> codes = spell.Split(' ').ToList();
        Stack stack = new Stack();

        for (int i = 0; i < codes.Count; i++)
        {
            (string syllable, InterpretState state) = InterpretCode(codes[i], stack);

            if (state == InterpretState.Passed) continue;
            if (state == InterpretState.Errored) break;
            if (state == InterpretState.Succeeded) yield return syllable;
            if (state == InterpretState.FunctionStart)
            {
                List<string> func = new();

                while (i + 1 < codes.Count)
                {
                    i++;

                    if (codes[i] == "define") break;
                    func.Add(codes[i]);
                }

                functions[(float)ParseConstant(func[func.Count - 1])] = func;
                func.RemoveAt(func.Count - 1);

                yield return syllables["define"];
            }
            if (state == InterpretState.FunctionCall)
            {
                codes.InsertRange(i + 1, functions[(float)stack.Pop()]);
                yield return syllables["call"];
            }
        }

        yield return "END";
    }

    (string, InterpretState) InterpretCode(string code, Stack stack)
    {
        if (code == "") return ("", InterpretState.Passed);
        if (code[0] == '>') // is constant value
        {
            stack.Push(ParseConstant(code));
            return (" ", InterpretState.Succeeded);
        }
        else if (code == "start_func")
        {
            return ("", InterpretState.FunctionStart);
        }
        else if (code == "call")
        {
            return ("", InterpretState.FunctionCall);
        }
        else
        {
            object[] arguments = new object[codeToFn[code].arguments];

            for (int j = arguments.Length - 1; j >= 0; j--)
            {
                arguments[j] = stack.Pop();
            }

            try
            {
                stack.Push(codeToFn[code].fn.DynamicInvoke(arguments));
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
                return ("", InterpretState.Errored);
            }

            return (syllables[code], InterpretState.Succeeded);
        }
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
        codeToFn["equals"] = (new Func<object, object, bool>(AreEqual), 2);
        codeToFn["less_than"] = (new Func<float, float, bool>(LessThan), 2);
        codeToFn["save"] = (new Func<object, float, object>(Save), 2);
        codeToFn["load"] = (new Func<float, object>(Load), 1);
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

    bool AreEqual(object input1, object input2)
    {
        return input1 == input2;
    }

    bool LessThan(float input1, float input2)
    {
        return input1 < input2;
    }

    object Save(object value, float id)
    {
        variables[id] = value;

        return null;
    }

    object Load(float id)
    {
        return variables[id];
    }
}
