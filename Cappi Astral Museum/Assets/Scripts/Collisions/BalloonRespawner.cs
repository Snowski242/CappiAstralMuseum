using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonRespawner : MonoBehaviour
{
    public GameObject balloon;

    public int waitingTime = 5;

    public IEnumerator RecreateBalloon()
    {
        yield return new WaitForSeconds(waitingTime);
        Instantiate(balloon, transform);
    }
}
