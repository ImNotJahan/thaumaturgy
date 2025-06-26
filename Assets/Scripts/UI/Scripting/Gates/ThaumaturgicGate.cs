// thaumaturgic gates are gates which cause change in the world
// they have no output nodes and are the final gates in a spell

using UnityEngine;

public abstract class ThaumaturgicGate : Gate
{
    void Start()
    {
        if (gismosHandler != null)
        {
            gismosHandler.AddThaumaturgicGate(this);

            onDestroy += () => gismosHandler.RemoveThaumaturgicGate(this);
        }
        else
        {
            Debug.LogWarning("Thaumaturgic gate has no gismosHandler set");
        }
    }
}
