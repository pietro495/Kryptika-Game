using UnityEngine;

public class monitor_obj_prefab : MonoBehaviour, Interactable
{
    [Header("figlio monitor icon")]
    [SerializeField] Interactionicon interactionicon;

    [Header ("Professore")]
    [SerializeField] Mail_Panel_Professor mailPanel; //professore
    [SerializeField] Professor professor;

    [Header ("Player")]
    [SerializeField] newPlayerController newPlayerController;
    [SerializeField] MailPanelPlayer mailPanelPlayer;

    [Header("GameController")]
    [SerializeField] GameController gameController;

    [Header ("Dialog")]
    [SerializeField] DialogueAsset mailOpenedDialogue;

    [SerializeField] GameState nextStateAfterMailOpened; //professor action

    [Header("Unlock Pc Scena 2")]
    [SerializeField] UnlockPcFlow unlockPc;

    [Header("Apri Monitor Istruzioni Scena 5")]
    [SerializeField] OpenInstructions openInstructions;

    
    [Header("Mini Game ")] //da dividere in base alla scena per rendere tutto piu ordinato da fare
   // [SerializeField] PhishingInteractableManager phishingInteractManager;
    [SerializeField] PasswordMiniGameManager PasswordMiniGameManager;
    [SerializeField] PuzzleManager puzzleManager;
    [SerializeField] EmailQuizManager emailQuizManager;


    public Interactionicon GetInteractionIcon()
    {
        return interactionicon;
    }


    public void Interact()
    {
        //SCENA 1
        if (GameController.Instance.CanPerfom(GameState.ProfessorAction))
        {
            Debug.Log("sono entrato nello stato PROFFESSOR" + GameController.Instance.CurrentState);
            mailPanel?.OpenMail(professor);
            gameController.StartDialogue(mailOpenedDialogue, GameState.ProfessorAction);
        }
        //Arthur analizza
        else if (GameController.Instance.CanPerfom(GameState.ReadMail))
        {
            Debug.Log("sono entrato nello stato READMail per analizzare la mail:" + GameController.Instance.CurrentState);
            //gameController.StartDialogue(mailOpenedDialogue, GameState.ReadMail);
            //Debug.Log($"Mostro dialog: {mailOpenedDialogue?.name} -> {mailOpenedDialogue?.Lines[0]}");
            mailPanelPlayer?.OpenMail(newPlayerController);
        }
        /*
        else if(GameController.Instance.CanPerfom(GameState.PhishingClick))
        {
            Debug.Log("Avvio Game Ricostruzione della mail ");
            emailQuizManager.StartQuiz();
        }
        */
        
        //SCENA 2
        if(GameController.Instance.CanPerfom(GameState.UnlockPc))
        {
            Debug.Log("Avvio Della Schermata per sbloccare il Pc");
           // gameController.StartDialogue(mailOpenedDialogue, GameState.UnlockPc);
            unlockPc.OpenPanel();
        }



        //Scena 3 per ora nulla!
        
        /*
        

        if (GameController.Instance.CanPerfom(GameState.Psw1MiniGame) && fogliettoPsw != null)
        {
            Debug.Log("sono entrato nello stato attuale:" + GameController.Instance.CurrentState);
            //Dialogo per avvisare che la psw sta scritta sul monitor appena si avvia il mini game
            gameController.StartDialogue(mailOpenedDialogue, GameState.Psw1MiniGame);
            Debug.Log("ho aperto il monitor della scena 2");
            fogliettoPsw.OpenFoglietto(newPlayerController);
        }   
        */
        //SCENA 4
        if(GameController.Instance.CanPerfom(GameState.CambiaPassword))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            PasswordMiniGameManager.StartMiniGame();
        }
        /*
        if(GameController.Instance.CanPerfom(GameState.MiniGameInteractable))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            openInstructions.OpenInstructionsPanel();
        }
        */
        
        if(GameController.Instance.CanPerfom(GameState.Puzzle))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            puzzleManager.StartPuzzle();
        }


        //Scena 5
        if(GameController.Instance.CanPerfom(GameState.MiniGameInteractable))
        {
            Debug.Log("Richiamo la Interact del miniGame Interactable sul monitor in scena 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.MiniGameInteractable);
            openInstructions.OpenInstructionsPanel();
        }

        //Da farlo cambiare poi in Door Exit
        




        //Scena 6 Per ora nulla!

        
    }
}

