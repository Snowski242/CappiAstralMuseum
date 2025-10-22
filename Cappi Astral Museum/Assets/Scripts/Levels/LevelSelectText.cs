using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectText : MonoBehaviour
{
    public Image gem;
    public Sprite completedGem;
    public Sprite incompleteGem;
    public TextMeshProUGUI text;
    public int gemID;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SaveManager.instance.gems.Count; i++)
        {
            if (SaveManager.instance.gems[i] == gemID)
            {
                gem.sprite = completedGem;

            }
        }

        if (ObjectiveManager.instance.level == 0)
        {
            if(gemID == 0)
            {
                text.text = "Reach the platform!";
            }

            if (gemID == 1)
            {
                text.text = "Jump past the cliff!";
            }

            if (gemID == 2)
            {
                text.text = "Defeat the giant Peon!";
            }

            if (gemID == 3)
            {
                text.text = "Get on top of a mountain";
            }

            if (gemID == 4)
            {
                text.text = "Defeat the wave of Cursed at the platform!";
            }

            if (gemID == 5)
            {
                text.text = "Get all red 8 muns";
            }

            if (gemID == 6)
            {
                text.text = "Open a hidden capsule";
            }

            if (gemID == 7)
            {
                text.text = "Get 50 muns!";
            }

            if (gemID == 8)
            {
                text.text = "Get 50 muns!";
            }
        }
    }
}
