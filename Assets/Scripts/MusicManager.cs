using UnityEngine;

/// <summary>
/// Keeps the soundtrack playing across scene loads / level reloads.
/// Put this on a GameObject with an AudioSource in your FIRST scene
/// (e.g. main menu or level 1). Duplicates in later scenes destroy themselves.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        // If a music player already exists (scene was reloaded), destroy this new copy
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    /// <summary>
    /// Optional: switch to a different track (e.g. boss music).
    /// Does nothing if that clip is already playing, so it never restarts.
    /// </summary>
    public void PlayTrack(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}