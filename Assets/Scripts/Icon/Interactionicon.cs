using UnityEngine;

public class Interactionicon : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(){gameObject.SetActive(true);}
    public void Hide(){gameObject.SetActive(false);}
}
