using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targ : MonoBehaviour
{
    public Transform targe;
	UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent= GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targe.position);
    }
}
