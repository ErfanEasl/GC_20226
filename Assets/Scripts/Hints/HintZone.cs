using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HintZone : MonoBehaviour
{
    [TextArea(2, 5)]
    [SerializeField] private string message;
    [Tooltip("Seconds the text stays. 0 = stays until the player leaves the zone.")]
    [SerializeField] private float showTime = 6f;
    [SerializeField] private bool onlyOnce = true;
    [SerializeField] private bool hideOnExit = false;

    private bool used;

    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (onlyOnce && used) return;
        if (HintDisplay.Instance == null) return;

        used = true;
        HintDisplay.Instance.Show(message, showTime);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (hideOnExit && HintDisplay.Instance != null)
            HintDisplay.Instance.Hide();
    }
}