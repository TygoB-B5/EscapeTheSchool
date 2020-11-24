using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Uihandler : MonoBehaviour
{
    public GameObject PressEui;
    public GameObject DieUI;
    public Slider Energy;
    public Slider Flashlight;
    public Text keyAmount;

    public Text vaultText;
    public GameObject vaultUi;

    private playerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<playerController>();
        vaultUi.SetActive(false);
    }

    public void EnablePressEui(bool isEnabled)
    {
        if(isEnabled)
        {
            PressEui.SetActive(true);
        }
        else
        {
            PressEui.SetActive(false);
        }
    }

    public void EnableDieUi(bool isEnabled)
    {
        if (isEnabled)
        {
            DieUI.SetActive(true);
        }
        else
        {
            DieUI.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateEnergy();
        UpdateFlashlight();
    }

    void UpdateEnergy()
    {
        Energy.value = playerController.Energy();
    }

    void UpdateFlashlight()
    {
        Flashlight.value = playerController.FlashlightEnergy();
    }

    public void UpdateKey(int keys)
    {
        keyAmount.text = "Keys: " + keys.ToString() + "/5";
    }

    public void VaultKeysNeeded(int keys, bool isEnabled)
    {
        if (keys == 5)
        {
            vaultText.text = keys.ToString() + "/5 Keys";
            vaultUi.SetActive(isEnabled);
        }
        else
        {
            vaultText.text = keys.ToString() + "/5 Keys, I need to find more!";
            vaultUi.SetActive(isEnabled);
        }
    }
}
