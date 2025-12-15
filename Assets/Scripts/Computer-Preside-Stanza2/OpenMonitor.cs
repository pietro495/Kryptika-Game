using UnityEngine;

public class OpenMonitor : MonoBehaviour,Interactable
{

    [SerializeField] GameController gameController;
    [SerializeField] FogliettoPsw fogliettoPsw;
    [SerializeField] newPlayerController newPlayerController;
    public void Interact()
    {
     if(GameController.Instance.CanPerfom(GameState.Psw1MiniGame) && fogliettoPsw != null)
        {
            Debug.Log("ho aperto il monitor");
            fogliettoPsw.OpenFoglietto(newPlayerController);
        }   
    }
}
