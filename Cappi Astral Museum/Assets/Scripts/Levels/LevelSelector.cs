using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int levelID;
    public GameObject offset;
    void Start()
    {
        if(ObjectiveManager.instance.level == levelID)
        {
            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;

            player.transform.position = offset.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ObjectiveManager.instance.level = levelID;
            SceneManager.LoadScene("LoadToLevel");
            
        }
    }
}
