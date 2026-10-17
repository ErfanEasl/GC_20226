using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.loopPointReached -= OnVideoEnd;
    }
}