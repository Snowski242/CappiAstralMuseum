using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{



    public GameObject PauseMenu;
    public Image Button1;
    public Image Button2;
    public Image Button3;
    public static bool isPaused;
    public static bool canPause = true;
    int inputDelay;



    public bool isInMenus;
    public int options;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        isPaused = false;
        inputDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (inputDelay > 0)
        {
            inputDelay -= 1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (options == 0)
            {
                ResumeGame();
            }

            if (options == 1)
            {
                RestartScene();
            }

            if (options == 2)
            {
                QuitGame();
            }
        }

        if (isPaused)
        {
            if (vertical == 1)
            {
                if (inputDelay <= 0)
                {
                    inputDelay = 60;
                    options--;
                    if (options < 0)
                    {
                        options = 2;
                    }
                }
            }

            if (vertical == -1)
            {
                if (inputDelay <= 0)
                {
                    inputDelay = 60;
                    options++;
                    if (options > 2)
                    {
                        options = 0;
                    }
                }
            }

            if (options == 0)
            {
                Button1.gameObject.SetActive(true);
            }
            else
            {
                Button1.gameObject.SetActive(false);
            }

            if (options == 1)
            {
                Button2.gameObject.SetActive(true);
            }
            else
            {
                Button2.gameObject.SetActive(false);
            }

            if (options == 2)
            {
                Button3.gameObject.SetActive(true);
            }
            else
            {
                Button3.gameObject.SetActive(false);
            }
        }
        
        if (Input.GetButtonDown("Pause") && canPause)
        {
            if(isPaused)
            {
                

            

                

                
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        
 
    }

    public void PauseGame()
    {

        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {

        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;


    }

    public void QuitGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Hub")
        {
            SceneManager.LoadScene("Hub");
        }
        else
        {
            SaveManager.instance.Save();
            Application.Quit();
        }

        
    }

    public void RestartScene()
    {

        ObjectiveManager.instance.ResetObjectives();
        StartCoroutine(LoadYourAsyncScene());

    }

 


    IEnumerator LoadYourAsyncScene()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/" + SceneManager.GetActiveScene().name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
