using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerMovement player;

    public float invulnTimerMax = 240f;
    public float invulnTimer = 0;

    public AudioClip hurtSound;
    public GameObject hurtFX;
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        invulnTimer -= 1f;

        if(invulnTimer < 0)
        {
            invulnTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (invulnTimer <= 0)
        {
            if(other.gameObject.tag == "enemy1" && player.state == "walk")
            {
                player.airBoostTime = 5;
                player.transformVelocity.y = Mathf.Sqrt(player.jump * -1.2f * player.gravity);
                Instantiate(hurtFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(hurtSound, transform.position);
                player.state = "hurt";

                invulnTimer = invulnTimerMax;
            }
        }
    }
}
