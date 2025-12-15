using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] int lettersPerSeconds;

    //Eventi
    public event Action onShowDialogue;
    public event Action onHideDialogue;
    public event Action onDialogueCompleted; //usato nel unlockPcFlow.cs

    DialogueAsset dialogueAsset;
    int currentLine;
    bool isTyping;
    bool isOpen;

    public  DialogManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        if(dialogBox != null)
        {
            dialogBox.SetActive(false);
        }
    }

    public IEnumerator ShowDialogue(DialogueAsset asset)
    {
        if (asset == null || asset.Lines.Count == 0)
            yield break;
        
        yield return new WaitForEndOfFrame();

        dialogueAsset = asset;
        currentLine = 0;
        isOpen = true;
        dialogBox.SetActive(true);

        onShowDialogue?.Invoke();
        StartCoroutine(TypeDialog(dialogueAsset.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (!isOpen || dialogueAsset == null) return;

        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            currentLine++;
            if (currentLine < dialogueAsset.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialogueAsset.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                isOpen = false;
                dialogBox.SetActive(false);
                onHideDialogue?.Invoke();
                onDialogueCompleted?.Invoke();
                dialogueAsset = null;
            }
        }
    }
    


    IEnumerator TypeDialog(DialogueLine line)
    {
        isTyping = true;
        dialogText.text = string.Empty;

        string fullText = string.IsNullOrEmpty(line.Speaker)
            ? line.Text
            : $"{line.Speaker}: {line.Text}";

        foreach (var letter in fullText)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.1f / lettersPerSeconds);
        }

        isTyping = false;
    }
}
