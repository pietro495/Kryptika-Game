using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fake_login_panel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button submitButton;
    //[SerializeField] Button cancelButton; // opzionale
    [SerializeField] Phishing_Effect_Panel effect_phishing_panel;
   // [SerializeField] Dialog phishingDialogue;

   
    public System.Action<string, string> onSubmit; // callback opzionale

    void Awake()
    {
        //gameObject.SetActive(false);
        
        if (submitButton != null)
            submitButton.onClick.AddListener(HandleSubmit);

        //if (cancelButton != null)
        //    cancelButton.onClick.AddListener(Hide);
        if(effect_phishing_panel != null)
        {
            effect_phishing_panel.Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (emailInput != null) emailInput.text = "";
        if (passwordInput != null) passwordInput.text = "";

        StartCoroutine(FocusEmailNextFrame());
    }

    //chiude il pannello
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    IEnumerator FocusEmailNextFrame()
    {
        yield return null;
        if (emailInput != null)
        {
            emailInput.Select();
            emailInput.ActivateInputField();
        }
    }

    void HandleSubmit()
    {
        string email = emailInput ? emailInput.text : "";
        string password = passwordInput ? passwordInput.text : "";

        Debug.Log($"Fake login submit -> {email} / {password}");
        //Debug.Log("OnSubmitCompleted fired");
        onSubmit?.Invoke(email, password);

        Hide();
        if (effect_phishing_panel != null)
            effect_phishing_panel.Show(); //mostra la nuova ui
        /*
        if (phishingDialogue != null && DialogManager.Instance != null)
        {
            DialogManager.Instance.onHideDialogue += OnDialogueClosed;
            StartCoroutine(DialogManager.Instance.ShowDialogue(phishingDialogue));
        }
        else
        {
            Hide();
        }
        */
    }
    
    void OnDialogueClosed()
    {
        DialogManager.Instance.onHideDialogue -= OnDialogueClosed;
        Hide(); //CHIUDI PANNELLO SOLO DOPO IL DIALOGO
    }
}
