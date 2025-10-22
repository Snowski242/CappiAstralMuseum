using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRing : MonoBehaviour
{
    public float delay = 200;
    public GameObject bc;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            other.transform.position = bc.transform.position;
            player.transform.rotation = transform.rotation;
            Instantiate(player.airDashFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(player.airDashSound, transform.position);
            Debug.Log("boost (jump)");
            player.ringBoostTimer = 20f;
            player.transformVelocity.y = Mathf.Sqrt(player.jump * -1.75f * player.gravity);
            player.canAirBoost = false;
            player.cloudVFX.Play();
            player.state = "boostring";
        }
    }
}
