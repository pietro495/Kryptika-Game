// PasswordValidator.cs
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordValidator : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] PasswordSlot[] slots;

    [Header("Indicators")]
    [SerializeField] Image lenIndicator;
    [SerializeField] Image upperIndicator;
    [SerializeField] Image lowerIndicator;
    [SerializeField] Image digitIndicator;
    [SerializeField] Image symbolIndicator;

    [Header("UI")]
    [SerializeField] Button confirmButton;
    [SerializeField] Slider strengthBar;
    [SerializeField] TMP_Text feedbackText;

    [Header("Colors")]
    [SerializeField] Color okColor = Color.green;
    [SerializeField] Color failColor = Color.red;

    [Header("Riferimento al Manager")]
    [SerializeField] PasswordMiniGameManager miniGameManager;

    /*
    public void Start()
    {
        confirmButton.onClick.AddListener(() =>
        {
            miniGameManager?.CloseMiniGame();
        });
    }
    */
    const string symbols = "!@#$%^&*?+=_-";

    public void RebuildPassword()
    {
        var pwd = string.Concat(slots
            .Where(s => s != null && s.GetChar().HasValue)
            .Select(s => s.GetChar().Value));

        
        bool hasLen      = pwd.Length == 12;
        bool firstUpper  = pwd.Length > 0 && char.IsUpper(pwd[0]);
        int digitCount   = pwd.Count(char.IsDigit);             //conta quanti caratteri ci sono
        int symbolCount  = pwd.Count(c => symbols.Contains(c));
        int lowerCount   = pwd.Count(char.IsLower);

        bool hasDigits   = digitCount >= 2;
        bool hasSymbols  = symbolCount >= 2;
        bool hasLower    = lowerCount >= 2;
        
        Debug.Log($"PWD: {pwd} len:{hasLen} first:{firstUpper} digits:{digitCount} symbols:{symbolCount}");

        // aggiorna gli indicatori come preferisci (riusa 5 cerchi oppure riduci a 4)
        SetIndicator(lenIndicator, hasLen);
        SetIndicator(upperIndicator, firstUpper);
        SetIndicator(digitIndicator, hasDigits);
        SetIndicator(symbolIndicator, hasSymbols);
        SetIndicator(lowerIndicator, hasLower);
        // puoi usare lowerIndicator per un’altra regola o disattivarlo

        bool allOk = hasLen && firstUpper && hasDigits && hasSymbols && hasLower;
        if (confirmButton) confirmButton.interactable = allOk;

        //if (allOk && pwd.Length == slots.Length) // tutti gli slot pieni e regole ok
        //  miniGameManager?.CloseMiniGame();
    } 

    void SetIndicator(Image img, bool ok)
    {
        if (!img) return;
        img.color = ok ? okColor : failColor;
    }
}
