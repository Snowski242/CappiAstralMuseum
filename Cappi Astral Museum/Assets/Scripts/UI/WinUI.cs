using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        CinemachineFreeLook cam = FindFirstObjectByType<CinemachineFreeLook>();
        if (cam != null)
        {
            cam.m_XAxis.m_MaxSpeed = 0f;
            cam.m_YAxis.m_MaxSpeed = 0f;

            cam.m_YAxis.Value = 0;
        }
    }

    public void GoBackToHub()
    {
        ObjectiveManager.instance.ResetObjectives();
        SaveManager.instance.Save();
        SceneManager.LoadScene("Hub");
    }
}
