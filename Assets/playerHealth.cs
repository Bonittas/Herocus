using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // To load Game Over scene

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void HealthDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optionally trigger death animation
        Debug.Log("Player is Dead");
        SceneManager.LoadScene("GameOver"); // Load the Game Over scene
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
}
