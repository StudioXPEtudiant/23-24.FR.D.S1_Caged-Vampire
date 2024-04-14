using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public float attackDistance = 1.0f; // distance to move forward during attack
    public float attackSpeed = 5.0f; // speed of the attack movement
    public int damageAmount = 10; // amount of damage dealt upon collision

    private Vector3 originalPosition;
    private bool isAttacking = false;

    void Start()
    {
        originalPosition = transform.localPosition; // store the original position
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) // check for attack input
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // Move forward
        float distanceMoved = 0f;
        while (distanceMoved < attackDistance)
        {
            float step = attackSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * step);
            distanceMoved += step;
            yield return null;
        }

        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Player")) // Check if collided object has the Player tag
        {
            HealthScript healthScript = other.GetComponent<HealthScript>();
            if (healthScript != null)
            {
                healthScript.TakeDamageFromSword(damageAmount); // Apply damage to the player
            }
        }
    }
}
