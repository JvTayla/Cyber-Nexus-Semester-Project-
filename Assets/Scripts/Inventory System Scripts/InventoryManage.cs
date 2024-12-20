using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManage : MonoBehaviour
{
    public static InventoryManage Instance;
    public List<item> inventory = new List<item>(); // List of items in the inventory
    public List<GameObject> inventoryButtons = new List<GameObject>(); // Track UI buttons

    public Transform itemContent;
    public GameObject itemPrefab;
    private Controls playerInput;

    private int currentButtonIndex = 0;

    // Colors for selected and unselected states
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    private const float movementThreshold = 0.01f;
    public GameObject Testtube;
    public GameObject Chemicals; 
    public GameObject SecurityTag;
    public GameObject NucleurBattery; 
    public Transform holdPosition;
    public Transform SecurityCardHold;
    private Vector3[] originalScales;

    private void Awake()
    {
        Instance = this;
        playerInput = new Controls();
        playerInput.Inventory.Enable();
        playerInput.Player.Disable();

        originalScales = new Vector3[inventoryButtons.Count];
        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            originalScales[i] = inventoryButtons[i].transform.localScale;
        }
    }

    public void SpawnItem(item newItem)
    {
        inventory.Add(newItem);
        ListItems(); // Refresh the inventory UI
    }

    public void ListItems()
    {
        ClearInventoryDisplay();
        inventoryButtons.Clear();

        foreach (var item in inventory)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent); // Instantiate item button
            Image image = obj.transform.Find("ItemIcon").GetComponent<Image>();
            Text itemName = obj.transform.Find("ItemName").GetComponent<Text>();

            image.sprite = item.image;
            itemName.text = item.ItemName;

            // Add a listener to use the item when the button is clicked
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => UseItem(item));

            inventoryButtons.Add(obj); // Add to list for navigation
        }

        UpdateButtonSelection(); // Highlight the first item if any
    }

    public void ClearInventoryDisplay()
    {
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }
        inventoryButtons.Clear();
    }

    private void Update()
    {
        Vector2 moveInput = playerInput.Inventory.Movement.ReadValue<Vector2>();

        if (Mathf.Abs(moveInput.y) > movementThreshold)
        {
            NavigateInventory(moveInput.y, true); // True indicates vertical navigation
        }

        if (Mathf.Abs(moveInput.x) > movementThreshold)
        {
            NavigateInventory(moveInput.x, false); // False indicates horizontal navigation
        }
        if (playerInput.Inventory.Select.triggered)
        {
            SelectButton();
        }
    }

    public void NavigateInventory(float input, bool isVertical)
    {
        if (inventoryButtons.Count == 0) return;

        if (isVertical)
        {
            if (input > 0) // Move up
            {
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = inventoryButtons.Count - 1; // Loop back to end
                }
            }
            else if (input < 0) // Move down
            {
                currentButtonIndex++;
                if (currentButtonIndex >= inventoryButtons.Count)
                {
                    currentButtonIndex = 0; // Loop back to start
                }
            }
        }
        else
        {
            if (input > 0) // Move right
            {
                currentButtonIndex++;
                if (currentButtonIndex >= inventoryButtons.Count)
                {
                    currentButtonIndex = 0; // Loop back to start
                }
            }
            else if (input < 0) // Move left
            {
                currentButtonIndex--;
                if (currentButtonIndex < 0)
                {
                    currentButtonIndex = inventoryButtons.Count - 1; // Loop back to end
                }
            }
        }

        UpdateButtonSelection();
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            Image buttonImage = inventoryButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = (i == currentButtonIndex) ? selectedColor : defaultColor;
            }
            
        }
    }
    public void SelectButton()
    {
        // Get the Button component on the current inventory button  
        Button selectedButton = inventoryButtons[currentButtonIndex].GetComponent<Button>(); 

        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("No Button component found on the selected inventory button.");
        }
    }


    // Simplified UseItem function to instantiate the item at holdPosition
    public void UseItem(item selectedItem)
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("No item selected to use.");
            return;
        }
        if (holdPosition.childCount > 0)
        {
            foreach (Transform child in holdPosition)
            {
                Destroy(child.gameObject);
            }
        }

        // Check the type of action and instantiate the corresponding object
        if (selectedItem.actionType == item.ActionType.Research)
        {
            GameObject testube = Instantiate(Testtube, holdPosition.position, Quaternion.identity);
            testube.transform.position = holdPosition.position;
            testube.transform.rotation = holdPosition.rotation;
            testube.transform.parent = holdPosition;


            Debug.Log("Instantiated Testtube at holdPosition.");
        }
        else if (selectedItem.actionType == item.ActionType.experiment)
        {
            // Calculate the position with the offset directly
            Vector3 offsetPosition = holdPosition.position + new Vector3(0.5f, -0.12f, 0f);

            // Instantiate the chemicals object at the offset position with the same rotation as holdPosition
            GameObject chemicals = Instantiate(Chemicals, offsetPosition, holdPosition.rotation);

            // Set the chemicals object as a child of holdPosition
            chemicals.transform.parent = holdPosition;

            // Ensure it keeps the modified offset position even after parenting
            chemicals.transform.localPosition = new Vector3(0.5f, -0.12f, 0f); // Relative to holdPosition's origin
            chemicals.transform.rotation = holdPosition.rotation;


            Debug.Log("Instantiated Chemicals at holdPosition.");
        }
        else if (selectedItem.actionType == item.ActionType.Security)
        {
            GameObject securitytag = Instantiate(SecurityTag, SecurityCardHold.position, Quaternion.identity);
            securitytag.GetComponent<Rigidbody>().isKinematic = true;
            securitytag.transform.position = SecurityCardHold.position;
            securitytag.transform.rotation = SecurityCardHold.rotation * Quaternion.Euler(180f, 0f, 180f);

            securitytag.transform.parent = SecurityCardHold;



            Debug.Log("Instantiated SecurityTag at holdPosition.");
        }
        else if (selectedItem.actionType == item.ActionType.Nuclear)
        {
            GameObject battery = Instantiate(NucleurBattery, holdPosition.position, Quaternion.identity);
            battery.GetComponent<Rigidbody>().isKinematic = true;
            battery.transform.position = holdPosition.position;
            battery.transform.rotation = holdPosition.rotation * Quaternion.Euler(180f, 0f, 180f); 
            battery.transform.parent = holdPosition;


            Debug.Log("Instantiated Battery at holdPosition.");
        }
        else
        {
            Debug.LogWarning("Action type not recognized or not set for this item.");
        }
    }

}
