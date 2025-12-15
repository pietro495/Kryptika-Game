using UnityEngine;

public class Mail_Panel_Professor : MonoBehaviour
{
    [Header ("Tasti di Open/Close")]
    [SerializeField] KeyCode openMailKey = KeyCode.Space; // tasto per aprire la mail

    [Header ("NPC")]
    [SerializeField] Professor currentProfessor;
    [SerializeField] newPlayerController currentPlayer;


    [Header ("Object Panel")]
    [SerializeField] GameObject mailUI;                   // pannello UI della mail
    [SerializeField] Fake_login_panel fake_login_panel;


    //Variabili booleane
    bool isOpen;
    bool isOpenPlayer;
    bool seenLink, seenOggetto,seenAnsia;




    void Awake()
    {
        if(mailUI!=null)
        {
            mailUI.SetActive(false);
        }

        if (fake_login_panel != null)
        {
            fake_login_panel.Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            return;
        }
        
        if(Input.GetKeyDown(openMailKey))
        {
            OpenMail(currentProfessor);
        }
    }

    public void OpenMail(Professor professor)
    {
        Debug.Log("current professore ", currentProfessor);
        currentProfessor  = professor;
        isOpen = true;

        if (mailUI != null)
            mailUI.SetActive(true);
        
        //blocca il movimento del professore
        if (currentProfessor != null)
        {
            currentProfessor.SetControlLock(true); //se true allora blocca il moviment (animator is moving ->false)
        }
    }


    void CloseMail()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        if (mailUI != null)
        {
            mailUI.SetActive(false);
        }

        if (gameObject != null)
        {
            currentProfessor.SetControlLock(false);
        }
    }
    
    public void ShowLoginProfessor()
    {
        if(GameController.Instance.CanPerfom(GameState.ProfessorAction))
        {
            //Debug.Log("valore della bool nel login del prof: "+seenLink);
            CloseMail();
            fake_login_panel.Show();     
        }
                   
    }


}
