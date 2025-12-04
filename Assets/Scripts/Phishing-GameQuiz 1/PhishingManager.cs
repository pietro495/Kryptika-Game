using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PhishingManager : MonoBehaviour
{
    [Header("Riferimenti UI")]
    [SerializeField] GameObject rootPanel;
    [SerializeField] TMP_Text senderLabel;
    [SerializeField] TMP_Text subjectLabel;
    [SerializeField] TMP_Text bodyLabel;
    [SerializeField] Button phishingButton;
    [SerializeField] Button legitButton;
    [SerializeField] GameObject feedbackPanel;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button feedbackContinueButton;
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMP_Text resultsText;
    [SerializeField] Button retryButton;
    [SerializeField] Button closeButton;

    [Header("Dati")]
    [SerializeField] PhishingEmailSet emailSet; //elenco delle mail

    [Header("Controllo Giocatore")]
    [SerializeField] newPlayerController playerController;

    readonly List<PhishingEmail> roundEmails = new List<PhishingEmail>();
    int currentIndex;
    int correctAnswers;//contatore risposte corrette dentro il round

    bool isRunning; //flag di lettura per sapere se dall'esterno il minigame è attivo


    void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }


    void OnEnable()
    {
        if (phishingButton != null) phishingButton.onClick.AddListener(OnPhishingClicked);
        if (legitButton != null) legitButton.onClick.AddListener(OnLegitClicked);
        if (feedbackContinueButton != null) feedbackContinueButton.onClick.AddListener(AdvanceToNextEmail);
        if (retryButton != null) retryButton.onClick.AddListener(RestartRound);
        if (closeButton != null) closeButton.onClick.AddListener(CloseMiniGame);
    }


    void OnDisable()
    {
        if (phishingButton != null) phishingButton.onClick.RemoveListener(OnPhishingClicked);
        if (legitButton != null) legitButton.onClick.RemoveListener(OnLegitClicked);
        if (feedbackContinueButton != null) feedbackContinueButton.onClick.RemoveListener(AdvanceToNextEmail);
        if (retryButton != null) retryButton.onClick.RemoveListener(RestartRound);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseMiniGame);
    }

    void OnPhishingClicked()
    {
        ProcessAnswer(true);
    }

    void OnLegitClicked()
    {
        ProcessAnswer(false);
    }

    public void StartMiniGame()
    {
        if (emailSet == null || emailSet.emails == null || emailSet.emails.Count == 0)
        {
            Debug.LogWarning("[PhishingMiniGame] Nessun set di mail configurato, minigioco completato automaticamente.");
            return;
        }

        PrepareRound();
        ShowCurrentEmail();

        if (rootPanel != null) rootPanel.SetActive(true);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);

        LockPlayer(true);
        isRunning = true;
    }


    void PrepareRound()
    {
        //roundEmails.Clear(): pulisce eventuali dati del round precedente.
        roundEmails.Clear();
        //copia tutte le email dal emailSet
        roundEmails.AddRange(emailSet.emails);

        //se shuffle è vero, mescola la lista chiamando Shuffle()
        if (emailSet.shuffle)
            Shuffle(roundEmails);

        if (emailSet.emailsPerRound > 0 && roundEmails.Count > emailSet.emailsPerRound)
            roundEmails.RemoveRange(emailSet.emailsPerRound, roundEmails.Count - emailSet.emailsPerRound);

        currentIndex = 0;
        correctAnswers = 0;
    }


    void ShowCurrentEmail()
    {
        //dopo che scorri tutto l'indice, mostra pannello risultati
        if (currentIndex >= roundEmails.Count)
        {
            ShowResult();
            return;
        }

        var email = roundEmails[currentIndex];

        if (senderLabel != null) senderLabel.text = email.sender;
        if (subjectLabel != null) subjectLabel.text = email.subject;
        if (bodyLabel != null) bodyLabel.text = email.body;

        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);

        if (phishingButton != null) phishingButton.interactable = true;
        if (legitButton != null) legitButton.interactable = true;
    }

    void ProcessAnswer(bool playerThinksPhishing)
    {
        if (phishingButton != null) phishingButton.interactable = false; //non lo rende cliccabile
        if (legitButton != null) legitButton.interactable = false;

        //email corrente, accede alla lista all'indice corrente
        var email = roundEmails[currentIndex];
        //is phishing var bool della classe phishingSet
        bool isCorrect;

        if (email.isPhishing == playerThinksPhishing)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }

        if (isCorrect)
        {
            correctAnswers++;
        }

        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(true);
            if (feedbackText != null)
                feedbackText.text = isCorrect ? email.correctFeedback : email.incorrectFeedback;
        }
    }

    void AdvanceToNextEmail()
    {
        currentIndex++;
        ShowCurrentEmail();
    }

    void ShowResult()
    {
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(true);

        bool passed = correctAnswers >= emailSet.minCorrectToPass;
        int total = roundEmails.Count;

        if (resultsText != null)
            resultsText.text = passed
            ? $"Ottimo lavoro! Risposte corrette: {correctAnswers}/{total}"
            : $"Hai risposto correttamente {correctAnswers}/{total}. Devi arrivare almeno a {emailSet.minCorrectToPass}. Riprova";


        if (retryButton != null) retryButton.gameObject.SetActive(!passed);
        if (closeButton != null) closeButton.gameObject.SetActive(true);

        if (!passed)
            return;
    }

    void RestartRound()
    {
        //GameController.Instance.OnPhishingMinigameClosed();

        PrepareRound();
        ShowCurrentEmail();

        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
    }
    void CloseMiniGame()
    {
        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (rootPanel != null) rootPanel.SetActive(false);

        LockPlayer(false);
        isRunning = false;        
    }

    void LockPlayer(bool lockMovement)
    {
        if (playerController == null)
            return;

        playerController.SetControlLock(lockMovement);
    }

    void Shuffle(List<PhishingEmail> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]); // swap
        }
    }










}


   

