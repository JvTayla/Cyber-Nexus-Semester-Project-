using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class InventoryManage : MonoBehaviour
{
    /*Tile: Unity INventory System
     Author: Solo Game Dev
     Date: 12-16 August 2024
     Code Version: Unity 2022.3.38f1
     Availability: https://www.youtube.com/watch?v=AoD_F1fSFFg&t=614s
     */
    public static InventoryManage Instance;
    public List<item> inventory = new List<item>();
    public List<item> InventoryItems = new List<item>();

    public Transform itemContent;
    public GameObject itemPrefab;
    private Controls playerInput;
    
    private int currentButtonIndex = 0;

    // Materials for selected and unselected states
    public Material selectedMaterial; // Material for selected state
    private Material defaultMaterial; // Material for default state

    private const float movementThreshold = 0.8f;

    private void Awake()
    {
        Instance = this;
        playerInput = new Controls();
        playerInput.Inventory.Enable();
        playerInput.Player.Disable();
    }

    public void SpawnItem(item newItem)
    {
        // Add the new item to the inventory list
        inventory.Add(newItem);

        // Add the new item to the InventoryItems list
        InventoryItems.Add(newItem); // Add new item directly to InventoryItems

        // Alternatively, if you want to maintain the fixed size of the array:
        // Ensure InventoryItems array has enough size
        if (InventoryItems.Count > inventory.Count)
        {
            // If there is an excess in InventoryItems, remove it
            InventoryItems.RemoveAt(inventory.Count);
        }
    }

    public void ListItems()
    {
        ClearInventoryDisplay();

        foreach (var item in inventory)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            Image image = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var ItemIcon = image;
            var ItemName = obj.transform.Find("ItemName").GetComponent<Text>();
            Button button = obj.transform.Find("RemoveButton").GetComponent<Button>();

            ItemIcon.sprite = item.image;
            ItemName.text = item.ItemName;
        }
    } 

    public void ClearInventoryDisplay()
    {
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void Update()
    {
        Vector2 moveInput = playerInput.PauseMenu.Movement.ReadValue<Vector2>();

        // Check for vertical movement
        if (Mathf.Abs(moveInput.y) > movementThreshold)
        {
            NavigateInventory(moveInput.y, true); // True indicates vertical navigation
        }

        // Check for horizontal movement
        if (Mathf.Abs(moveInput.x) > movementThreshold)
        {
            NavigateInventory(moveInput.x, false); // False indicates horizontal navigation
        }
    }

    public void NavigateInventory(float input, bool isVertical)
    {
        if (isVertical) // Vertical navigation
        {
            if (input > 0) // Move up
            {
                currentButtonIndex++;
                if (currentButtonIndex >= InventoryItems.Count)
                {
                    currentButtonIndex = 0; // Loop back to the start
                }
                Debug.Log("Moved up, current button index: " + currentButtonIndex);
            }
            else if (input < 0) // Move down
            {
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = InventoryItems.Count - 1; // Loop back to the end
                }
            }
        }
        else // Horizontal navigation
        {
            if (input > 0) // Move right
            {
                currentButtonIndex++;
                if (currentButtonIndex >= InventoryItems.Count)
                {
                    currentButtonIndex = 0; // Loop back to the start
                }
                Debug.Log("Moved right, current button index: " + currentButtonIndex);
            }
            else if (input < 0) // Move left
            {
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = InventoryItems.Count - 1; // Loop back to the end
                }
            }
        }

        UpdateButtonSelection();
    }


    private void UpdateButtonSelection()
    {
        for (int i = 0; i < InventoryItems.Count; i++)
        {
            // Assuming each item has a Renderer component for the button material
            Renderer buttonRenderer = InventoryItems[i].GetComponent<Renderer>();
            if (i == currentButtonIndex)
            {
                buttonRenderer.material = selectedMaterial; // Ensure selectedMaterial is defined
            }
            else
            {
                buttonRenderer.material = defaultMaterial; // Ensure defaultMaterial is defined
            }
        }
    }
}
