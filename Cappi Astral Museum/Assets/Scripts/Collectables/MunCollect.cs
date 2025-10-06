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
