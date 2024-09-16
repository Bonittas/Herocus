using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] public float damage = 5f;
    private Coroutine damageCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            damageCoroutine = StartCoroutine(ApplyDamage(playerHealth));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator ApplyDamage(PlayerHealth playerHealth)
    {
        while (true)
        {
            playerHealth.HealthDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }
}