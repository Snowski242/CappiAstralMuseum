using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerMovement player;

    public float invulnTimerMax = 240f;
    public float invulnTimer = 0;

    public AudioClip hurtSound;
    public GameObject hurtFX;

    public GameObject dedUI;

    public int healthPoints = 8;
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        invulnTimer -= 1f;

        if (invulnTimer < 0)
        {
            invulnTimer = 0;
        }

        if (healthPoints <= 0)
        {
            if (player.canMove)
            {
                Instantiate(dedUI);
                player.state = "ded";
            }

        }

        CheckHurt(transform.position, 1);
    }

    void CheckHurt(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Homing"));

        if (hitColliders.Length > 0 && invulnTimer == 0 && player.state == "walk" || hitColliders.Length > 0 && invulnTimer == 0 && player.state == "idle" || hitColliders.Length > 0 && invulnTimer == 0 && player.state == "rev" || hitColliders.Length > 0 && invulnTimer == 0 && player.state == "revrun")
        {
            Hurt();
        }


    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (invulnTimer <= 0)
    //    {
    //        if (other.gameObject.tag == "enemy1" && player.state == "walk" || other.gameObject.tag == "enemy1" && player.state == "idle" || other.gameObject.tag == "enemy1" && player.state == "rev" || other.gameObject.tag == "enemy1" && player.state == "revrun")
    //        {
    //            Hurt();
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (invulnTimer <= 0)
    //    {
    //        if (other.gameObject.tag == "enemy1" && player.state == "walk" || other.gameObject.tag == "enemy1" && player.state == "idle" || other.gameObject.tag == "enemy1" && player.state == "rev" || other.gameObject.tag == "enemy1" && player.state == "revrun")
    //        {
    //            Hurt();
    //        }
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (invulnTimer <= 0)
    //    {
    //        if (other.gameObject.tag == "enemy1" && player.state == "walk" || other.gameObject.tag == "enemy1" && player.state == "idle" || other.gameObject.tag == "enemy1" && player.state == "rev" || other.gameObject.tag == "enemy1" && player.state == "revrun")
    //        {
    //            Hurt();
    //        }
    //    }
    //}

    public void Hurt()
    {
        player.airBoostTime = 10;
        player.transformVelocity.y = Mathf.Sqrt(player.jump * -1.2f * player.gravity);
        Instantiate(hurtFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(hurtSound, transform.position);
        player.isGrounded = false;

        healthPoints--;
        if (healthPoints <= 0)
        {
            if (player.canMove)
            {
                Instantiate(dedUI);
                player.state = "ded";
            }

        }
        else
        {
            player.state = "hurt";
        }

        invulnTimer = invulnTimerMax;

    }

}
