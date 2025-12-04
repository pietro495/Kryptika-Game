// Assets/Scripts/Dialog/DialogTrigger.cs

/*
Spiegazione delle mode:
Mail_Panel:Dialogo antecedente del Professore all'apertuta della mail la 1 volta sullo schermo
Phishing:Dialogo del player con il professore che gli spiega cosa è accaduto
*/
using UnityEngine;

public class DialogTrigger : MonoBehaviour, Interactable
{
    //[SerializeField] Dialog dialog;
    public enum DialogueTriggerMode { DirectAsset,Mail_Panel, Phishing, Preside,MiniGamePhishing,AfterPsw1Game,BeforePuzzle}
    [SerializeField] DialogueTriggerMode mode = DialogueTriggerMode.DirectAsset;
    [SerializeField] DialogueAsset dialogueAsset;

    [SerializeField] GameController gameController;
    [SerializeField] GameState stateAfterDialogue = GameState.FreeRoam;


    public void dialogue_btn()
    {
        if (dialogueAsset != null && gameController != null)
        {
            gameController.StartDialogue(dialogueAsset, GameState.GoToProfessor);
        }
    }

    //Player apre link de phishing nella prima scena
    public void player_open_link()
    {
        if (gameController.CurrentState == GameState.GoToProfessor)
        {
            gameController.StartDialogue(dialogueAsset, GameState.GoToProfessor);
        }
    }


    public void Interact()
    {
        switch (mode)
        {
            //Dialogo assegnato al prof quando apre la mail 
            case DialogueTriggerMode.Mail_Panel:
                if (gameController.CurrentState == GameState.ProfessorAction)
                gameController.StartDialogue(dialogueAsset, stateAfterDialogue);
                    break;

            //Dialoghi per spiegare perchè la mail è di phishing, analisi del player delle caratteristiche
            case DialogueTriggerMode.Phishing:
                if(GameController.Instance.CanPerfom(GameState.ReadMail))
                gameController.StartDialogue(dialogueAsset,stateAfterDialogue);
                    break;

            //DIALOGO ALLA FINE DEL GIOCO DEL PHISHING, DOOR EXIT PER 2 SCENA
            case DialogueTriggerMode.MiniGamePhishing:
                if(GameController.Instance.CanPerfom(GameState.PhishingMiniGame))
                    gameController.StartDialogue(dialogueAsset,GameState.DoorExit);    
                break;

            //DIALOGO TRA PLAYER E PRESIDE NELLA 2 SCENA INIZIALE
            case DialogueTriggerMode.Preside:
                if(GameController.Instance.CanPerfom(GameState.ParlaConPreside))
                gameController.StartDialogue(dialogueAsset, stateAfterDialogue);
                    break;

        
            //DOPO CHE FINISCE 1 GAME SUL PSW MANAGAMENT, IMPOSTO STATO A DOOR EXIT PER ANDARE NELLA 3 SCENA
            case DialogueTriggerMode.AfterPsw1Game:
                    if(GameController.Instance.CanPerfom(GameState.Psw1MiniGame))
                    gameController.StartDialogue(dialogueAsset,stateAfterDialogue);    
                break;


            //Stanza 4: Dialogo che si attiva quando si chiude il 1 mini game sul cambio password 
            case DialogueTriggerMode.BeforePuzzle:
                if(GameController.Instance.CanPerfom(GameState.CambiaPassword))
                    gameController.StartDialogue(dialogueAsset,stateAfterDialogue);
                break;

            case DialogueTriggerMode.DirectAsset:
            default:
                if (dialogueAsset != null)
                gameController.StartDialogue(dialogueAsset, stateAfterDialogue);
                    break;
        }
    }
    

    
}




