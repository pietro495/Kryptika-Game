using UnityEngine;

public class FogliettoPsw : MonoBehaviour
{

    [Header("Apertura Foglietto")]
    [SerializeField] GameObject fogliettoPsw;
    [SerializeField] KeyCode openMailKey = KeyCode.Space; // tasto per aprire la mail
    [SerializeField] KeyCode closeMailKey = KeyCode.Escape;
    [SerializeField] newPlayerController currentPlayer;
    [SerializeField] PanelToUnlockPc loginPanel;
    bool isOpen;


    void Awake()
    {
        if(loginPanel != null)
        {
            loginPanel.Hide();
        }
        if (fogliettoPsw != null)
        {
            fogliettoPsw.SetActive(false);
        }
    }
    
    void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (Input.GetKeyDown(closeMailKey))
        {
            CloseFoglietto();
         
        }
    }

    public void OpenFoglietto(newPlayerController player)
    {
        isOpen = true;
        currentPlayer = player;

        if (fogliettoPsw != null)
        {
            fogliettoPsw.SetActive(true);
        }
        if(currentPlayer != null)
        {
            currentPlayer.SetControlLock(true);
        }
    }

    void CloseFoglietto()
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        if (fogliettoPsw != null)
        {
            fogliettoPsw.SetActive(false);
        }
        if (currentPlayer != null)
        {
            currentPlayer.SetControlLock(true);
        }

        ShowLogin();
    }
    
    
    public void ShowLogin()
    {
        if (loginPanel == null)
        {
            Debug.Log("pannello voto");
            return;
        }
        //CloseFoglietto();     // nascondi la mail
        loginPanel.Show();    // il pannello login si occupa del resto
    }
    

}
