using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
/*
stati:

//SCENA 1
Introdialog: Dialoghi introduttivi
ProfessorAction: Il Professore dopo i dialoghi introduttivi ha controllo e apre la mail
Dialog: Il gioco va in dialog dopo aver visto la mail
GoToProfessor: Il player deve parlare con il professore prima di mostrare la mail di phishing
ReadMail: Il player legge la mail per spiegare perchè è di phishing, clicca sul monitor e si apre la mail
PhishingMiniGame: Dopo che il player ha analizzato la mail parte un dialogo automatico e poi vorrei far partire il gioco
DoorEXIT: Il player finisce il minigame sul phishing ed esce dalla stanza

//SCENA 2
ParlaConPreside: Player deve parlare con preside per poi avviare il minigame
UNLOCKPC: Stato in cui si apre il pc e drag & drop e poi si avvia mini game
PhishingQuiz: 2 Mini Game sul phishing, costruzione di mail di phishing.

//SCENA 3
ParlaConSara: Il player esce dalla stanza del preside, va nel corridoio e parla con Sara Kryptika la responsabile IT della scuola
DoorExit: Dopo aver parlato con Sara il player deve uscire dal corridoio ed entrare nella scena 4

//SCENA 4
CambiaPassword: Il player sta nella scena 4 e deve cambiare la password; avvio del Mini Game sul cambio psw.
Puzzle: Scena 4, dopo aver cambiato password si scovano indizi sul presunto hacker della scuola

//5 SCENA
MiniGameInteractable: Mini Game sul phishing interattivo
*/

public enum GameState 
{
    //1Scena
    FreeRoam,
    Dialog, 
    IntroDialog, 
    ProfessorAction, 
    GoToProfessor,
    ReadMail,
    DoorExit,
    PhishingClick, //ricostruzione della mail

    //Inizio 2 scena
    ParlaConPreside,
    UnlockPc,
    PhishingQuiz,
    Psw1MiniGame,
    //dopo dovrebbe esserci un door exit

    //Inizio 3 Scena
    ParlaSara,
    //DOOR EXIT

    //SCENA 4
    CambiaPassword,
    Puzzle,

    //5 Scena
    MiniGameInteractable,

    //Scena 6
    SmishingMiniGame
}

public class GameController : MonoBehaviour
{
    public static GameController  Instance { get; private set;}
    public GameState CurrentState => state; //mi ritorna il valore di state

    [Header("NPC")]
    [SerializeField] newPlayerController playerController;
    [SerializeField] Professor professor;
    //[SerializeField] Preside preside;

    [Header("Dialoghi")]
    [SerializeField] private DialogManager dialogManager;

    [SerializeField] DialogueAsset introDialogue;

    [Header("MiniGame")]
    [SerializeField] PhishingManager phishingManager; //quizzone
    [SerializeField] EmailQuizManager emailQuizManager; //costruzione mail
    
    [Header("Stato")]
    [SerializeField] GameState state;

    [Header("Stato dopo dialogo")]
    [SerializeField] GameState nextStateAfterDialogue = GameState.FreeRoam;

    bool game1Started;
    bool phishingStarted;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var session = GameSession.Instance; //mi restituisce l'unica istanza esistente di gamesession e mi permette di accedere ai dati salvati in gamesession
//        Debug.Log($"Pending: {session.HasPendingConfig}, dlg: {session.NextDialogue}");

        if (session != null && session.HasPendingConfig)
        {
            Debug.Log($"Pending: {session.HasPendingConfig}, dlg: {session.NextDialogue}");
            state = session.NextState;

            //dovrebbe servirmi se voglio avviare dei dialoghi quando cambio scena (?)
            if (session.NextDialogue != null)
            {
                StartDialogue(session.NextDialogue, session.NextState);
                session.ClearConfig();
                return;
            }
            session.ClearConfig();
        }
        else
        {

            // fallback (es. intro della prima scena)
            
            state = GameState.GoToProfessor;
            //if (introDialogue != null) StartDialogue(introDialogue, GameState.ProfessorAction);
            
            
            //state = GameState.PhishingMiniGame;
            /*
            if (state == GameState.IntroDialog && introDialogue != null)
                StartDialogue(introDialogue, GameState.ProfessorAction);
            */
        }

