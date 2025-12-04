using UnityEngine;

public class CloseParentPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public void Close()
    {
        var target = panel != null ? panel : transform.parent.gameObject;
        target.SetActive(false);
    }
}
