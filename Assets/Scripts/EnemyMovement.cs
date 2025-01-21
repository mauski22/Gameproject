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
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        if (player != null && playerController != null && playerController.GameStarted && !isChasing)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
