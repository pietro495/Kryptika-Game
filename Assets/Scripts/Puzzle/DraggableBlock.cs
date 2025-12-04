// DraggableBlock.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform rect;
    [SerializeField] Canvas canvas;              // assegna il canvas UI
    [SerializeField] CanvasGroup cg;
    [SerializeField] TMP_Text label;

    // tipo del blocco (deve combaciare con lo slot)
    public SlotPuzzle.SlotType blockType;

    Vector2 startPos;
    Transform startParent;
    PuzzleManager manager;

    public void Init(PuzzleManager mgr) => manager = mgr;

    void Awake()
    {
        if (!rect) rect = GetComponent<RectTransform>();
        if (!cg) cg = GetComponent<CanvasGroup>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
        startParent = transform.parent;
        cg.blocksRaycasts = false; // permette agli slot di ricevere il drop
    }

    public void OnDrag(PointerEventData eventData)
    {
        float scale = canvas ? canvas.scaleFactor : 1f;
        rect.anchoredPosition += eventData.delta / scale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;

        // cerca uno slot sotto il puntatore
        var slot = eventData.pointerEnter ? eventData.pointerEnter.GetComponentInParent<SlotPuzzle>() : null;
        if (slot != null && slot.TryPlace(this))
            return;

        // se drop fallito, torna alla posizione iniziale
        rect.SetParent(startParent, false);
        rect.anchoredPosition = startPos;
    }

    // chiamato dallo slot corretto per “agganciare” il blocco
    public void SnapToSlot(SlotPuzzle slot)
    {
        rect.SetParent(slot.transform, false);
        rect.anchoredPosition = Vector2.zero;
    }
}
