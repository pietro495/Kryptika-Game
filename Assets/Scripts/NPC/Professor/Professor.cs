using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Professor : MonoBehaviour
{

    [Header("Interazione monitor")]
    [SerializeField] Transform monitorTransform;          // assegna il computer (o un empty) dall’Inspector
    [SerializeField] float monitorInteractionRadius = 1f; // raggio entro cui abilitare l’interazione    [SerializeField] KeyCode interactKey = KeyCode.Space;
    [SerializeField] KeyCode interactKey = KeyCode.Space;
    [SerializeField] float movementSpeed = 5f;
    private bool isMoving;


    [Header ("Layers")]
    public LayerMask solidObjectsLayer;
    public LayerMask InteractableLayer;
    public LayerMask DialogLayer;
    public LayerMask Characters;


    Vector2 movement; //vettore che indica la direzione del movimento in base all'input da tastiera (1,0)
    Animator animator;
    Dialog dialog;
    Rigidbody2D rb;

    bool isControlLocked;
    [SerializeField] ProfInteractable professorInteractable;
    [SerializeField] int interactProf;


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

        //Interazione del professore all'avvio del pc per leggere la mail iniziale
        if (nearMonitor() && Input.GetKeyDown(interactKey))
        {
            UnityEngine.Debug.Log("Hai cliccato sul monitor");
            Interact();
        }


    }


    public bool NearProf()
    {
        bool nearProf = false;
        Vector2 posProf = professorInteractable.transform.position;
        if (Vector2.Distance(posProf, rb.position) < interactProf)
        {
            UnityEngine.Debug.Log("sei vicino al prof");
            nearProf = true;
        }
        return nearProf;
     
    }

    public void SetControlLock(bool value)
    {
        //se true allora bloccati, se false muoviti
        //lock a true allora bloccati
        isControlLocked = value;
        if(value)
            animator.SetBool("isMoving", false);
    }

     public  bool nearMonitor()
    {
        bool nearMonitor = monitorTransform != null && Vector3.Distance(transform.position, monitorTransform.position) <= monitorInteractionRadius;
        return nearMonitor;
    }
    //capisce se si trova davanti ad un oggetto interagibile ovvero capisce se c'è una collisione 
    public void Interact()
    {
        //ricorda che moveX o Y dell'animator sono (-1,1) dove sta guardando il personaggio, ritorna i valori dei parametri x e y
        var facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        //calcolo il punto dove sta guardando
        var interactPos = rb.position + facingDir*0.6f;
        var mask = InteractableLayer;
        //gestione della collisione per interazioni
        var collider = Physics2D.OverlapCircle(interactPos, 0.8f,mask);
        //se collider trova oggetto tramie overlap, trova uno script(component)
        //di tipo T (interactable) attaccato all'oggetto del collider
        if (collider != null)
        {
            collider.gameObject.GetComponent<Interactable>()?.Interact();
        }
        else
        {
            UnityEngine.Debug.Log("Nessun collider trovato");
        }
    }


        public void ChangeRb(bool value)
    {   
        if(value)   //qualsiasi è il valore della value tu lo imposti a statico; il suo contrario lo imposti a dynamic
        {
            UnityEngine.Debug.Log("valore isStatic"+value);
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            UnityEngine.Debug.Log("ho cambiato type a static"); 
        }
        
        else if (!value)
        {
            UnityEngine.Debug.Log("valore isStatic"+value);
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            UnityEngine.Debug.Log("Ho cambiato tipo a dynamic");

        }
        
       // rb = RigidbodyType2D.Dynamic;
    }

 

    
}

