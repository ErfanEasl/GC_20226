using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectionKeeper : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    private GameObject last;

    void Update()
    {
        var es = EventSystem.current;
        if (es == null) return;

        if (es.currentSelectedGameObject != null)
            last = es.currentSelectedGameObject;
        else
            es.SetSelectedGameObject(last != null ? last : firstSelected);
    }
}