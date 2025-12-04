
using UnityEngine;
public class ProfInteractable : MonoBehaviour, Interactable
{
    [SerializeField] Professor professor;
    [SerializeField] GameController gameController;
    [SerializeField] DialogueAsset dialogueAsset;
    [SerializeField] GameState nextStateAfterDialogue = GameState.ReadMail;

   // bool hasSpoken;

    void Awake()
    {
        if (professor == null)
            professor = GetComponentInParent<Professor>();
    }

    public void Interact()
    {
        if (GameController.Instance.CanPerfom(GameState.GoToProfessor))
        {
            var controller = GameController.Instance;

            Debug.Log("currentSTATE ====" + gameController.CurrentState);
            Debug.Log("STAI NEL GO TO PROFESSORE PER PARLARE COL PROF");
            //if (professor != null && gameController.CurrentState == GameState.GoToProfessor)
            //{
            // Debug.Log("Hai interagito con il figlio del professore, ora partirà il dialogo");
            // hasSpoken = true;
            //}
            gameController.StartDialogue(dialogueAsset, nextStateAfterDialogue); // o quello che ti serve
        }
        
    }
}
