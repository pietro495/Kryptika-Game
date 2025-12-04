using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_To_Open : MonoBehaviour, Interactable
{
    [SerializeField] string scene_name;

    [SerializeField] GameState stateAfterLoad ;
    [SerializeField] DialogueAsset dialogueOnLoad;

    public void Interact()
    {
        Debug.Log($"DoorLoading :{scene_name}");
        if (string.IsNullOrEmpty(scene_name))
        {
            Debug.LogWarning("[SceneDoor] sceneName non impostato");
            return;
        }

        if (!SceneLoader.Instance.SceneExists(scene_name))
        {
            Debug.LogWarning($"[SceneDoor] '{scene_name}' non è nelle Build Settings");
            return;
        }
        Debug.Log($"Door -> scene {scene_name}, dialog {dialogueOnLoad}");
        GameSession.Instance?.ConfigureNextScene(stateAfterLoad, dialogueOnLoad);
        SceneManager.LoadScene(scene_name);  
        Debug.Log($"Door -> scene {scene_name}, dialog {dialogueOnLoad}");    
        /*
        else if (!SceneManager.GetSceneByName(scene_name).IsValid())
        {
            bool isValid = SceneManager.GetSceneByName(scene_name).IsValid();
            Debug.LogWarning("valore di IsValid: {isValid}");
            Debug.LogWarning($"[SceneDoor] '{scene_name}' non è nelle Build Settings");
            SceneLoader.Instance.LoadScene(scene_name);
        }
        */
    }

}

