using System;
using UnityEngine;

public class Slime : Entity
{
    [SerializeField]
    GameObject player;
    Rigidbody body;
    [SerializeField]
    Renderer slimeRenderer;

    bool followingPlayer = false;
    bool jumping = false;
    [SerializeField]
    float timeSinceJump = 0f;
    [SerializeField]
    float intervalJump = 4f;
    [SerializeField]
    float maxForwardForce = 5;
    [SerializeField]
    float jumpForce = 5;
    [SerializeField]
    protected float velocityTolerance = 10; // if hits object at velocity higher than this, has damage delt

    protected override void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!followingPlayer && Math.Abs(Vector3.Angle(transform.forward, player.transform.position - transform.position)) < 70)
        {
            followingPlayer = true;
        }

        if (followingPlayer)
        {
            timeSinceJump += Time.deltaTime;

            if (jumping && IsGrounded() && body.linearVelocity.y == 0)
            {
                jumping = false;
            }

            if (!jumping)
            {
                Vector3 adjusted = player.transform.position;
                adjusted.y = transform.position.y;
                transform.LookAt(adjusted);
            }

            if (!jumping && timeSinceJump >= intervalJump)
            {
                jumping = true;
                timeSinceJump = 0;

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                float forwardForce = Math.Min(distanceToPlayer * body.mass / Mathf.Sqrt(2f * jumpForce / (5f * body.mass)), maxForwardForce);
                body.AddForce(transform.forward * forwardForce + transform.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > velocityTolerance)
        {
            Hurt((int)Math.Pow(collision.relativeVelocity.magnitude - velocityTolerance, 2));
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Hurt((int)collision.relativeVelocity.magnitude);
        }
    }

    public override void Hurt(int amount)
    {
        base.Hurt(amount);
        slimeRenderer.material.SetFloat("_Health", health / (float)MAX_HEALTH);
    }
}
