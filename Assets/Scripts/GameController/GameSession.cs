using UnityEngine;
using UnityEngine.SceneManagement;


//tiene in memoria lo stato con cui avviare la prossima scena
public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    public GameState NextState { get; private set; } = GameState.FreeRoam;
    public DialogueAsset NextDialogue { get; private set; } = null;
    public bool HasPendingConfig { get; private set; } = false;


    void Awake()
    {
        //se esiste lo stesso oggetto nella scena allora eliminalo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //Se invece non esisteva nessun’altra istanza, allora questa diventa quella “ufficiale”.
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Qui si aggancia un evento: ogni volta che una scena viene caricata, verrà chiamato automaticamente il metodo OnSceneLoaded.
        //this oggetto che sta eseguendo il codice in quel momento quello corrente
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    public void ConfigureNextScene(GameState state, DialogueAsset dialogue = null)
    {
        NextState = state;
        NextDialogue = dialogue;
        HasPendingConfig = true;
    }

    public void ClearConfig()
    {
        HasPendingConfig = false;
        NextDialogue = null;
    }
    
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Non serve far nulla qui: la scena nuova leggerà HasPendingConfig nel suo GameController
    }


}
