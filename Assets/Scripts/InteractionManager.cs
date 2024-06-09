using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public GameObject shopPanel;
    public Text goldText;
    public GameObject itemPreviewPanel;
    public Image itemPreviewImage;
    public Button buyButton;
    public int playerGold = 100;
    public float interactRange = 3f;
    public LayerMask merchantLayer;
    public Color highlightColor = Color.yellow;
    public List<GameObject> itemPrefabs = new List<GameObject>(); // List of item prefabs

    private GameObject currentMerchant;
    private bool inDialogue = false;
    private bool inShop = false;
    private Item selectedItem; // Selected item for purchase

    private Dictionary<GameObject, List<Item>> merchantItems = new Dictionary<GameObject, List<Item>>();

    void Start()
    {
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(false);
        itemPreviewPanel.SetActive(false);

        buyButton.onClick.AddListener(BuyItem);

        // Example setup for merchant items (replace with your own logic)
        PopulateMerchantItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!inDialogue && !inShop && currentMerchant != null)
            {
                inDialogue = true;
                dialoguePanel.SetActive(true);
                dialogueText.text = "Welcome! What would you like to buy?";

                ShowMerchantItems(currentMerchant);
            }
            else if (inDialogue)
            {
                inDialogue = false;
                dialoguePanel.SetActive(false);
                HideItemPreview();
            }
            else if (inShop)
            {
                inShop = false;
                shopPanel.SetActive(false);
            }
        }

        DetectNearbyMerchant();
        CheckWeaponPickup();
    }

    void DetectNearbyMerchant()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, merchantLayer))
        {
            GameObject merchant = hit.collider.gameObject;
            HighlightMerchant(merchant);
            currentMerchant = merchant;
        }
        else
        {
            RemoveHighlight();
            currentMerchant = null;
        }
    }

    void HighlightMerchant(GameObject merchant)
    {
        Renderer merchantRenderer = merchant.GetComponent<Renderer>();
        if (merchantRenderer != null)
        {
            merchantRenderer.material.color = highlightColor;
        }
    }

    void RemoveHighlight()
    {
        if (currentMerchant != null)
        {
            Renderer merchantRenderer = currentMerchant.GetComponent<Renderer>();
            if (merchantRenderer != null)
            {
                merchantRenderer.material.color = Color.white;
            }
        }
    }

    void ShowMerchantItems(GameObject merchant)
    {
        itemPreviewPanel.SetActive(false);

        if (merchantItems.ContainsKey(merchant))
        {
            List<Item> items = merchantItems[merchant];
            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                CreateItemButton(currentItem, i);
            }
        }
    }

    void CreateItemButton(Item item, int index)
    {
        GameObject buttonGO = new GameObject("ItemButton" + index);
        buttonGO.transform.SetParent(shopPanel.transform);

        Button itemButton = buttonGO.AddComponent<Button>();
        itemButton.onClick.AddListener(() => SelectItem(item));

        Text buttonText = buttonGO.AddComponent<Text>();
        buttonText.text = item.itemName + " - " + item.itemValue + " Gold";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 16;
        buttonText.alignment = TextAnchor.MiddleCenter;

        RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(200f, 30f);
        buttonRect.anchoredPosition = new Vector2(0f, -40f * index);
    }

    void SelectItem(Item item)
    {
        selectedItem = item;
        ShowItemPreview(selectedItem);
    }

    void ShowItemPreview(Item item)
    {
        itemPreviewPanel.SetActive(true);
        itemPreviewImage.sprite = Resources.Load<Sprite>(item.itemName + "Sprite"); // Load item sprite (adjust path as needed)
    }

    void HideItemPreview()
    {
        itemPreviewPanel.SetActive(false);
    }

    void BuyItem()
    {
        if (currentMerchant != null && playerGold >= selectedItem.itemValue && selectedItem != null)
        {
            playerGold -= selectedItem.itemValue;
            goldText.text = "Gold: " + playerGold.ToString();

            // Instantiate the selected item prefab and equip it
            if (itemPrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, itemPrefabs.Count);
                GameObject itemObject = Instantiate(itemPrefabs[randomIndex]);

                // Check if the instantiated item has WeaponPickup component
                WeaponPickup weaponPickup = itemObject.GetComponent<WeaponPickup>();
                if (weaponPickup != null)
                {
                    // Set the item details using WeaponPickup component
                    weaponPickup.SetItem(selectedItem);
                }
                else
                {
                    Debug.LogWarning("WeaponPickup component not found on instantiated item prefab.");
                }
            }

            Debug.Log("Item purchased: " + selectedItem.itemName);
        }
        else
        {
            Debug.Log("Cannot buy item!");
        }
    }

    void PopulateMerchantItems()
    {
        // Example setup for merchant items (replace with your own logic)
        GameObject merchant1 = GameObject.FindWithTag("Merchant1");
        GameObject merchant2 = GameObject.FindWithTag("Merchant2");

        if (merchant1 != null && merchant2 != null)
        {
            merchantItems.Add(merchant1, new List<Item>
            {
                new Item("Sword", 5, 15, 0, 1,"nothing","the starter weapon"),
                new Item("Bomb", 30, 30 , 0,1, "explosion", "explosive device"),
                new Item("Bow", 20, 2 , 0, 1, "nothing", "Wodden bow"),
                new Item ("Arrows", 7, 0, 0, 5, "nothing","ammo for bow")
                
            });

            merchantItems.Add(merchant2, new List<Item>
            {
                new Item("Healpotion", 30, 0, 5, 1,"regenarate health","healing potion to gain back health" ),
                new Item("Magnet", 10, 0 , 0,1, "attract coins", "magnet that attract coins for 10 seconds"),
                new Item("Shield", 10, 0 , 0,5,"blocks arrows", "shield that protects you from arrow shots but not from melee")
                
            });
        }
    }

    void CheckWeaponPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            GameObject weapon = hit.collider.gameObject;
            WeaponPickup weaponPickup = weapon.GetComponent<WeaponPickup>();
            if (weaponPickup != null && !weaponPickup.isPickedUp)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    weaponPickup.PickUp();
                    Debug.Log("Weapon picked up: " + weapon.name);
                }
            }
        }
    }




















}

