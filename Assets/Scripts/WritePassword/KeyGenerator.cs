using UnityEngine;
using TMPro;

public class KeyGenerator : MonoBehaviour
{
    [SerializeField] DraggableKey keyPrefab;
    [SerializeField] Transform keyboardGrid;        // GridLayoutGroup
    [SerializeField] PasswordValidator validator;

    const string upper   = "ABCDEFGHILMNOPQRSTUVZ";
    const string lower   = "abcdefghilmnopqrstuvz";
    const string digits  = "0123";
    const string symbols = "!@#$,-";

    void OnEnable() => GenerateKeys();

    void GenerateKeys()
    {
        if (!keyPrefab || !keyboardGrid) return;

        // pulisci eventuali figli precedenti
        for (int i = keyboardGrid.childCount - 1; i >= 0; i--)
            Destroy(keyboardGrid.GetChild(i).gameObject);

        string chars = upper + lower + digits + symbols;

        foreach (char c in chars)
        {
            var key = Instantiate(keyPrefab, keyboardGrid);
            key.SetValidator(validator);
            key.SetCharacter(c);
            key.SetHome(keyboardGrid);
            key.Unlock();

            // collega validator (se serve)
            key.SendMessage("SetValidator", validator, SendMessageOptions.DontRequireReceiver);

            var label = key.GetComponentInChildren<TMP_Text>();
            if (label) label.text = c.ToString();
        }

        validator?.RebuildPassword();
    }
}
