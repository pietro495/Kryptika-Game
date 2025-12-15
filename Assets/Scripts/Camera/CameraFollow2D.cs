
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Transform playerTarget;
    [SerializeField] Transform professorTarget;

    [SerializeField]  public float FollowSpeed = 2f;

    Transform target = null;

    void LateUpdate()
    {
        if (gameController == null)
            return;

        switch (gameController.CurrentState)
        {
            case GameState.IntroDialog:
            case GameState.ProfessorAction:
                target = professorTarget;
                break;

            case GameState.GoToProfessor:
            case GameState.FreeRoam:
            case GameState.ReadMail:
            case GameState.ParlaConPreside:
           // case GameState.Psw1MiniGame:
            case GameState.ParlaSara:
            case GameState.DoorExit:
            case GameState.CambiaPassword:
            case GameState.MiniGameInteractable:
            case GameState.Puzzle:
            case GameState.PhishingClick:
            case GameState.SmishingMiniGame:
                target = playerTarget;
                break;
            default:
                target = transform; //nessun follow
                break;
            
        }
        
        if(target != null)
        {
            //calcolo le posizioni del mio target in modo che la camera lo possa seguire
            Vector3 newPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            //per rendere il movimento morbido uso la Lerp
            transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed*Time.deltaTime);
        }
    }
}
