using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public bool inMenu;
    public bool darkness;

    public GameObject slot;
    public Animator files1;
    public Animator files2;
    public Animator files3;

    public void GoToHub()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "Hub")
        {
            SceneManager.LoadScene("Hub");
        }
    }

    public void File()
    {
        if(!inMenu)
        {
            slot.SetActive(false);
            files1.SetBool("Slide", true);
            files2.SetBool("Poll", true);
            inMenu = true;
        }
        
    }

    public void Prepare()
    {
        files3.SetBool("KK", true);
        darkness = true;
        if (!darkness)
        {
            files3.SetBool("KK", true);
            darkness = true;
        }
    }

    public void DeleteFile()
    {
        QuickSaveWriter.DeleteRoot("Gems");
    }
}
