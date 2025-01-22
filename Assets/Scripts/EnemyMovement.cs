using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    private PlayerController playerController;
    private bool isChasing = false;
    public float updateInterval = 2f;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerController.OnGameStart += StartChasing;
        }
    
    }
    void StartChasing()
    {
        isChasing = true;
        StartCoroutine(UpdatePlayerPosition());
    }
    private IEnumerator UpdatePlayerPosition()
    {
        while(isChasing)
        {
            if(player != null)
            {
                navMeshAgent.SetDestination(player.position);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
