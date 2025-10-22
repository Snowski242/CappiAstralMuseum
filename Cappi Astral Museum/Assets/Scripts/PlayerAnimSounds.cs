using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSounds : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip railSound;

    public void RunSound()
    {
        AudioSource.PlayClipAtPoint(walkSound, transform.position);
    }

    public void RailSound()
    {
        AudioSource.PlayClipAtPoint(railSound, transform.position);
    }
}
