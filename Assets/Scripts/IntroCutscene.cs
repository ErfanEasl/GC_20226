using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [Tooltip("-1 = load the next scene. 0 = go back to the main menu.")]
    [SerializeField] private int sceneIndexOverride = -1;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        int target = sceneIndexOverride >= 0
            ? sceneIndexOverride
            : SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(target);
    }

    void OnDestroy()
    {
        if (videoPlayer != null) videoPlayer.loopPointReached -= OnVideoEnd;
    }
}