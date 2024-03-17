using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int startingHealth = 100; // the starting health of this object
    private int currentHealth; // the current health of this object
    private bool isInvulnerable = false; // flag to track if the object is currently invulnerable
    private float invulnerabilityTimer = 0.0f; // timer to keep track of how long the object has been invulnerable
    public float invulnerabilityTime = 1.5f; // the amount of time the object is invulnerable after taking damage
    internal Action OnDeath;


    public List<string> ignoredTags = new List<string>();
    // this method will be called when this object takes damage
    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable)
        {
            // if the object is currently invulnerable, do nothing
            return;
        }
        currentHealth -= damageAmount; // subtract the damage from the current health
        Debug.Log("il te reste" + currentHealth + "point de vie");
        if (currentHealth <= 0f)
        {
            Die();  // if the health reaches zero or below, call the Die method
        }

        isInvulnerable = true;
        invulnerabilityTimer = 0.0f;

    }

    // this method will be called when this object dies
    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject); // destroy this object
    }

    // this method is called when the object is instantiated
    private void Start()
    {
        currentHealth = startingHealth; // set the current health to the starting health
    }

    private void Update()
    {
        // update the invulnerability timer if the object is currently invulnerable
        if (isInvulnerable)
        {
            invulnerabilityTimer += Time.deltaTime;
            if (invulnerabilityTimer >= invulnerabilityTime)
            {
                isInvulnerable = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has any of the ignored tags
        if (ignoredTags.Contains(other.tag))
        {
            // Ignore the collision with the ignored tags
            return;
        }

        // Perform actions specific to colliding with other objects
        Debug.Log("Collided with object: " + other.gameObject.name);
    }
}
