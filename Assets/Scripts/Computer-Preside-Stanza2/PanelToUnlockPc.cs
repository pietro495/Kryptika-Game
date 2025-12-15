using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    //classe per gestire la interazione con il 2 computer nell'aula del preside 
public class PanelToUnlockPc : MonoBehaviour
{

    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button submitButton;
    //[SerializeField] Button cancelButton; // opzionale
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject panelRoot;
    [SerializeField] newPlayerController player;   // se vuoi bloccare/sbloccare il movimento

    public System.Action<string, string> onSubmit; // callback opzionale

    void Awake()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(HandleSubmit);
        }
    }

    public void Show()
    {   
        if(panelRoot == null)
        { Debug.Log("paneloROOT NULLO"); }
        if (panelRoot != null)
        {
            gameObject.SetActive(true);
        }
        if (emailInput != null) emailInput.text = "";
        if (passwordInput != null) passwordInput.text = "";

        StartCoroutine(FocusEmailNextFrame());
        player?.SetControlLock(true);   // se ti serve bloccare il player
    }

    //chiude il pannello
    public void Hide()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        player?.SetControlLock(false);   // se ti serve bloccare il player

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
    }

}
