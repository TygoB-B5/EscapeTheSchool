using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent enemy;
    private playerController playerController;
    public SkinnedMeshRenderer mesh;
    private bool isSeen;
    private float t;
    private Spawning spawning;

    void Start()
    {
        spawning = FindObjectOfType<Spawning>();
        playerController = FindObjectOfType<playerController>();
        enemy = GetComponent<NavMeshAgent>();
        GameObject a = GameObject.FindGameObjectWithTag("Player");
        target = a.GetComponent<Transform>();
    }

    private void Update()
    {
        TestIfSeen();
        SetEnemySpeed();
        SetDestination();
    }

    void TestIfSeen()
    {
        if(mesh.isVisible)
        {
            isSeen = true;
        }
        else
        {
            isSeen = false;
        }
    }

    void SetDestination()
    {
        enemy.destination = target.position;
    }
    void SetEnemySpeed()
    {
        if(isSeen && playerController.flashLight.enabled == true)
        {
            enemy.speed = 4;
            LookTime();
        }
        else if(isSeen)
        {
            enemy.speed = 15;
        }
        else
        {
            enemy.speed = 8;
        }
    }

    void LookTime()
    {
        t += 1 * Time.deltaTime;
        if(t > 3)
        {
            spawning.RespawnEnemy();
            Destroy(gameObject, 0);
            t = 0;
        }
    }
}
