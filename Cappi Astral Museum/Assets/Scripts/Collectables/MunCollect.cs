using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunCollect : MonoBehaviour
{
    public AudioClip soundEffect;

    public static event Action OnCoinCollect;

    public GameObject collectFX;

    public Vector3 offset;

    public bool redMun;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Instantiate(collectFX, transform.position + offset, Quaternion.identity);
            PlayerDamage player = FindFirstObjectByType(typeof(PlayerDamage)) as PlayerDamage;
            
            if(player.healthPoints < 8)
            {
                if(!redMun)
                {
                    ObjectiveManager.instance.munHealth++;
                    if (ObjectiveManager.instance.munHealth >= 6)
                    {
                        player.healthPoints++;
                        ObjectiveManager.instance.munHealth = 0;
                    }
                }
                else
                {
                    ObjectiveManager.instance.munHealth+= 2;
                    if (ObjectiveManager.instance.munHealth >= 6)
                    {
                        player.healthPoints++;
                        ObjectiveManager.instance.munHealth = 0;
                    }
                }
                
            }
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            OnCoinCollect?.Invoke();
            if(!redMun)
            {
                ObjectiveManager.instance.munCount++;
            }
            else
            {
                ObjectiveManager.instance.munCount+= 2;
                ObjectiveManager.instance.redMun++;
            }
            

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
