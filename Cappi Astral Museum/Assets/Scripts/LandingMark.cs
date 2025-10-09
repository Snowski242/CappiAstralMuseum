using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingMark : MonoBehaviour
{
    public Transform player;
    public float offset = 0.05f; // small offset to avoid z-fighting

    void Update()
    {
        // Cast a ray down from the player
        if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hit, 100f))
        {
            transform.position = hit.point + Vector3.up * offset;

            // Rotate to match the ground
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            // If no ground found, hide marker
            transform.position = new Vector3(0, -9999, 0);
        }
    }
}
