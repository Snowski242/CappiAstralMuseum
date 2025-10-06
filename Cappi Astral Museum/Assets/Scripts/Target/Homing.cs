using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public GameObject homingTarget;
    public bool canHome = true;
    public PlayerMovement pl;

    private void Update()
    {
        transform.position = pl.transform.position;
        CheckHoming(transform.position, 10);
    }

    void CheckHoming(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Homing"));

        if(Input.GetButtonDown("Fire1"))
        if(hitColliders.Length > 0 )
        {
            for(int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<HomingTarget>().homingDelay <= 0)
                    {
                        Debug.Log("tmnt");
                        canHome = false;
                        homingTarget = hitColliders[i].gameObject;
                        hitColliders[i].GetComponent<HomingTarget>().homingDelay = 120;
                        pl.homingTime = 100;
                        pl.state = "homing";
                    }
                }
            
        }
        

    }

    


}
