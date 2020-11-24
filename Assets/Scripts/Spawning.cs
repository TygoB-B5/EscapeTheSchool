using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    private Transform[] spawnPoints;

    public Transform player;
    public GameObject enemy;

    private int randomInt;

    void Start()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("SpawnTarget");
        spawnPoints = new Transform[a.Length];
        for(int i = 0; i < a.Length; i++)
        {
            spawnPoints[i] = a[i].GetComponent<Transform>();
        }
        RespawnPlayer();
        RespawnEnemy();
    }

    public void RespawnPlayer()
    {
        randomInt = Random.Range(0, spawnPoints.Length);
        player.transform.position = spawnPoints[randomInt].position;
    }

    public void RespawnEnemy()
    {
        int randomint2 = Random.Range(0, spawnPoints.Length);
        if (Vector3.Distance(spawnPoints[randomint2].transform.position, player.transform.position) > 100)
        {
            Instantiate(enemy, spawnPoints[randomint2].position, Quaternion.Euler(0,0,0));
        }
        else
        {
            RespawnEnemy();
        }

    }

}
