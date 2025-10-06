using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTarget : MonoBehaviour
{
    public float homingDelayMax = 200;
    public float homingDelay = 200;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(homingDelay > 0)
        {
            homingDelay--;
        }
    }
}
