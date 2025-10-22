using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public bool waveTriggered;

    public bool waveDone;

    public int enemyAmount;

    public int waveID;

    public int waveStage = 0;

    public bool waveActive = false;

    public float waveSpawnDelay = 5;

    public GameObject stellarineObj;

    [Header("Regions")]
    public GameObject waveSpawn1;
    public GameObject waveSpawn2;
    public GameObject waveSpawn3;
    public GameObject waveSpawn4;
    public GameObject waveSpawn5;
    public GameObject waveSpawn6;
    public GameObject waveSpawn7;
    public GameObject waveSpawn8;

    [Header("Enemy Prefabs")]
    public GameObject peon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyAmount <= 0 && !waveDone)
        {
            if(waveActive)
            {
                StartCoroutine(WaveDelay());
                waveActive = false;

            }
        }
    }

    void StartWave()
    {
        switch (waveID)
        {
            case 0:
                if (!waveActive && waveStage < 4)
                {
                    waveStage++;
                    if (waveStage == 1)
                    {
                        enemyAmount = 4;
                        Debug.Log("wave start");
                       var e1 = Instantiate(peon, waveSpawn1.transform);
                        e1.GetComponent<EnemyBehavior>().inWave = true;

                        var e2 = Instantiate(peon, waveSpawn4.transform);
                        e2.GetComponent<EnemyBehavior>().inWave = true;

                        var e3 = Instantiate(peon, waveSpawn6.transform);
                        e3.GetComponent<EnemyBehavior>().inWave = true;

                        var e4 = Instantiate(peon, waveSpawn7.transform);
                        e4.GetComponent<EnemyBehavior>().inWave = true;
                    }
                    else if (waveStage == 2)
                    {
                        enemyAmount = 4;
                        Debug.Log("wave start");
                        var e1 = Instantiate(peon, waveSpawn1.transform);
                        e1.GetComponent<EnemyBehavior>().inWave = true;

                        var e2 = Instantiate(peon, waveSpawn3.transform);
                        e2.GetComponent<EnemyBehavior>().inWave = true;

                        var e3 = Instantiate(peon, waveSpawn2.transform);
                        e3.GetComponent<EnemyBehavior>().inWave = true;

                        var e4 = Instantiate(peon, waveSpawn5.transform);
                        e4.GetComponent<EnemyBehavior>().inWave = true;
                    }
                    else if (waveStage == 3)
                    {
                        enemyAmount = 4;
                        Debug.Log("wave start");
                        var e1 = Instantiate(peon, waveSpawn4.transform);
                        e1.GetComponent<EnemyBehavior>().inWave = true;

                        var e2 = Instantiate(peon, waveSpawn8.transform);
                        e2.GetComponent<EnemyBehavior>().inWave = true;

                        var e3 = Instantiate(peon, waveSpawn2.transform);
                        e3.GetComponent<EnemyBehavior>().inWave = true;

                        var e4 = Instantiate(peon, waveSpawn7.transform);
                        e4.GetComponent<EnemyBehavior>().inWave = true;
                    }
                    else if (waveStage == 4)
                    {
                        enemyAmount = 4;
                        Debug.Log("wave start");
                        var e1 = Instantiate(peon, waveSpawn1.transform);
                        e1.GetComponent<EnemyBehavior>().inWave = true;

                        var e2 = Instantiate(peon, waveSpawn4.transform);
                        e2.GetComponent<EnemyBehavior>().inWave = true;

                        var e3 = Instantiate(peon, waveSpawn6.transform);
                        e3.GetComponent<EnemyBehavior>().inWave = true;

                        var e4 = Instantiate(peon, waveSpawn7.transform);
                        e4.GetComponent<EnemyBehavior>().inWave = true;
                    }
                    waveActive = true;
                }
                else if(waveStage >= 4)
                {
                    waveDone = true;
                    var stellarine = Instantiate(stellarineObj, transform.position + new Vector3(0f, 4.2f, 0f), transform.rotation);
                    stellarine.GetComponent<StellarineBehavior>().gemID = 4;
                    stellarine.GetComponent<StellarineBehavior>().justSpawned = true;
                }

                break;
        }
    }

    IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(waveSpawnDelay);
        StartWave();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!waveTriggered)
            {
                StartWave();
                waveTriggered = true;
            }
        }
    }
}
