// DraggableKey.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DraggableKey : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform rect;
    [SerializeField] CanvasGroup cg;
    [SerializeField] TMP_Text label;
    [SerializeField] Canvas canvas;                  // assign the UI canvas (for proper scaling)
    [SerializeField] Transform homeContainer;        // keyboard grid
    [SerializeField] PasswordValidator validator;    // set in inspector

    public char Character { get; private set; }
    public PasswordSlot CurrentSlot { get; set; }

    Vector2 startPos;
    public void SetValidator(PasswordValidator v) => validator = v;

    void Awake()
    {
        if (!rect) rect = GetComponent<RectTransform>();
        if (!cg) cg = GetComponent<CanvasGroup>();
        if (!label) label = GetComponentInChildren<TMP_Text>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    public void SetCharacter(char c)
    {
        Character = c;
        if (label) label.text = c.ToString();
    }

    public void SetHome(Transform home) => homeContainer = home;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
        if (cg) cg.blocksRaycasts = false;
        // detach from slot while dragging
        if (CurrentSlot)
        {
            CurrentSlot.CurrentKey = null;
            CurrentSlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float scale = canvas ? canvas.scaleFactor : 1f;
        rect.anchoredPosition += eventData.delta / scale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cg) cg.blocksRaycasts = true;

        var drop = eventData.pointerEnter ? eventData.pointerEnter.GetComponentInParent<PasswordSlot>() : null;
        if (drop != null && drop.TryPlaceKey(this))
            return;

        ReturnHome();
    }

    public void ReturnHome()
    {
        if (homeContainer == null) return;
        transform.SetParent(homeContainer, false);
        var rt = GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.localScale = Vector3.one;
        CurrentSlot = null;
        if (validator) validator.RebuildPassword();
        if (validator == null) Debug.Log("Validator is null nel draggable key");
    }

    public void LockToSlot(PasswordSlot slot)
    {
        CurrentSlot = slot;
        transform.SetParent(slot.transform, false);
        rect.anchoredPosition = Vector2.zero;
        if (cg)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false; // avoid dragging from slot
        }
    }

    public void Unlock()
    {
        if (cg)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }
}
