using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class QuizPasswordManager : MonoBehaviour
{
    [Header("Dialogo dopo completamento")]
    [SerializeField] DialogueAsset dialogueAsset; 

    [Header("UI")]
    [SerializeField] GameObject passwordPanel;
    [SerializeField] TMP_Text passwordText;
    [SerializeField] Button strongButton;
    [SerializeField] Button weakButton;

    [SerializeField] GameObject feedbackPanel;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button feedbackContinueButton;

    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMP_Text resultsText;
    [SerializeField] Button retryButton;
    [SerializeField] Button closeButton;


    [Header("DATI")]
    [SerializeField] PasswordSet passwordSet;

    readonly List<PasswordEntry> passwordPerRounds = new List<PasswordEntry>();

    [Header("Controllo Giocatore")]
    [SerializeField] newPlayerController playerController;
    bool isRunning;

    int currentIndex = 0;
    int correctAnswers = 0;
    bool lockMovement = true;

    void Awake()
    {
        if (passwordPanel != null) passwordPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);

    }

    void OnEnable()
    {
        if(strongButton != null) strongButton.onClick.AddListener(OnStrongClicked);
        if (weakButton != null) weakButton.onClick.AddListener(OnWeakClicked);
        if (feedbackContinueButton != null) feedbackContinueButton.onClick.AddListener(AdvanceToNextPsw);
        if (retryButton != null) retryButton.onClick.AddListener(RestartRound);
        if (closeButton != null) closeButton.onClick.AddListener(CloseMiniGame);
    }
    void OnDisable()
    {
        if(strongButton != null) strongButton.onClick.RemoveListener(OnStrongClicked);
        if (weakButton != null) weakButton.onClick.RemoveListener(OnWeakClicked);
        if (feedbackContinueButton != null) feedbackContinueButton.onClick.RemoveListener(AdvanceToNextPsw);
        if (retryButton != null) retryButton.onClick.RemoveListener(RestartRound);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseMiniGame);
    }
    private void OnWeakClicked()
    {
        ProcessAnswer(false);
    }

    private void OnStrongClicked()
    {
        ProcessAnswer(true);
    }

    public void StartMiniGame()
    {
        if(lockMovement)
        {
            Debug.Log("Player Bloccato nello startminigame");
            playerController.SetControlLock(lockMovement);
        
        
        if (passwordSet == null || passwordSet.PasswordList == null || passwordSet.PasswordList.Count == 0)
        {
            Debug.LogWarning("[PhishingMiniGame] Nessun set di mail configurato, minigioco completato automaticamente.");
            return;
        }

        PrepareRound();
        ShowCurrentPassword();
        }

        if (passwordPanel != null) passwordPanel.SetActive(true);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);

        //LockPlayer(true);
        isRunning = true;
    }

    void PrepareRound()
    {
        //passwordPerRounds.Clear(): pulisce eventuali dati del round precedente.
        passwordPerRounds.Clear();
        //copia tutte le email dal emailSet
        passwordPerRounds.AddRange(passwordSet.PasswordList);

        //se shuffle è vero, mescola la lista chiamando Shuffle()
        if (passwordSet.shuffle)
            Shuffle(passwordPerRounds);

        if (passwordSet.passwordPerRound > 0 && passwordPerRounds.Count > passwordSet.passwordPerRound)
            passwordPerRounds.RemoveRange(passwordSet.passwordPerRound, passwordPerRounds.Count - passwordSet.passwordPerRound);

        currentIndex = 0;
        correctAnswers = 0;
    }


    void ShowCurrentPassword()
    {
        if (currentIndex >= passwordPerRounds.Count)
        {
            ShowResult();
            return;
        }

        var psw = passwordPerRounds[currentIndex];
        if (passwordText != null) passwordText.text = psw.password;
        
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);

        if (strongButton != null) strongButton.interactable = true;
        if (weakButton != null) weakButton.interactable = true;

    }

    void ProcessAnswer(bool choosePlayer)
    {
        if (strongButton != null) strongButton.interactable = false;
        if (weakButton != null) weakButton.interactable = false;

        bool isCorrect;
        var psw = passwordPerRounds[currentIndex];

        if (psw.isStrong == choosePlayer)
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
        if(feedbackPanel != null)
        {
            feedbackPanel.SetActive(true);
            if(feedbackText != null)
            {
                if (isCorrect == true)
                {
                    feedbackText.text = psw.correctFeedback;
                }
                else if(isCorrect == false)
                {
                    feedbackText.text = psw.incorrectFeedback;
                }
            }
        }
    }


    void AdvanceToNextPsw()
    {
        currentIndex++;
        ShowCurrentPassword();
    }

    void ShowResult()
    {
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(true);

        
        int total = passwordPerRounds.Count;
        bool passed = correctAnswers >= passwordSet.minCorrectToPass;
 

         if (resultsText != null)
            resultsText.text = passed
            ? $"Ottimo lavoro! Risposte corrette: {correctAnswers}/{total}"
            : $"Hai risposto correttamente {correctAnswers}/{total}. Devi arrivare almeno a {passwordSet.minCorrectToPass}. Riprova";


        if (retryButton != null) retryButton.gameObject.SetActive(!passed);
        if (closeButton != null) closeButton.gameObject.SetActive(true);

        if (!passed)
            return;
    }
 void RestartRound()
    {
        PrepareRound();
        ShowCurrentPassword();

        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
    }
    void CloseMiniGame()
    {
        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (passwordPanel != null) passwordPanel.SetActive(false);

        //LockPlayer(false);
        playerController.SetControlLock(false);
        isRunning = false;
        //GameController.Instance.StartDialogue(dialogueAsset, GameState.Psw1MiniGame);
    }

    void LockPlayer(bool lockMovement)
    {
        if (playerController == null)
            return;

        playerController.SetControlLock(lockMovement);
    }

    void Shuffle(List<PasswordEntry> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]); // swap
        }
    }







}
