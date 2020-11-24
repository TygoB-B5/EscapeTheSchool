using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Won : MonoBehaviour
{
    private SceneHandler sceneHandler;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            sceneHandler = FindObjectOfType<SceneHandler>();
            sceneHandler.load("Win");
        }
    }
}
