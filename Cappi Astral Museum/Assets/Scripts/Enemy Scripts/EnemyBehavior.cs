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

    public float wanderRadius = 15f;
    public float wanderTimer = 5f;

    private float timer;
    private Vector3 wanderTarget;

    PlayerMovement target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true; //  auto-rotate

        dmg = GetComponent<EnemyHP>();
        fx = GetComponent<EnemyDamage>();

        target = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        Behavior(enemyType);
        UpdateAnimation(); //  FIX walking in place
    }

    private void OnDestroy()
    {
        if (inWave)
        {
            WaveSpawner wave = FindFirstObjectByType<WaveSpawner>();
            wave.enemyAmount--;
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                wanderTarget = hit.position;
                agent.SetDestination(wanderTarget);
            }

            timer = 0;
        }
    }

    void Behavior(int type)
    {
        switch (type)
        {
            case 0:

                float distance = Vector3.Distance(target.transform.position, transform.position);

                if (distance < enemyRadius && distance > agent.stoppingDistance)
                {
                    agent.SetDestination(target.transform.position);
                }
                else
                {
                    Wander();
                }

                // Jump attack check
                RaycastHit downHit;
                Vector3 p1 = transform.position;

                if (Physics.SphereCast(p1, 0.4f, transform.up, out downHit, 1.1f, LayerMask.GetMask("Player")))
                {
                    PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

                    if (player.canMove)
                    {
                        dmg.HP -= 1;

                        player.state = "jump";
                        player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                        player.isGrounded = false;

                        Instantiate(fx.hitFX, transform.position, Quaternion.identity);
                    }
                }

                // Death
                if (dmg.HP <= 0)
                {
                    if (hasStellar)
                    {
                        var stellarine = Instantiate(
                            stellarineObj,
                            transform.position + new Vector3(0f, 3f, 0f),
                            transform.rotation
                        );

                        stellarine.GetComponent<StellarineBehavior>().gemID = stellarInd;
                        stellarine.GetComponent<StellarineBehavior>().justSpawned = true;
                    }

                    PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
                    player.tensionGauge = Mathf.MoveTowards(
                        player.tensionGauge,
                        player.tensionGaugeMax,
                        2.995f
                    );

                    Destroy(gameObject);
                }

                break;
        }
    }

    // Animation based on REAL movement
    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("walk", true);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("idle", true);
        }
    }
}