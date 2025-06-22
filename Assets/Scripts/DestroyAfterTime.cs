using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    float timePassed = 0;
    [SerializeField]
    float timeUntilDestruction = 5f;

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= timeUntilDestruction) Destroy(gameObject);
    }
}