        dialogManager.onShowDialogue += HandleDialogueOpened;
        dialogManager.onHideDialogue += HandleDialogueClosed;
    }

    public GameState setState(GameState newState)
    {
        state = newState;
        return state;
    }


    void Update()
    {
        switch (state)
        {
            case GameState.Dialog:
                dialogManager.HandleUpdate();
                break;    
                
            case GameState.IntroDialog:
                //dialogManager.HandleUpdate();
                StartDialogue(introDialogue, GameState.ProfessorAction);
                break;

            case GameState.FreeRoam:
                Debug.Log("sei nello stato FreeRoam");
                playerController.HandleUpdate();
                break;

            case GameState.ProfessorAction: //PROF APRE LA MAIL
                Debug.LogWarning("sei nello stato ProfessorAction");
                professor.HandleUpdate();
                playerController.ChangeRb(true);
                break;



            case GameState.GoToProfessor: //PLAYER DEVE PARLARE CON IL PROFESSORE
                Debug.LogWarning("sei nello stato GoToProfessor");
                playerController.HandleUpdate();
                professor.ChangeRb(true); //prof to static
                playerController.ChangeRb(!true); //player to dinamic
                
                //StartDialogue(asset, nextStateAfterDialogue); // o quello che ti serve
                break;

            case GameState.ReadMail:
                Debug.LogWarning("sei nello stato ReadMail");
                playerController.HandleUpdate();
                break;

            case GameState.PhishingClick:
                Debug.LogWarning("sei nello stato PhishingClick");
                if(!game1Started)
                {
                    game1Started = true;
                    emailQuizManager.StartQuiz();
                }
                break;

            case GameState.DoorExit:
                Debug.Log("Sei nello stato DoorExit");
                playerController.HandleUpdate();
                break;


            //2 Scena
            case GameState.ParlaConPreside:
                 Debug.LogWarning("sei nello stato ParlaConPreside");
                playerController.HandleUpdate();
                break;

            case GameState.UnlockPc:
                Debug.LogWarning("sei nello stato UnlockPc");
                playerController.HandleUpdate();
                break;
            
            case GameState.PhishingQuiz:
                Debug.LogWarning("sei nello stato del PhishingMinigame1");
                //avvio del minigame del phishing in automatico.
                if(!phishingStarted)
                {                  
                phishingManager.StartMiniGame();
                phishingStarted = true;
                }
                break;
                

            //SCENA 3
            case GameState.ParlaSara:
                Debug.LogWarning("sei nello stato ParlaSara");
                playerController.HandleUpdate();
                break;

            /*
            case GameState.WritePassword:
                Debug.LogWarning("sei nello stato WritePassword");
                playerController.HandleUpdate();
                break;
            */

            //SCENA 4
            
            case GameState.CambiaPassword:
                Debug.LogWarning("sei nello stato CambiaPassword");
                //StartDialogue(, nextStateAfterDialogue);
                playerController.HandleUpdate();
                break;
            
            case GameState.Puzzle:
                Debug.LogWarning("sei nello stato Puzzle");
                playerController.HandleUpdate();
                break;


            //Scena 5
            case GameState.MiniGameInteractable:
                Debug.LogWarning("sei nello stato MiniGameInteractable");
                playerController.HandleUpdate();
                break;


            //Scena 6
            case GameState.SmishingMiniGame:
                Debug.LogWarning("sei nello stato SmishingMiniGame");
                playerController.HandleUpdate();
                break;
        }
    }


    /*
    3>=2 true

    read Mail = 4, 4>2 quindi mi riapri ildialo
    true se currentstate == stato
    goTOProfessore = 
    */
    public bool CanPerfom(GameState requireState)
    {
        return CurrentState == requireState; //confronti di enum che sono numeri interi
    }

    public void StartDialogue(DialogueAsset asset,GameState stateAfter)
    {
        if (asset == null || dialogManager == null)
            return;

        nextStateAfterDialogue = stateAfter;
       
        state = GameState.Dialog;
        StartCoroutine(dialogManager.ShowDialogue(asset));
    }

    //avvio la conversazione col professore o una qualsiasi conversazione
    public void StartPlayerProfessorConversation(DialogueAsset dialogue, GameState stateAfter)
    {
        StartDialogue(dialogue, stateAfter);
    }


    void HandleDialogueOpened()
    {
        if (state != GameState.IntroDialog)
            state = GameState.Dialog;
    }

    void HandleDialogueClosed()
    {
        state = nextStateAfterDialogue;
        nextStateAfterDialogue = GameState.FreeRoam;
    }

    /*
    public void OnPhishingMinigameClosed()
    {
        phishingStarted = false;
    }
    */
    void OnDestroy()
    {
        if (dialogManager == null) return;

        dialogManager.onShowDialogue -= HandleDialogueOpened;
        dialogManager.onHideDialogue -= HandleDialogueClosed;
    }
}

