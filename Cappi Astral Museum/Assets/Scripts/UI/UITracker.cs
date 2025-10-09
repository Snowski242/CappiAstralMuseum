using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITracker : MonoBehaviour
{
    public TextMeshProUGUI munAmount;
    public TextMeshProUGUI gemAmount;
    public Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        munAmount.text = ObjectiveManager.instance.munCount.ToString();
        gemAmount.text = SaveManager.instance.gems.Count.ToString();
    }

    private void OnEnable()
    {
        MunCollect.OnCoinCollect += CoinTrig;
    }

    private void OnDisable()
    {
        MunCollect.OnCoinCollect -= CoinTrig;
    }

    private void CoinTrig()
    {
        animator.SetTrigger("Coin");
    }
}
