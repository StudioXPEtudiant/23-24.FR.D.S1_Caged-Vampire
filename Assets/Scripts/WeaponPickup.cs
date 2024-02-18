using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public float pickupDistance = 3f; // Distance at which the pickup is activated
    public Color highlightColor = Color.yellow; // Color to highlight the weapon
    private GameObject player; // Reference to the player object
    private bool isInRange = false; // Flag to indicate if the player is in range
    private bool isHighlighted = false; // Flag to indicate if the weapon is highlighted

    private bool isPickedUp = false; // Flag to indicate if the weapon is picked up

    private Vector3 pickupPosition; // The position where the weapon will go when picked up

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isPickedUp)
        {
            if (player == null) // Ensure player reference is valid
                return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= pickupDistance && !isInRange)
            {
                // Player is within pickup distance
                isInRange = true;
                HighlightWeapon(true);
            }
            else if (distanceToPlayer > pickupDistance && isInRange)
            {
                // Player is out of pickup distance
                isInRange = false;
                HighlightWeapon(false);
            }

            if (isHighlighted && isInRange && Input.GetKeyDown(KeyCode.E))
            {
                // If the weapon is highlighted and player is in range and E key is pressed, pick up the weapon
                PickUp();
            }
        }
        else
        {
            // If picked up, move the weapon to the pickup position
            transform.position = player.transform.position + new Vector3 (1,-0.1f,0);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                // If Q key is pressed, drop the weapon on the ground
                Drop();
            }
        }
    }

    private void HighlightWeapon(bool highlight)
    {
        if (highlight)
        {
            // Optionally, you can change the color of the weapon sprite to indicate it's highlighted
            GetComponent<SpriteRenderer>().color = highlightColor;
            isHighlighted = true;
        }
        else
        {
            // Optionally, you can revert the color of the weapon sprite to its original color
            GetComponent<SpriteRenderer>().color = Color.white;
            isHighlighted = false;
        }
    }

    private void PickUp()
    {
        isPickedUp = true;
        // Disable the highlight when the weapon is picked up
        HighlightWeapon(false);
        // Optionally, you may want to disable the collider of the weapon so it doesn't interfere with the player's movement
        GetComponent<Collider2D>().enabled = false;

        // Set the pickup position to the current player position
        pickupPosition = player.transform.position + new Vector3 (1,-0.1f,0);
    }

    private void Drop()
    {
        isPickedUp = false;
        // Re-enable the collider of the weapon
        GetComponent<Collider2D>().enabled = true;

        // Optionally, you may want to add some offset so the weapon doesn't collide with the player
        transform.position = player.transform.position + Vector3.right; // Drop the weapon slightly above the player

        // Re-enable the highlight if the player is in range
        if (isInRange)
        {
            HighlightWeapon(true);
        }
    }
}
