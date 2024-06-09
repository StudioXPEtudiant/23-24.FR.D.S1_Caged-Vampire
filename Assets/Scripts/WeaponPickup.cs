using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public float pickupDistance = 3f;
    public Color highlightColor = Color.yellow;
    private GameObject player;
    private bool isInRange = false;
    public bool isPickedUp = false;
    private Item item; // The item associated with this pickup

    private Vector3 originalLocalPosition; // Store the original local position relative to the player

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Store the original local position relative to the player
        if (player != null)
        {
            originalLocalPosition = transform.localPosition;
        }
    }

    private void Update()
    {
        if (!isPickedUp)
        {
            if (player == null)
                return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= pickupDistance && !isInRange)
            {
                isInRange = true;
                HighlightWeapon(true);
            }
            else if (distanceToPlayer > pickupDistance && isInRange)
            {
                isInRange = false;
                HighlightWeapon(false);
            }

            if (isInRange && Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }
        else
        {
            // When picked up, move the weapon to the pickup position near the player
            transform.position = player.transform.position + new Vector3(1, -0.1f, 0);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }
    }

    private void HighlightWeapon(bool highlight)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = highlight ? highlightColor : Color.white;
        }
    }

    public void PickUp()
    {
        isPickedUp = true;
        HighlightWeapon(false);
        GetComponent<Collider2D>().enabled = false;

        // Calculate the offset based on player's facing direction when picking up
        if (player != null)
        {
            float horizontalInput = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
            transform.localPosition = originalLocalPosition * horizontalInput;
        }
    }

    public void Drop()
    {
        isPickedUp = false;
        GetComponent<Collider2D>().enabled = true;
        HighlightWeapon(true);
    }

    // Method to set the item details
    public void SetItem(Item newItem)
    {
        item = newItem;
        // Update the item's attributes as needed, such as sprite, name, etc.
        Debug.Log("Item set: " + item.itemName);
    }
}
