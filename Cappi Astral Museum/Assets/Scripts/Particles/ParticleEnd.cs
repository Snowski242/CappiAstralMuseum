using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnd : MonoBehaviour
{
    ParticleSystem _system;
    void Start()
    {
        _system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_system.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
