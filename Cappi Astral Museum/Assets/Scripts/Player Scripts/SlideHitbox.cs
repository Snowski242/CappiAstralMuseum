using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHitbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            transform.position = player.transform.position;
            if(player.state != "slide")
            {
                Destroy(gameObject);
            }
        }
    }
}
