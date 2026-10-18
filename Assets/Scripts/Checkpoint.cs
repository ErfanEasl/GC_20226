using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private bool requireThrownLight = true;

    [Header("Respawn")]
    [Tooltip("Where the player returns after falling. Empty = next to this vase.")]
    [SerializeField] private Transform respawnPoint;

    [Header("Level loading")]
    [Tooltip("UNTICK this on all 5 vases in the final level.")]
    [SerializeField] private bool loadsNextLevel = true;
    [SerializeField] private float loadDelay = 3f;
    [SerializeField] private bool useNextBuildIndex = true;
    [SerializeField] private string nextSceneName = "Level2";

    [Header("When lit")]
    public UnityEvent onLit;

    public static Vector3 LastLitPosition;
    public static bool HasLitPosition;

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private bool isLit;

    public bool IsLit => isLit;

    void Awake()
    {
        HasLitPosition = false;

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

        LastLitPosition = respawnPoint != null
            ? respawnPoint.position
            : transform.position + Vector3.up * 0.5f;
        HasLitPosition = true;

        onLit.Invoke();

        if (loadsNextLevel) StartCoroutine(LoadNext());
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
                Debug.Log("This was the last scene in Build Settings.");
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}