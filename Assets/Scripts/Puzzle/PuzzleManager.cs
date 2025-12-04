// PuzzleManager.cs
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] SlotPuzzle[] slots;
    [SerializeField] DraggableBlock[] blocks;
    [SerializeField] GameObject panel;

    bool dataOk, ipOk, postOk;

    void Awake()
    {
        panel?.SetActive(false);
    }

    void Start()
    {
        // inizializza riferimenti
        foreach (var s in slots) s.Init(this);
        foreach (var b in blocks) b.Init(this);
    }

    public void StartPuzzle()
    {
        panel?.SetActive(true);
    }
    
    public void NotifySlotCorrect(SlotPuzzle.SlotType type)
    {
        switch (type)
        {
            case SlotPuzzle.SlotType.Data: dataOk = true; break;
            case SlotPuzzle.SlotType.IP: ipOk = true; break;
            case SlotPuzzle.SlotType.Postazione: postOk = true; break;
        }
        if (dataOk && ipOk && postOk)
            PuzzleCompletato();
    }

    void PuzzleCompletato()
    {
        Debug.Log("Puzzle completato!");
        // qui puoi notificare il GameManager, avviare un dialogo, caricare scena, ecc.
        panel?.SetActive(false);
    }

    // opzionale: reset generale
    public void ResetPuzzle()
    {
        dataOk = ipOk = postOk = false;
        foreach (var s in slots) s.ResetSlot();
        // se vuoi riportare i blocchi alla posizione iniziale, gestiscili qui
    }
}
