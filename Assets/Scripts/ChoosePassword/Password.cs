using UnityEngine;

[CreateAssetMenu(menuName = "MinigamePSW/PasswordEntry")]

public class PasswordEntry : ScriptableObject
{
    [TextArea] public string password;
    [TextArea] public string correctFeedback;
    [TextArea] public string incorrectFeedback;

    public bool isStrong; //se la mail è sicura o no


}
