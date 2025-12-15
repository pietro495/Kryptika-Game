using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MailPhishingRound", menuName = "Phishing/Mail Round")]
public class MailPhishingRound : ScriptableObject
{
    [System.Serializable]
    public class Block
    {
        public string text;      // contenuto (mittente, destinatario, body, ecc.)
        public bool isPhishing;  // vero se questo blocco è sospetto
    }

    public List<Block> blocks = new List<Block>();
}
