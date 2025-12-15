using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    [SerializeField] TMP_Text label;
    CanvasGroup cg;

    public bool isPhishing;
    public bool scored;

    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (rect == null) rect = GetComponent<RectTransform>();
    } 


    //salva testo, flag e resetta scored=false e la posizione iniziale.
    public void SetData(string text, bool phishing)
    {
        if (label != null) label.text = text;
        isPhishing = phishing;
        scored = false;
        if (rect != null) rect.anchoredPosition = startPos;
    }

    //memorizza la posizione StartPos
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
    }


    //sposta il RectTransform 
    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / rect.lossyScale; // per Canvas overlay
    }

    //cerca se il puntatore � sopra una DropZone e se � valida la drop
    public void OnEndDrag(PointerEventData eventData)
    {
        //se la zona accetta, bene; altrimenti rimetti la pos su rect anchoredPosition
        var drop = eventData.pointerEnter ? eventData.pointerEnter.GetComponentInParent<DropZone>() : null;
        if (drop != null && drop.TryAccept(this))
            return;

        rect.anchoredPosition = startPos; // reset se non droppi in una zona valida
    }

    //qui mi disabilito il drag
    public void DisableDrag()
    {
        scored = true;
        if (cg == null) cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

}
