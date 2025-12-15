using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fake_login_panel : MonoBehaviour
{
    [Header("DialoManager fix")]
    [SerializeField] DialogManager dialogManager;
    
    [Header("Button")]
    [SerializeField] Button submitButton;


    [Header("Panel")]
    [SerializeField] Phishing_Effect_Panel effect_phishing_panel;

    [Header("InputValidator")]
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;

   [SerializeField] private string expEmail ="professore@professore.com";
   [SerializeField] private string expPassword ="Forzabari1";

   
    public System.Action<string, string> onSubmit; // callback opzionale

    void Awake()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(HandleSubmit);
    
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
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        bool okEmail = email.Equals(expEmail);
        bool okPassword = password.Equals(expPassword);

        if (okEmail && okPassword)
        {
            Debug.Log("Email e password corrette!");
            onSubmit?.Invoke(email, password);
        Hide();
        if (effect_phishing_panel != null)
            effect_phishing_panel.Show(); //mostra la nuova ui
        }
        else
        {
            Debug.Log("Email o password errate!");
            // Pulizia campi
            emailInput.text = "";
            passwordInput.text = "";

            // Opzionale: porta focus al campo email
            emailInput.Select();
            emailInput.ActivateInputField();
        }
    }
    
    void OnDialogueClosed()
    {
        dialogManager.onHideDialogue -= OnDialogueClosed;
        Hide(); //CHIUDI PANNELLO SOLO DOPO IL DIALOGO
    }
}
