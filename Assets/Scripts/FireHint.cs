using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FireHint : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;

    private CanvasGroup group;
    private bool hidden;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 1f;
    }

    void Update()
    {
        if (hidden) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightShift))
        {
            hidden = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}