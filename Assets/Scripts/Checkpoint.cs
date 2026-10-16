using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    [Header("References")]
    [SerializeField] private GameObject fire;

    [Header("Level loading")]
    [SerializeField] private float loadDelay = 3f;
    [SerializeField] private string nextSceneName = "Level2";

    private SpriteRenderer sr;
    private bool isLit;
    public bool IsLit => isLit;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (offSprite != null) sr.sprite = offSprite;
        if (fire != null) fire.SetActive(false);
        else Debug.LogWarning("Fire is not assigned on " + name);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isLit) return;
        if (!other.CompareTag("PlayerFire")) return;

        isLit = true;
        Debug.Log("Checkpoint lit by " + other.name);

        if (onSprite != null) sr.sprite = onSprite;
        if (fire != null) fire.SetActive(true);

        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        Debug.Log("Loading " + nextSceneName + " in " + loadDelay + "s");
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(nextSceneName);
    }
}