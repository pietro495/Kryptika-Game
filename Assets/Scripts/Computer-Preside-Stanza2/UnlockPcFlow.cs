using UnityEngine;

public class UnlockPcFlow : MonoBehaviour
{
    [Header("GameController")]
    [SerializeField] GameController gameController;

    [Header("Dialogo")]
    [SerializeField] DialogueAsset dialog;

    [Header("Dialogo Manager")]
    [SerializeField] DialogManager dialogManager;


    [Header("Pannello Principale")]
    [SerializeField] GameObject panelRoot;

    [Header("Pannello di Istruzioni")]
    [SerializeField] GameObject panelInstructions;

    [Header("PostIt")]
    [SerializeField] PostIt postIt;

    private bool dialogActive;


    void OnEnable()
    {
        dialogManager.onDialogueCompleted += SbloccaPostIt;
    }

    void OnDisable()
    {
        dialogManager.onDialogueCompleted -= SbloccaPostIt; //lo disiscrivo, non verrà chiamato quando l'evento scatta
    }

    public void SbloccaPostIt()
    {
        postIt.SetDraggable(true);
        return;
    }

    void Awake()
    {
        panelRoot.SetActive(false);
        panelInstructions.SetActive(false);
    }

    public void OpenPanel()
    {
        if(!dialogActive)
        {
            Debug.Log("valore dialogActive prima" +dialogActive);
            panelRoot.SetActive(true);
            postIt.SetDraggable(false);
            //OnDisable();
            gameController.StartDialogue(dialog,GameState.UnlockPc);
            //dialogActive =true;            
        }
        if(dialogActive)
        {
            postIt.SetDraggable(true);
        }

    }



    //apertura istruzioni dopo drag & drop del post-it
    public void OnPostItPlaced()
    {
        postIt.Hide();
        panelInstructions.SetActive(true);
    }
  


}
