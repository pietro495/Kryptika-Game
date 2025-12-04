using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObject = new GameObject[3];
    public int objectIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (persistentObject[objectIndex] == null)
        {
            persistentObject[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if(persistentObject[objectIndex] != null)
        {
            Destroy(gameObject);
        }
    }
}
