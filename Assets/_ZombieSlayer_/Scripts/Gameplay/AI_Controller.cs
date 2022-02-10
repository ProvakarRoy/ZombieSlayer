using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AI_Controller : MonoBehaviour
{
    public PlayerLocomotion Player;
    private NavMeshAgent agent;
    private bool isAware=false;
    [SerializeField]private float FieldOfView = 120f;
    [SerializeField]private float ViewDistance = 10f;
    private Animator animator;
    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }
    void Update()
    {
        if (isAware)
        {
            agent.SetDestination(Player.transform.position);
        }
        else
        {
            PlayerSearching();  
        }
        
    }
    public void PlayerSearching()
    {
        animator.SetBool("ZombieIdle", true);
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(Player.transform.position)) < FieldOfView / 2)
        {
            if (Vector3.Distance(Player.transform.position, this.transform.position) < ViewDistance)
            {
                OnAware();
            }
        }
    }

    public void OnAware()
    {
        isAware = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
