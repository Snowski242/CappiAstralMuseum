using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITracker : MonoBehaviour
{
    public TextMeshProUGUI munAmount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        munAmount.text = ObjectiveManager.instance.munCount.ToString();
    }
}
