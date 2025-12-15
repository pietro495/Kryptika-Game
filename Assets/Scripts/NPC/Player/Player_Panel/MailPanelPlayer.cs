using UnityEngine;
using UnityEngine.Rendering;

public class MailPanelPlayer : MonoBehaviour
{
    [Header ("UI")]
    [SerializeField] GameObject panelMail;

    [Header ("GameObj")]
    [SerializeField] GameController gameController;

    [Header ("Player")]
    [SerializeField] newPlayerController currentPlayer;

    [Header("Dialogo apertura mail")]
    [SerializeField] DialogueAsset dialogBeforeAnalyze;

    [Header ("Dialogo a chisura della mail")]
    [SerializeField] DialogueAsset dialogFinale;

    bool letto= false;
    
    bool seenLink, seenOggetto, seenAnsia,seenMittente,seenCredenziali;

    void Awake()
    {
        panelMail?.SetActive(false);
    }

    public void OpenMail(newPlayerController player)
    {
        currentPlayer = player;
        Debug.Log("Apertura della dialog box");
        if(!letto)
        {
            panelMail.SetActive(true);
            gameController.StartDialogue(dialogBeforeAnalyze, GameState.ReadMail);
            letto=true;
        }
        if(letto==true)
        {
        seenLink = seenOggetto = seenAnsia = seenMittente = seenCredenziali = false;
       // panelMail.SetActive(true);
        currentPlayer.SetControlLock(true);
        }
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
    {
        Mark("credenziali");

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
        panelMail.SetActive(false);
        currentPlayer.SetControlLock(false);
        gameController.StartDialogue(dialogFinale,GameState.PhishingClick);
    }
}
