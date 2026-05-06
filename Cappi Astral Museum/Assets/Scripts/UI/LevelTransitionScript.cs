using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour
{
    public Animator whiteFlash;
    public void WhiteFlash()
    {
        whiteFlash.SetBool("F", true);
    }

    public void TriggerLevel()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 12, LayerMask.GetMask("Painting"));
        LevelSelector selector = hitColliders[0].GetComponent<LevelSelector>();
        ObjectiveManager.instance.level = selector.levelID;
        SceneManager.LoadScene("LoadToLevel");
    }
}
