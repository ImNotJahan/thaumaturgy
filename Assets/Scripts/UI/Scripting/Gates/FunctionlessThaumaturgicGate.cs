using UnityEngine;

public class FunctionlessThaumaturgicGate : ThaumaturgicGate
{
    [SerializeField]
    string code;

    public override string GetCode()
    {
        return code;
    }
}
