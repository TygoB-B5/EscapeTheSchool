using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Canvas exit;

    public void Start()
    {
        exit.enabled = false;
    }
    public void Open()
    {
        exit.enabled = true;
        StartCoroutine(TurnoffExit());
    }

    IEnumerator TurnoffExit()
    {
        yield return new WaitForSeconds(3);
        exit.enabled = false;
        gameObject.SetActive(false);
        yield return null;
    }
}
