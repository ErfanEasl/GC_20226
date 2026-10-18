using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3;
    public float currentHealth = 1;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        
        if (currentHealth <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
    }
   public void Die()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Watcher"))
        {
            TakeDamage(.1f);
        }
    }
}
