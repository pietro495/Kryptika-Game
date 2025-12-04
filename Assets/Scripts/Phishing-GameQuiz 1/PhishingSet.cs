using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MiniGame/Phishing Email Set")]

public class PhishingEmailSet : ScriptableObject
{
    [Tooltip("Inserisci almeno 10 email. Se sono più di 10 verranno estratte in ordine/mescolate.")]
    public List<PhishingEmail> emails = new List<PhishingEmail>();
    public int emailsPerRound = 10;
    public int minCorrectToPass = 10;
    public bool shuffle = true;
    public int currentSet = 0;

}
