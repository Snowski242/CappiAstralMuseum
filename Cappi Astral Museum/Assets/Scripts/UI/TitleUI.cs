using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    public bool inMenu;
    public bool darkness;

    [SerializeField] UIDocument mainMenuDocument;

    public GameObject slot;
    public Animator files1;
    public Animator files2;
    public Animator files3;

    private Button optionsButton;
    private Button deleteButton;
    private Button playButton;

   

    private void Awake()
    {
        VisualElement root = mainMenuDocument.rootVisualElement;

        optionsButton = root.Q<Button>("settings");
        playButton = root.Q<Button>("play");
        deleteButton = root.Q<Button>("delete");

        optionsButton.clickable.clicked += ShowOptionsMenu;
        playButton.clickable.clicked += PlayGame;
        deleteButton.clickable.clicked += DeleteSave;
    }

    public void PlayGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "Hub")
        {
            SceneManager.LoadScene("Hub");
        }
    }

    private void ShowOptionsMenu()
    {
        print("Options menu will be here");
    }

    
    private void DeleteSave()
    {
        QuickSaveWriter.DeleteRoot("Gems");
        print("Save deleted");
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
