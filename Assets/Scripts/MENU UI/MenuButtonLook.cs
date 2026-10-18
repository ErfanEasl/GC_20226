using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonLook : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private CanvasGroup litLayer;
    [SerializeField] private CanvasGroup dimLayer;
    [SerializeField] private float fadeSpeed = 8f;
    [SerializeField] private AudioClip hoverSound;

    private AudioSource audioSource;
    private float target;
    private float current;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        Apply(0f);
    }

    void Update()
    {
        current = Mathf.MoveTowards(current, target, fadeSpeed * Time.unscaledDeltaTime);
        Apply(current);
    }

    void Apply(float v)
    {
        if (litLayer != null) litLayer.alpha = v;
        if (dimLayer != null) dimLayer.alpha = 1f - v;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData e)
    {
        target = 1f;
        if (hoverSound != null && Time.timeSinceLevelLoad > 0.3f)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnDeselect(BaseEventData e)
    {
        target = 0f;
    }
}