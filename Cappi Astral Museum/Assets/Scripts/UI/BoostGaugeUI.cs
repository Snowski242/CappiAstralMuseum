using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tensionGaugeUI : MonoBehaviour
{
    public PlayerMovement player;
    public Image healthBar, ringHealthBar;
    public Image[] healthPoints;


    float lerpSpeed;

    private void Start()
    {

    }

    private void Update()
    {


        if(player.tensionGauge == player.tensionGaugeMax)
        {
            
        }
        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (player.tensionGauge / player.tensionGaugeMax), lerpSpeed);
        ringHealthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (player.tensionGauge / player.tensionGaugeMax), lerpSpeed);

        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(player.tensionGauge, i);
        }
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.blue, Color.cyan, (player.tensionGauge / player.tensionGaugeMax));
        healthBar.color = healthColor;
        ringHealthBar.color = healthColor;
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }
}
