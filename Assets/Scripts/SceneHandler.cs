﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour
{
    public void load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
