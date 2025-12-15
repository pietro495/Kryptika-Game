using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenInstructions : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject panelRoot;
    [SerializeField] GameObject emailUIGame;
    [SerializeField] GameObject panelMail;

    [Header("MiniGame Ph Drag & Drop")]

    [SerializeField] PhishingInteractableManager phishingInteractableManager;

   // [SerializeField] Button button;

    

    void Awake()
    {   
        panelRoot.SetActive(false);
        instructionsPanel.SetActive(false);
        emailUIGame.SetActive(false);
        panelMail.SetActive(false);
    }

    public void OpenInstructionsPanel()
    {
        emailUIGame.SetActive(false);
        panelMail.SetActive(false);
        panelRoot.SetActive(true);
        instructionsPanel.SetActive(true);
    }


    public void OnButtonClick()
    {
        instructionsPanel.SetActive(false);
    
        emailUIGame.SetActive(true);
        panelMail.SetActive(true);
        phishingInteractableManager.StartMiniGame();

    }

    /*
    public void CloseInstructionsPanel()
    {
        instructionsPanel.SetActive(false);

       // phishingInteractableManager.StartMiniGame();
    }
    */
}
