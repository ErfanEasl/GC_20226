using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevelManager : MonoBehaviour
{
    [SerializeField] private Checkpoint[] vases;
    [SerializeField] private float delayAfterLastVase = 3f;
    [SerializeField] private AudioClip finalSound;
    [SerializeField, Range(0f, 1f)] private float finalVolume = 1f;

    private bool finished;

    void Start()
    {
        foreach (var v in vases)
            if (v == null) Debug.LogWarning("A vase slot is empty on " + name);
    }

    void Update()
    {
        if (finished) return;

        foreach (var v in vases)
            if (v == null || !v.IsLit) return;

        finished = true;
        StartCoroutine(Finish());
    }

    IEnumerator Finish()
    {
        if (finalSound != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(finalSound, Camera.main.transform.position, finalVolume);

        yield return new WaitForSeconds(delayAfterLastVase);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
