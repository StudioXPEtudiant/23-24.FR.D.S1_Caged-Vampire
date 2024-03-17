


using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public int damageAmount = 1; // the amount of damage this script will do
    public string WeakPointTag = "Weakpoint"; // the tag of the weak point hitbox

    // this method will be called when this object collides with another object

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthScript? healthScript = other.GetComponentInParent<HealthScript?>();

        if (healthScript != null)
        {

            // check if the object we collided with has a hitbox with the specified tag
            if (other.CompareTag("Weakpoint"))
            {
                // if the object has a hitbox with the specified tag, apply double damage to it
                healthScript.TakeDamage(damageAmount * 2);

            }
            else
            {
                // if the object does not have a hitbox with the specified tag, apply normal damage to it
                healthScript.TakeDamage(damageAmount * 1);

            }
        }
        else
        {

        }
    }
}
