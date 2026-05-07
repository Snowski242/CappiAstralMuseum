using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDamage : MonoBehaviour
{
    public GameObject hitFX;
    public AudioClip hitSFX;

    public float enemyInvuln;
    public float enemyInvulnMax = 60f;


    // Update is called once per frame
    void Update()
    {
        CheckHoming(transform.position, 1);
        enemyInvuln = Mathf.MoveTowards(enemyInvuln, 0, 1f);
    }

    void CheckHoming(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("HomeAtk"));

        if (hitColliders.Length > 0 && enemyInvuln == 0)
        {
            Debug.Log("tmnt balloon");

            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
            if (Input.GetButtonDown("Fire1"))
            {
                player.speed = 15;
            }
            player.jumpFeedback?.PlayFeedbacks();
            player.state = "jump";
            player.transformVelocity.y = Mathf.Sqrt(player.jump * -4f * player.gravity);
            player.isGrounded = false;

            Collider[] hitColliders1 = Physics.OverlapSphere(center, 10, LayerMask.GetMask("Balloon"));
            if (hitColliders1.Length > 0)
            {
                Debug.Log("respawner found");
                BalloonRespawner balloon = hitColliders1[0].GetComponent<BalloonRespawner>();
                balloon.StartCoroutine(balloon.RecreateBalloon());
            }

                AudioSource.PlayClipAtPoint(hitSFX, transform.position);
            Instantiate(hitFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
            enemyInvuln = enemyInvulnMax;
        }


    }
}
