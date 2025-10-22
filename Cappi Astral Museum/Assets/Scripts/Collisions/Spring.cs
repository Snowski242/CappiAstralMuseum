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
        RaycastHit downHit;

        Vector3 p1 = transform.position;

        //shoots raycast forward to see if theres a raycast hit
        if (Physics.SphereCast(p1, 0.4f, transform.up, out downHit, 1.1f, LayerMask.GetMask("Player")))
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
