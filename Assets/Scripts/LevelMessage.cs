using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LevelMessage : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float showTime = 5f;
    [SerializeField] private float fadeTime = 1.5f;

    private CanvasGroup group;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(showTime);
        yield return Fade(1f, 0f);
        gameObject.SetActive(false);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }
        group.alpha = to;
    }
}