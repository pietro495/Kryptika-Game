// PasswordSlot.cs
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordSlot : MonoBehaviour
{
    [SerializeField] PasswordValidator validator;
    [SerializeField] Image slotImage;
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashTime = 0.1f;

    Color originalColor;
    public DraggableKey CurrentKey;

    void Awake()
    {
        if (slotImage) originalColor = slotImage.color;
    }

    public bool TryPlaceKey(DraggableKey dragged)
    {
        if (dragged == null) return false;

        if (CurrentKey) Destroy(CurrentKey.gameObject);

        var clone = Instantiate(dragged.gameObject, transform);
        var keyCopy = clone.GetComponent<DraggableKey>();
        keyCopy.SetCharacter(dragged.Character);
        keyCopy.CurrentSlot = this;
        keyCopy.SetValidator(validator);

        var cg = keyCopy.GetComponent<CanvasGroup>();
        if (cg) { cg.blocksRaycasts = false; cg.interactable = false; }

        var rt = keyCopy.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.localScale = Vector3.one;

        CurrentKey = keyCopy;
        dragged.ReturnHome();
        validator?.RebuildPassword();

        if (slotImage) StartCoroutine(Flash());

        return true;
    }


    IEnumerator Flash()
    {
        slotImage.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        slotImage.color = originalColor;
    }

    
    public char? GetChar()
    {
        return CurrentKey ? CurrentKey.Character : (char?)null;
    }
}



