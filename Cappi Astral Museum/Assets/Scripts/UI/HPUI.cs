using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Sprite hp8;
    public Sprite hp7;
    public Sprite hp6;
    public Sprite hp5;
    public Sprite hp4;
    public Sprite hp3;
    public Sprite hp2;
    public Sprite hp1;
    public Sprite hp0;

    public Image hp;



    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        hp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDamage health = FindFirstObjectByType<PlayerDamage>();

        if (health.healthPoints == 8)
        {
            hp.sprite = hp8;
            animator.SetBool("hp", false);
        }
        else
        {
            animator.SetBool("hp", true);

            if(health.healthPoints == 7)
            {
                hp.sprite = hp7;
            }
            else if (health.healthPoints == 6)
            {
                hp.sprite = hp6;
            }
            else if (health.healthPoints == 5)
            {
                hp.sprite = hp5;
            }
            else if (health.healthPoints == 4)
            {
                hp.sprite = hp4;
            }
            else if (health.healthPoints == 3)
            {
                hp.sprite = hp3;
            }
            else if (health.healthPoints == 2)
            {
                hp.sprite = hp2;
            }
            else if (health.healthPoints == 1)
            {
                hp.sprite = hp1;
            }
            else if (health.healthPoints <= 0)
            {
                hp.sprite = hp0;
            }
        }
    }
}
