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
        Collider[] hitColliders1 = Physics.OverlapSphere(center, radius, LayerMask.GetMask("HomingSafe"));

        if (Input.GetButtonDown("Fire1"))
        {
            if (!pl.groundChecking)
            {
                if (hitColliders.Length > 0)
                {
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].GetComponent<HomingTarget>().homingDelay <= 0)
                        {
                            Debug.Log("tmnt");
                            canHome = false;
                            homingTarget = hitColliders[i].gameObject;
                            hitColliders[i].GetComponent<HomingTarget>().homingDelay = 20;
                            pl.homingFeedback?.PlayFeedbacks();
                            pl.homingTime = 40;
                            pl.state = "homing";
                        }
                    }

                }
                else if (hitColliders1.Length > 0)
                {
                    for (int i = 0; i < hitColliders1.Length; i++)
                    {
                        if (hitColliders1[i].GetComponent<HomingTarget>().homingDelay <= 0)
                        {
                            Debug.Log("tmnt");
                            canHome = false;
                            homingTarget = hitColliders1[i].gameObject;
                            hitColliders1[i].GetComponent<HomingTarget>().homingDelay = 20;
                            pl.homingFeedback?.PlayFeedbacks();
                            pl.homingTime = 40;
                            pl.state = "homing";
                        }
                    }

                }
            }
        }
        
        

    }

    


}
