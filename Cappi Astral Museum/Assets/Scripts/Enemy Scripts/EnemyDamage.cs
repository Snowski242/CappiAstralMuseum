using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    EnemyHP enemHealth;

    public GameObject hitFX;
    public AudioClip hitSFX;

    public float enemyInvuln;
    public float enemyInvulnMax = 60f;


    void Start()
    {
       enemHealth = GetComponent<EnemyHP>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckHoming(transform.position, 1);
        CheckSliding(transform.position, 1);
        enemyInvuln = Mathf.MoveTowards(enemyInvuln, 0, 1f);
    }

    void CheckHoming(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("HomeAtk"));

            if (hitColliders.Length > 0 && enemyInvuln == 0)
            {
                Debug.Log("tmnt");

                PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
                if(Input.GetButtonDown("Fire1"))
                {
                    player.speed = 12;
                }
                player.state = "jump";
                player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                player.isGrounded = false;

            AudioSource.PlayClipAtPoint(hitSFX, transform.position);
            Instantiate(hitFX, transform.position, Quaternion.identity);

            enemHealth.HP -= 1;

            enemyInvuln = enemyInvulnMax;
            }


    }

    void CheckSliding(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("SlideAtk"));

        if (hitColliders.Length > 0 && enemyInvuln == 0)
        {
            Debug.Log("slide hit");

            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;

            AudioSource.PlayClipAtPoint(hitSFX, transform.position);
            Instantiate(hitFX, transform.position, Quaternion.identity);

            enemHealth.HP -= 1;

            enemyInvuln = enemyInvulnMax;
        }


    }
}
