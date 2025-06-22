using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpellParticlesHandler : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    ParticleSystem purpleSparkles;

    public void DrawRay(Ray ray, float length)
    {
        // we move the line origin slightly down for visiblity in cases where the line has the position and facing of the player
        StartCoroutine(DrawLine(ray.origin + Vector3.down * 0.1f, ray.origin + ray.direction.normalized * length, 0.5f));
    }

    public IEnumerator DrawLine(Vector3 start, Vector3 end, float forTime)
    {
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        yield return new WaitForSeconds(forTime);

        lineRenderer.enabled = false;
    }

    public void CastSparkles()
    {
        purpleSparkles.Play();
    }
}
