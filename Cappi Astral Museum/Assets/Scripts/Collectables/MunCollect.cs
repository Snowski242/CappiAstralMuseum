using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunCollect : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ObjectiveManager.instance.munCount++;

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
