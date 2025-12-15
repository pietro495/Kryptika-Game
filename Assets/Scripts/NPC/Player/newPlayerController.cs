using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class newPlayerController : MonoBehaviour
{
    [Header("Interazione monitor")]
    //[SerializeField] monitor_obj_prefab monitorInteract;
    [SerializeField] monitor_obj_prefab monitor;
    [SerializeField] Transform monitorTransform;          // assegna il computer (o un empty) dall’Inspector
    [SerializeField] float monitorInteractionRadius = 5f; // raggio entro cui abilitare l’interazione    [SerializeField] KeyCode interactKey = KeyCode.Space;
    [SerializeField] KeyCode interactKey = KeyCode.Space;
    KeyCode openMonitor = KeyCode.B;
    [SerializeField] float movementSpeed = 5f;
  
   
    [Header ("Layers")]
    public LayerMask solidObjectsLayer;
    public LayerMask InteractableLayer;
    public LayerMask DialogLayer;
    public LayerMask Characters;
    


    [Header("Interazione figlio del Professore")]
    [SerializeField] ProfInteractable professorInteractable;
    [SerializeField] int interactProf;

    [Header("Interazione Preside")]
    [SerializeField] Preside preside;

    [Header("Interazione Sara")]
    [SerializeField] SaraPlayer sara;

    [Header("Interazione Hacker")]
    [SerializeField] Hacker hacker;


    [Header("Interazione Porta")]
    [SerializeField] Door_To_Open door;
    [SerializeField] int interactDoor;

    [Header("Phishing game")]
    [SerializeField] PhishingManager phishingManager;

    [Header ("Variabili per inizializzazione del player")]
    Vector2 movement; //vettore che indica la direzione del movimento in base all'input da tastiera (1,0)
    Animator animator;
    Dialog dialog;
    Rigidbody2D rb;
    
    bool isControlLocked;
    private Interactable currentTarget;
    private Interactable lastTarget;
    //private bool parla;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    

    public Dialog GetDialog
    { get { return dialog; } }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void HandleUpdate()
    {
        
        if(!isControlLocked)
        {
          rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);


        if (movement.x != 0 || movement.y != 0)
        {

            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        }else
        {
            animator.SetBool("isMoving", false);
        }


        Detect();

        //SCENA 1
        //Player va a parlare con il professore
        if (GameController.Instance.CanPerfom(GameState.GoToProfessor)&& NearProf() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Inizierà il dialogo con il Professore");
            professorInteractable.Interact();
        }
        //Analisi della Mail
        else if(GameController.Instance.CanPerfom(GameState.ReadMail)&& NearMonitor() && Input.GetKeyDown(openMonitor))
        {
            Debug.Log("Hai cliccato sul monitor per analizzare la mail DEL Professore");
            SetControlLock(true);
            monitor.Interact();
            Debug.Log("dopo la interact per aprire la mail");
        }
        //Avvio del Game di Ricostruzione della mail
        else if(GameController.Instance.CanPerfom(GameState.PhishingClick) && NearMonitor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Stai per avviare il gioco Interattivo sul phishing");
            monitor.Interact();
        }
        //Esci dalla stanza 
        else if(GameController.Instance.CanPerfom(GameState.DoorExit) && NearDoor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Apri la porta");
            door.Interact();
        }

        //SCENA 2
        //Parla con il preside
        if(GameController.Instance.CanPerfom(GameState.ParlaConPreside) && NearPreside() && Input.GetKeyDown(interactKey))
        {
            preside.Interact();
        }
        //Vai al computer e sblocca il pc
        else if(GameController.Instance.CanPerfom(GameState.UnlockPc) && NearMonitor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Apri il monitor per sbloccare il pc");
            monitor.Interact();
        }
        //Avvio del mini game del quiz sul phishing
        else if(GameController.Instance.CanPerfom(GameState.PhishingQuiz)&& NearMonitor() && Input.GetKeyDown(openMonitor))
        {
            //Debug.Log("stato attuale" + GameController.Instance.CurrentState);
            Debug.Log("Hai cliccato sul monitor scrivere la password del preside");
            Interact();
        }

        //SCENA 3
        //Parla con Sara
        if(GameController.Instance.CanPerfom(GameState.ParlaSara))
        {
            if(Input.GetKeyDown(interactKey) && NearSara())
            {
                Debug.Log("Sei nella 3 scena, parla con Sara");
                sara.Interact();
            }
        }
            
        //SCENA 4 Riscrivi la password; Da rivedere

        if(GameController.Instance.CanPerfom(GameState.CambiaPassword) && NearMonitor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("devi aprire il monitor cliccando space");
            monitor.Interact();
        }

        if(GameController.Instance.CanPerfom(GameState.Puzzle) && NearMonitor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("devi aprire il monitor cliccando space");
            monitor.Interact();
        }

    
        //SCENA 5
        //Entra nella stanza 5 e sblocca il pc
        if(GameController.Instance.CanPerfom(GameState.MiniGameInteractable) && NearMonitor() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Stai per avviare il gioco Interattivo sul phishing");
            monitor.Interact();
        }


        //Scena 6
        
        if(GameController.Instance.CanPerfom(GameState.SmishingMiniGame) && NearHacker() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Stai per avviare il gioco Interattivo sul phishing");
            hacker.Interact();
        }
        
    }


    

    //capisce se si trova davanti ad un oggetto interagibile ovvero capisce se c'è una collisione 
    public void Interact()
    {
        //ricorda che moveX o Y dell'animator sono (-1,1) dove sta guardando il personaggio, ritorna i valori dei parametri x e y
        var facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        //calcolo il punto dove sta guardando
        var interactPos = rb.position + facingDir * 0.5f;
        var mask = InteractableLayer | DialogLayer;
        //gestione della collisione per interazioni
        var collider = Physics2D.OverlapCircle(interactPos, 0.6f, mask);
        //se collider trova oggetto tramie overlap, trova uno script(component)
        //di tipo T (interactable) attaccato all'oggetto del collider
        if (collider != null)
        {
            //currentTarget = collider.gameObject.GetComponent<Interactable>();
            collider.gameObject.GetComponent<Interactable>()?.Interact();
        }
        else
        {
            Debug.Log("Nessun collider trovato");
        }
    }

    public void Detect()
    {
        //ricorda che moveX o Y dell'animator sono (-1,1) dove sta guardando il personaggio, ritorna i valori dei parametri x e y
        var facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        //calcolo il punto dove sta guardando
        var interactPos = rb.position + facingDir*0.6f;
        var mask = InteractableLayer;
        //gestione della collisione per interazioni
        var collider = Physics2D.OverlapCircle(interactPos, 0.8f,mask);
        //se collider trova oggetto tramie overlap, trova uno script(component)
        if (collider != null)
        {
           UnityEngine.Debug.Log("Ho trovato un iconaaa");
           if(sara != null) sara.GetInteractionIcon().Show();
           if (monitor != null) monitor.GetInteractionIcon().Show();
            //collider.gameObject.GetComponent<Interactable>()?.Interact();
        }
        else if(collider == null)
        {
            UnityEngine.Debug.Log("Non trovo icone");
            if(sara != null) sara.GetInteractionIcon().Hide();
            if (monitor != null)
                monitor.GetInteractionIcon().Hide();
        }
    }
    
    
    public void SetControlLock(bool value)
    {
        //se true allora bloccati, se false muoviti
        //lock a true allora bloccati
        isControlLocked = value;
        if(value)
            animator.SetBool("isMoving", false);
    }

    //cambia Type corpo rigido: se true allora vai a static, se false rimani su dynamic
    //se non è static allora cambia, poi aggiorna valore per 
    //se istatic = true
    public void ChangeRb(bool value)
    {   
        if(value)   //qualsiasi è il valore della value tu lo imposti a statico; il suo contrario lo imposti a dynamic
        {
            Debug.Log("valore isStatic"+value);
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Debug.Log("ho cambiato type a static"); 
        }
        
        else if (!value)
        {
            Debug.Log("valore isStatic"+value);
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("Ho cambiato tipo a dynamic");
        }
        
    }
    



        bool NearSara()
    {

        bool nearsara = false;
        Vector2 posSara = sara.transform.position;
        if (Vector2.Distance(rb.position, posSara) < 5f)
        {
            Debug.Log("sei vicino a SARA");
            nearsara = true;
        }
        return nearsara;
    } 

    bool NearHacker()
    {
        bool nearHacker = false;
        Vector2 posHacker = hacker.transform.position;
        if (Vector2.Distance(rb.position, posHacker) < 5f)
        {
            Debug.Log("sei vicino a HACKER");
            nearHacker = true;
        }
        return nearHacker;
    }

    bool NearPreside()
    {

        bool nearPreside = false;
        Vector2 posPreside = preside.transform.position;
        if (Vector2.Distance(rb.position, posPreside) < 5f)
        {
            Debug.Log("sei vicino al preside");
            nearPreside = true;
        }
        return nearPreside;
    } 
    
    bool NearProf()
    {
        bool nearProf = false;
        Vector2 posProf = professorInteractable.transform.position;
        if (Vector2.Distance(posProf, rb.position) < interactProf)
        {
            Debug.Log("sei vicino al prof");
            nearProf = true;
        }
        return nearProf;

    }

    bool NearMonitor()
    {
        bool nearMonitor = false;
        Vector2 posMonitor = monitorTransform.position;
        if (Vector2.Distance(posMonitor, rb.position) < monitorInteractionRadius)
        {
            Debug.Log("sei vicino al monitor");
            nearMonitor = true;
        }
        return nearMonitor;
    }

    bool NearDoor()
    {
        bool nearDoor = false;
        if(nearDoor == false)
        {
            Debug.Log("sei lontano dalla porta");
        }
        Vector2 posDoor = door.transform.position;
        if(Vector2.Distance(rb.position,posDoor) < interactDoor)
        {
            Debug.Log("SEI VICINO alla porta, aprila");
            nearDoor = true;
        }
        return nearDoor;
    }
}

