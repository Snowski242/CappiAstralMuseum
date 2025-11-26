using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public int capsuleType;

    public GameObject stellarineObj;
    public GameObject mun;
    public GameObject spawn;
    public int stellarInd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit downHit;

        Vector3 p1 = transform.position;

        //shoots raycast forward to see if theres a raycast hit
        if (Physics.SphereCast(p1, 0.9f, transform.up, out downHit, 0.4f, LayerMask.GetMask("Player")))
        {


            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
            if (player.canMove)
            {
                player.state = "jump";
                player.transformVelocity.y = Mathf.Sqrt(player.jump * -2f * player.gravity);
                player.isGrounded = false;

                if(capsuleType == 0)
                {
                    Instantiate(mun, spawn.transform.position + new Vector3(Random.Range(0.1f, 3.1f), 0f, Random.Range(0.1f, 3.1f)), transform.rotation);
                    Instantiate(mun, spawn.transform.position + new Vector3(Random.Range(0.1f,3.1f), 0f, Random.Range(0.1f, 3.1f)), transform.rotation);
                    Instantiate(mun, spawn.transform.position + new Vector3(Random.Range(0.1f, 3.1f), 0f, Random.Range(0.1f, 3.1f)), transform.rotation);
                    Instantiate(mun, spawn.transform.position + new Vector3(Random.Range(0.1f, 3.1f), 0f, Random.Range(0.1f, 3.1f)), transform.rotation);
                    Instantiate(mun, spawn.transform.position + new Vector3(Random.Range(0.1f, 3.1f), 0f, Random.Range(0.1f, 3.1f)), transform.rotation);
                }
                else if (capsuleType == 1)
                {
                    var stellarine = Instantiate(stellarineObj, transform.position + new Vector3(0f, 5f, 0f), transform.rotation);
                    stellarine.GetComponent<StellarineBehavior>().gemID = stellarInd;
                    stellarine.GetComponent<StellarineBehavior>().justSpawned = true;
                }

                Destroy(gameObject);

            }

        }
    }
}
