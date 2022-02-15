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
    public LayerMask ZombieLayers;


    private AudioSource audioSource;
    public AudioClip ShootSound;
    [SerializeField]private float SoundIntensity=5f;

    private Vector3 WanderPoint;
    [SerializeField] private float WanderRadius = 7f;


    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        audioSource = this.GetComponent<AudioSource>();
        WanderPoint = RandamWanderPoint();
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
            Wander();
        }
        
    }
    public void PlayerSearching()
    {
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

    public void Wander()
    {
        if (Vector3.Distance(this.transform.position, WanderPoint) < 3f)
        {
            WanderPoint = RandamWanderPoint();
        }
        else
        {
            agent.SetDestination(WanderPoint);
        }
    }

    public Vector3 RandamWanderPoint()
    {
        Vector3 randampoint = (Random.insideUnitSphere*WanderRadius)+this.transform.position;
        NavMeshHit NavHit;
        NavMesh.SamplePosition(randampoint, out NavHit, WanderRadius, -1);
        return new Vector3(NavHit.position.x, this.transform.position.y, NavHit.position.z);
    }
}
