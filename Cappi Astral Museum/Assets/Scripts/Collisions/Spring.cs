using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 p1 = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(p1, 1, LayerMask.GetMask("Player"));
        //shoots raycast forward to see if theres a raycast hit
        if (hitColliders.Length > 0)
        {


            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
            if (player.canMove)
            {
                player.state = "jump";
                player.transformVelocity.y = Mathf.Sqrt(player.jump * -10f * player.gravity);
                player.isGrounded = false;

                //Instantiate(fx.hitFX, transform.position, Quaternion.identity);
            }

        }
    }
}
