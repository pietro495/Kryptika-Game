using UnityEngine;

public enum DropType { Phishing, Legit }

public class DropZone : MonoBehaviour
{
    [SerializeField] DropType dropType;
    [SerializeField] PhishingInteractableManager manager;

    public bool TryAccept(DragItem item)
    {
        if (item == null || item.scored) return false;

        bool correct = (dropType == DropType.Phishing && item.isPhishing) ||
                       (dropType == DropType.Legit && !item.isPhishing);

        manager.AddScore(correct ? +5 : -5);
        item.DisableDrag();
        item.scored = true;
        manager.NotifyBlockScored();

        // opzionale: lascia il blocco qui, altrimenti:
        //item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return true;
    }
}
