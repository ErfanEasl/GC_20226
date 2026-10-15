using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // auto-found by the "Player" tag if empty
    public Animator animator;         // auto-found if empty
    public Transform firePoint;       // empty child at the enemy's head
    [FormerlySerializedAs("fireballPrefab")]
    public GameObject doomballPrefab; // the doomball prefab

    [Header("Ranges")]
    public float detectionRange = 8f;
    public float attackRange = 4f;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public float attackCooldown = 3f;

    [Header("Testing / Fallbacks")]
    [Tooltip("ON = use animation events. OFF = shoot using timers (for testing).")]
    public bool useAnimationEvents = true;
    public float shootDelay = 0.5f;      // used only when events are OFF
    public float attackDuration = 1f;    // used only when events are OFF
    [Tooltip("Safety: if the attack never ends, unfreeze after this many seconds.")]
    public float maxAttackLock = 3f;

    [Header("Debug")]
    public bool debugLogs = true;

    private float nextAttackTime = 0f;
    private bool isAttacking = false;
    private float attackStartTime = 0f;
    private float nextLogTime = 0f;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;

        // Auto-find references if you forgot to fill them in.
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log("[EnemyAI] Player was empty, found it by tag: " + p.name);
            }
            else
            {
                Debug.LogError("[EnemyAI] No Player assigned and no object tagged 'Player' found!");
            }
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            if (animator == null)
                Debug.LogError("[EnemyAI] No Animator found on the enemy or its children!");
            else
                Debug.Log("[EnemyAI] Animator was empty, found: " + animator.gameObject.name);
        }

        // Check the Animator has what we need.
        if (animator != null)
        {
            if (animator.runtimeAnimatorController == null)
                Debug.LogError("[EnemyAI] The Animator has NO Controller assigned!");
            else
            {
                if (!HasParameter("IsWalking", AnimatorControllerParameterType.Bool))
                    Debug.LogError("[EnemyAI] Animator is missing a BOOL parameter named exactly 'IsWalking'");
                if (!HasParameter("Attack", AnimatorControllerParameterType.Trigger))
                    Debug.LogError("[EnemyAI] Animator is missing a TRIGGER parameter named exactly 'Attack'");
            }
        }

        if (firePoint == null) Debug.LogWarning("[EnemyAI] Fire Point is not assigned.");
        if (doomballPrefab == null) Debug.LogWarning("[EnemyAI] Doomball Prefab is not assigned.");
    }

    void Update()
    {
        if (player == null || animator == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (debugLogs && Time.time >= nextLogTime)
        {
            nextLogTime = Time.time + 0.5f;
            Debug.Log("[EnemyAI] distance=" + distance.ToString("F1")
                + " | detection=" + detectionRange
                + " | attackRange=" + attackRange
                + " | isAttacking=" + isAttacking
                + " | cooldownLeft=" + Mathf.Max(0f, nextAttackTime - Time.time).ToString("F1"));
        }

        // Safety: never stay locked in "attacking" forever.
        if (isAttacking && Time.time - attackStartTime > maxAttackLock)
        {
            Debug.LogWarning("[EnemyAI] Attack lasted too long. Is the EndAttack animation event missing? Unlocking.");
            isAttacking = false;
        }

        if (isAttacking)
        {
            animator.SetBool("IsWalking", false);
            return;
        }

        FacePlayer();

        if (distance <= attackRange)
        {
            animator.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                StartAttack();
            }
        }
        else if (distance <= detectionRange)
        {
            animator.SetBool("IsWalking", true);
            MoveTowardPlayer();
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void FacePlayer()
    {
        // Assumes your sprite faces RIGHT by default.
        // Uses the ORIGINAL scale so the enemy never changes size.
        Vector3 scale = baseScale;
        float size = Mathf.Abs(baseScale.x);
        scale.x = (player.position.x >= transform.position.x) ? size : -size;
        transform.localScale = scale;
    }

    void StartAttack()
    {
        Debug.Log("[EnemyAI] >>> Starting attack");
        isAttacking = true;
        attackStartTime = Time.time;
        nextAttackTime = Time.time + attackCooldown;
        animator.SetTrigger("Attack");

        if (!useAnimationEvents)
        {
            Invoke(nameof(ShootDoomball), shootDelay);
            Invoke(nameof(EndAttack), attackDuration);
        }
    }

    // ---- Called by ANIMATION EVENTS (or by Invoke when events are OFF) ----

    public void ShootDoomball()
    {
        Debug.Log("[EnemyAI] ShootDoomball() called");

        if (doomballPrefab == null) { Debug.LogError("[EnemyAI] Can't shoot: Doomball Prefab is empty!"); return; }
        if (firePoint == null)      { Debug.LogError("[EnemyAI] Can't shoot: Fire Point is empty!"); return; }
        if (player == null)         { Debug.LogError("[EnemyAI] Can't shoot: Player is empty!"); return; }

        GameObject ball = Instantiate(doomballPrefab, firePoint.position, Quaternion.identity);
        Doomball doomball = ball.GetComponent<Doomball>();

        if (doomball == null)
        {
            //Debug.LogError("[EnemyAI] The prefab has no Doomball script on it!");
            return;
        }

        Vector2 direction = (player.position - firePoint.position).normalized;
        doomball.Launch(direction);
        //Debug.Log("[EnemyAI] Doomball spawned at " + firePoint.position);
    }

    public void EndAttack()
    {
        //Debug.Log("[EnemyAI] EndAttack() called");
        isAttacking = false;
    }

    bool HasParameter(string paramName, AnimatorControllerParameterType type)
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == paramName && p.type == type) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}