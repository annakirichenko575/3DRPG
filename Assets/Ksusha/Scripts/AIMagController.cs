using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMagController : MonoBehaviour
{
    public Transform playerTransform;
    public float maxTime = 0.5f;
    public float maxDistance = 1.0f;

    NavMeshAgent agent;
    Animator animator;
    float timer = 0.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            float sqrDistance = (playerTransform.position - agent.destination).sqrMagnitude;
            if (sqrDistance > maxDistance * maxDistance) {
                agent.destination = playerTransform.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
