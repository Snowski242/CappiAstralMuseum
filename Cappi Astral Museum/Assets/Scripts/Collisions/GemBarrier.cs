using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemBarrier : MonoBehaviour
{
    public int gemRequirement = 2;
    public TextMeshProUGUI textM;

    public int id = 0;
    void Start()
    {
        if (CutsceneFlags.instance.removedWindyDoor)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        textM.text = gemRequirement.ToString();

        CheckGemCount(transform.position, 3.5f);
    }

    void CheckGemCount(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Player"));

        if (hitColliders.Length > 0 && SaveManager.instance.gems.Count >= gemRequirement)
        {
            Debug.Log("player hit");

            Destroy(gameObject);

            if(id == 0)
            {
                CutsceneFlags.instance.removedWindyDoor = true;

                CutsceneFlags.instance.SavingCutsceneFlags();
            }
        }


    }
}
