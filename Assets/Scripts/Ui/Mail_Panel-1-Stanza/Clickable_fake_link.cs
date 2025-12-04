
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
/*
OnPointerClick è il callback che Unity chiama quando l’utente preme (e rilascia) il mouse sulla tua UI.
Implementando l’interfaccia IPointerClickHandler il tuo script mostra a Unity che vuole gestire i clic su quel GameObject.
*/

public class Clickable_fake_link  : MonoBehaviour, IPointerClickHandler
{
    /*
    [Header("Dialogo per il player")]
    [SerializeField] GameController gameController;
    [SerializeField] newPlayerController player;
    [SerializeField] DialogueAsset linkExplanation;
    [SerializeField] GameState stateAfterDialogue = GameState.ProfessorAction;
    */

    [Header("Per il professore")]
    [SerializeField] TextMeshProUGUI mailText;
    [SerializeField] GameObject phishingPanel;   // pannello/immagine da aprire
    [SerializeField] GameObject mailPanel;       // pannello con il testo della mail (facoltativo)
    [SerializeField] string linkId = "phishing"; // ID usato nel tag <link="...">
    [SerializeField] GameController gameController;



    void Awake()
    {
        if (phishingPanel != null)
        {
            phishingPanel.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameController.CurrentState == GameState.ProfessorAction)
        {
            if (mailText == null)
                return;

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(mailText, eventData.position, eventData.pressEventCamera);
            if (linkIndex == -1)
                return;
            
            string clickedId = mailText.textInfo.linkInfo[linkIndex].GetLinkID();
            if (clickedId != linkId)
                return;

            if (phishingPanel != null)
            {
                phishingPanel.SetActive(true);
                phishingPanel.GetComponent<Fake_login_panel>()?.Show(); //riattiva il pannello e i campi in unico posto
            }
            if (mailPanel != null)
            {
                mailPanel.SetActive(false);
            }
            return;
        }
    }
    
}
