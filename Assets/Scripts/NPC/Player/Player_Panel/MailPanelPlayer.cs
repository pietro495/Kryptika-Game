using UnityEngine;
using UnityEngine.Rendering;

public class MailPanelPlayer : MonoBehaviour
{
   // [Header ("GameObj")]
    [SerializeField] GameObject mailUi;
   // [Header ("Player")]
    [SerializeField] newPlayerController currentPlayer;
   // [Header ("Gamecontroller")]
    [SerializeField] GameController gameController;

    [Header ("Dialogo a chisuura della mail")]
    [SerializeField] DialogueAsset dialog;
    
    bool seenLink, seenOggetto, seenAnsia,seenMittente,seenCredenziali;

    void Awake()
    {
        mailUi?.SetActive(false);
    }

    
    public void OnClickLink()
    {
        //gameController.StartDialogue(dialogueAsset,GameState.ReadMail);
        Mark("link");   
    }
     
    public void OnClickObj()
    {
       // gameController.StartDialogue(dialogueAsset,GameState.ReadMail);
        Mark("oggetto");
    }     
    public void OnClickAnsia()
    {       
        // gameController.StartDialogue(dialogueAsset,GameState.ReadMail);
        Mark("ansia");
    }

    public void onClickMittente()
    {
        Mark("mittente");
    }
    public void onCredenziali()
    {Mark("credenziali");}
    public void OpenMail()
    {
        seenLink = seenOggetto = seenAnsia = false;
        mailUi.SetActive(true);
        currentPlayer.SetControlLock(true);
    }

    public void Mark(string id)
    {
        switch (id)
        {
            case "link": seenLink = true;break;
            case "oggetto": seenOggetto = true; break;
            case "ansia" : seenAnsia = true; break;
            case "mittente" : seenMittente = true; break;
            case "credenziali" : seenCredenziali = true; break;

        }
        Debug.Log("$ valore LINK"+seenAnsia+"valore OGGETTO"+seenOggetto+"VALORE ANSIA"+seenAnsia +"Valore Mittente:"+seenMittente);
        if(seenLink & seenOggetto && seenAnsia && seenMittente && seenCredenziali)
        {
            CloseMail();
        }
    }
//IMPORTANTE CAMBIO DI STATO A PHISHING MINI GAME TRAMITE DIALOGO QUI!!!
    public void CloseMail()
    {
        mailUi.SetActive(false);
        currentPlayer.SetControlLock(false);
        gameController.StartDialogue(dialog,GameState.PhishingMiniGame);
        
    }
}
