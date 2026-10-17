using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Leave empty to load the next scene in Build Settings.")]
    [SerializeField] private string firstLevelName = "";

    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    void Awake()
    {
        Time.timeScale = 1f; // in case the game was paused before returning here

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayGame()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);

        if (!string.IsNullOrEmpty(firstLevelName))
            SceneManager.LoadScene(firstLevelName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);

        Debug.Log("Exit pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Application.Quit does nothing in the editor
#else
        Application.Quit();
#endif
    }
}