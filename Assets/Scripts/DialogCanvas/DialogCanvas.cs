using UnityEngine;

public class DialogUICanvasToggle : MonoBehaviour
{
    [SerializeField] GameObject dialogCanvas;

    void Awake() => dialogCanvas.SetActive(false);

    public void OpenDialogBox()
    {
        dialogCanvas.SetActive(true);
        // qui puoi anche chiamare il DialogTrigger o DialogManager
    }
}
