using UnityEngine;

public class PasswordMiniGameManager : MonoBehaviour
{
    [SerializeField] GameObject keyboardRoot;          // pannello UI della tastiera (Canvas/Panel), tenuto OFF di default
    [SerializeField] PasswordValidator validator;      // già in scena, aggiorna regole/slot
    [SerializeField] newPlayerController player;       // opzionale: per bloccare il movimento

    bool isRunning;

    void Awake()
    {
        if (keyboardRoot) keyboardRoot.SetActive(false);
    }

    public void StartMiniGame()
    {
        if (isRunning) return;
        if (keyboardRoot) keyboardRoot.SetActive(true);
        player?.SetControlLock(true);
        // opzionale: azzera lo stato del validator/slot se serve
        validator?.RebuildPassword();
        isRunning = true;
    }

    public void CloseMiniGame()
    {
        if (!isRunning) return;
        if (keyboardRoot) keyboardRoot.SetActive(false);
        player?.SetControlLock(false);
        isRunning = false;
    }
}
