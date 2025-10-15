using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public int level;
    public int gemCount;
    public int munCount;
    public bool munGem;

    public int munHealth;

    public GameObject stellarineObj;



    private void Awake()
    {
        ResetObjectives();
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

    // Start is called before the first frame update
    void Start()
    {
        ResetObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        if(munCount >= 50 && !munGem)
        {
            PlayerMovement player = FindAnyObjectByType(typeof(PlayerMovement)) as PlayerMovement;
            var stellarine = Instantiate(stellarineObj, player.transform.position + new Vector3(0f, 3f, 0f), player.transform.rotation);
            stellarine.GetComponent<StellarineBehavior>().gemID = 7;
            stellarine.GetComponent<StellarineBehavior>().justSpawned = true;

            munGem = true;
        }
    }

    public void ResetObjectives()
    {
        munCount = 0;
        munGem = false;
    }

    public int SetLevel(int lvl) => level = lvl;
}
