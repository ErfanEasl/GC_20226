using UnityEngine;

public class Doomball : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 4f;
    public int damage = 1;
    public Animator animator; // optional: impact animation

    private Vector2 direction;
    private bool hasHit = false;

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (hasHit) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // Never react to the enemy that shot it.
        if (other.GetComponentInParent<EnemyAI>() != null) return;

        if (other.CompareTag("Player"))
        {
            // Find the health script on whatever we hit.
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);   // damage happens ONCE, here
                Debug.Log("[Doomball] Hit the player for " + damage);
            }
            else
            {
                Debug.LogWarning("[Doomball] Hit the player but found no PlayerHealth script on it.");
            }

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

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

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
}