using System.Collections;
using UnityEngine;
using TMPro;

public class VaseCounter : MonoBehaviour
{
    [SerializeField] private Checkpoint[] vases;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private float pulseScale = 1.4f;
    [SerializeField] private float pulseTime = 0.3f;

    private int lastCount = -1;

    void Update()
    {
        int count = 0;
        foreach (var v in vases)
            if (v != null && v.IsLit) count++;

        if (count != lastCount)
        {
            counterText.text = count + "/" + vases.Length;
            if (lastCount >= 0) StartCoroutine(Pulse());
            lastCount = count;
        }
    }

    IEnumerator Pulse()
    {
        Transform t = counterText.transform;
        float time = 0f;
        while (time < pulseTime)
        {
            time += Time.deltaTime;
            float k = Mathf.Sin(time / pulseTime * Mathf.PI);
            t.localScale = Vector3.one * Mathf.Lerp(1f, pulseScale, k);
            yield return null;
        }
        t.localScale = Vector3.one;
    }
}