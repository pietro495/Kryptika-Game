using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MinigamePSW/Password Set")]
public class PasswordSet : ScriptableObject
{
    public List<PasswordEntry> PasswordList = new List<PasswordEntry>();
    public int passwordPerRound = 0;
    public int minCorrectToPass = 5;
    public bool shuffle = true;
}
