using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class StarPulse : MonoBehaviour
{
    [SerializeField] private float minAlpha = 0.75f;
    [SerializeField] private float speed = 1.2f;

    private CanvasGroup group;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.unscaledTime * speed) + 1f) * 0.5f;
        group.alpha = Mathf.Lerp(minAlpha, 1f, t);
    }
}