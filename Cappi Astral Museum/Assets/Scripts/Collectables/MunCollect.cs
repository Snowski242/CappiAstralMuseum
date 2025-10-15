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
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Instantiate(collectFX, transform.position + offset, Quaternion.identity);
            PlayerDamage player = FindFirstObjectByType(typeof(PlayerDamage)) as PlayerDamage;
            
            if(player.healthPoints < 8)
            {
                ObjectiveManager.instance.munHealth++;
                if (ObjectiveManager.instance.munHealth >= 6)
                {
                    player.healthPoints++;
                    ObjectiveManager.instance.munHealth = 0;
                }
                
            }
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            OnCoinCollect?.Invoke();
            ObjectiveManager.instance.munCount++;

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
