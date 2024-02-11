using UnityEngine;

public class PassiveAIWalk : MonoBehaviour
{
    public float speed = 3f; // The movement speed of the AI object
    public float detectionDistance = 5f; // The distance at which the AI detects the player
    public string[] colliderTags; // Array of collider tags that trigger the direction change
    private bool movingRight = true; // Flag to keep track of the movement direction
    private GameObject player; // Reference to the player object

    private void Start()
    {
        // Find the player object using its tag
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Move the AI object in the current direction
        if (player != null) // Check if player is found
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionDistance)
            {
                // Move towards the player
                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.Translate(direction * speed * Time.deltaTime);
            }
            else
            {
                // Move in the default direction
                if (movingRight)
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                else
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the AI object has collided with a collider with any of the specified tags
        if (ArrayContainsTag(other.tag))
        {
            // Reverse the direction of movement
            movingRight = !movingRight;
        }
    }

    private bool ArrayContainsTag(string tag)
    {
        // Check if the specified tag is present in the colliderTags array
        for (int i = 0; i < colliderTags.Length; i++)
        {
            if (colliderTags[i] == tag)
            {
                return true;
            }
        }
        return false;
    }
}

    
