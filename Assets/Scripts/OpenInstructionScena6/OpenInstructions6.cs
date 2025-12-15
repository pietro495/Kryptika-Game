using TMPro;
using UnityEngine;

public class OpenInstructions6 : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] GameObject instructionsPanel;
        [SerializeField] GameObject phonePanel;
        [SerializeField] GameObject panelSenderBody;
        [SerializeField] GameObject panelButton;
        [SerializeField] GameObject score;

        [Header("MiniGame Smishing ")]

        [SerializeField] SMSMiniGame smsMiniGame;

    void Awake()
    {
        phonePanel.SetActive(false);
        instructionsPanel.SetActive(false);
        panelSenderBody.SetActive(false);
        panelButton.SetActive(false);
        score.SetActive(false);
    }

    public void OpenInstructionsPanel()
    {
        phonePanel.SetActive(true);
        instructionsPanel.SetActive(true);
        panelSenderBody.SetActive(false);
        panelButton.SetActive(false);
        score.SetActive(false);
    }

    public void OnButtonClick()
    {
        panelSenderBody.SetActive(true);
        panelButton.SetActive(true);
        score.SetActive(true);
        instructionsPanel.SetActive(false);
        smsMiniGame.StartMiniGame();
    }
}
