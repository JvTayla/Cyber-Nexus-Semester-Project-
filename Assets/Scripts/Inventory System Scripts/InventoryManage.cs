using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManage : MonoBehaviour
{
    public static InventoryManage Instance;
    public List<item> inventory = new List<item>();

    public Transform itemContent;
    public GameObject itemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnItem(item item)
    {
        inventory.Add(item);
    }

    public void ListItems()
    {
        // No need to clear here; this will be handled by ClearInventoryDisplay() when closing the inventory
        foreach (var item in inventory)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            Image image = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var ItemIcon = image;
            var ItemName = obj.transform.Find("ItemName").GetComponent<Text>();

            ItemIcon.sprite = item.image;
            ItemName.text = item.ItemName;
        }
    }

    public void ClearInventoryDisplay()
    {
        // Clear the existing content from the itemContent transform
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }
    }
}
