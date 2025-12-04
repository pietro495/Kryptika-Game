using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [SerializeField] string speaker;    //restituisce il nome del personaggio
    [TextArea] [SerializeField] string text;

    public string Speaker => speaker;
    public string Text => text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues/Dialogue Asset")]

//dialogueAsset una lista di DialogueLine (speaker + text)
public class DialogueAsset : ScriptableObject
{
    [SerializeField] List<DialogueLine> lines = new List<DialogueLine>();
    public IReadOnlyList<DialogueLine> Lines => lines;
}
