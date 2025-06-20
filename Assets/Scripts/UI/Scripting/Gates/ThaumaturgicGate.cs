// thaumaturgic gates are gates which cause change in the world
// they have no output nodes and are the final gates in a spell
public abstract class ThaumaturgicGate : Gate
{
    void Start()
    {
        if (gismosHandler != null)
        {
            gismosHandler.thaumaturgicGates.Add(this);

            onDestroy += () => gismosHandler.thaumaturgicGates.Remove(this);
        }
    }
}
