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

    [Header("Sound")]
    [SerializeField] private AudioClip lightSound;
    [SerializeField, Range(0f, 1f)] private float lightVolume = 1f;

    [Header("Lighting rules")]
    [Tooltip("If ticked, the light must be thrown. Walking into the vase won't light it.")]
    [SerializeField] private bool requireThrownLight = true;

    [Header("Level loading")]
    [SerializeField] private float loadDelay = 3f;
    [Tooltip("If ticked, loads the next scene in Build Settings and ignores the name below.")]
    [SerializeField] private bool useNextBuildIndex = true;
    [SerializeField] private string nextSceneName = "Level2";

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private bool isLit;

    public bool IsLit => isLit;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (offSprite != null) sr.sprite = offSprite;

        if (fire != null) fire.SetActive(false);
        else Debug.LogWarning("Fire is not assigned on " + name);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isLit) return;
        if (!other.CompareTag("PlayerFire")) return;

        if (requireThrownLight)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet == null || !bullet.IsShot) return;
        }

        isLit = true;

        if (onSprite != null) sr.sprite = onSprite;
        if (fire != null) fire.SetActive(true);
        if (lightSound != null) audioSource.PlayOneShot(lightSound, lightVolume);

        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(loadDelay);

        if (useNextBuildIndex)
        {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            if (next < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(next);
            else
                Debug.Log("This was the last level. Add an ending or menu scene to Build Settings.");
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}