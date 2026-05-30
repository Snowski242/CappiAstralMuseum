using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    int id = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(id == 0)
            {
                CutsceneFlags.instance.introCutscene = true;

                CutsceneFlags.instance.SavingCutsceneFlags();

                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name != "Hub")
                {
                    SceneManager.LoadScene("Hub");
                }
            }
            
        }
    }
}
