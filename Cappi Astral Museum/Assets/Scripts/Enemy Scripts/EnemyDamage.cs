using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    EnemyHP enemHealth;

    public GameObject hitFX;


    void Start()
    {
       enemHealth = GetComponent<EnemyHP>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CheckHoming(transform.position, 1);
    }

    void CheckHoming(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("HomeAtk"));

            if (hitColliders.Length > 0)
            {
                Debug.Log("tmnt");

                PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
                player.state = "jump";
                player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                player.isGrounded = false;

            Instantiate(hitFX, transform.position, Quaternion.identity);

            enemHealth.HP -= 1;
            }


    }
}
