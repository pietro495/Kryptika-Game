using UnityEngine;

public class Hacker : MonoBehaviour, Interactable
{

    [SerializeField] GameController gameController;
   // [SerializeField] DialogueAsset dialog;

   [SerializeField] OpenInstructions6 openInstructions6;

    [SerializeField] SMSMiniGame smsminigame;

    public void Interact()
    {
        if(GameController.Instance.CanPerfom(GameState.SmishingMiniGame))
        {
            //gameController.StartDialogue(dialog,GameState.SmishingMiniGame);
            Debug.Log("avvio del gioco smishing, scena 6 file hacker");
            openInstructions6.OpenInstructionsPanel();
        }
        else
        {
            Debug.Log("non sei nello stato"+gameController.CurrentState);
        }    
    }

}
