using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MailPhishingSet", menuName = "Phishing/Mail Set")]
public class MailPhishingSet : ScriptableObject
{
    public List<MailPhishingRound> mails = new List<MailPhishingRound>();
    public bool shuffle = true;
}

