// EmailQuizManager.cs
using UnityEngine;
using TMPro;

public class EmailQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class MailConfig
    {
        public GameObject mailRoot;   // contenitore della singola mail
        public GameObject row1;       // riga con 2 bottoni (Psh/Leg)
        public GameObject row2;       // riga 2 (disattiva di default)
        public GameObject row3;       // riga 3 (disattiva di default)
    }

    [Header("Panel Root")]
    [SerializeField] GameObject panelRoot; // contenitore del pannello del quiz]

    [Header("Config")]
    [SerializeField] int minTotalScoreToPass = 10;
    [SerializeField] MailConfig[] mails;

    EmailQuizButton quizButton;

    [Header("UI")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] TMP_Text dialogText;

    [Header("Gamecontroller")]
    [SerializeField] GameController gameController;


    [Header("Dialogo fino e cambio stato")]

    [SerializeField] DialogueAsset dialog;

    int score;
    int currentMail;
    int[] correctClicksPerMail;

    void Awake()
    {
      panelRoot.SetActive(false);
    }
    void OnEnable()
    {
        StartQuiz();
    }

    public void StartQuiz()
    {
        score = 0;
        currentMail = 0;
        correctClicksPerMail = new int[mails.Length];
        dialogBox?.SetActive(false);
        panelRoot.SetActive(true);
        ResetAllMails();
        ActivateMail(0);
        UpdateScoreUI();
    }

    void ResetAllMails()
    {
        foreach (var mail in mails)
        {
            if (mail.mailRoot != null) mail.mailRoot.SetActive(false);
            if (mail.row1 != null) mail.row1.SetActive(true);
            if (mail.row2 != null) mail.row2.SetActive(false);
            if (mail.row3 != null) mail.row3.SetActive(false);
            ResetButtons(mail.mailRoot);
        }
    }

    void ResetButtons(GameObject root)
    {
        if (root == null) return;
        foreach (var btn in root.GetComponentsInChildren<EmailQuizButton>(true))
        {
            var img = btn.GetComponent<UnityEngine.UI.Image>();
            if (img != null) img.color = Color.white;
            var uiBtn = btn.GetComponent<UnityEngine.UI.Button>();
            if (uiBtn != null) uiBtn.interactable = true;
        }
    }

    void ActivateMail(int index)
    {
        for (int i = 0; i < mails.Length; i++)
            if (mails[i].mailRoot != null) mails[i].mailRoot.SetActive(i == index);
    }

    public void OnCorrectChoice(EmailQuizButton btn,string msg)
    {
        score += 1;
        correctClicksPerMail[currentMail]++;
        UpdateScoreUI();

        if (dialogBox != null && dialogText != null)
        {
            dialogText.text = msg;
            dialogBox.SetActive(true);
            CancelInvoke(nameof(HideDialog));
            Invoke(nameof(HideDialog), 2f);
        }

        var mail = mails[currentMail];
        int clicks = correctClicksPerMail[currentMail];
        if (clicks == 1 && mail.row2 != null) mail.row2.SetActive(true);
        else if (clicks == 2 && mail.row3 != null) mail.row3.SetActive(true);

        if (clicks >= 3)
            AdvanceMail();
    }

    public void OnWrongChoice(EmailQuizButton btn,string msg)
    {
        score -= 1;
        correctClicksPerMail[currentMail]++;

        UpdateScoreUI();
        if (dialogBox != null && dialogText != null)
        {
            dialogText.text = msg;
            dialogBox.SetActive(true);
            CancelInvoke(nameof(HideDialog));
            Invoke(nameof(HideDialog), 2f);
        }

        var mail = mails[currentMail];
        int clicks = correctClicksPerMail[currentMail];
        if (clicks == 1 && mail.row2 != null) mail.row2.SetActive(true);
        else if (clicks == 2 && mail.row3 != null) mail.row3.SetActive(true);

        if (clicks >= 3)
            AdvanceMail();

    }

    void HideDialog() => dialogBox?.SetActive(false);

    void AdvanceMail()
    {
        if (currentMail < mails.Length - 1)
        {
            currentMail++;
            ActivateMail(currentMail);
        }
        else
        {
            EndGameCheck();
        }
    }

    void EndGameCheck()
    {
        if (score >= minTotalScoreToPass)
        {
            CloseQuiz();
        }
        else
        {
            StartQuiz(); // retry tutto
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    void CloseQuiz()
    {
        gameObject.SetActive(false);
        // opzionale: notifica al GameController qui
        panelRoot.SetActive(false);
        dialogBox?.SetActive(false);
        gameController.StartDialogue(dialog, GameState.DoorExit);
    }
}
