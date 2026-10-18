using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class HintDisplay : MonoBehaviour
{
    public static HintDisplay Instance { get; private set; }

    [SerializeField] private TMP_Text label;
    [SerializeField] private float fadeTime = 0.6f;

    private CanvasGroup group;
    private Coroutine routine;

    void Awake()
    {
        Instance = this;
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    public void Show(string message, float duration)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine(message, duration));
    }

    public void Hide()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeTo(0f));
    }

    IEnumerator ShowRoutine(string message, float duration)
    {
        label.text = message;
        yield return FadeTo(1f);

        if (duration > 0f)
        {
            yield return new WaitForSecondsRealtime(duration);
            yield return FadeTo(0f);
        }
    }

    IEnumerator FadeTo(float target)
    {
        float start = group.alpha;
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t / fadeTime);
            yield return null;
        }
        group.alpha = target;
    }
}