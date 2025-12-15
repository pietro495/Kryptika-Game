using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostIt : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drop Area")]
    [SerializeField] RectTransform dropArea; // l’area dove droppare l’oggetto

    [Header("FlowManagaer")]
    [SerializeField] UnlockPcFlow flowManager; 

   // [Header("Immagine Foglietto")]
   // [SerializeField] Image image; 

    [Header("Canvas e snap")]
    [SerializeField] Canvas canvas;          // il Canvas di riferimento (quello che contiene l’oggetto)
    [SerializeField] bool snapToTarget = false;
    [SerializeField] RectTransform snapTarget; // opzionale: dove “agganciare” al drop



    RectTransform rect;
    CanvasGroup cg;
    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
        if (cg != null) cg.blocksRaycasts = false; // lascia passare il raycast se devi droppare su slot
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void SetDraggable(bool value)
    {
        enabled = value;               // disabilita l’handler di drag
        if (cg != null) cg.blocksRaycasts = value; // opzionale per bloccare i click
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cg != null) cg.blocksRaycasts = true;

        // verifica se sei sopra l’area schermo
        bool inside = dropArea != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(
                        dropArea, eventData.position, eventData.pressEventCamera);

        if (inside)
        {
            Debug.Log("si aprirà la 2 immagine?");
            rect.position = dropArea.position; // snap al monitor
            flowManager?.OnPostItPlaced();     // delega al manager cosa fare dopo
        }
        else
        {
            Debug.Log("AAAAAAAAAAAA");
            rect.anchoredPosition = startPos;  // opzionale: torna indietro
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
