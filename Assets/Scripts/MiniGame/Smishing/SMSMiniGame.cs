using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SMSMiniGame : MonoBehaviour
{
    [Serializable]
    public class SMSData
    {
        public string sender;
        public string body;
        public int[] correctOptions; // indici corretti (0-3)
        public string[] options;     // 4 opzioni
    }

    [Header("Istruzioni")]
    [SerializeField] GameObject instructionsPanel;

    [Header("UI References")]
    [SerializeField] GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI senderText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Button[] optionButtons;        // 4 bottoni
    [SerializeField] private Image[] optionBackgrounds;     // 4 immagini dei bottoni
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;

    [Header("Data")]
    [SerializeField] private List<SMSData> smsList = new List<SMSData>();

    private int currentIndex;
    private int score;
    private int correctNeededThisSms;
    private int correctClickedThisSms;
    private bool allSmsCompleted;
    private readonly HashSet<int> remainingCorrect = new HashSet<int>();

    private void Awake()
    {
        panelRoot.SetActive(false); 
        InitializeSampleData();
    }

    public void StartMiniGame()
    {
        Debug.Log("avvio del mini game");
        panelRoot.SetActive(true);
        SetupButtonListeners();
        DisplaySMS(0);
        UpdateScoreUI();
    }

    private void SetupButtonListeners()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int idx = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionClicked(idx));
        }
    }

    private void DisplaySMS(int index)
    {
        if (smsList == null || smsList.Count == 0) return;
        currentIndex = Mathf.Clamp(index, 0, smsList.Count - 1);
        SMSData data = smsList[currentIndex];

        senderText.text = data.sender;
        bodyText.text = data.body;

        remainingCorrect.Clear();
        correctClickedThisSms = 0;
        correctNeededThisSms = data.correctOptions != null ? data.correctOptions.Length : 0;
        foreach (int c in data.correctOptions)
        {
            remainingCorrect.Add(c);
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].interactable = true;
            if (optionBackgrounds != null && i < optionBackgrounds.Length)
            {
                optionBackgrounds[i].color = defaultColor;
            }

            TextMeshProUGUI label = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (label != null && data.options != null && i < data.options.Length)
            {
                label.text = data.options[i];
            }
        }
    }

    private void OnOptionClicked(int optionIndex)
    {
        if (smsList == null || smsList.Count == 0) return;
        if (optionButtons == null || optionIndex >= optionButtons.Length) return;
        if (!optionButtons[optionIndex].interactable) return;

        optionButtons[optionIndex].interactable = false;

        bool isCorrect = remainingCorrect.Contains(optionIndex);
        if (optionBackgrounds != null && optionIndex < optionBackgrounds.Length)
        {
            optionBackgrounds[optionIndex].color = isCorrect ? correctColor : wrongColor;
        }

        if (isCorrect)
        {
            score++;
            correctClickedThisSms++;
            remainingCorrect.Remove(optionIndex);

            if (remainingCorrect.Count == 0 && correctClickedThisSms >= correctNeededThisSms)
            {
                StartCoroutine(NextSMSCoroutine());
            }
        }
        else
        {
            score--;
        }

        UpdateScoreUI();
    }

    private IEnumerator NextSMSCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        NextSMS();
    }

    public void NextSMS()
    {
        if (smsList == null || smsList.Count == 0) return;

        bool isLast = currentIndex >= smsList.Count - 1;
        if (isLast)
        {
            allSmsCompleted = true;
            CloseGame();
            return;
        }

        currentIndex = currentIndex + 1;
        DisplaySMS(currentIndex);
    }

    public void CloseGame()
    {
        StopAllCoroutines();

        if (score >= 10 && allSmsCompleted)
        {
            ShowMessage("Hai terminato il gioco!");
            DisableAllButtons();
            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }
        }
        else
        {
            ShowMessage("Punteggio insufficiente, riprova il gioco.");
            StartCoroutine(RestartAfterDelay());
        }
    }

    private void ShowMessage(string message)
    {
        if (senderText != null)
        {
            senderText.text = string.Empty;
        }

        if (bodyText != null)
        {
            bodyText.text = message;
        }
    }

    private void DisableAllButtons()
    {
        if (optionButtons == null) return;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].interactable = false;
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        RestartGame();
    }

    private void RestartGame()
    {
        score = 0;
        allSmsCompleted = false;
        UpdateScoreUI();
        DisplaySMS(0);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void InitializeSampleData()
    {
        if (smsList != null && smsList.Count > 0) return;

        smsList = new List<SMSData>
        {
            new SMSData
            {
                sender = "Banca Online",
                body = "Il tuo conto è stato sospeso. Accedi subito per verificare.",
                correctOptions = new[] {1, 2},
                options = new[]
                {
                    "Apri il link nel messaggio",
                    "Chiama il numero ufficiale della banca",
                    "Ignora e segnala come phishing",
                    "Inserisci i dati richiesti"
                }
            },
            new SMSData
            {
                sender = "Corriere XYZ",
                body = "Consegna fallita. Paga 1€ per sbloccare il pacco.",
                correctOptions = new[] {2, 3},
                options = new[]
                {
                    "Paga subito con la carta",
                    "Inserisci i dati del bancomat",
                    "Ignora il messaggio",
                    "Verifica lo stato solo dall'app ufficiale"
                }
            },
            new SMSData
            {
                sender = "PayPal",
                body = "Aggiorna il metodo di pagamento cliccando qui.",
                correctOptions = new[] {1, 3},
                options = new[]
                {
                    "Condividi la password via SMS",
                    "Apri l'app PayPal ufficiale",
                    "Segui il link e inserisci i dati",
                    "Non cliccare il link sospetto"
                }
            },
            new SMSData
            {
                sender = "Operator Mobile",
                body = "Hai vinto un premio! Ritiralo subito compilando il form.",
                correctOptions = new[] {2, 3},
                options = new[]
                {
                    "Compila il modulo con dati personali",
                    "Fornisci numero di carta",
                    "Ignora il messaggio",
                    "Blocca il numero mittente"
                }
            },
            new SMSData
            {
                sender = "Università",
                body = "Account sospeso: invia le tue credenziali per riattivare l'accesso.",
                correctOptions = new[] {0, 3},
                options = new[]
                {
                    "Accedi manualmente dal portale ufficiale",
                    "Rispondi con username e password",
                    "Clicca il link nel messaggio",
                    "Contatta l'assistenza attraverso i canali ufficiali"
                }
            },
            new SMSData
            {
                sender = "Carta di Credito",
                body = "Carta bloccata per sospetto uso. Conferma i dati al link.",
                correctOptions = new[] {1, 2},
                options = new[]
                {
                    "Inserisci numero carta nel link",
                    "Chiama il numero sul retro della carta",
                    "Controlla notifiche nell'app ufficiale",
                    "Invia i dati via SMS"
                }
            },
            new SMSData
            {
                sender = "Fornitore Energia",
                body = "Promemoria: bolletta disponibile. Puoi pagarla dall'app.",
                correctOptions = new[] {0, 2},
                options = new[]
                {
                    "Apri direttamente l'app ufficiale",
                    "Clicca un link accorciato",
                    "Imposta un promemoria per il pagamento",
                    "Invia dati di carta via SMS"
                }
            },
            new SMSData
            {
                sender = "Corriere Ufficiale",
                body = "Il tuo pacco è arrivato al punto ritiro scelto.",
                correctOptions = new[] {1},
                options = new[]
                {
                    "Paga un supplemento al link",
                    "Mostra il codice ritiro nell'app ufficiale",
                    "Fornisci dati personali via SMS",
                    "Reindirizza il pacco con link non ufficiale"
                }
            },
            new SMSData
            {
                sender = "Amministratore Condominio",
                body = "Riunione straordinaria venerdì alle 18 in sala condominiale.",
                correctOptions = new[] {0, 3},
                options = new[]
                {
                    "Segna in agenda e partecipa",
                    "Condividi i tuoi dati bancari",
                    "Rispondi con foto del documento",
                    "Chiedi l'ordine del giorno ufficiale"
                }
            },
            new SMSData
            {
                sender = "Amico Luca",
                body = "Stasera pizza a casa mia alle 20, ci sei?",
                correctOptions = new[] {0},
                options = new[]
                {
                    "Rispondi che arrivi",
                    "Invia dati carta per contribuire",
                    "Clicca un link sconosciuto",
                    "Condividi codice PIN"
                }
            }
        };
    }
}
