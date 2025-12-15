using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhishingUI : MonoBehaviour
{
    [Header("Apertura Gioco")]
    [SerializeField] GameObject phishingUI;
    [SerializeField] KeyCode openMailKey = KeyCode.Space; // tasto per aprire la mail
    [SerializeField] KeyCode closeMailKey = KeyCode.Escape;

    [SerializeField] newPlayerController currentPlayer;


    bool isOpen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(phishingUI != null)
        {
            phishingUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOpen)
        {
            return;
        }
        if (Input.GetKeyDown(openMailKey))
        {
            OpenGame(currentPlayer);
        }
        else if (Input.GetKeyDown(closeMailKey))
        {
            CloseGame(currentPlayer);
        }
    }


    public void OpenGame(newPlayerController player)
    {
        currentPlayer = player;
        isOpen = true;
        if (phishingUI != null)
            phishingUI.SetActive(true);

        if (currentPlayer != null)
        {
            player.SetControlLock(true);
        }
    }
    

    public void CloseGame(newPlayerController player)
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        if (phishingUI != null)
        {
            phishingUI.SetActive(false);
        }

        if(currentPlayer != null)
        {
            Debug.Log("Player uscito può muoversi");
            currentPlayer.SetControlLock(false);
        }
    
    }
}
