using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    public Vector3 offset = new Vector3(0, 4, 0);
    public Transform cam;
    
    void Start()
    {
        cam = Camera.main.transform;
        
        
    }
    private void OnEnable()
    {
        transform.localPosition += offset;
    }

    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
