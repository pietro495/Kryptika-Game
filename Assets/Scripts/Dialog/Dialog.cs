using System.Collections.Generic;
using UnityEngine;

//la classe Dialog mi gestisce la LISTA di dialoghi
[System.Serializable]

public class DialogLine
{
    [SerializeField] string speaker;
    [TextArea] [SerializeField] string text;

    public string Speaker => speaker;
    public string Text => text;
}


//la lista "lines" è una lista di DialogLine quindi speaker+text
public class Dialog
{
    [SerializeField] List<DialogLine> lines = new List<DialogLine>();
    public IReadOnlyList<DialogLine> Lines => lines;
}


/*
Dialog myDialog = new Dialog();
Ora hai creato un oggetto myDialog di tipo Dialog.
Questo oggetto contiene una lista chiamata lines, che è inizialmente vuota (new List<string>()).

*/
