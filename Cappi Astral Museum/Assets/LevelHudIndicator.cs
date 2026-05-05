using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelHudIndicator : MonoBehaviour
{
    public GameObject blackbar;
    public TextMeshProUGUI blackText;

    public Image gem1;
    public Image gem2;
    public Image gem3;
    public Image gem4;
    public Image gem5;
    public Image gem6;
    public Image gem7;
    public Image gem8;
    public Sprite completedGem;
    public Sprite incompleteGem;
    

    

    // Update is called once per frame
    void Update()
    {
        CheckPlayer(transform.position, 12);
    }

    void CheckPlayer(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Painting"));

        if (hitColliders.Length > 0)
        {
            blackbar.SetActive(true);

            LevelSelector selector = hitColliders[0].GetComponent<LevelSelector>();

            for (int i = 0; i < SaveManager.instance.gems.Count; i++)
            {
                if (selector.gemList[0] == SaveManager.instance.gems[i])
                {
                    gem1.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[1] == SaveManager.instance.gems[i])
                {
                    gem2.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[2] == SaveManager.instance.gems[i])
                {
                    gem3.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[3] == SaveManager.instance.gems[i])
                {
                    gem4.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[4] == SaveManager.instance.gems[i])
                {
                    gem5.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[5] == SaveManager.instance.gems[i])
                {
                    gem6.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[6] == SaveManager.instance.gems[i])
                {
                    gem7.sprite = completedGem;

                }
                else
                {
                    
                }

                if (selector.gemList[7] == SaveManager.instance.gems[i])
                {
                    gem8.sprite = completedGem;

                }
                else
                {
                    
                }
            }

            switch (selector.levelID)
            {
                case 0:
                    blackText.text = "Mystic Plant";

                    break;

                case 1:
                    blackText.text = "Emberion";

                    break;
            }


        }
        else
        {
            blackbar.SetActive(false);
            gem1.sprite = incompleteGem;
            gem2.sprite = incompleteGem;
            gem3.sprite = incompleteGem;
            gem4.sprite = incompleteGem;
            gem5.sprite = incompleteGem;
            gem6.sprite = incompleteGem;
            gem7.sprite = incompleteGem;
            gem8.sprite = incompleteGem;
        }


    }
}
