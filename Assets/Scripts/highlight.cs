using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlight : MonoBehaviour
{

    public float pickupDistance = 3f;
    public Color highlightColor = Color.yellow;

    private GameObject player;
    private bool isInRange = false;
    private bool isPickedUp = false;

    private Vector3 originalLocalPosition; // Store the original local position relative to the player


    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Store the original local position relative to the player
        if (player != null)
        {
            originalLocalPosition = transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
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

        }
        
    
    }

    private void HighlightWeapon(bool highlight)
    {
        GetComponent<SpriteRenderer>().color = highlight ? highlightColor : Color.white;
    }
}

