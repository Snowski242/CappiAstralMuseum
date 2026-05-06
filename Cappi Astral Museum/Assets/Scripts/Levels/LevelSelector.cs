using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int levelID;
    public GameObject offset;
    public GameObject player;
    public List<int> gemList = new List<int>();

    bool interact = true;

    public bool tp = true;
    void Start()
    {
        if(ObjectiveManager.instance.level == levelID)
        {
            //PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;

            player.transform.position = offset.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectiveManager.instance.level == levelID && tp)
        {
            
            //PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;

            if (player.transform.position != offset.transform.position)
            {
                player.transform.position = offset.transform.position;
            }
            else
            {
                tp = false;
            }
            

            
        }

        CheckPlayer(transform.position, 12);
    }

    void CheckPlayer(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Player"));
        PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;

        if (hitColliders.Length > 0)
        {

            if (Input.GetMouseButtonDown(0) && interact && player.isGrounded)
            {
                
                player.animator.SetBool("Level", true);

                player.transform.rotation = Quaternion.Euler(0,0,0);
                player.state = "null";

                CinemachineFreeLook cam = FindFirstObjectByType<CinemachineFreeLook>();
                if (cam != null)
                {
                    cam.m_XAxis.m_MaxSpeed = 0f;
                    cam.m_YAxis.m_MaxSpeed = 0f;

                    cam.m_XAxis.m_MinValue = 0f;
                    cam.m_YAxis.Value = 0f;


                    //cam.LookAt = player.transform;
                }
                interact = false;
            }
            
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            
        }
    }
}
