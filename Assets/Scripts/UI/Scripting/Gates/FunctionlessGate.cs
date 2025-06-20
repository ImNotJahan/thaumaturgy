using UnityEngine;

public class FunctionlessGate : Gate
{
    [SerializeField]
    string code;

    public override string GetCode()
    {
        return code;
    }
}
