using UnityEngine;

public class Doomball : MonoBehaviour
{
   public PlayerHealth health;
    public float speed = 6f;
    public float lifetime = 4f;
    public Animator animator; // optional: impact animation

    private Vector2 direction;
    private bool hasHit = false;

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
        Debug.Log("[Doomball] Launched. Direction: " + direction + " | Speed: " + speed);
    }

    void Update()
    {
        if (hasHit) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        Debug.Log("[Doomball] Touched: " + other.name + " (tag: " + other.tag + ")");

        // Never react to the enemy that shot it.
        if (other.GetComponentInParent<EnemyAI>() != null) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Doomball] HIT THE PLAYER!");
            Impact();
        }
        else if (other.CompareTag("Ground"))
        {
            Impact();
        }
    }

    void Impact()
    {
        hasHit = true;
        GetComponent<Collider2D>().enabled = false;

        // Only play the impact animation if the Animator really has a "Hit" trigger.
        if (animator != null && HasHitTrigger())
        {
            animator.SetTrigger("Hit");
            Destroy(gameObject, 2f); // safety in case the DestroySelf event is missing
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool HasHitTrigger()
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == "Hit" && p.type == AnimatorControllerParameterType.Trigger) return true;
        }
        return false;
    }

    // Called by an animation event at the end of the impact clip
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        health.TakeDamage(1);
    }
}