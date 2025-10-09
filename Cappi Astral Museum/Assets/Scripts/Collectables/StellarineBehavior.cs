using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarineBehavior : MonoBehaviour
{
    [SerializeField]public int gemID = 0;
    public bool gotten = false;
    public GameObject winScreen;
    public AudioClip sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {

        for (int i = 0; i < SaveManager.instance.gems.Count; i++)
        {
            if (SaveManager.instance.gems[i] == gemID)
            {
                gotten = true;

            }
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
            for (int i = 0; i < SaveManager.instance.gems.Count; i++)
            {
                if (SaveManager.instance.gems[i] == gemID)
                {
                    gotten = true;

                }
            }
            if (!gotten)
            {
                Debug.Log("got star!");
                SaveManager.instance.gems.Insert(0, gemID);
            }
            else {
                Debug.Log("already got star..");
            }

            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

            pauseMenu.canPause = false;
            player.state = "win";
            AudioSource.PlayClipAtPoint(sfx, transform.position);
            Instantiate(winScreen);
            SaveManager.instance.Save();
            transform.localScale = Vector3.zero;
        }
    }
}
