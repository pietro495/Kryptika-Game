using UnityEngine;
using UnityEngine.UI;

public class Phishing_Effect_Panel : MonoBehaviour
{
    [Header("Opzionale")]
    [SerializeField] Button closeButton;          
    [SerializeField] AudioSource revealSfx;       // 
    [SerializeField] Dialog dialog_phishing_effect;

    void Awake()
    {
        gameObject.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
       // StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }


    public void Hide()
    {
        /*
        if (dialog_phishing_effect != null && DialogManager.Instance != null)
        {
            DialogManager.Instance.StartCoroutine(
            DialogManager.Instance.ShowDialogue(dialog_phishing_effect));        
        }
        */
        gameObject.SetActive(false);
    }


}
