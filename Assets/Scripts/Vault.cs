using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    private Uihandler uiHandler;
    private Playerstats playerStats;
    private bool mainKeyObtained = false;
    private Door door;

    void Start()
    {
        playerStats = FindObjectOfType<Playerstats>();
        uiHandler = FindObjectOfType<Uihandler>();
        door = FindObjectOfType<Door>();
    }

    public void CheckVaultStatus(bool isRunning)
    {
        if (!isRunning)
        {
            uiHandler.VaultKeysNeeded(playerStats.Keys(), false);
            return;
        }

        if (mainKeyObtained)
            return;

        uiHandler.VaultKeysNeeded(playerStats.Keys(), true);

    }

    public void VaultOpen()
    {
        if (playerStats.Keys() != 5)
            return;

        mainKeyObtained = true;
        door.Open();
        uiHandler.VaultKeysNeeded(playerStats.Keys(), false);
    }
}
