using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmailQuizButton : MonoBehaviour
{
    [SerializeField] bool isPhishing;
    [SerializeField] string aimMessage;
    [SerializeField] EmailQuizManager quizManager;
    [SerializeField] Button button;
    [SerializeField] Image image;
    [SerializeField] TMP_Text label;

    Color baseColor;

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (image == null) image = GetComponent<Image>();
        baseColor = image.color;
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (quizManager == null) return;

        if (isPhishing)
        {
            quizManager.OnCorrectChoice(this, aimMessage);
            image.color = Color.green;
            button.interactable = false;
        }
        else
        {
            quizManager.OnWrongChoice(this,aimMessage);
            image.color = Color.red;
            Invoke(nameof(ResetColor), 0.25f);
        }
        button.interactable = false;
    }

    void ResetColor() => image.color = baseColor;
}
