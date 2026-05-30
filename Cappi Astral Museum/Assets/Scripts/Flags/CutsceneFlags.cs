using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneFlags : MonoBehaviour
{
    public static CutsceneFlags instance;

    public bool introCutscene;
    public bool removedWindyDoor;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SaveManager.instance.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavingCutsceneFlags()
    {
        QuickSaveWriter.Create("Flags").Write("Intro", introCutscene)
            .Commit();

        QuickSaveWriter.Create("Flags").Write("WindyCut", removedWindyDoor)
            .Commit();
    }
}
