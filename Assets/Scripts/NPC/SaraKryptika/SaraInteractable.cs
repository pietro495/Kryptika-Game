using UnityEngine;

public class SaraPlayer : MonoBehaviour,Interactable
{

    [SerializeField] DialogueAsset dialogueAsset;
    [SerializeField] GameController gameController;
    [SerializeField] GameState stateAfterDialogue;
 

    public void Interact()
    {
        if(GameController.Instance.CanPerfom(GameState.ParlaSara))
        {
            gameController.StartDialogue(dialogueAsset,stateAfterDialogue);
        }
        else
        {
            Debug.Log("non sei nello stato"+gameController.CurrentState);
        }
    }


}
