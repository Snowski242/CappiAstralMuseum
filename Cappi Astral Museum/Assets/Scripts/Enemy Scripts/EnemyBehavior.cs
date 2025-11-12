using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public int enemyType;
    public float enemyRadius = 10f;
    public bool hasStellar;
    public int stellarInd;

    public bool inWave;

    EnemyHP dmg;
    EnemyDamage fx;
    NavMeshAgent agent;
    public Animator animator;

    public GameObject stellarineObj;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dmg = GetComponent<EnemyHP>();
        fx = GetComponent<EnemyDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        Behavior(enemyType);
    }

    private void OnDestroy()
    {
        if (inWave)
        {
            WaveSpawner wave = FindFirstObjectByType<WaveSpawner>();
            wave.enemyAmount--;
        }
    }

    void Behavior(int type)
    {
        switch(type)
        {
            case 0:
                PlayerMovement target = FindFirstObjectByType<PlayerMovement>();
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance < enemyRadius && distance > agent.stoppingDistance)
                {
                    animator.SetBool("idle", false);
                    animator.SetBool("walk", true);

                    agent.SetDestination(target.transform.position);
                    FaceTarget();
                }
                else
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("idle", true);
                }

                ///jumping on top of them

                RaycastHit downHit;

                Vector3 p1 = transform.position;

                //shoots raycast forward to see if theres a raycast hit
                if (Physics.SphereCast(p1, 0.4f, transform.up, out downHit, 1.1f, LayerMask.GetMask("Player")))
                {
                   

                    PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
                    if(player.canMove)
                    {
                        dmg.HP -= 1;

                        player.state = "jump";
                        player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                        player.isGrounded = false;

                        Instantiate(fx.hitFX, transform.position, Quaternion.identity);
                    }
                    
                }

                if(dmg.HP <= 0)
                {
                    if(hasStellar)
                    {
                        var stellarine = Instantiate(stellarineObj, transform.position + new Vector3(0f, 3f, 0f), transform.rotation);
                        stellarine.GetComponent<StellarineBehavior>().gemID = stellarInd;
                        stellarine.GetComponent<StellarineBehavior>().justSpawned = true;
                    }

                    PlayerMovement player = FindAnyObjectByType(typeof( PlayerMovement)) as PlayerMovement;
                    player.tensionGauge = Mathf.MoveTowards(player.tensionGauge, player.tensionGaugeMax, 0.395f);

                    Destroy(gameObject);
                }

                break;
        }
    }

    void FaceTarget()
    {
        PlayerMovement target = FindFirstObjectByType<PlayerMovement>();
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
    }
}
