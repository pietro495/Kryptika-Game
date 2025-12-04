// SlotPuzzle.cs
using UnityEngine;
using UnityEngine.UI;

public class SlotPuzzle : MonoBehaviour
{
    public enum SlotType { Data, IP, Postazione }

    [SerializeField] SlotType slotType;
    [SerializeField] Image slotImage;
    [SerializeField] Color okColor = Color.green;
    [SerializeField] Color defaultColor = Color.white;

    DraggableBlock current;
    PuzzleManager manager;

    public void Init(PuzzleManager mgr)
    {
        manager = mgr;
        if (slotImage) slotImage.color = defaultColor;
    }

    public bool TryPlace(DraggableBlock block)
    {
        if (block == null || manager == null) return false;

        // accetta solo se il tipo combacia e lo slot è libero
        if (current == null && block.blockType == slotType)
        {
            current = block;
            current.SnapToSlot(this);
            if (slotImage) slotImage.color = okColor;
            manager.NotifySlotCorrect(slotType);
            return true;
        }

        return false;
    }

    public void ResetSlot()
    {
        current = null;
        if (slotImage) slotImage.color = defaultColor;
    }
}
