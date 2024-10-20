using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class AI_Control : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(player.transform.position);
    }

    void Update()
    {

        agent.SetDestination(player.transform.position);
    }



}
