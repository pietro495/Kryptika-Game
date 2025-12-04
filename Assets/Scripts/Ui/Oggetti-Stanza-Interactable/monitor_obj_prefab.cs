using UnityEngine;

public class monitor_obj_prefab : MonoBehaviour, Interactable
{
    [SerializeField] Mail_Panel mailPanel;
    [SerializeField] Professor professor;
    [SerializeField] GameController gameController;
    [SerializeField] DialogueAsset mailOpenedDialogue;
    [SerializeField] GameState nextStateAfterMailOpened; //professor action
    [SerializeField] FogliettoPsw fogliettoPsw;
    [SerializeField] newPlayerController newPlayerController;
    [SerializeField] MailPanelPlayer mailPanelPlayer;
    [SerializeField] PhishingInteractableManager phishingInteractManager;
    [SerializeField] PasswordMiniGameManager PasswordMiniGameManager;
    [SerializeField] PuzzleManager puzzleManager;


    public void Interact()
    {
        //SCENA 1
        if (GameController.Instance.CanPerfom(GameState.ProfessorAction))
        {
            Debug.Log("sono entrato nello stato PROFFESSOR" + GameController.Instance.CurrentState);
            mailPanel?.OpenMail(professor);

            gameController.StartDialogue(mailOpenedDialogue, GameState.ProfessorAction);
        }
        else if (GameController.Instance.CanPerfom(GameState.ReadMail))
        {
            Debug.Log("sono entrato nello stato READMail per analizzare la mail:" + GameController.Instance.CurrentState);
            mailPanelPlayer?.OpenMail();
        }
        
        //SCENA 2
        if (GameController.Instance.CanPerfom(GameState.Psw1MiniGame) && fogliettoPsw != null)
        {
            Debug.Log("sono entrato nello stato attuale:" + GameController.Instance.CurrentState);
            //Dialogo per avvisare che la psw sta scritta sul monitor appena si avvia il mini game
            gameController.StartDialogue(mailOpenedDialogue, GameState.Psw1MiniGame);
            Debug.Log("ho aperto il monitor della scena 2");
            fogliettoPsw.OpenFoglietto(newPlayerController);
        }   
        
        //SCENA 4
        if(GameController.Instance.CanPerfom(GameState.CambiaPassword))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            PasswordMiniGameManager.StartMiniGame();
        }
        
        if(GameController.Instance.CanPerfom(GameState.MiniGameInteractable))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            phishingInteractManager.StartMiniGame();
        }
        
        
        if(GameController.Instance.CanPerfom(GameState.Puzzle))
        {
            Debug.Log("Sto per aprire monitor stanza 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.CambiaPassword);
            puzzleManager.StartPuzzle();
        }

        /*
        if(GameController.Instance.CanPerfom(GameState.MiniGameInteractable))
        {
            Debug.Log("Richiamo la Interact del miniGame Interactable sul monitor in scena 4");
            //gameController.StartDialogue(mailOpenedDialogue, GameState.MiniGameInteractable);
            phishingInteractManager.StartMiniGame();
        }
        */
    }
    
}

