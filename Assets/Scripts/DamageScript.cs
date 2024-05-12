using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float attackRange = 2f; // Range of the weapon's attack
    public int attackDamage = 20; // Amount of damage inflicted by the weapon

    private GameObject player; // Reference to the player object

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
            return;

        // Check for input (e.g., left mouse button click) to trigger an attack
        if (Input.GetMouseButtonDown(0))
        {
            // Ensure the weapon is held by the player (attached to player GameObject)
            if (transform.parent != null && transform.parent.CompareTag("Player"))
            {
                PerformAttack();
            }
        }
    }

    private void PerformAttack()
    {
        // Calculate the attack direction (forward direction of the player)
        Vector3 attackDirection = player.transform.right;

        // Move the weapon forward (translate its position in the attack direction)
        transform.Translate(attackDirection * attackRange);

        // Debug information
        Debug.DrawRay(transform.position, attackDirection * attackRange, Color.red); // Visualize the raycast

        // Perform a raycast to detect enemies within attack range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange);

        if (hit.collider != null)
        {
            // Check if the collided object is an enemy (tagged as "Enemy")
            if (hit.collider.CompareTag("Enemy"))
            {
                // Attempt to access the HealthScript component of the enemy
                HealthScript enemyHealth = hit.collider.GetComponent<HealthScript>();

                if (enemyHealth != null)
                {
                    // Debug information
                    Debug.Log("Enemy hit: " + hit.collider.gameObject.name);

                    // Inflict damage on the enemy by calling the TakeDamageFromSword method
                    enemyHealth.TakeDamageFromSword(attackDamage);

                    // Debug information
                    Debug.Log("Damage inflicted: " + attackDamage);
                }
            }
        }
    }


}
