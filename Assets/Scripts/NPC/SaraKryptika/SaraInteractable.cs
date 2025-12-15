using UnityEngine;

public class SaraPlayer : MonoBehaviour,Interactable
{
    [Header("Dialogue")]
    [SerializeField] DialogueAsset dialogueAsset;
    [SerializeField] GameState stateAfterDialogue;

    [Header("GameController")]
    [SerializeField] GameController gameController;

    [Header("IconSara")]
    [SerializeField] Interactionicon interactionicon;
 
    public Interactionicon GetInteractionIcon()
    {
        return interactionicon; 
    }

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
