using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null )
        {
            damageDealer.Hit();
            TakeDamage(damageDealer.getDamage());
        }
    }

    void TakeDamage(int damageTaken)
    {
        Debug.Log(health);
        health -= damageTaken;
        if (health <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
