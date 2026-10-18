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

    public bool hasLight => lightNumber < lightStamina.Length;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health != null && healthBar != null)
        {
            healthBar.fillAmount = (float)health.currentHealth / health.maxHealth;
        }
    }
    public void UpdateStamina(float staminaPercent)
    {
        if (!hasLight)
            return;

        lightStamina[lightNumber].fillAmount = Mathf.Clamp01(staminaPercent);

    }
    public void LoseLight()
    {
        if (!hasLight)
            return;
        lightStamina[lightNumber].fillAmount = 0f;
        lightStamina[lightNumber].enabled = false;
        lightNumber++;
        if (lightNumber >= lightStamina.Length)
        {
            health.Die();
            return;
        }
        lightStamina[lightNumber].enabled = true;
        lightStamina[lightNumber].fillAmount = 1f;
    }
}
