using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header ("Layers")]
    public LayerMask solidObjectsLayer;
    public LayerMask InteractableLayer;
    public LayerMask DialogLayer;
    public LayerMask Characters;

    [Header("Variabili booleane")]
    [SerializeField] private float movementSpeed = 2;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;



    private bool isControlLocked;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }


    public void Move()
    {
        //è una delle chiamate fondamentali del nuovo Input System di Unity, 
        // e serve a leggere il valore corrente dell’input associato (nel tuo caso, il movimento)
        //  e salvarlo come vettore bidimensionale (Vector2).
        //
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
    }

 
    
    
 
    //capisce se si trova davanti ad un oggetto interagibile ovvero capisce se c'è una collisione 
    void Interact()
    {
        //ricorda che moveX o Y dell'animator sono (-1,1) dove sta guardando il personaggio, ritorna i valori dei parametri x e y
        var facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        //calcolo il punto dove sta guardando
        var interactPos = rb.position + facingDir*0.6f;
        var mask = InteractableLayer | DialogLayer;
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
            Debug.Log("Nessun collider trovato");
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

}
