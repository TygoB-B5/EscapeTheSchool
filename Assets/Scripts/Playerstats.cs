using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstats : MonoBehaviour
{
    private int keys = 0;

    private Uihandler uiHandler;

    void Start()
    {
        uiHandler = FindObjectOfType<Uihandler>();
    }

    public void AddKey()
    {
        keys += 1;
        uiHandler.UpdateKey(keys);
    }

    public int Keys()
    {
        return keys;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            AddKey();
    }
}
