using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public int enemyType;
    public float enemyRadius = 10f;
    EnemyHP dmg;
    EnemyDamage fx;
    NavMeshAgent agent;
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

    void Behavior(int type)
    {
        switch(type)
        {
            case 0:
                PlayerMovement target = FindFirstObjectByType<PlayerMovement>();
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance < enemyRadius)
                {
                    agent.SetDestination(target.transform.position);
                    FaceTarget();
                }

                RaycastHit downHit;

                Vector3 p1 = transform.position;

                //shoots raycast forward to see if theres a raycast hit
                if (Physics.SphereCast(p1, 0.4f, transform.up, out downHit, 1.1f, LayerMask.GetMask("Player")))
                {
                    dmg.HP -= 1;

                    PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
                    player.state = "jump";
                    player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                    player.isGrounded = false;

                    Instantiate(fx.hitFX, transform.position, Quaternion.identity);
                }

                if(dmg.HP <= 0)
                {
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
        transform.rotation = lookRotation;
    }
}
