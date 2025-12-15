using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//CLASSE PER LA GESTIONE DEL GIOCO DRAG & DROP
public class PhishingInteractableManager : MonoBehaviour
{
//
    [Header("Dati")]
    [SerializeField] MailPhishingSet mailSet;
    [SerializeField] bool shuffle = true;

    [Header("UI")]
    [SerializeField] GameObject rootPanel;

    [Header("Close instruzione")]
    [SerializeField] GameObject panelInstructions;
    [SerializeField] List<DragItem> dragItems; // stessi elementi ogni mail, riempiti in ordine
    [SerializeField] DropZone phishingZone;
    [SerializeField] DropZone legitZone;
    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text mailCounterLabel;

    //[Header("Panel Instructions")]
  //  [SerializeField] GameObject instructionsPanel;

    int mailIndex;
    int score;
    int blocksToScore;
    List<MailPhishingRound> workingList = new();



   void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        panelInstructions.SetActive(false);
        
        
    }


    public void StartMiniGame()
    {

        Debug.Log("Prova per veder se chiama la funzione");
       
        if (mailSet == null || mailSet.mails.Count == 0) return;

        workingList.Clear();
        workingList.AddRange(mailSet.mails);
        if (shuffle || mailSet.shuffle) Shuffle(workingList);

        mailIndex = 0;
        score = 0;
        LoadMail();

        panelInstructions.SetActive(false);
        if (rootPanel != null) rootPanel.SetActive(true);
    }

    void LoadMail()
    {
        if (mailIndex >= workingList.Count)
        {
            ShowResult();
            return;
        }

        var mail = workingList[mailIndex];
        blocksToScore = mail.blocks.Count;

        for (int i = 0; i < dragItems.Count; i++)
        {
            if (i < mail.blocks.Count)
            {
                dragItems[i].gameObject.SetActive(true);
                var b = mail.blocks[i];
                dragItems[i].SetData(b.text, b.isPhishing);
            }
            else
            {
                dragItems[i].gameObject.SetActive(false);
            }
        }

        UpdateLabels();
    }

    public void AddScore(int delta)
    {
        score += delta;
        UpdateLabels();
    }

    public void NotifyBlockScored()
    {
        blocksToScore--;
        if (blocksToScore <= 0)
        {
            mailIndex++;
            LoadMail();
        }
    }

    void ShowResult()
    {
        // esempio: punteggio minimo 10
        if (score < 10)
        {
            // fai riavviare
            mailIndex = 0;
            score = 0;
            LoadMail();
        }
        else
        {
            CloseMiniGame();
        }
    }

    public void CloseMiniGame()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        // sblocca player se necessario
    }

    void UpdateLabels()
    {
        if (scoreLabel != null) scoreLabel.text = $"Score: {score}";
        if (mailCounterLabel != null) mailCounterLabel.text = $"Mail {mailIndex + 1}/{workingList.Count}";
    }

    void Shuffle(List<MailPhishingRound> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }


}
