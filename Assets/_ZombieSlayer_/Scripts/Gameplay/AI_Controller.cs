using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AI_Controller : MonoBehaviour
{
    public HumanoidlandController Player;
    private NavMeshAgent agent;
    private bool isAware=false;
    [SerializeField]private float FieldOfView = 120f;
    [SerializeField]private float ViewDistance = 10f;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip ShootSound;
    [SerializeField]private float SoundIntensity=5f;
    public LayerMask ZombieLayers;

    public object Zombies { get; private set; }

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
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
                RaycastHit hit;
                if(Physics.Linecast(this.transform.position,Player.transform.position,out hit, -1))
                {
                    if (hit.transform.tag == "Player")
                    {
                        OnAware();
                    }
                }
            }
        }
    }

    public void OnAware()
    {
        isAware = true;
    }

    public void Onfire()
    {
        AI_Controller callingOnAware = new AI_Controller();
        audioSource.PlayOneShot(ShootSound);
        Collider[] Zombies = Physics.OverlapSphere(transform.position, SoundIntensity, ZombieLayers);
        for(int i = 0; i < Zombies.Length; i++)
        {
            Zombies[i].GetComponent<AI_Controller>().OnAware(); 
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
