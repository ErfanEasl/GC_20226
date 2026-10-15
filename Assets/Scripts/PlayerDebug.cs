using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    public Animator animator;
    public Collider2D playerCollider;
    public float groundCheckDistance = 0.3f;

    private Dictionary<string, string> lastValues = new Dictionary<string, string>();
    private Vector2 lastPosition;
    private Vector2 velocity;
    private string groundInfo = "";

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
        lastPosition = transform.position;

        if (animator == null) Debug.LogError("[PlayerDebug] No Animator found!");
        if (playerCollider == null) Debug.LogWarning("[PlayerDebug] No Collider2D found, ground check disabled.");
    }

    void Update()
    {
        // Velocity measured from position changes (works in every Unity version).
        velocity = ((Vector2)transform.position - lastPosition) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = transform.position;

        CheckGround();
        LogParameterChanges();
    }

    void CheckGround()
    {
        if (playerCollider == null) return;

        Bounds b = playerCollider.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, groundCheckDistance);

        groundInfo = "NOTHING below within " + groundCheckDistance;
        foreach (RaycastHit2D h in hits)
        {
            if (h.collider == playerCollider) continue; // skip ourselves
            groundInfo = h.collider.name + " | layer: " + LayerMask.LayerToName(h.collider.gameObject.layer)
                       + " | tag: " + h.collider.tag + " | dist: " + h.distance.ToString("F2");
            break;
        }
    }

    void LogParameterChanges()
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;

        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            string value = GetValue(p);
            string old;
            if (lastValues.TryGetValue(p.name, out old) && old != value)
            {
                Debug.Log("[PlayerDebug] " + p.name + ": " + old + " -> " + value);
            }
            lastValues[p.name] = value;
        }
    }

    string GetValue(AnimatorControllerParameter p)
    {
        switch (p.type)
        {
            case AnimatorControllerParameterType.Bool:  return animator.GetBool(p.name).ToString();
            case AnimatorControllerParameterType.Float: return animator.GetFloat(p.name).ToString("F2");
            case AnimatorControllerParameterType.Int:   return animator.GetInteger(p.name).ToString();
            default: return "(trigger)";
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;

        float y = 10f;
        GUI.Label(new Rect(10, y, 700, 24), "Horizontal input: " + Input.GetAxisRaw("Horizontal"), style); y += 22;
        GUI.Label(new Rect(10, y, 700, 24), "Jump button held: " + Input.GetButton("Jump"), style); y += 22;
        GUI.Label(new Rect(10, y, 700, 24), "Velocity: " + velocity.ToString("F2"), style); y += 22;
        GUI.Label(new Rect(10, y, 900, 24), "Ground below: " + groundInfo, style); y += 22;

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimatorControllerParameter p in animator.parameters)
            {
                GUI.Label(new Rect(10, y, 700, 24), "Anim " + p.name + " = " + GetValue(p), style);
                y += 22;
            }
        }
    }
}