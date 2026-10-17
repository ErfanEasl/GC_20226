using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public PlayerHealth health;
    public Image[] lightStamina;
    public int lightNumber = 0;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health != null && healthBar != null)
        {
            healthBar.fillAmount = (float)health.currentHealth/health.maxHealth;
        }
    }
    public void UpdateStamina(float staminaPercent)
    {
        if(lightNumber>= lightStamina.Length)
        {
            return;

            
        }
    }
    public void LoseLight()
    {
        if(lightNumber>= lightStamina.Length)
        
            return;

        lightStamina[lightNumber].fillAmount = 0f;
        lightNumber++;
    }
}
