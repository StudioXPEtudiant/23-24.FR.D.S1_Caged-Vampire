using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public float pickupRange = 2f; // Adjust as needed for your game
    public KeyCode pickupKey = KeyCode.E; // Key to pick up the weapon
    private GameObject highlightedWeapon; // Reference to the highlighted weapon

    void Update()
    {
        // Check if the player presses the pickup key
        if (Input.GetKeyDown(pickupKey) && highlightedWeapon != null)
        {
            // Perform pickup action
            PickUpWeapon(highlightedWeapon);
        }

        // Raycast to detect nearby weapons
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            // Check if the detected object has the "Weapon" tag
            if (hitObject.CompareTag("Weapon"))
            {
                // Highlight the weapon
                HighlightWeapon(hitObject);
                highlightedWeapon = hitObject;
            }
            else
            {
                // If the detected object is not a weapon, remove highlight
                RemoveHighlight();
                highlightedWeapon = null;
            }
        }
        else
        {
            // If no object is detected, remove highlight
            RemoveHighlight();
            highlightedWeapon = null;
        }
    }

    void HighlightWeapon(GameObject weapon)
    {
        // Add your highlighting effect here, for example changing color or adding glow
        Renderer renderer = weapon.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Example: Change material color to yellow
            renderer.material.color = Color.yellow;
        }
    }

    void RemoveHighlight()
    {
        // Reset highlighted weapon to its original state
        if (highlightedWeapon != null)
        {
            Renderer renderer = highlightedWeapon.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Example: Reset material color
                renderer.material.color = Color.white;
            }
        }
    }

    void PickUpWeapon(GameObject weapon)
    {
        // Add your pickup logic here, for example adding the weapon to player's inventory
        Debug.Log("Picked up: " + weapon.name);
        // Example: Destroy the weapon object
        Destroy(weapon);
    
    }
   

}


