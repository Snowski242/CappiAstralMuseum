using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;
using TMPro;

public class GoToLevel : MonoBehaviour
{
    public float inputDelay = 200;
    public TextMeshProUGUI text;

    public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputDelay = Mathf.MoveTowards(inputDelay, 0, 1);

        if(inputDelay <= 0 && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Trigger");
        }

        if(ObjectiveManager.instance.level == 0)
        {
            text.text = "Mystic Plant";
        }
    }

    public void GetReady()
    {
        if (ObjectiveManager.instance.level == 0)
        {
            SceneManager.LoadScene("MysticPlant");
        }
    }
}
