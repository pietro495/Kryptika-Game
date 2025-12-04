using UnityEngine;

public class Preside : MonoBehaviour, Interactable
{
    [SerializeField] DialogueAsset dialogueAsset;
    [SerializeField] GameController gameController;
    [SerializeField] GameState stateAfterDialogue;
    public void Interact()
    {
        if (GameController.Instance.CanPerfom(GameState.ParlaConPreside))
        {
            gameController.StartDialogue(dialogueAsset, stateAfterDialogue);
        }
        else Debug.Log("non sei nello stato Parla con preside");
    }
}
