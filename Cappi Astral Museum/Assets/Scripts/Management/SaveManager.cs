using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public List<int> gems;

    public static SaveManager instance;

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
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        QuickSaveWriter.Create("Gems").Write("Amount", gems)
            .Commit();

        CutsceneFlags.instance.SavingCutsceneFlags();
    }

    public void Load()
    {
        var reader = QuickSaveReader.Create("Gems");
        reader.Read<List<int>>("Amount", r => gems = r);


        var reader2 = QuickSaveReader.Create("Flags");
        reader2.Read<bool>("Intro", r => CutsceneFlags.instance.introCutscene = r);
        reader2.Read<bool>("WindyCut", r => CutsceneFlags.instance.removedWindyDoor = r);

    }
}
