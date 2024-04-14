using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public int damageAmount = 1; // the amount of damage this script will do
    public string playerTag = "Player"; // the tag of the player GameObject
    public float attackCooldown = 1f; // cooldown time between attacks
    private bool isOnCooldown = false; // flag to track if the sword is on cooldown
    private float cooldownTimer = 0f; // timer to keep track of cooldown

    // Update is called once per frame
    void Update()
    {
        // Update cooldown timer if the sword is on cooldown
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCooldown)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    // this method will be called when this object collides with another object
    private void OnTriggerEnter(Collider other)
    {
        if (!isOnCooldown && Input.GetMouseButtonDown(0)) // Check if left mouse button is pressed and sword is not on cooldown
        {
            if (other.CompareTag(playerTag)) // Check if collided object has the Player tag
            {
                HealthScript healthScript = other.GetComponent<HealthScript>();
                if (healthScript != null)
                {
                    healthScript.TakeDamageFromSword(damageAmount); // Apply damage to the player with the sword
                    isOnCooldown = true; // Put the sword on cooldown
                }
            }
        }
    }
}
