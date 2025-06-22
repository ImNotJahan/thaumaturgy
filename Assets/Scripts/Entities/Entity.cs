using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected int MAX_HEALTH;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float height = 0;

    protected virtual void Start()
    {
        health = MAX_HEALTH;
    }

    public virtual void Hurt(int amount)
    {
        health -= amount;

        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.1f + height);
    }
}
