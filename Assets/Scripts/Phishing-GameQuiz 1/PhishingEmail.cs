//Classe PhishingEmail in cui definisco la struttura della mail di phishing

using UnityEngine;
[CreateAssetMenu(fileName = "NewEmail", menuName = "MiniGame/Phishing/Email")]

public class PhishingEmail : ScriptableObject
{
    [TextArea] public string sender;
    [TextArea] public string subject;
    [TextArea] public string body;
    public bool isPhishing;
    [TextArea] public string correctFeedback = "Hai riconosciuto il segnale di phishing!";
    [TextArea] public string incorrectFeedback = "Attenzione: questa era un'email legittima.";
}
